using System;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RS232_PC.Models;
using RS232_PC.Services;

namespace RS232_PC
{
    public partial class MainForm
    {
        private void ButtonRefreshPorts_Click(object sender, EventArgs e)
        {
            RefreshSerialPortList();
        }

        private void ButtonOpenSerial_Click(object sender, EventArgs e)
        {
            if (_sensorService.IsOpen)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(comboSerialPorts.Text))
            {
                ShowWarning("Please select a COM port first.");
                return;
            }

            try
            {
                _sensorService.PortName = comboSerialPorts.Text.Trim();
                _sensorService.Open();
                AppendSystemLog("Serial port opened on " + _sensorService.PortName + ".");
                UpdateUiState();
            }
            catch (Exception ex)
            {
                ShowWarning("Unable to open the serial port.\r\n" + ex.Message);
            }
        }

        private void ButtonCloseSerial_Click(object sender, EventArgs e)
        {
            if (_runMode != RunMode.Idle)
            {
                StopRun("Run stopped because the serial port was closed.");
            }

            _sensorService.Close();
            AppendSystemLog("Serial port closed.");
            UpdateUiState();
        }

        private void ButtonConnectRobot_Click(object sender, EventArgs e)
        {
            if (_robotService.Connect(textBoxRobotIp.Text, out var message))
            {
                AppendSystemLog(message);
                UpdateRobotStateDisplay();
            }
            else
            {
                ShowWarning(message);
            }

            UpdateUiState();
        }

        private void ButtonDisconnectRobot_Click(object sender, EventArgs e)
        {
            if (_runMode != RunMode.Idle)
            {
                StopRun("Run stopped because the robot was disconnected.");
            }

            _robotService.Disconnect();
            AppendSystemLog("Robot disconnected.");
            UpdateRobotStateDisplay();
            UpdateUiState();
        }

        private void ButtonEnableMotion_Click(object sender, EventArgs e)
        {
            if (_robotService.EnableMotion(out var message))
            {
                AppendSystemLog(message);
                RefreshRobotState(false);
            }
            else
            {
                ShowWarning(message);
            }

            UpdateRobotStateDisplay();
            UpdateUiState();
        }

        private void ButtonDisableMotion_Click(object sender, EventArgs e)
        {
            if (_runMode != RunMode.Idle)
            {
                StopRun("Run stopped because robot motion was disabled.");
            }

            _robotService.DisableMotion();
            AppendSystemLog("Robot motion disabled.");
            UpdateRobotStateDisplay();
            UpdateUiState();
        }

        private async Task RequestSingleMeasurementAsync()
        {
            if (!_sensorService.IsOpen)
            {
                ShowWarning("Open the serial port first.");
                return;
            }

            try
            {
                await _sensorService.RequestMeasurementAsync(MeasurementTimeoutMilliseconds);
            }
            catch (Exception ex)
            {
                ShowWarning("Measurement failed.\r\n" + ex.Message);
            }
        }

        private void ButtonSendCustomSensorCommand_Click(object sender, EventArgs e)
        {
            var command = textBoxCustomSensorCommand.Text.Trim();
            if (string.IsNullOrEmpty(command))
            {
                ShowWarning("Enter a sensor command first.");
                return;
            }

            SendSensorCommand(command);
        }

        private void ButtonRefreshRobotState_Click(object sender, EventArgs e)
        {
            RefreshRobotState(true);
        }

        private void ButtonMoveHome_Click(object sender, EventArgs e)
        {
            if (_robotService.MoveHome(out var message))
            {
                AppendSystemLog(message);
                RefreshRobotState(false);
            }
            else
            {
                ShowWarning(message);
            }
        }

        private void ButtonResetRobot_Click(object sender, EventArgs e)
        {
            if (_robotService.Reset(out var message))
            {
                AppendSystemLog(message);
            }
            else
            {
                ShowWarning(message);
            }
        }

        private void ButtonEmergencyStop_Click(object sender, EventArgs e)
        {
            if (_runMode != RunMode.Idle)
            {
                StopRun("Run stopped by emergency stop.");
            }

            if (_robotService.EmergencyStop(out var message))
            {
                AppendSystemLog(message);
            }
            else
            {
                ShowWarning(message);
            }

            UpdateRobotStateDisplay();
            UpdateUiState();
        }

