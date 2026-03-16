using System;
using System.Management;    
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace RS232_PC
{
    public partial class MainForm : Form
    {
        private sealed class SerialRecord
        {
            public DateTime Timestamp { get; set; }
            public double? Force { get; set; }
            public string Unit { get; set; }
            public string RawLine { get; set; }
        }

        private static readonly Regex ReadingRegex = new Regex(
            @"^Reading:\s*(?<value>[+-]?\d+(?:[.,]\d+)?)\s*(?<unit>[A-Za-z]+)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private const int AcquisitionIntervalMilliseconds = 300;

        private string portnumber = "COM1";
        private string rs232 = "";
        private string messagetotarget = "";
        private string logfile = "";
        private readonly object _syncRoot = new object();
        private readonly StringBuilder _receiveBuffer = new StringBuilder();
        private readonly List<SerialRecord> _records = new List<SerialRecord>();
        private Timer _acquisitionTimer;
        private bool _acquisitionRunning;

        public string PortNumber { set { portnumber = value; } get { return portnumber; } }
        public string RS232 { set { rs232 = value; } get { return rs232; } }
        public string MessageToTarget { set { messagetotarget = value; } get { return messagetotarget; } }
        public string LogFile { set { logfile = value; } get { return logfile; } }
        public System.IO.Ports.SerialPort RS232Port { set { } get { return this.SerialPort; } }
        
        public MainForm()
        {
            InitializeComponent();
            textBoxSerial.Text = PortNumber;
            btSave.Enabled = false;
            _acquisitionTimer = new Timer();
            _acquisitionTimer.Interval = AcquisitionIntervalMilliseconds;
            _acquisitionTimer.Tick += acquisitionTimer_Tick;
        }

        // Run the program
        private void btStart_Click(object sender, EventArgs e)
        {
            if (_acquisitionRunning)
            {
                stopAcquisition();
                return;
            }

            if (!RS232Port.IsOpen)
            {
                MessageBox.Show("Open the serial port first");
                return;
            }

            lock (_syncRoot)
            {
                _records.Clear();
                _receiveBuffer.Clear();
            }

            _acquisitionRunning = true;
            btStart.Text = "Stop";
            btSave.Enabled = false;
            setReceived("[RUN] Acquisition started (" + AcquisitionIntervalMilliseconds.ToString(CultureInfo.InvariantCulture) + " ms)" + Environment.NewLine);
            _acquisitionTimer.Start();
        }

        // Select file to save in
        private void btSelect_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = @"C:\";
                saveFileDialog.Title = "Save acquisition CSV";
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.DefaultExt = "csv";
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LogFile = saveFileDialog.FileName;
                    btSave.Enabled = true;
                    MessageBox.Show("Selected file:\r\n" + LogFile);
                }
            }
        }
        
        // Save data to file
        private void btSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LogFile))
            {
                MessageBox.Show("Select a file first");
                return;
            }

            if (_acquisitionRunning)
            {
                MessageBox.Show("Stop the acquisition before saving");
                return;
            }

            List<SerialRecord> records;
            lock (_syncRoot)
            {
                records = _records.ToList();
            }

            using (var writer = new StreamWriter(LogFile, false, Encoding.UTF8))
            {
                writer.WriteLine("Timestamp,Force,Unit,RawLine");
                foreach (var record in records)
                {
                    writer.WriteLine(string.Join(",",
                        escape(record.Timestamp.ToString("O", CultureInfo.InvariantCulture)),
                        record.Force.HasValue ? record.Force.Value.ToString("G17", CultureInfo.InvariantCulture) : string.Empty,
                        escape(record.Unit ?? string.Empty),
                        escape(record.RawLine ?? string.Empty)));
                }
            }

            MessageBox.Show("Acquisition CSV saved:\r\n" + LogFile);
        }

        // Open RS232 port
        private void btOpen_Click(object sender, EventArgs e)
        {
            if (RS232Port.IsOpen)
            {
                MessageBox.Show("Serial Port already opened");
            }
            else
            {
                RS232Port.Open();
                btOpen.Enabled = false;
                btClose.Enabled = true;
            }
        }

        // Close RS232 port
        private void btClose_Click(object sender, EventArgs e)
        {
            if (RS232Port.IsOpen)
            {
                if (_acquisitionRunning)
                {
                    stopAcquisition();
                }
                RS232Port.Close();
                btOpen.Enabled = true;
                btClose.Enabled = false;
                btConfiguration.Enabled = true;
            }
            else
                MessageBox.Show("Serial Port is not opened");
        }

        // Open the RS232Form to setup the serial port parameters
        private void btConfiguration_Click(object sender, EventArgs e)
        {
            // Add here to code for the action
            RS232Form rsform = new RS232Form();
            // Set actual values to rsform
            rsform.PortName = PortNumber;
            rsform.BaudRate = RS232Port.BaudRate.ToString();
            rsform.Parity = RS232Port.Parity.ToString();
            rsform.StopBits = RS232Port.StopBits.ToString();
            rsform.DataBits = RS232Port.DataBits.ToString();
            rsform.WriteBufferSize = RS232Port.WriteBufferSize.ToString();
            // Update user Interface
            rsform.UpdateGUI();
            // Show rsform as a modal dialog and determine if DialogResult = OK.
            if (rsform.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of rsform's TextBox & ComboBox.
                RS232Port.PortName = rsform.PortName;
                RS232Port.BaudRate = int.Parse(rsform.BaudRate);
                // Search the corresponding parity in the Parity Enum define
                switch (rsform.Parity)
                {
                    case "Even":
                        RS232Port.Parity = Parity.Even;
                        break;
                    case "Mark":
                        RS232Port.Parity = Parity.Mark;
                        break;
                    case "None":
                        RS232Port.Parity = Parity.None;
                        break;
                    case "Odd":
                        RS232Port.Parity = Parity.Odd;
                        break;
                    case "Space":
                        RS232Port.Parity = Parity.Space;
                        break;
                    default:
                        break;
                }
                // Search the corresponding stopbits in the StopBits Enum define
                switch (rsform.StopBits)
                {
                    case "None":
                        RS232Port.StopBits = StopBits.None;
                        break;
                    case "One":
                        RS232Port.StopBits = StopBits.One;
                        break;
                    case "OnePointFive":
                        RS232Port.StopBits = StopBits.OnePointFive;
                        break;
                    case "Two":
                        RS232Port.StopBits = StopBits.Two;
                        break;
                    default:
                        break;
                }
                RS232Port.DataBits = int.Parse(rsform.DataBits);
                RS232Port.WriteBufferSize = int.Parse(rsform.WriteBufferSize);
                // bt Open ENABLE
                btOpen.Enabled = true;
                // bt Configuration DISABLE
                btConfiguration.Enabled = false;
            }
            else
            {
                MessageBox.Show("Cancelled");
            }
            rsform.Dispose();
        }

        // Open the About Form
        private void btAbout_Click(object sender, EventArgs e)
        {
            AboutFrom aboutForm = new AboutFrom();
            aboutForm.Show();
        }

        // 
        /// <summary>
        /// Method linked to the DataReceived event
        /// ATTENTION : ReadExisting char  in buffer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialport_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // store in rs232 all existing bytes in the serial port buffer
            rs232 = "";
            rs232 = RS232Port.ReadExisting();
            processReceivedChunk(rs232);
        }

        /// <summary>
        /// thread-safe call for Windows Forms Update (asynchronous call).
        /// </summary>
        /// <param name="message"></param>
        delegate void SetMessageCallBack(string message);
        /// <summary>
        /// Update textBoxRS232
        /// </summary>
        /// <param name="message"></param>
        private void setReceived(string message)
        {
            if (textBoxRS232.InvokeRequired) ///InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread. If these threads are different, it returns true.
            {
                SetMessageCallBack d = new SetMessageCallBack(setReceived);
                Invoke(d, new object[] { message });
            }
            else
            {
                textBoxRS232.AppendText(message);                
            }

        }

        private void acquisitionTimer_Tick(object sender, EventArgs e)
        {
            if (!_acquisitionRunning)
            {
                return;
            }

            if (!RS232Port.IsOpen)
            {
                stopAcquisition();
                MessageBox.Show("Serial Port not opened");
                return;
            }

            var command = string.IsNullOrWhiteSpace(MessageToTarget) ? "M" : MessageToTarget.Trim();
            RS232Port.WriteLine(command);
            setReceived("[TX] " + command + Environment.NewLine);
        }

        private void stopAcquisition()
        {
            _acquisitionTimer.Stop();
            _acquisitionRunning = false;
            btStart.Text = "Start";
            btSave.Enabled = !string.IsNullOrWhiteSpace(LogFile);
            setReceived("[RUN] Acquisition stopped" + Environment.NewLine);
        }

        private void processReceivedChunk(string chunk)
        {
            if (string.IsNullOrEmpty(chunk))
            {
                return;
            }

            setReceived(chunk);

            lock (_syncRoot)
            {
                _receiveBuffer.Append(chunk);

                while (true)
                {
                    var bufferContent = _receiveBuffer.ToString();
                    var lineBreakIndex = bufferContent.IndexOf('\n');
                    if (lineBreakIndex < 0)
                    {
                        break;
                    }

                    var line = bufferContent.Substring(0, lineBreakIndex).Trim('\r', '\n', '\t', ' ');
                    _receiveBuffer.Remove(0, lineBreakIndex + 1);

                    if (line.Length == 0)
                    {
                        continue;
                    }

                    _records.Add(parseRecord(line));
                }
            }
        }

        private static SerialRecord parseRecord(string line)
        {
            var record = new SerialRecord
            {
                Timestamp = DateTime.Now,
                Unit = string.Empty,
                RawLine = line
            };

            var match = ReadingRegex.Match(line);
            if (!match.Success)
            {
                return record;
            }

            var numericText = match.Groups["value"].Value.Replace(',', '.');
            if (double.TryParse(numericText, NumberStyles.Float, CultureInfo.InvariantCulture, out var force))
            {
                record.Force = force;
            }

            record.Unit = match.Groups["unit"].Value;
            return record;
        }

        private static string escape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (!value.Contains(",") && !value.Contains("\"") && !value.Contains("\r") && !value.Contains("\n"))
            {
                return value;
            }

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        /// <summary>
        /// When text changed schroll to the end
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxRS232_TextChanged(object sender, EventArgs e)
        {
            textBoxRS232.Refresh();
        }

        /// <summary>
        /// When Message to System has changed
        /// Update MessageToTarget string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxToSystem_TextChanged(object sender, EventArgs e)
        {
            MessageToTarget = textBoxToSystem.Text;
        }

        /// <summary>
        /// Reset Text in the textBoxRS232 control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReset_Click(object sender, EventArgs e)
        {
            textBoxRS232.Text = "";
            textBoxRS232.Refresh();
        }

        /// <summary>
        /// Write on serial port the text of the textBoxToSystem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSend_Click(object sender, EventArgs e)
        {
            string Message = MessageToTarget;
            if (RS232Port.IsOpen)
            {
                RS232Port.WriteLine(Message);
            }
            else 
            {
                MessageBox.Show("Serial Port not opened");
            }
        }

        /// <summary>
        /// Check all serial ports available in the machine
        /// Add then to listBox in GUI
        /// NEED : add Reference = System.Management
        /// NEED : using System.Management;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btListPort_Click(object sender, EventArgs e)
        {
            //show list of valid com ports
            listBoxSerialPort.Items.Clear();
            listBoxSerialDevice.Items.Clear();
            // Get all port names
            string[] portnames = SerialPort.GetPortNames();
            // Search more info on device
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                var tListNames = (from n in portnames
                             join p in ports on n equals p["DeviceID"].ToString()
                             select p["Caption"]).ToList();
                var tListDevice = (from n in portnames
                             join p in ports on n equals p["DeviceID"].ToString()
                             select n).ToList();
                if (tListDevice.Count != 0)
                {
                    foreach (string s in tListDevice)
                    {
                        listBoxSerialPort.Items.Add(s);
                    }
                    foreach (string s in tListNames)
                    {
                        listBoxSerialDevice.Items.Add(s);
                    }
                    MessageBox.Show("Select by double clic");
                }
                else
                    MessageBox.Show("No Serial Port on the PC");
            }
        }

        /// <summary>
        /// When Serial Port is selected then select Device Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxSerialDevice.SelectedIndex = listBoxSerialPort.SelectedIndex;
        }

        /// <summary>
        /// When Device Name is selected then select Serial Port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxSerialDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxSerialPort.SelectedIndex = listBoxSerialDevice.SelectedIndex;
        }
        
        /// <summary>
        /// When double click on list item then update serial port name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxSerialPort_DoubleClick(object sender, EventArgs e)
        {
            string text = listBoxSerialPort.GetItemText(listBoxSerialPort.SelectedItem);
            textBoxSerial.Text = text;
            PortNumber = textBoxSerial.Text;
            if (RS232Port.IsOpen)
                MessageBox.Show("Serial port is opened must be closed");
            else
                RS232Port.PortName = PortNumber;

        }

        /// <summary>
        /// When double click on list item then update serial port name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxSerialDevice_DoubleClick(object sender, EventArgs e)
        {
            listBoxSerialPort.SelectedItem = listBoxSerialDevice.SelectedIndex;
            string text = listBoxSerialPort.GetItemText(listBoxSerialPort.SelectedItem);
            textBoxSerial.Text = text;
            PortNumber = textBoxSerial.Text;
            if (RS232Port.IsOpen)
                MessageBox.Show("Serial port is opened must close");
            else
                RS232Port.PortName = PortNumber;
        }

        /// <summary>
        /// When new port name has been changed then update serial port name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSerial_TextChanged(object sender, EventArgs e)
        {
            PortNumber = textBoxSerial.Text;
            if (RS232Port.IsOpen)
                MessageBox.Show("Serial port is opened must close");
            else
                RS232Port.PortName = PortNumber;
        }
    }
}
