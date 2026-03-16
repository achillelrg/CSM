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
                RefreshLegacyRobotState();
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

            if (legacyRobotTimer.Enabled)
            {
                legacyRobotTimer.Stop();
                buttonLegacyTimer.Text = "Start Timer";
            }

            _robotService.Disconnect();
            AppendSystemLog("Robot disconnected.");
            UpdateRobotStateDisplay();
            RefreshLegacyRobotState();
            UpdateUiState();
        }

        private void ButtonEnableMotion_Click(object sender, EventArgs e)
        {
            if (_robotService.EnableMotion(out var message))
            {
                AppendSystemLog(message);
                RefreshRobotState(false);
                RefreshLegacyRobotState();
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

            if (legacyRobotTimer.Enabled)
            {
                legacyRobotTimer.Stop();
                buttonLegacyTimer.Text = "Start Timer";
            }

            _robotService.DisableMotion();
            AppendSystemLog("Robot motion disabled.");
            UpdateRobotStateDisplay();
            RefreshLegacyRobotState();
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
                RefreshLegacyRobotState();
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
                RefreshLegacyRobotState();
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

            if (legacyRobotTimer.Enabled)
            {
                legacyRobotTimer.Stop();
                buttonLegacyTimer.Text = "Start Timer";
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
            RefreshLegacyRobotState();
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

        private async void ButtonStartHandGuiding_Click(object sender, EventArgs e)
        {
            await StartHandGuidingAsync();
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
            var zWindow = _runMode == RunMode.HandGuiding
                ? _handGuidingLoop.ZWindow
                : (_runMode == RunMode.ForceControl ? _forceControlLoop.ZWindow : (double)numericZWindow.Value);
            var threshold = _runMode == RunMode.HandGuiding
                ? _handGuidingLoop.HardForceThreshold
                : (_runMode == RunMode.ForceControl ? _forceControlLoop.HardForceThreshold : (double)numericForceThreshold.Value);
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
            else if (_runMode == RunMode.HandGuiding)
            {
                if (!IsKilogramUnit(reading.Unit))
                {
                    stopReason = "Hand guiding requires the sensor unit Kg.";
                }
                else
                {
                    var controlResult = _handGuidingLoop.Compute(reading.Force, currentZ);
                    filteredForce = controlResult.FilteredForce;
                    setpoint = 0.0;
                    error = controlResult.Error;
                    plannedDeltaZ = controlResult.DeltaZ;
                    if (string.IsNullOrEmpty(stopReason) && !string.IsNullOrEmpty(controlResult.StopReason))
                    {
                        stopReason = controlResult.StopReason;
                    }
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
                Mode = _runMode == RunMode.MechanicalTest
                    ? "MechanicalTest"
                    : (_runMode == RunMode.ForceControl ? "ForceControl" : "HandGuiding"),
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
            if (mode == RunMode.HandGuiding)
            {
                return;
            }

            if (!EnsureRunCanStart())
            {
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

        private async Task StartHandGuidingAsync()
        {
            if (!EnsureRunCanStart())
            {
                return;
            }

            _runSetupInProgress = true;
            UpdateUiState();

            try
            {
                ForceReading initialReading;
                try
                {
                    initialReading = await _sensorService.RequestMeasurementAsync(MeasurementTimeoutMilliseconds);
                }
                catch (Exception ex)
                {
                    ShowWarning("Unable to validate the sensor unit before hand guiding.\r\n" + ex.Message);
                    return;
                }

                if (!IsKilogramUnit(initialReading.Unit))
                {
                    ShowWarning("Hand guiding requires the sensor unit Kg.\r\nUse Change unit (U) until the sensor replies in Kg.");
                    return;
                }

                if (checkBoxHandGuidingAutoTare.Checked)
                {
                    try
                    {
                        _sensorService.SendCommand("T");
                        AppendSystemLog("Sensor tare command sent before hand guiding.");
                    }
                    catch (Exception ex)
                    {
                        ShowWarning("Unable to tare the sensor before hand guiding.\r\n" + ex.Message);
                        return;
                    }

                    await Task.Delay(300);
                }

                ForceReading validationReading;
                try
                {
                    validationReading = await _sensorService.RequestMeasurementAsync(MeasurementTimeoutMilliseconds);
                }
                catch (Exception ex)
                {
                    ShowWarning("Unable to validate the sensor reading before hand guiding.\r\n" + ex.Message);
                    return;
                }

                if (!IsKilogramUnit(validationReading.Unit))
                {
                    ShowWarning("Hand guiding requires the sensor unit Kg.\r\nUse Change unit (U) until the sensor replies in Kg.");
                    return;
                }

                var pose = _robotService.GetCurrentPosition();
                _sessionStartZ = pose.Length > 2 ? pose[2] : 0.0;
                _session.Begin("HandGuiding");

                _handGuidingLoop.Reset(_sessionStartZ);
                _handGuidingLoop.Alpha = 0.35;
                _handGuidingLoop.Deadband = (double)numericHandGuidingDeadband.Value;
                _handGuidingLoop.Gain = (double)numericHandGuidingGain.Value;
                _handGuidingLoop.MaxDeltaZPerTick = (double)numericHandGuidingMaxDeltaZ.Value;
                _handGuidingLoop.ZWindow = (double)numericHandGuidingZWindow.Value;
                _handGuidingLoop.HardForceThreshold = (double)numericHandGuidingForceThreshold.Value;
                _handGuidingLoop.OutputSign = checkBoxHandGuidingInvertControl.Checked ? -1.0 : 1.0;

                runTimer.Interval = (int)numericHandGuidingTimerInterval.Value;
                _runMode = RunMode.HandGuiding;
                _tickInProgress = false;
                UpdateSessionSummary();

                AppendSystemLog("Hand guiding started from Z=" +
                    _sessionStartZ.ToString("F3", CultureInfo.InvariantCulture) +
                    " with a zero-force target on TOOL Z.");
                runTimer.Start();
            }
            finally
            {
                _runSetupInProgress = false;
                UpdateUiState();
            }
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

        private bool EnsureRunCanStart()
        {
            if (_runMode != RunMode.Idle || _runSetupInProgress)
            {
                ShowWarning("A run is already active or starting.");
                return false;
            }

            if (legacyRobotTimer.Enabled)
            {
                legacyRobotTimer.Stop();
                buttonLegacyTimer.Text = "Start Timer";
            }

            if (!_sensorService.IsOpen)
            {
                ShowWarning("Open the sensor serial port before starting a run.");
                return false;
            }

            if (!_robotService.IsConnected)
            {
                ShowWarning("Connect the robot before starting a run.");
                return false;
            }

            if (!_robotService.MotionEnabled)
            {
                ShowWarning("Enable robot motion before starting a run.");
                return false;
            }

            return true;
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
                RefreshLegacyRobotState();
                return;
            }

            try
            {
                textBoxRobotPose.Text = string.Join(" | ",
                    _robotService.GetCurrentPosition().Take(6).Select(value => value.ToString("F3", CultureInfo.InvariantCulture)));
                textBoxRobotJoints.Text = string.Join(" | ",
                    _robotService.GetCurrentJoints().Take(XArmRobotService.AxisCount).Select(value => value.ToString("F3", CultureInfo.InvariantCulture)));
                UpdateRobotStateDisplay();
                RefreshLegacyRobotState();
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
            var runBusy = runActive || _runSetupInProgress;

            labelSerialState.Text = serialOpen ? "Open" : "Closed";
            toolStripSerialStatus.Text = "Serial: " + (serialOpen ? "open" : "closed");
            UpdateRobotStateDisplay();

            buttonOpenSerial.Enabled = !serialOpen && !runBusy;
            buttonCloseSerial.Enabled = serialOpen && !runBusy;
            buttonRefreshPorts.Enabled = !runBusy;
            comboSerialPorts.Enabled = !runBusy && !serialOpen;

            buttonMeasure.Enabled = serialOpen && !runBusy;
            buttonCalibrationStart.Enabled = serialOpen && !runBusy;
            buttonCalibrationStop.Enabled = serialOpen && !runBusy;
            buttonTare.Enabled = serialOpen && !runBusy;
            buttonUnitToggle.Enabled = serialOpen && !runBusy;
            buttonSendCustomSensorCommand.Enabled = serialOpen && !runBusy;
            textBoxCustomSensorCommand.Enabled = serialOpen && !runBusy;

            buttonConnectRobot.Enabled = !robotConnected && !runBusy;
            buttonDisconnectRobot.Enabled = robotConnected && !runBusy;
            buttonEnableMotion.Enabled = robotConnected && !motionEnabled && !runBusy;
            buttonDisableMotion.Enabled = robotConnected && motionEnabled && !runBusy;
            buttonRefreshRobotState.Enabled = robotConnected && !runBusy;
            buttonMoveHome.Enabled = robotConnected && motionEnabled && !runBusy;
            buttonResetRobot.Enabled = robotConnected && !runBusy;
            buttonEmergencyStop.Enabled = robotConnected;

            buttonStartMechanical.Enabled = serialOpen && robotConnected && motionEnabled && !runBusy;
            buttonStartForceControl.Enabled = serialOpen && robotConnected && motionEnabled && !runBusy;
            buttonStartHandGuiding.Enabled = serialOpen && robotConnected && motionEnabled && !runBusy;
            buttonStopRun.Enabled = runActive;
            buttonBrowseCsv.Enabled = !runBusy;
            buttonClearSession.Enabled = !runBusy;
            buttonExportCsv.Enabled = !runBusy && _session.Count > 0;

            numericHandGuidingTimerInterval.Enabled = !runBusy;
            numericHandGuidingDeadband.Enabled = !runBusy;
            numericHandGuidingGain.Enabled = !runBusy;
            numericHandGuidingMaxDeltaZ.Enabled = !runBusy;
            numericHandGuidingForceThreshold.Enabled = !runBusy;
            numericHandGuidingZWindow.Enabled = !runBusy;
            checkBoxHandGuidingAutoTare.Enabled = !runBusy;
            checkBoxHandGuidingInvertControl.Enabled = !runBusy;

            if (buttonLegacyCreateArm != null)
            {
                buttonLegacyCreateArm.Enabled = !robotConnected && !runBusy;
                buttonLegacyMotionArm.Enabled = robotConnected && !runBusy;
                buttonLegacyResetArm.Enabled = robotConnected && !runBusy;
                buttonLegacyGetJoint.Enabled = robotConnected && !runBusy;
                buttonLegacyGetPosition.Enabled = robotConnected && !runBusy;
                buttonLegacyGetBase.Enabled = robotConnected && !runBusy;
                buttonLegacyGetTcp.Enabled = robotConnected && !runBusy;
                buttonLegacyMoveHome.Enabled = robotConnected && motionEnabled && !runBusy;
                buttonLegacyMoveBase.Enabled = robotConnected && motionEnabled && !runBusy;
                buttonLegacyMoveTcp.Enabled = robotConnected && motionEnabled && !runBusy;
                buttonLegacyMoveAngle.Enabled = robotConnected && motionEnabled && !runBusy;
                buttonLegacyGetCollisionSensitivity.Enabled = robotConnected && !runBusy;
                buttonLegacySetCollisionSensitivity.Enabled = robotConnected && !runBusy;
                comboBoxLegacyCollisionSensitivity.Enabled = robotConnected && !runBusy;
                buttonLegacySelfCollision.Enabled = robotConnected && !runBusy;
                checkBoxLegacySelfCollision.Enabled = robotConnected && !runBusy;
                buttonLegacyTimer.Enabled = robotConnected && motionEnabled && !runBusy;
                textBoxLegacyRobotIp.Enabled = !robotConnected && !runBusy;
                buttonLegacyMotionArm.Text = motionEnabled ? "Stop Motion xARM" : "Motion xARM";
                if (!robotConnected && legacyRobotTimer.Enabled)
                {
                    legacyRobotTimer.Stop();
                    buttonLegacyTimer.Text = "Start Timer";
                }
            }

            labelRunState.Text = runActive ? _runMode.ToString() : (_runSetupInProgress ? "Starting" : "Idle");
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