        private void ButtonBrowseCsv_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "Save measurements as CSV";
                dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                dialog.DefaultExt = "csv";
                dialog.AddExtension = true;
                dialog.FileName = string.IsNullOrEmpty(_csvFilePath) ? "force-position.csv" : _csvFilePath;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _csvFilePath = dialog.FileName;
                    textBoxCsvPath.Text = _csvFilePath;
                }
            }
        }

        private void ButtonExportCsv_Click(object sender, EventArgs e)
        {
            if (_session.Count == 0)
            {
                ShowWarning("No measurement session to export.");
                return;
            }

            if (string.IsNullOrEmpty(_csvFilePath))
            {
                ButtonBrowseCsv_Click(sender, e);
                if (string.IsNullOrEmpty(_csvFilePath))
                {
                    return;
                }
            }

            _csvExportService.Export(_csvFilePath, _session.Samples);
            AppendSystemLog("CSV exported to " + _csvFilePath + ".");
        }

        private void ButtonClearSession_Click(object sender, EventArgs e)
        {
            if (_runMode != RunMode.Idle)
            {
                ShowWarning("Stop the current run before clearing the session.");
                return;
            }

            _session.Reset();
            UpdateSessionSummary();
            AppendSystemLog("Measurement session cleared.");
        }

        private void ButtonStartMechanical_Click(object sender, EventArgs e)
        {
            StartRun(RunMode.MechanicalTest);
        }

        private void ButtonStartForceControl_Click(object sender, EventArgs e)
        {
            StartRun(RunMode.ForceControl);
        }

        private void ButtonStopRun_Click(object sender, EventArgs e)
        {
            StopRun("Run stopped by the user.");
        }

        private async void RunTimer_Tick(object sender, EventArgs e)
        {
            if (_tickInProgress || _runMode == RunMode.Idle)
            {
                return;
            }

            _tickInProgress = true;
            try
            {
                await ExecuteRunTickAsync();
            }
            finally
            {
                _tickInProgress = false;
            }
        }

        private async Task ExecuteRunTickAsync()
        {
            if (!_sensorService.IsOpen)
            {
                StopRun("Serial port is not open.");
                return;
            }

            if (!_robotService.IsConnected || !_robotService.MotionEnabled)
            {
                StopRun("Robot is not ready for motion.");
                return;
            }

            ForceReading reading;
            try
            {
                reading = await _sensorService.RequestMeasurementAsync(MeasurementTimeoutMilliseconds);
            }
            catch (Exception ex)
            {
                StopRun("Failed to acquire the force measurement.\r\n" + ex.Message);
                return;
            }

            var pose = _robotService.GetCurrentPosition();
            var currentZ = pose.Length > 2 ? pose[2] : 0.0;
            var zWindow = (double)numericZWindow.Value;
            var threshold = (double)numericForceThreshold.Value;
            var plannedDeltaZ = _runMode == RunMode.MechanicalTest
                ? (double)numericMechanicalStepZ.Value
                : 0.0;
            double? filteredForce = reading.Force;
            double? setpoint = null;
            double? error = null;
            string stopReason = string.Empty;

            if (Math.Abs(reading.Force) >= threshold)
            {
                stopReason = "Force threshold exceeded.";
            }

            if (string.IsNullOrEmpty(stopReason) && Math.Abs(currentZ - _sessionStartZ) > zWindow)
            {
                stopReason = "Current Z position is outside the safety window.";
            }

            if (_runMode == RunMode.ForceControl)
            {
                var controlResult = _forceControlLoop.Compute(reading.Force, currentZ, runTimer.Interval / 1000.0);
                filteredForce = controlResult.FilteredForce;
                setpoint = _forceControlLoop.Setpoint;
                error = controlResult.Error;
                plannedDeltaZ = controlResult.DeltaZ;
                if (string.IsNullOrEmpty(stopReason) && !string.IsNullOrEmpty(controlResult.StopReason))
                {
                    stopReason = controlResult.StopReason;
                }
            }

            if (string.IsNullOrEmpty(stopReason) && Math.Abs((currentZ + plannedDeltaZ) - _sessionStartZ) > zWindow)
            {
                stopReason = "Planned move would leave the Z safety window.";
            }

            var commandedDelta = string.IsNullOrEmpty(stopReason) ? plannedDeltaZ : 0.0;

            _session.Add(new MeasurementSample
            {
                Timestamp = reading.Timestamp,
                Mode = _runMode == RunMode.MechanicalTest ? "MechanicalTest" : "ForceControl",
                Force = reading.Force,
                Unit = reading.Unit,
                FilteredForce = filteredForce,
                Setpoint = setpoint,
                Error = error,
                DeltaZ = commandedDelta,
                PositionZ = currentZ,
                RawLine = reading.RawLine
            });
            UpdateSessionSummary();

            if (!string.IsNullOrEmpty(stopReason))
            {
                StopRun(stopReason);
                return;
            }

            if (Math.Abs(commandedDelta) > 1e-6)
            {
                if (!_robotService.MoveToolRelative(0.0, 0.0, commandedDelta, true, out var moveMessage))
                {
                    StopRun(moveMessage);
                    return;
                }
            }

            RefreshRobotState(false);
        }

        private void StartRun(RunMode mode)
        {
            if (_runMode != RunMode.Idle)
            {
                ShowWarning("A run is already active.");
                return;
            }

            if (!_sensorService.IsOpen)
            {
                ShowWarning("Open the sensor serial port before starting a run.");
                return;
            }

            if (!_robotService.IsConnected)
            {
                ShowWarning("Connect the robot before starting a run.");
                return;
            }

            if (!_robotService.MotionEnabled)
            {
                ShowWarning("Enable robot motion before starting a run.");
                return;
            }

            var pose = _robotService.GetCurrentPosition();
            _sessionStartZ = pose.Length > 2 ? pose[2] : 0.0;
            _session.Begin(mode == RunMode.MechanicalTest ? "MechanicalTest" : "ForceControl");

            if (mode == RunMode.ForceControl)
            {
                _forceControlLoop.Reset(_sessionStartZ);
                _forceControlLoop.Setpoint = (double)numericForceSetpoint.Value;
                _forceControlLoop.Kp = (double)numericKp.Value;
                _forceControlLoop.Ki = (double)numericKi.Value;
                _forceControlLoop.Kd = (double)numericKd.Value;
                _forceControlLoop.Alpha = (double)numericAlpha.Value;
                _forceControlLoop.MaxDeltaZPerTick = (double)numericMaxDeltaZ.Value;
                _forceControlLoop.ZWindow = (double)numericZWindow.Value;
                _forceControlLoop.HardForceThreshold = (double)numericForceThreshold.Value;
                _forceControlLoop.OutputSign = checkBoxInvertControl.Checked ? -1.0 : 1.0;
            }

            runTimer.Interval = (int)numericTimerInterval.Value;
            _runMode = mode;
            _tickInProgress = false;
            UpdateSessionSummary();
            UpdateUiState();

            AppendSystemLog((mode == RunMode.MechanicalTest ? "Mechanical test" : "Force control") +
                " started from Z=" + _sessionStartZ.ToString("F3", CultureInfo.InvariantCulture) + ".");
            runTimer.Start();
        }

        private void StopRun(string reason)
        {
            if (_runMode == RunMode.Idle)
            {
                return;
            }

            runTimer.Stop();
            _runMode = RunMode.Idle;
            _tickInProgress = false;
            UpdateUiState();
            AppendSystemLog(reason);
            ShowInformation(reason, "Run stopped");
        }

        private void RefreshSerialPortList()
        {
            var currentSelection = comboSerialPorts?.Text ?? string.Empty;
            var ports = SerialPort.GetPortNames().OrderBy(port => port).ToArray();
            comboSerialPorts.Items.Clear();
            comboSerialPorts.Items.AddRange(ports);

            if (!string.IsNullOrEmpty(currentSelection))
            {
                comboSerialPorts.Text = currentSelection;
            }
            else if (ports.Length > 0)
            {
                comboSerialPorts.SelectedIndex = 0;
            }
        }

        private void SendSensorCommand(string command)
        {
            if (!_sensorService.IsOpen)
            {
                ShowWarning("Open the serial port first.");
                return;
            }

            try
            {
                _sensorService.SendCommand(command);
                AppendSystemLog("Sensor command sent: " + command + ".");
            }
            catch (Exception ex)
            {
                ShowWarning("Unable to send the sensor command.\r\n" + ex.Message);
            }
        }

        private void SensorService_RawLineReceived(object sender, string line)
        {
            BeginInvokeSafe(() =>
            {
                textBoxSensorLog.AppendText(line + Environment.NewLine);
            });
        }

        private void SensorService_MeasurementReceived(object sender, ForceReading reading)
        {
            BeginInvokeSafe(() =>
            {
                _activeUnit = reading.Unit;
                labelLastForceValue.Text = reading.Force.ToString("F3", CultureInfo.InvariantCulture);
                labelLastUnitValue.Text = reading.Unit;
                textBoxLastRaw.Text = reading.RawLine;
                UpdateCurrentUnitLabel();
            });
        }

        private void JogTool(string axis, double delta)
        {
            if (!_robotService.MoveToolRelative(
                    axis == "X" ? delta : 0.0,
                    axis == "Y" ? delta : 0.0,
                    axis == "Z" ? delta : 0.0,
                    true,
                    out var message))
            {
                ShowWarning(message);
                return;
            }

            AppendSystemLog(axis + " jog sent: " + delta.ToString("F2", CultureInfo.InvariantCulture) + " mm.");
            RefreshRobotState(false);
        }

        private void RefreshRobotState(bool showWarningOnFailure)
        {
            if (!_robotService.IsConnected)
            {
                textBoxRobotPose.Text = string.Empty;
                textBoxRobotJoints.Text = string.Empty;
                UpdateRobotStateDisplay();
                return;
            }

            try
            {
                textBoxRobotPose.Text = string.Join(" | ",
                    _robotService.GetCurrentPosition().Take(6).Select(value => value.ToString("F3", CultureInfo.InvariantCulture)));
                textBoxRobotJoints.Text = string.Join(" | ",
                    _robotService.GetCurrentJoints().Take(XArmRobotService.AxisCount).Select(value => value.ToString("F3", CultureInfo.InvariantCulture)));
                UpdateRobotStateDisplay();
            }
            catch (Exception ex)
            {
                if (showWarningOnFailure)
                {
                    ShowWarning("Unable to read the robot state.\r\n" + ex.Message);
                }
            }
        }

        private void UpdateRobotStateDisplay()
        {
            labelRobotState.Text = _robotService.IsConnected
                ? (_robotService.MotionEnabled ? "Connected / motion enabled" : "Connected / motion disabled")
                : "Disconnected";
            labelRobotVersion.Text = _robotService.IsConnected ? _robotService.Version : "-";
            toolStripRobotStatus.Text = "Robot: " + labelRobotState.Text;
        }

        private void UpdateUiState()
        {
            var serialOpen = _sensorService.IsOpen;
            var robotConnected = _robotService.IsConnected;
            var motionEnabled = _robotService.MotionEnabled;
            var runActive = _runMode != RunMode.Idle;

            labelSerialState.Text = serialOpen ? "Open" : "Closed";
            toolStripSerialStatus.Text = "Serial: " + (serialOpen ? "open" : "closed");
            UpdateRobotStateDisplay();

            buttonOpenSerial.Enabled = !serialOpen && !runActive;
            buttonCloseSerial.Enabled = serialOpen && !runActive;
            buttonRefreshPorts.Enabled = !runActive;
            comboSerialPorts.Enabled = !runActive && !serialOpen;

            buttonMeasure.Enabled = serialOpen && !runActive;
            buttonCalibrationStart.Enabled = serialOpen && !runActive;
            buttonCalibrationStop.Enabled = serialOpen && !runActive;
            buttonTare.Enabled = serialOpen && !runActive;
            buttonUnitToggle.Enabled = serialOpen && !runActive;
            buttonSendCustomSensorCommand.Enabled = serialOpen && !runActive;
            textBoxCustomSensorCommand.Enabled = serialOpen && !runActive;

            buttonConnectRobot.Enabled = !robotConnected && !runActive;
            buttonDisconnectRobot.Enabled = robotConnected && !runActive;
            buttonEnableMotion.Enabled = robotConnected && !motionEnabled && !runActive;
            buttonDisableMotion.Enabled = robotConnected && motionEnabled && !runActive;
            buttonRefreshRobotState.Enabled = robotConnected && !runActive;
            buttonMoveHome.Enabled = robotConnected && motionEnabled && !runActive;
            buttonResetRobot.Enabled = robotConnected && !runActive;
            buttonEmergencyStop.Enabled = robotConnected;

            buttonStartMechanical.Enabled = serialOpen && robotConnected && motionEnabled && !runActive;
            buttonStartForceControl.Enabled = serialOpen && robotConnected && motionEnabled && !runActive;
            buttonStopRun.Enabled = runActive;
            buttonBrowseCsv.Enabled = !runActive;
            buttonClearSession.Enabled = !runActive;
            buttonExportCsv.Enabled = !runActive && _session.Count > 0;

            labelRunState.Text = runActive ? _runMode.ToString() : "Idle";
            toolStripRunStatus.Text = "Run: " + labelRunState.Text;
            UpdateCurrentUnitLabel();
        }

        private void UpdateSessionSummary()
        {
            labelSampleCount.Text = _session.Count.ToString(CultureInfo.InvariantCulture) + " sample(s)";
            buttonExportCsv.Enabled = _runMode == RunMode.Idle && _session.Count > 0;
        }
    }
}
