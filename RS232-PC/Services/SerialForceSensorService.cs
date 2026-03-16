using System;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RS232_PC.Models;

namespace RS232_PC.Services
{
    public sealed class SerialForceSensorService : IDisposable
    {
        private static readonly Regex ReadingRegex = new Regex(
            @"^Reading:\s*(?<value>[+-]?\d+(?:[.,]\d+)?)\s*(?<unit>[A-Za-z]+)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly object _syncRoot = new object();
        private readonly StringBuilder _buffer = new StringBuilder();
        private readonly SerialPort _serialPort;
        private TaskCompletionSource<ForceReading> _pendingMeasurement;

        public SerialForceSensorService()
        {
            _serialPort = new SerialPort
            {
                PortName = "COM5",
                BaudRate = 115200,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                WriteBufferSize = 2048,
                NewLine = "\n"
            };
            _serialPort.DataReceived += SerialPortOnDataReceived;
        }

        public event EventHandler<string> RawLineReceived;

        public event EventHandler<ForceReading> MeasurementReceived;

        public string PortName
        {
            get => _serialPort.PortName;
            set => _serialPort.PortName = value;
        }

        public int BaudRate
        {
            get => _serialPort.BaudRate;
            set => _serialPort.BaudRate = value;
        }

        public Parity Parity
        {
            get => _serialPort.Parity;
            set => _serialPort.Parity = value;
        }

        public StopBits StopBits
        {
            get => _serialPort.StopBits;
            set => _serialPort.StopBits = value;
        }

        public int DataBits
        {
            get => _serialPort.DataBits;
            set => _serialPort.DataBits = value;
        }

        public bool IsOpen => _serialPort.IsOpen;

        public ForceReading LastReading { get; private set; }

        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }

            lock (_syncRoot)
            {
                _buffer.Clear();
                _pendingMeasurement = null;
            }
        }

        public void SendCommand(string command)
        {
            if (!_serialPort.IsOpen)
            {
                throw new InvalidOperationException("Serial port is not open.");
            }

            _serialPort.WriteLine((command ?? string.Empty).Trim());
        }

        public async Task<ForceReading> RequestMeasurementAsync(int timeoutMilliseconds)
        {
            if (!_serialPort.IsOpen)
            {
                throw new InvalidOperationException("Serial port is not open.");
            }

            TaskCompletionSource<ForceReading> pending;
            lock (_syncRoot)
            {
                if (_pendingMeasurement != null && !_pendingMeasurement.Task.IsCompleted)
                {
                    throw new InvalidOperationException("A measurement request is already pending.");
                }

                _pendingMeasurement = new TaskCompletionSource<ForceReading>(TaskCreationOptions.RunContinuationsAsynchronously);
                pending = _pendingMeasurement;
            }

            SendCommand("M");

            var completedTask = await Task.WhenAny(pending.Task, Task.Delay(timeoutMilliseconds)).ConfigureAwait(false);
            if (completedTask != pending.Task)
            {
                lock (_syncRoot)
                {
                    if (ReferenceEquals(_pendingMeasurement, pending))
                    {
                        _pendingMeasurement = null;
                    }
                }

                throw new TimeoutException("Timeout while waiting for the force sensor response.");
            }

            return await pending.Task.ConfigureAwait(false);
        }

        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string chunk;
            try
            {
                chunk = _serialPort.ReadExisting();
            }
            catch
            {
                return;
            }

            if (string.IsNullOrEmpty(chunk))
            {
                return;
            }

            string[] lines;
            lock (_syncRoot)
            {
                _buffer.Append(chunk);
                lines = ExtractCompleteLines();
            }

            foreach (var line in lines)
            {
                ProcessLine(line);
            }
        }

        private string[] ExtractCompleteLines()
        {
            var content = _buffer.ToString();
            var split = content.Split('\n');
            if (!content.EndsWith("\n", StringComparison.Ordinal))
            {
                _buffer.Clear();
                _buffer.Append(split[split.Length - 1]);
                Array.Resize(ref split, split.Length - 1);
            }
            else
            {
                _buffer.Clear();
            }

            for (var index = 0; index < split.Length; index++)
            {
                split[index] = split[index].Trim('\r', ' ', '\t');
            }

            return split;
        }

        private void ProcessLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            RawLineReceived?.Invoke(this, line);

            if (!TryParseReading(line, out var reading))
            {
                return;
            }

            LastReading = reading;
            MeasurementReceived?.Invoke(this, reading);

            TaskCompletionSource<ForceReading> pending = null;
            lock (_syncRoot)
            {
                pending = _pendingMeasurement;
                _pendingMeasurement = null;
            }

            pending?.TrySetResult(reading);
        }

        private static bool TryParseReading(string line, out ForceReading reading)
        {
            var match = ReadingRegex.Match(line);
            if (!match.Success)
            {
                reading = null;
                return false;
            }

            var numericText = match.Groups["value"].Value.Replace(',', '.');
            if (!double.TryParse(numericText, NumberStyles.Float, CultureInfo.InvariantCulture, out var force))
            {
                reading = null;
                return false;
            }

            reading = new ForceReading
            {
                Timestamp = DateTime.Now,
                Force = force,
                Unit = match.Groups["unit"].Value,
                RawLine = line
            };

            return true;
        }

        public void Dispose()
        {
            Close();
            _serialPort.DataReceived -= SerialPortOnDataReceived;
            _serialPort.Dispose();
        }
    }
}
