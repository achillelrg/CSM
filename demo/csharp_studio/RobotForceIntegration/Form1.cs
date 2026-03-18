using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RobotForceIntegration
{
    public partial class Form1 : Form
    {
        private sealed class Measure
        {
            public DateTime Timestamp { get; set; }
            public float Force { get; set; }
            public float PositionZ { get; set; }
        }

        private static readonly Regex ReadingRegex = new Regex(
            @"(?:Reading|Force)\s*:\s*(?<value>[+-]?\d+(?:[.,]\d+)?)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private static readonly Regex GenericNumberRegex = new Regex(
            @"(?<value>[+-]?\d+(?:[.,]\d+)?)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private const string DefaultRobotIp = "192.168.1.223";
        private const int ConnectionProbeIntervalMilliseconds = 1000;
        private const int AcquisitionIntervalMilliseconds = 300;
        private const int DodgeIntervalMilliseconds = 50;
        private const int DodgeForceSampleTimeoutMilliseconds = 100;
        private const int DodgeForceWatchdogMilliseconds = 100;
        private const float DodgeStopWatchdogSeconds = 0.03F;
        private const float DodgeVelocityWatchdogSeconds = 0.08F;
        private const float DodgeBaseLinearGainMmPerSecondPerKg = 440.0F;
        private const float DodgeVelocityGain = 4.0F;
        private const float DodgeMinimumVelocityMmPerSecond = 2.0F;
        private const int DodgeResponseGainPercent = 220;
        private const int DodgeSpeedLimitPercent = 220;
        private const int DodgeAccelerationPercent = 260;
        private const int DodgeBrakingPercent = 1400;
        private const int DodgeFilterPercent = 35;
        private const double ToolStepMinMm = 1.0;
        private const double ToolStepMaxMm = 100.0;
        private const double JointStepMinDegrees = 1.0;
        private const double JointStepMaxDegrees = 20.0;
        private const double DodgeThresholdMinKg = 0.1;
        private const double DodgeThresholdMaxKg = 1.0;
        private const int DodgeManualOverrideWindowMilliseconds = 1500;

        private readonly Robot xARM;
        private readonly List<Measure> measures;
        private readonly StringBuilder serialBuffer;
        private readonly float[] presetPose = { 300.0F, 0.0F, 200.0F, 180.0F, 0.0F, 0.0F };

        private string csvFile;
        private float currentForce;
        private float filteredForce;
        private float pendingPositionZ;
        private bool awaitingForceSample;
        private bool acquisitionRunning;
        private bool syncingSelfCollisionCheck;
        private bool dodgeAwaitingForceSample;
        private bool dodgeVelocityModeActive;
        private bool robotErrorDialogVisible;
        private bool robotDisconnectAlertShown;
        private bool robotWasConnected;
        private DateTime lastForceSampleUtc;
        private DateTime lastDodgeRequestUtc;
        private DateTime lastManualMotionUtc;
        private float lastDodgeVelocityMmPerSecond;
        private bool dodgeManualMotionActive;

        private float forceIntegralError = 0.0F;
        private float forceLastError = 0.0F;
        private bool forceControlAwaitingSample = false;
        private bool forceControlVelocityModeActive = false;
        private float lastForceControlVelocityMmPerSecond = 0.0F;
        private DateTime lastForceControlRequestUtc = DateTime.MinValue;


        public Form1()
        {
            InitializeComponent();

            xARM = new Robot();
            measures = new List<Measure>();
            serialBuffer = new StringBuilder();
            csvFile = string.Empty;
            currentForce = 0.0F;
            filteredForce = 0.0F;
            robotErrorDialogVisible = false;
            robotDisconnectAlertShown = false;
            robotWasConnected = false;

            SetSelfCollisionCheck(true);
            comboBoxCollisionSensitivity.SelectedIndex = 3;
            comboBoxBaudRate.SelectedItem = "115200";
            comboBoxParity.SelectedItem = Parity.None.ToString();
            comboBoxStopBits.SelectedItem = StopBits.One.ToString();
            comboBoxDataBits.SelectedItem = "8";
            textBoxRobotIp.Text = DefaultRobotIp;
            textBoxSensorCommand.Text = "M";
            trackBarToolStep.Value = MapLogValueToTrackBar(10.0, trackBarToolStep.Minimum, trackBarToolStep.Maximum, ToolStepMinMm, ToolStepMaxMm);
            trackBarJointStep.Value = 10;
            trackBarMotionSpeed.Value = 50;
            trackBarDodgeThreshold.Value = trackBarDodgeThreshold.Minimum;

            timerConnection.Interval = ConnectionProbeIntervalMilliseconds;
            timerCMD.Interval = AcquisitionIntervalMilliseconds;
            timerDodge.Interval = DodgeIntervalMilliseconds;

            UpdateChartDeadbandLines();
            lastForceSampleUtc = DateTime.MinValue;
            lastDodgeRequestUtc = DateTime.MinValue;
            
            timerForceControl.Interval = 50;
            lastForceControlRequestUtc = DateTime.MinValue;

            ApplyMotionSpeedProfile();
            UpdateToolStepDisplay();
            UpdateJointStepDisplay();
            UpdateDodgeThresholdDisplay();
            RefreshSerialPorts();
            UpdateRobotStatus(false);
            UpdateSerialStatus(false);
            ClearRobotTelemetry();
            UpdateForceDisplay();
            SyncUiState();

            timerConnection.Start();
        }

        private bool IsRobotConnected()
        {
            return xARM.IsCreated() && xARM.IsConnected();
        }

        private bool IsRobotReady()
        {
            return IsRobotConnected() && xARM.IsEnableMotion();
        }

        private bool IsSerialReady()
        {
            return serialPortSensor.IsOpen;
        }

        private void SetMotionControlsEnabled(bool enabled)
        {
            comboBoxCollisionSensitivity.Enabled = enabled;
            checkBoxSelfCollision.Enabled = enabled;
            buttonMoveHome.Enabled = enabled;
            buttonMovePreset.Enabled = enabled;
            buttonSetPreset.Enabled = enabled;
            trackBarToolStep.Enabled = enabled;
            trackBarJointStep.Enabled = enabled;
            buttonToolXMinus.Enabled = enabled;
            buttonToolXPlus.Enabled = enabled;
            buttonToolYMinus.Enabled = enabled;
            buttonToolYPlus.Enabled = enabled;
            buttonToolZMinus.Enabled = enabled;
            buttonToolZPlus.Enabled = enabled;
            buttonJoint1Minus.Enabled = enabled;
            buttonJoint1Plus.Enabled = enabled;
            buttonJoint2Minus.Enabled = enabled;
            buttonJoint2Plus.Enabled = enabled;
            buttonJoint3Minus.Enabled = enabled;
            buttonJoint3Plus.Enabled = enabled;
            buttonJoint4Minus.Enabled = enabled;
            buttonJoint4Plus.Enabled = enabled;
            buttonJoint5Minus.Enabled = enabled;
            buttonJoint5Plus.Enabled = enabled;
            buttonJoint6Minus.Enabled = enabled;
            buttonJoint6Plus.Enabled = enabled;
        }

        private void SyncUiState()
        {
            bool robotConnected = IsRobotConnected();
            bool robotReady = IsRobotReady();
            bool serialReady = IsSerialReady();
            bool dodgeAvailable = robotReady && serialReady && !acquisitionRunning;
            bool pidAvailable = dodgeAvailable;

            buttonToggleMotion.Enabled = robotConnected;
            buttonToggleMotion.Text = robotReady ? "Disable Motion" : "Enable Motion";
            trackBarMotionSpeed.Enabled = robotConnected;
            buttonSelectCsv.Enabled = !acquisitionRunning;
            buttonStartAcquisition.Enabled = robotReady && serialReady;
            buttonSaveCsv.Enabled = measures.Count > 0 && !string.IsNullOrWhiteSpace(csvFile) && !acquisitionRunning;
            buttonOpenSerial.Enabled = !serialReady;
            buttonCloseSerial.Enabled = serialReady;
            buttonSensorConfiguration.Enabled = !serialReady;
            textBoxSensorCommand.Enabled = serialReady;
            buttonSendCommand.Enabled = serialReady;
            buttonTare.Enabled = serialReady;
            buttonStartAcquisition.Text = acquisitionRunning ? "Stop Test" : "Start Test";
            checkBoxDodgeEnabled.Enabled = dodgeAvailable;
            trackBarDodgeThreshold.Enabled = dodgeAvailable;
            checkBoxForceControl.Enabled = pidAvailable;
            numericUpDownTargetForce.Enabled = pidAvailable;
            numericUpDownKp.Enabled = pidAvailable;
            numericUpDownKi.Enabled = pidAvailable;
            numericUpDownKd.Enabled = pidAvailable;
            numericUpDownMaxVelocity.Enabled = pidAvailable;
            checkBoxInvertForce.Enabled = pidAvailable;

            SetMotionControlsEnabled(robotReady);
            SyncDodgeTimerState();
            SyncForceControlTimerState();
        }

        private void UpdateRobotStatus(bool connected)
        {
            toolStripStatusLabelRobot.Text = connected ? "Robot: connected" : "Robot: not connected";
        }

        private void UpdateSerialStatus(bool connected)
        {
            toolStripStatusLabelSensor.Text = connected ? "Sensor: serial open" : "Sensor: serial closed";
        }

        private void RefreshRobotConnection()
        {
            bool connected = xARM.ProbeConnection();
            UpdateRobotStatus(connected);
            if (connected)
            {
                robotWasConnected = true;
                robotDisconnectAlertShown = false;
                RefreshRobotTelemetry();
                SetSelfCollisionCheck(xARM.GetSelfCollision());
            }
            else
            {
                HandleRobotDisconnected("Robot disconnected.", robotWasConnected && !robotDisconnectAlertShown);
                return;
            }

            SyncUiState();
        }

        private void RefreshRobotTelemetry()
        {
            if (!IsRobotConnected())
            {
                ClearRobotTelemetry();
                return;
            }

            textBoxJoint.Text = FormatValues(xARM.GetCurrentJoint(), 6);
            textBoxPosition.Text = FormatValues(xARM.GetCurrentPosition(), 6);
        }

        private static string FormatValues(float[] values, int count)
        {
            return string.Join(" | ", values.Take(count).Select(value => value.ToString("F2", CultureInfo.InvariantCulture)));
        }

        private void ClearRobotTelemetry()
        {
            textBoxJoint.Text = string.Empty;
            textBoxPosition.Text = string.Empty;
            SetSelfCollisionCheck(true);
        }

        private void SetSelfCollisionCheck(bool value)
        {
            syncingSelfCollisionCheck = true;
            checkBoxSelfCollision.Checked = value;
            syncingSelfCollisionCheck = false;
        }

        private void ApplyMotionSpeedProfile()
        {
            xARM.SetMotionScale(trackBarMotionSpeed.Value);
            labelMotionSpeedValue.Text = trackBarMotionSpeed.Value.ToString(CultureInfo.InvariantCulture) + "%";
        }

        private static double MapTrackBarToLogValue(int trackValue, int minimum, int maximum, double minValue, double maxValue)
        {
            double ratio = (double)(trackValue - minimum) / Math.Max(1, maximum - minimum);
            double minLog = Math.Log10(minValue);
            double maxLog = Math.Log10(maxValue);
            return Math.Pow(10.0, minLog + (maxLog - minLog) * ratio);
        }

        private static int MapLogValueToTrackBar(double value, int minimum, int maximum, double minValue, double maxValue)
        {
            double clampedValue = Math.Max(minValue, Math.Min(maxValue, value));
            double minLog = Math.Log10(minValue);
            double maxLog = Math.Log10(maxValue);
            double ratio = (Math.Log10(clampedValue) - minLog) / Math.Max(double.Epsilon, maxLog - minLog);
            return minimum + (int)Math.Round(ratio * (maximum - minimum), MidpointRounding.AwayFromZero);
        }

        private double GetDodgeThresholdKg()
        {
            return MapTrackBarToLogValue(
                trackBarDodgeThreshold.Value,
                trackBarDodgeThreshold.Minimum,
                trackBarDodgeThreshold.Maximum,
                DodgeThresholdMinKg,
                DodgeThresholdMaxKg);
        }

        private float GetToolStep()
        {
            return Math.Max(1.0F, (float)Math.Round(MapTrackBarToLogValue(
                trackBarToolStep.Value,
                trackBarToolStep.Minimum,
                trackBarToolStep.Maximum,
                ToolStepMinMm,
                ToolStepMaxMm), MidpointRounding.AwayFromZero));
        }

        private float GetJointStepDegrees()
        {
            return trackBarJointStep.Value;
        }

        private void UpdateToolStepDisplay()
        {
            labelToolStepValue.Text = GetToolStep().ToString("0", CultureInfo.InvariantCulture) + " mm";
        }

        private void UpdateJointStepDisplay()
        {
            labelJointStepValue.Text = GetJointStepDegrees().ToString("0", CultureInfo.InvariantCulture) + " deg";
        }

        private void UpdateDodgeThresholdDisplay()
        {
            labelDodgeThresholdValue.Text = GetDodgeThresholdKg().ToString("0.000", CultureInfo.InvariantCulture) + " kg";
        }

        private bool IsDodgeRunning()
        {
            return checkBoxDodgeEnabled.Checked && IsRobotReady() && IsSerialReady() && !acquisitionRunning;
        }

        private void SyncDodgeTimerState()
        {
            if (IsDodgeRunning())
            {
                if (!timerDodge.Enabled)
                    timerDodge.Start();
            }
            else
            {
                timerDodge.Stop();
                dodgeAwaitingForceSample = false;
                StopDodgeVelocityControl();
            }
        }

        private void RefreshSerialPorts()
        {
            string currentSelection = comboBoxSerialPort.Text;
            string[] ports = SerialPort.GetPortNames().OrderBy(port => port).ToArray();

            comboBoxSerialPort.Items.Clear();
            comboBoxSerialPort.Items.AddRange(ports);

            if (ports.Contains(currentSelection))
                comboBoxSerialPort.SelectedItem = currentSelection;
            else if (ports.Contains("COM4"))
                comboBoxSerialPort.SelectedItem = "COM4";
            else if (ports.Length > 0)
                comboBoxSerialPort.SelectedIndex = 0;
            else
                comboBoxSerialPort.Text = "COM4";
        }

        private void ConfigureSerialPort()
        {
            serialPortSensor.PortName = comboBoxSerialPort.Text.Trim();
            serialPortSensor.BaudRate = int.Parse(comboBoxBaudRate.Text, CultureInfo.InvariantCulture);
            serialPortSensor.Parity = (Parity)Enum.Parse(typeof(Parity), comboBoxParity.Text);
            serialPortSensor.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBoxStopBits.Text);
            serialPortSensor.DataBits = int.Parse(comboBoxDataBits.Text, CultureInfo.InvariantCulture);
            serialPortSensor.NewLine = "\r\n";
            serialPortSensor.ReadTimeout = 500;
            serialPortSensor.WriteTimeout = 500;
        }

        private void ShowSensorConfigurationDialog()
        {
            RefreshSerialPorts();

            using (Form dialog = new Form())
            {
                dialog.Text = "Sensor Configuration";
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.ClientSize = new Size(360, 265);

                Label labelSerial = new Label { Text = "Serial port", Location = new Point(16, 20), Size = new Size(100, 20) };
                ComboBox comboSerial = new ComboBox { Location = new Point(132, 17), Size = new Size(200, 24), DropDownStyle = ComboBoxStyle.DropDown };
                comboSerial.Items.AddRange(comboBoxSerialPort.Items.Cast<object>().ToArray());
                comboSerial.Text = comboBoxSerialPort.Text;

                Label labelBaud = new Label { Text = "Baud rate", Location = new Point(16, 56), Size = new Size(100, 20) };
                ComboBox comboBaud = new ComboBox { Location = new Point(132, 53), Size = new Size(200, 24), DropDownStyle = ComboBoxStyle.DropDownList };
                comboBaud.Items.AddRange(comboBoxBaudRate.Items.Cast<object>().ToArray());
                comboBaud.Text = comboBoxBaudRate.Text;

                Label labelParityDialog = new Label { Text = "Parity", Location = new Point(16, 92), Size = new Size(100, 20) };
                ComboBox comboParityDialog = new ComboBox { Location = new Point(132, 89), Size = new Size(200, 24), DropDownStyle = ComboBoxStyle.DropDownList };
                comboParityDialog.Items.AddRange(comboBoxParity.Items.Cast<object>().ToArray());
                comboParityDialog.Text = comboBoxParity.Text;

                Label labelStopDialog = new Label { Text = "Stop bits", Location = new Point(16, 128), Size = new Size(100, 20) };
                ComboBox comboStopDialog = new ComboBox { Location = new Point(132, 125), Size = new Size(200, 24), DropDownStyle = ComboBoxStyle.DropDownList };
                comboStopDialog.Items.AddRange(comboBoxStopBits.Items.Cast<object>().ToArray());
                comboStopDialog.Text = comboBoxStopBits.Text;

                Label labelDataDialog = new Label { Text = "Data bits", Location = new Point(16, 164), Size = new Size(100, 20) };
                ComboBox comboDataDialog = new ComboBox { Location = new Point(132, 161), Size = new Size(200, 24), DropDownStyle = ComboBoxStyle.DropDownList };
                comboDataDialog.Items.AddRange(comboBoxDataBits.Items.Cast<object>().ToArray());
                comboDataDialog.Text = comboBoxDataBits.Text;

                Label labelHint = new Label
                {
                    Text = "Changes apply next time the serial port is opened.",
                    Location = new Point(16, 194),
                    Size = new Size(316, 20)
                };

                Button buttonOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(176, 224), Size = new Size(75, 28) };
                Button buttonCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(257, 224), Size = new Size(75, 28) };

                dialog.Controls.Add(labelSerial);
                dialog.Controls.Add(comboSerial);
                dialog.Controls.Add(labelBaud);
                dialog.Controls.Add(comboBaud);
                dialog.Controls.Add(labelParityDialog);
                dialog.Controls.Add(comboParityDialog);
                dialog.Controls.Add(labelStopDialog);
                dialog.Controls.Add(comboStopDialog);
                dialog.Controls.Add(labelDataDialog);
                dialog.Controls.Add(comboDataDialog);
                dialog.Controls.Add(labelHint);
                dialog.Controls.Add(buttonOk);
                dialog.Controls.Add(buttonCancel);
                dialog.AcceptButton = buttonOk;
                dialog.CancelButton = buttonCancel;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                comboBoxSerialPort.Text = comboSerial.Text.Trim();
                comboBoxBaudRate.Text = comboBaud.Text;
                comboBoxParity.Text = comboParityDialog.Text;
                comboBoxStopBits.Text = comboStopDialog.Text;
                comboBoxDataBits.Text = comboDataDialog.Text;
            }
        }

        private void AppendSensorLog(string line)
        {
            textBoxSensorLog.AppendText("[" + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + "] " + line + Environment.NewLine);
        }

        private void UpdateForceDisplay()
        {
        }

        private void TareSensor()
        {
            currentForce = 0.0F;
            filteredForce = 0.0F;
            lastForceSampleUtc = DateTime.MinValue;
            UpdateForceDisplay();
            SendSensorCommand("T");
        }

        private void ShowRobotError(string action, int code)
        {
            if (IsRobotDisconnectCode(code))
            {
                HandleRobotDisconnected(action + " failed: robot disconnected.", robotWasConnected && !robotDisconnectAlertShown);
                return;
            }

            string message = action + " failed (code " + code.ToString(CultureInfo.InvariantCulture) + ").";
            if (code == 9)
                message = action + " failed: robot state is not ready to move (code 9).";

            ShowRobotMessage(message);
        }

        private bool IsRobotDisconnectCode(int code)
        {
            if (code == Robot.API_NOT_CONNECTED || !xARM.IsConnected())
                return true;

            if (code == 1)
                return !xARM.ProbeConnection();

            return false;
        }

        private void ShowRobotMessage(string message)
        {
            if (robotErrorDialogVisible)
                return;

            robotErrorDialogVisible = true;
            try
            {
                MessageBox.Show(message, "Robot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                robotErrorDialogVisible = false;
            }
        }

        private void HandleRobotDisconnected(string message, bool showAlert)
        {
            bool wasConnected = robotWasConnected;
            robotWasConnected = false;

            StopAcquisition(false);
            if (checkBoxDodgeEnabled.Checked)
                checkBoxDodgeEnabled.Checked = false;
            else
                StopDodgeVelocityControl();
                
            if (checkBoxForceControl.Checked)
                checkBoxForceControl.Checked = false;
            else
                StopForceControlVelocity();

            ClearRobotTelemetry();
            UpdateRobotStatus(false);
            SyncUiState();

            if (!showAlert || !wasConnected || robotDisconnectAlertShown)
                return;

            robotDisconnectAlertShown = true;
            ShowRobotMessage(message);
        }

        private bool ExecuteRobotCommand(Func<int> command, string action)
        {
            if (!IsRobotReady())
                return false;

            ApplyMotionSpeedProfile();

            int code = command();
            if (code == 9)
            {
                int recoverCode = xARM.PrepareMotion();
                if (recoverCode == 0)
                    code = command();
                else
                    code = recoverCode;
            }

            if (code != 0)
            {
                ShowRobotError(action, code);
                SyncUiState();
                return false;
            }

            RefreshRobotTelemetry();
            SyncUiState();
            return true;
        }

        private void MoveToolByStep(float dx, float dy, float dz, string action)
        {
            bool dodgeProtected = IsManualMotionProtectedByDodge();
            if (dodgeProtected)
                StopDodgeVelocityControl();

            bool executed = ExecuteRobotCommand(() => xARM.MoveToolRelative(dx, dy, dz, !dodgeProtected), action);
            if (!executed)
                return;

            if (dodgeProtected)
                MarkManualMotionForDodge();
        }

        private bool EnsureDodgeVelocityMode()
        {
            if (!IsRobotReady())
                return false;

            if (dodgeVelocityModeActive)
                return true;

            int code = xARM.PrepareCartesianVelocityMotion();
            if (code == 0)
            {
                dodgeVelocityModeActive = true;
                AppendSensorLog("[DODGE] Cartesian velocity mode active");
                return true;
            }

            if (code == 10)
            {
                AppendSensorLog("[DODGE] Cartesian velocity mode not ready yet (code 10), keeping dodge enabled");
                return false;
            }

            ShowRobotError("Enable dodge velocity mode", code);
            checkBoxDodgeEnabled.Checked = false;
            return false;
        }

        private void StopDodgeVelocityControl()
        {
            if (!dodgeVelocityModeActive && Math.Abs(lastDodgeVelocityMmPerSecond) < 0.001F)
                return;

            if (IsRobotConnected())
            {
                xARM.SetCartesianVelocity(0.0F, 0.0F, 0.0F, isToolCoord: true, duration: DodgeVelocityWatchdogSeconds);
                xARM.SetCartesianVelocityContinuous(false);
                if (xARM.IsEnableMotion())
                    xARM.PrepareMotion();
            }

            dodgeVelocityModeActive = false;
            lastDodgeVelocityMmPerSecond = 0.0F;
            ClearManualMotionForDodge();
        }

        private bool HasFreshDodgeForceSample()
        {
            return lastForceSampleUtc != DateTime.MinValue &&
                (DateTime.UtcNow - lastForceSampleUtc).TotalMilliseconds <= DodgeForceWatchdogMilliseconds;
        }

        private bool EvaluateDodgeImmediateStop()
        {
            if (!checkBoxDodgeEnabled.Checked)
                return false;

            if (!HasFreshDodgeForceSample())
                return false;

            if (Math.Abs(currentForce) >= (float)GetDodgeThresholdKg())
                return false;

            if (Math.Abs(lastDodgeVelocityMmPerSecond) < 0.001F)
                return false;

            if (!SendImmediateZeroDodgeVelocity())
                return true;

            if (!InterruptDodgeMotionImmediately("Immediate dodge stop"))
                return true;

            AppendSensorLog("[DODGE] Immediate stop on first raw sample below threshold");
            return true;
        }

        private void EnsureDefaultSelfCollisionEnabled()
        {
            if (!IsRobotReady() || !checkBoxSelfCollision.Checked)
                return;

            if (xARM.GetSelfCollision())
                return;

            int code = xARM.SetSelfCollision(true);
            if (code == 0)
            {
                code = xARM.PrepareMotion();
                if (code == 0)
                {
                    AppendSensorLog("Self collision enabled by default");
                    return;
                }
            }

            ShowRobotError("Enable default self collision", code);
        }

        private bool SendImmediateZeroDodgeVelocity()
        {
            if (!dodgeVelocityModeActive && Math.Abs(lastDodgeVelocityMmPerSecond) < 0.001F)
                return true;

            if (!IsRobotConnected())
            {
                lastDodgeVelocityMmPerSecond = 0.0F;
                return true;
            }

            int code = xARM.SetCartesianVelocity(0.0F, 0.0F, 0.0F, isToolCoord: true, duration: DodgeStopWatchdogSeconds);
            if (code != 0 && code != 9)
            {
                ShowRobotError("Zero dodge velocity", code);
                checkBoxDodgeEnabled.Checked = false;
                return false;
            }

            lastDodgeVelocityMmPerSecond = 0.0F;
            return true;
        }

        private bool InterruptDodgeMotionImmediately(string action)
        {
            if (IsRobotConnected())
            {
                int code = xARM.SetCartesianVelocity(0.0F, 0.0F, 0.0F, isToolCoord: true, duration: DodgeStopWatchdogSeconds);
                if (code != 0 && code != 9)
                {
                    ShowRobotError(action, code);
                    checkBoxDodgeEnabled.Checked = false;
                    return false;
                }

                xARM.SetCartesianVelocityContinuous(false);

                code = xARM.StopCurrentMotion();
                if (code != 0)
                {
                    ShowRobotError(action, code);
                    checkBoxDodgeEnabled.Checked = false;
                    return false;
                }

                if (xARM.IsEnableMotion())
                {
                    code = xARM.PrepareMotion();
                    if (code != 0)
                    {
                        ShowRobotError("Re-arm after dodge stop", code);
                        checkBoxDodgeEnabled.Checked = false;
                        return false;
                    }
                }
            }

            dodgeVelocityModeActive = false;
            lastDodgeVelocityMmPerSecond = 0.0F;
            dodgeAwaitingForceSample = false;
            ClearManualMotionForDodge();
            return true;
        }

        private bool IsDodgeObstacleDetected()
        {
            return checkBoxDodgeEnabled.Checked &&
                HasFreshDodgeForceSample() &&
                Math.Abs(GetDodgeEffectiveForceKg()) >= (float)GetDodgeThresholdKg();
        }

        private bool IsManualMotionProtectedByDodge()
        {
            return checkBoxDodgeEnabled.Checked && IsSerialReady() && !acquisitionRunning;
        }

        private void MarkManualMotionForDodge()
        {
            dodgeManualMotionActive = true;
            lastManualMotionUtc = DateTime.UtcNow;
        }

        private void ClearManualMotionForDodge()
        {
            dodgeManualMotionActive = false;
        }

        private bool StopManualMotionForDodge()
        {
            if (!dodgeManualMotionActive)
                return true;

            AppendSensorLog("[DODGE] Obstacle detected, cancelling manual motion");
            int code = xARM.StopCurrentMotion();
            if (code != 0)
            {
                ShowRobotError("Stop manual motion for dodge", code);
                checkBoxDodgeEnabled.Checked = false;
                return false;
            }

            ClearManualMotionForDodge();
            return true;
        }

        private float GetDodgeVelocityCapMmPerSecond()
        {
            return Math.Max(10.0F, trackBarMotionSpeed.Value * 4.0F * (DodgeSpeedLimitPercent / 100.0F));
        }

        private float GetDodgeResponseGain()
        {
            return DodgeResponseGainPercent / 100.0F;
        }

        private float GetDodgeLinearGainMmPerSecondPerKg()
        {
            return Math.Max(5.0F, DodgeBaseLinearGainMmPerSecondPerKg * GetDodgeResponseGain());
        }

        private float GetDodgeFilterAlpha()
        {
            return Math.Max(0.05F, Math.Min(0.95F, DodgeFilterPercent / 100.0F));
        }

        private float GetDodgeEffectiveForceKg()
        {
            float thresholdKg = (float)GetDodgeThresholdKg();
            if (Math.Abs(currentForce) <= thresholdKg)
                return currentForce;

            if (Math.Sign(currentForce) != Math.Sign(filteredForce) ||
                Math.Abs(currentForce) < Math.Abs(filteredForce))
            {
                return currentForce;
            }

            return filteredForce;
        }

        private static float MoveTowards(float currentValue, float targetValue, float maxDelta)
        {
            if (currentValue < targetValue)
                return Math.Min(currentValue + maxDelta, targetValue);

            return Math.Max(currentValue - maxDelta, targetValue);
        }

        private float ApplyDodgeVelocityDynamics(float targetVelocityMmPerSecond)
        {
            if (Math.Abs(targetVelocityMmPerSecond) < 0.001F)
                return 0.0F;

            float intervalSeconds = DodgeIntervalMilliseconds / 1000.0F;
            float cap = Math.Max(10.0F, GetDodgeVelocityCapMmPerSecond());
            float accelerationStep = Math.Max(5.0F, cap * (DodgeAccelerationPercent / 100.0F) * intervalSeconds);
            float brakingStep = Math.Max(accelerationStep, cap * (DodgeBrakingPercent / 100.0F) * intervalSeconds);
            bool braking = Math.Sign(targetVelocityMmPerSecond) != Math.Sign(lastDodgeVelocityMmPerSecond) ||
                Math.Abs(targetVelocityMmPerSecond) < Math.Abs(lastDodgeVelocityMmPerSecond);

            return MoveTowards(
                lastDodgeVelocityMmPerSecond,
                targetVelocityMmPerSecond,
                braking ? brakingStep : accelerationStep);
        }

        private float ComputeDodgeVelocityMmPerSecond()
        {
            if (!HasFreshDodgeForceSample())
                return 0.0F;

            float thresholdKg = (float)GetDodgeThresholdKg();
            float signedForceKg = GetDodgeEffectiveForceKg();
            float magnitudeKg = Math.Abs(signedForceKg);
            float excessKg = magnitudeKg - thresholdKg;
            if (excessKg <= 0.0F)
                return 0.0F;

            float targetVelocity = excessKg * GetDodgeLinearGainMmPerSecondPerKg();
            targetVelocity = Math.Max(DodgeMinimumVelocityMmPerSecond, targetVelocity);
            targetVelocity = Math.Min(GetDodgeVelocityCapMmPerSecond(), targetVelocity);

            return signedForceKg >= 0.0F ? targetVelocity : -targetVelocity;
        }

        private bool SendDodgeVelocityCommand(float velocityMmPerSecond)
        {
            if (Math.Abs(velocityMmPerSecond) > 0.001F &&
                dodgeManualMotionActive &&
                (DateTime.UtcNow - lastManualMotionUtc).TotalMilliseconds <= DodgeManualOverrideWindowMilliseconds)
            {
                if (!StopManualMotionForDodge())
                    return false;
            }

            if (dodgeManualMotionActive &&
                (DateTime.UtcNow - lastManualMotionUtc).TotalMilliseconds > DodgeManualOverrideWindowMilliseconds)
            {
                ClearManualMotionForDodge();
            }

            if (!EnsureDodgeVelocityMode())
                return false;

            float commandedVelocity = ApplyDodgeVelocityDynamics(velocityMmPerSecond);
            int code = xARM.SetCartesianVelocity(
                0.0F,
                0.0F,
                commandedVelocity,
                isToolCoord: true,
                duration: DodgeVelocityWatchdogSeconds);

            if (code == 9)
            {
                dodgeVelocityModeActive = false;
                if (!EnsureDodgeVelocityMode())
                    return false;

                code = xARM.SetCartesianVelocity(
                    0.0F,
                    0.0F,
                    commandedVelocity,
                    isToolCoord: true,
                    duration: DodgeVelocityWatchdogSeconds);
            }

            if (code != 0)
            {
                ShowRobotError("Dodge velocity control", code);
                checkBoxDodgeEnabled.Checked = false;
                return false;
            }

            lastDodgeVelocityMmPerSecond = commandedVelocity;
            return true;
        }

        private void MoveJointByStep(int jointIndex, float deltaDegrees)
        {
            if (!IsRobotReady())
                return;

            string action = "Move J" + (jointIndex + 1).ToString(CultureInfo.InvariantCulture);
            bool dodgeProtected = IsManualMotionProtectedByDodge();
            if (dodgeProtected)
                StopDodgeVelocityControl();

            float[] angles = xARM.GetCurrentJoint();
            float nextValue = angles[jointIndex] + deltaDegrees;
            nextValue = Math.Max(xARM.MinJoint[jointIndex], Math.Min(xARM.MaxJoint[jointIndex], nextValue));
            angles[jointIndex] = nextValue;
            bool executed = ExecuteRobotCommand(
                () => xARM.MoveJointValues(angles, wait: !dodgeProtected),
                action);

            if (executed && dodgeProtected)
                MarkManualMotionForDodge();
        }

        private bool EnsureCsvFileSelected()
        {
            if (!string.IsNullOrWhiteSpace(csvFile))
                return true;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = @"C:\";
                saveFileDialog.Title = "Save Force vs PositionZ CSV";
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.DefaultExt = "csv";
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return false;

                csvFile = saveFileDialog.FileName;
                textBoxCsvPath.Text = csvFile;
                SyncUiState();
                return true;
            }
        }

        private static string BuildFallbackCsvPath(string path)
        {
            string directory = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);

            if (string.IsNullOrWhiteSpace(directory))
                directory = Environment.CurrentDirectory;

            return Path.Combine(directory, fileName + "_" + timestamp + extension);
        }

        private void SaveMeasuresToCsv()
        {
            if (measures.Count == 0)
            {
                MessageBox.Show("No measurement available yet.", "CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!EnsureCsvFileSelected())
                return;

            string outputPath = csvFile;
            try
            {
                WriteMeasures(outputPath);
            }
            catch (IOException)
            {
                outputPath = BuildFallbackCsvPath(outputPath);
                WriteMeasures(outputPath);
                csvFile = outputPath;
                textBoxCsvPath.Text = csvFile;
                MessageBox.Show(
                    "The selected file is used by another process.\r\nA copy was saved here:\r\n" + csvFile,
                    "CSV",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            MessageBox.Show("CSV saved:\r\n" + outputPath, "CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SyncUiState();
        }

        private void WriteMeasures(string path)
        {
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("Timestamp,Force,PositionZ");
                foreach (Measure measure in measures)
                {
                    writer.WriteLine(
                        measure.Timestamp.ToString("O", CultureInfo.InvariantCulture) + "," +
                        measure.Force.ToString("G9", CultureInfo.InvariantCulture) + "," +
                        measure.PositionZ.ToString("G9", CultureInfo.InvariantCulture));
                }
            }
        }

        private void StartAcquisition()
        {
            if (!IsRobotReady())
            {
                MessageBox.Show("Connect the robot and enable motion first.", "Acquisition", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!IsSerialReady())
            {
                MessageBox.Show("Open the sensor serial port first.", "Acquisition", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!EnsureCsvFileSelected())
                return;

            measures.Clear();
            awaitingForceSample = false;
            acquisitionRunning = true;
            timerCMD.Start();
            AppendSensorLog("Acquisition started");
            SyncUiState();
        }

        private void StopAcquisition(bool logStop)
        {
            if (!acquisitionRunning && !timerCMD.Enabled)
            {
                SyncUiState();
                return;
            }

            timerCMD.Stop();
            acquisitionRunning = false;
            awaitingForceSample = false;

            if (logStop)
                AppendSensorLog("Acquisition stopped");

            SyncUiState();
        }

        private void SendSensorCommand(string command)
        {
            if (!IsSerialReady())
            {
                MessageBox.Show("Serial port is not open.", "Sensor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            serialPortSensor.WriteLine(command);
            AppendSensorLog("[TX] " + command);
        }

        private static bool TryParseForce(string line, out float force)
        {
            Match match = ReadingRegex.Match(line);
            if (!match.Success)
                match = GenericNumberRegex.Match(line);

            if (!match.Success)
            {
                force = 0.0F;
                return false;
            }

            string numericText = match.Groups["value"].Value.Replace(',', '.');
            return float.TryParse(numericText, NumberStyles.Float, CultureInfo.InvariantCulture, out force);
        }

        private void HandleSensorLine(string line)
        {
            AppendSensorLog("[RX] " + line);

            float force;
            if (!TryParseForce(line, out force))
                return;

            currentForce = force;
            if (lastForceSampleUtc == DateTime.MinValue)
            {
                filteredForce = force;
            }
            else
            {
                float appliedAlpha = GetDodgeFilterAlpha();
                if (Math.Sign(force) != Math.Sign(filteredForce) ||
                    Math.Abs(force) < Math.Abs(filteredForce))
                {
                    appliedAlpha = Math.Max(0.85F, appliedAlpha);
                }

                filteredForce = ((1.0F - appliedAlpha) * filteredForce) + (appliedAlpha * force);
            }

            if (Math.Abs(force) <= (float)GetDodgeThresholdKg())
                filteredForce = force;

            lastForceSampleUtc = DateTime.UtcNow;
            bool dodgeStopped = EvaluateDodgeImmediateStop();
            UpdateForceDisplay();

            if (forceControlAwaitingSample)
            {
                forceControlAwaitingSample = false;
                if (IsForceControlRunning())
                {
                    // --- ETAPE 1 : Lecture des consignes ---
                    float target = (float)numericUpDownTargetForce.Value;
                    float currentForceNewtons = currentForce * 9.81f; // Conversion Kg -> Newtons
                    float rawError = target - currentForceNewtons;
                    float dt = timerForceControl.Interval / 1000.0f;
                    
                    float Kp = (float)numericUpDownKp.Value;
                    float Ki = (float)numericUpDownKi.Value;
                    float Kd = (float)numericUpDownKd.Value;
                    
                    // --- ETAPE 2 : Traitement de la Zone Morte (Deadband) ---
                    float deadband = (float)numericUpDownDeadband.Value;
                    float Vz = 0.0f;
                    
                    if (Math.Abs(rawError) < deadband)
                    {
                        // Dans la zone morte : on evite la derive temporelle de l'integrateur
                        // et on trace l'erreur brute absolue pour ne pas fausser le D au prochain mouvement.
                        forceIntegralError = 0.0f;
                        forceLastError = rawError;
                        Vz = 0.0f;
                    }
                    else
                    {
                        // --- ETAPE 3 : Reprise du calcul PID decale (evite les a-coups) ---
                        float effectiveError = rawError > 0 ? (rawError - deadband) : (rawError + deadband);
                        
                        forceIntegralError += effectiveError * dt;
                        
                        // La derivee s'appuie sur la variation reelle d'effort
                        float derivative = (rawError - forceLastError) / dt;
                        forceLastError = rawError;
                        
                        // --- ETAPE 4 : Calcul PID ---
                        Vz = (Kp * effectiveError) + (Ki * forceIntegralError) + (Kd * derivative);
                    }
                    
                    // --- ETAPE 5 : Saturation (securite) et inversion optionnelle ---
                    float maxVel = (float)numericUpDownMaxVelocity.Value;
                    Vz = Math.Max(-maxVel, Math.Min(maxVel, Vz));
                    
                    if (!checkBoxInvertForce.Checked)
                        Vz = -Vz;

                    // --- ETAPE 5 : Envoi de la commande de vitesse au robot ---
                    SendForceControlVelocityCommand(Vz);
                    
                    // --- ETAPE 6 : Mise a jour du graphique en temps reel ---
                    chartForceVelocity.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                        chartForceVelocity.Series[0].Points.AddY(currentForceNewtons);
                        float velocityPercent = (maxVel > 0) ? (Vz / maxVel) * 100.0f : 0.0f;
                        chartForceVelocity.Series[1].Points.AddY(velocityPercent);
                        if (chartForceVelocity.Series[0].Points.Count > 100)
                        {
                            chartForceVelocity.Series[0].Points.RemoveAt(0);
                            chartForceVelocity.Series[1].Points.RemoveAt(0);
                        }
                    });
                }
            }

            if (dodgeAwaitingForceSample)
            {
                dodgeAwaitingForceSample = false;
                if (IsDodgeRunning() && !dodgeStopped && Math.Abs(currentForce) >= (float)GetDodgeThresholdKg())
                {
                    if (!SendDodgeVelocityCommand(ComputeDodgeVelocityMmPerSecond()))
                        return;
                }
            }

            if (!acquisitionRunning || !awaitingForceSample)
                return;

            measures.Add(new Measure
            {
                Timestamp = DateTime.Now,
                Force = currentForce,
                PositionZ = pendingPositionZ
            });
            awaitingForceSample = false;
        }

        private void ProcessSerialChunk(string chunk)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ProcessSerialChunk), chunk);
                return;
            }

            serialBuffer.Append(chunk);

            while (true)
            {
                string buffer = serialBuffer.ToString();
                int lineBreakIndex = buffer.IndexOf('\n');
                if (lineBreakIndex < 0)
                    break;

                string line = buffer.Substring(0, lineBreakIndex).Trim('\r', '\n', '\t', ' ');
                serialBuffer.Remove(0, lineBreakIndex + 1);

                if (line.Length == 0)
                    continue;

                HandleSensorLine(line);
            }
        }

        private void ButtonConnectRobot_Click(object sender, EventArgs e)
        {
            if (xARM.Create(textBoxRobotIp.Text.Trim()))
            {
                robotWasConnected = true;
                robotDisconnectAlertShown = false;
                ApplyMotionSpeedProfile();
                UpdateRobotStatus(true);
                AppendSensorLog("Robot connected");
                RefreshRobotTelemetry();
                SetSelfCollisionCheck(xARM.GetSelfCollision());
            }
            else
            {
                robotWasConnected = false;
                UpdateRobotStatus(false);
                string diagnostic = xARM.GetLastFailureMessage();
                AppendSensorLog("[ROBOT] Connection failed: " + diagnostic);
                MessageBox.Show(
                    diagnostic,
                    "Robot",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            SyncUiState();
        }

        private void ButtonToggleMotion_Click(object sender, EventArgs e)
        {
            if (!IsRobotConnected())
                return;

            int code;
            if (xARM.IsEnableMotion())
            {
                StopAcquisition(false);
                StopDodgeVelocityControl();
                code = xARM.EnableMotion(false);
                if (code != 0)
                    ShowRobotError("Disable motion", code);
            }
            else
            {
                code = xARM.EnableMotion(true);
                if (code == 0)
                {
                    code = xARM.PrepareMotion();
                    if (code == 0)
                    {
                        EnsureDefaultSelfCollisionEnabled();
                        ApplyMotionSpeedProfile();
                        RefreshRobotTelemetry();
                        AppendSensorLog("Robot motion enabled");
                    }
                    else
                    {
                        ShowRobotError("Prepare motion", code);
                    }
                }
                else
                {
                    ShowRobotError("Enable motion", code);
                }
            }

            SyncUiState();
        }

        private void ApplyCollisionSensitivitySelection()
        {
            if (!IsRobotReady())
                return;

            int sensitivity = comboBoxCollisionSensitivity.SelectedIndex;
            int code = xARM.SetCollisionSensitivity(sensitivity);
            if (code == 0)
            {
                code = xARM.PrepareMotion();
                if (code != 0)
                    ShowRobotError("Re-arm after sensitivity change", code);
            }
            else
            {
                ShowRobotError("Set collision sensitivity", code);
            }

            SyncUiState();
        }

        private void comboBoxCollisionSensitivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCollisionSensitivitySelection();
        }

        private void checkBoxSelfCollision_CheckedChanged(object sender, EventArgs e)
        {
            if (syncingSelfCollisionCheck)
                return;

            if (!IsRobotReady())
            {
                SetSelfCollisionCheck(xARM.GetSelfCollision());
                return;
            }

            int code = xARM.SetSelfCollision(checkBoxSelfCollision.Checked);
            if (code == 0)
            {
                code = xARM.PrepareMotion();
                if (code != 0)
                    ShowRobotError("Re-arm after self collision toggle", code);
            }
            else
            {
                bool previousValue = !checkBoxSelfCollision.Checked;
                SetSelfCollisionCheck(previousValue);
                ShowRobotError("Toggle self collision", code);
            }

            SyncUiState();
        }

        private void trackBarMotionSpeed_Scroll(object sender, EventArgs e)
        {
            ApplyMotionSpeedProfile();
        }

        private void trackBarDodgeThreshold_Scroll(object sender, EventArgs e)
        {
            UpdateDodgeThresholdDisplay();
        }

        private void checkBoxDodgeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDodgeEnabled.Checked && checkBoxForceControl.Checked)
                checkBoxForceControl.Checked = false;
                
            AppendSensorLog(checkBoxDodgeEnabled.Checked ? "[DODGE] Enabled" : "[DODGE] Disabled");
            if (checkBoxDodgeEnabled.Checked)
                TareSensor();
            else
                StopDodgeVelocityControl();
            SyncUiState();
        }

        private void ButtonMoveHome_Click(object sender, EventArgs e)
        {
            ExecuteRobotCommand(() => xARM.MoveHome(wait: true), "Move Home");
        }

        private void ButtonMovePreset_Click(object sender, EventArgs e)
        {
            ExecuteRobotCommand(() => xARM.MoveBase(presetPose, true), "Move To Preset");
        }

        private void ButtonSetPreset_Click(object sender, EventArgs e)
        {
            if (!IsRobotReady())
                return;

            float[] pose = xARM.GetCurrentPosition();
            for (int i = 0; i < 6; i++)
                presetPose[i] = pose[i];

            AppendSensorLog("Preset updated: " + FormatValues(presetPose, 6));
            RefreshRobotTelemetry();
        }

        private void ButtonToolXMinus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(-GetToolStep(), 0.0F, 0.0F, "Move Tool -X");
        }

        private void ButtonToolXPlus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(GetToolStep(), 0.0F, 0.0F, "Move Tool +X");
        }

        private void ButtonToolYMinus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(0.0F, -GetToolStep(), 0.0F, "Move Tool -Y");
        }

        private void ButtonToolYPlus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(0.0F, GetToolStep(), 0.0F, "Move Tool +Y");
        }

        private void ButtonToolZMinus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(0.0F, 0.0F, -GetToolStep(), "Move Tool -Z");
        }

        private void ButtonToolZPlus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(0.0F, 0.0F, GetToolStep(), "Move Tool +Z");
        }

        private void ButtonJoint1Minus_Click(object sender, EventArgs e) { MoveJointByStep(0, -GetJointStepDegrees()); }
        private void ButtonJoint1Plus_Click(object sender, EventArgs e) { MoveJointByStep(0, GetJointStepDegrees()); }
        private void ButtonJoint2Minus_Click(object sender, EventArgs e) { MoveJointByStep(1, -GetJointStepDegrees()); }
        private void ButtonJoint2Plus_Click(object sender, EventArgs e) { MoveJointByStep(1, GetJointStepDegrees()); }
        private void ButtonJoint3Minus_Click(object sender, EventArgs e) { MoveJointByStep(2, -GetJointStepDegrees()); }
        private void ButtonJoint3Plus_Click(object sender, EventArgs e) { MoveJointByStep(2, GetJointStepDegrees()); }
        private void ButtonJoint4Minus_Click(object sender, EventArgs e) { MoveJointByStep(3, -GetJointStepDegrees()); }
        private void ButtonJoint4Plus_Click(object sender, EventArgs e) { MoveJointByStep(3, GetJointStepDegrees()); }
        private void ButtonJoint5Minus_Click(object sender, EventArgs e) { MoveJointByStep(4, -GetJointStepDegrees()); }
        private void ButtonJoint5Plus_Click(object sender, EventArgs e) { MoveJointByStep(4, GetJointStepDegrees()); }
        private void ButtonJoint6Minus_Click(object sender, EventArgs e) { MoveJointByStep(5, -GetJointStepDegrees()); }
        private void ButtonJoint6Plus_Click(object sender, EventArgs e) { MoveJointByStep(5, GetJointStepDegrees()); }

        private void trackBarToolStep_Scroll(object sender, EventArgs e)
        {
            UpdateToolStepDisplay();
        }

        private void trackBarJointStep_Scroll(object sender, EventArgs e)
        {
            UpdateJointStepDisplay();
        }

        private void ButtonSensorConfiguration_Click(object sender, EventArgs e)
        {
            ShowSensorConfigurationDialog();
        }

        private void ButtonOpenSerial_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigureSerialPort();
                serialPortSensor.Open();
                UpdateSerialStatus(true);
                AppendSensorLog("Serial port opened on " + serialPortSensor.PortName);
            }
            catch (Exception ex)
            {
                UpdateSerialStatus(false);
                MessageBox.Show("Unable to open serial port:\r\n" + ex.Message, "Sensor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            SyncUiState();
        }

        private void ButtonCloseSerial_Click(object sender, EventArgs e)
        {
            StopAcquisition(false);
            StopDodgeVelocityControl();

            if (serialPortSensor.IsOpen)
            {
                serialPortSensor.Close();
                AppendSensorLog("Serial port closed");
            }

            UpdateSerialStatus(false);
            SyncUiState();
        }

        private void ButtonSendCommand_Click(object sender, EventArgs e)
        {
            string command = textBoxSensorCommand.Text.Trim();
            if (string.IsNullOrWhiteSpace(command))
                return;

            SendSensorCommand(command);
        }

        private void ButtonTare_Click(object sender, EventArgs e)
        {
            TareSensor();
        }

        private void ButtonSelectCsv_Click(object sender, EventArgs e)
        {
            EnsureCsvFileSelected();
        }

        private void ButtonSaveCsv_Click(object sender, EventArgs e)
        {
            SaveMeasuresToCsv();
        }

        private void ButtonStartAcquisition_Click(object sender, EventArgs e)
        {
            if (acquisitionRunning)
            {
                StopAcquisition(true);
                return;
            }

            StartAcquisition();
        }

        private void ButtonClearLog_Click(object sender, EventArgs e)
        {
            textBoxSensorLog.Clear();
        }


        /// <summary>
        /// Verifie si l'asservissement en force (Boucle PID) doit etre actif.
        /// </summary>
        private bool IsForceControlRunning()
        {
            return checkBoxForceControl.Checked && IsRobotReady() && IsSerialReady() && !acquisitionRunning;
        }

        /// <summary>
        /// Synchronise l'etat du timer d'acquisition dedie a l'asservissement PID.
        /// </summary>
        private void SyncForceControlTimerState()
        {
            if (IsForceControlRunning())
            {
                if (!timerForceControl.Enabled)
                    timerForceControl.Start();
            }
            else
            {
                timerForceControl.Stop();
                forceControlAwaitingSample = false;
                StopForceControlVelocity();
            }
        }

        /// <summary>
        /// Interrompt le mode vitesse continue du PID et reinitialise les erreurs (integrale/derivee).
        /// </summary>
        private void StopForceControlVelocity()
        {
            if (!forceControlVelocityModeActive && Math.Abs(lastForceControlVelocityMmPerSecond) < 0.001F)
                return;

            if (IsRobotConnected())
            {
                xARM.SetCartesianVelocity(0.0F, 0.0F, 0.0F, isToolCoord: true, duration: DodgeVelocityWatchdogSeconds);
                xARM.SetCartesianVelocityContinuous(false);
                if (xARM.IsEnableMotion())
                    xARM.PrepareMotion();
            }

            forceControlVelocityModeActive = false;
            lastForceControlVelocityMmPerSecond = 0.0F;
            forceIntegralError = 0.0F;
            forceLastError = 0.0F;
        }

        /// <summary>
        /// Bascule le controleur du robot en mode vitesse cartesienne continue si necessaire.
        /// </summary>
        private bool EnsureForceControlVelocityMode()
        {
            if (!IsRobotReady())
                return false;

            if (forceControlVelocityModeActive)
                return true;

            int code = xARM.PrepareCartesianVelocityMotion();
            if (code == 0)
            {
                forceControlVelocityModeActive = true;
                AppendSensorLog("[PID] Cartesian velocity mode active");
                return true;
            }

            ShowRobotError("Enable PID velocity mode", code);
            checkBoxForceControl.Checked = false;
            return false;
        }

        /// <summary>
        /// Envoie la consigne de vitesse calculee par le correcteur PID au controleur du bras.
        /// </summary>
        private bool SendForceControlVelocityCommand(float velocityMmPerSecond)
        {
            if (!EnsureForceControlVelocityMode())
                return false;

            int code = xARM.SetCartesianVelocity(
                0.0F,
                0.0F,
                velocityMmPerSecond,
                isToolCoord: true,
                duration: DodgeVelocityWatchdogSeconds);

            if (code == 9)
            {
                forceControlVelocityModeActive = false;
                if (!EnsureForceControlVelocityMode())
                    return false;

                code = xARM.SetCartesianVelocity(0.0F, 0.0F, velocityMmPerSecond, isToolCoord: true, duration: DodgeVelocityWatchdogSeconds);
            }

            if (code != 0)
            {
                ShowRobotError("PID velocity control", code);
                checkBoxForceControl.Checked = false;
                return false;
            }

            lastForceControlVelocityMmPerSecond = velocityMmPerSecond;
            return true;
        }


        private void numericUpDownDeadband_ValueChanged(object sender, EventArgs e)
        {
            UpdateChartDeadbandLines();
        }

        private void UpdateChartDeadbandLines()
        {
            if (chartForceVelocity.InvokeRequired)
            {
                chartForceVelocity.Invoke((System.Windows.Forms.MethodInvoker)UpdateChartDeadbandLines);
                return;
            }

            double deadband = (double)numericUpDownDeadband.Value;
            chartForceVelocity.ChartAreas[0].AxisY.StripLines.Clear();

            if (deadband > 0)
            {
                System.Windows.Forms.DataVisualization.Charting.StripLine topLine = new System.Windows.Forms.DataVisualization.Charting.StripLine();
                topLine.IntervalOffset = deadband;
                topLine.StripWidth = 0;
                topLine.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
                topLine.BorderColor = System.Drawing.Color.DarkGray;
                topLine.BorderWidth = 2;

                System.Windows.Forms.DataVisualization.Charting.StripLine bottomLine = new System.Windows.Forms.DataVisualization.Charting.StripLine();
                bottomLine.IntervalOffset = -deadband;
                bottomLine.StripWidth = 0;
                bottomLine.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
                bottomLine.BorderColor = System.Drawing.Color.DarkGray;
                bottomLine.BorderWidth = 2;

                chartForceVelocity.ChartAreas[0].AxisY.StripLines.Add(topLine);
                chartForceVelocity.ChartAreas[0].AxisY.StripLines.Add(bottomLine);
            }
        }

        private void checkBoxForceControl_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxForceControl.Checked && checkBoxDodgeEnabled.Checked)
            {
                checkBoxDodgeEnabled.Checked = false;
            }
            
            AppendSensorLog(checkBoxForceControl.Checked ? "[PID] Enabled" : "[PID] Disabled");
            if (checkBoxForceControl.Checked)
            {
                forceIntegralError = 0.0F;
                forceLastError = 0.0F;
                TareSensor();
            }
            else
            {
                StopForceControlVelocity();
            }
            SyncUiState();
        }

        private void timerForceControl_Tick(object sender, EventArgs e)
        {
            if (!IsForceControlRunning())
            {
                SyncForceControlTimerState();
                return;
            }

            if (forceControlAwaitingSample &&
                (DateTime.UtcNow - lastForceControlRequestUtc).TotalMilliseconds > DodgeForceSampleTimeoutMilliseconds)
            {
                forceControlAwaitingSample = false;
            }

            if (!forceControlAwaitingSample)
            {
                forceControlAwaitingSample = true;
                lastForceControlRequestUtc = DateTime.UtcNow;
                SendSensorCommand("M");
            }
        }

        private void timerDodge_Tick(object sender, EventArgs e)
        {
            if (!IsDodgeRunning())
            {
                SyncDodgeTimerState();
                return;
            }

            if (dodgeAwaitingForceSample &&
                (DateTime.UtcNow - lastDodgeRequestUtc).TotalMilliseconds > DodgeForceSampleTimeoutMilliseconds)
            {
                dodgeAwaitingForceSample = false;
            }

            if (!dodgeAwaitingForceSample)
            {
                dodgeAwaitingForceSample = true;
                lastDodgeRequestUtc = DateTime.UtcNow;
                SendSensorCommand("M");
            }
        }

        private void timerCMD_Tick(object sender, EventArgs e)
        {
            if (!IsRobotReady() || !IsSerialReady())
            {
                StopAcquisition(true);
                MessageBox.Show("Robot motion or sensor serial connection is not ready.", "Acquisition", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ExecuteRobotCommand(() => xARM.MoveToolRelative(0.0F, 0.0F, GetToolStep(), true), "Move Tool +Z"))
            {
                StopAcquisition(true);
                return;
            }

            pendingPositionZ = xARM.GetCurrentPosition()[2];
            awaitingForceSample = true;
            SendSensorCommand("M");
        }

        private void timerConnection_Tick(object sender, EventArgs e)
        {
            RefreshRobotConnection();
        }

        private void serialPortSensor_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string chunk = serialPortSensor.ReadExisting();
            if (!string.IsNullOrEmpty(chunk))
                ProcessSerialChunk(chunk);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopAcquisition(false);
            StopDodgeVelocityControl();
            StopForceControlVelocity();
            timerConnection.Stop();

            if (serialPortSensor.IsOpen)
                serialPortSensor.Close();

            if (xARM.IsCreated())
                xARM.EnableMotion(false);
        }
    }
}
