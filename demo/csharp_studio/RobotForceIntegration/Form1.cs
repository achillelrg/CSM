using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
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
        private const float JointStepDegrees = 10.0F;

        private readonly Robot xARM;
        private readonly List<Measure> measures;
        private readonly StringBuilder serialBuffer;
        private readonly float[] presetPose = { 300.0F, 0.0F, 200.0F, 180.0F, 0.0F, 0.0F };

        private string csvFile;
        private float currentForce;
        private float pendingPositionZ;
        private bool awaitingForceSample;
        private bool acquisitionRunning;
        private bool syncingSelfCollisionCheck;

        public Form1()
        {
            InitializeComponent();

            xARM = new Robot();
            measures = new List<Measure>();
            serialBuffer = new StringBuilder();
            csvFile = string.Empty;
            currentForce = 0.0F;

            comboBoxCollisionSensitivity.SelectedIndex = 3;
            comboBoxBaudRate.SelectedItem = "115200";
            comboBoxParity.SelectedItem = Parity.None.ToString();
            comboBoxStopBits.SelectedItem = StopBits.One.ToString();
            comboBoxDataBits.SelectedItem = "8";
            textBoxRobotIp.Text = DefaultRobotIp;
            textBoxSensorCommand.Text = "M";
            numericUpDownToolStep.Value = 10;
            trackBarMotionSpeed.Value = 50;

            timerConnection.Interval = ConnectionProbeIntervalMilliseconds;
            timerCMD.Interval = AcquisitionIntervalMilliseconds;

            ApplyMotionSpeedProfile();
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
            buttonApplyCollisionSensitivity.Enabled = enabled;
            checkBoxSelfCollision.Enabled = enabled;
            buttonMoveHome.Enabled = enabled;
            buttonMovePreset.Enabled = enabled;
            buttonSetPreset.Enabled = enabled;
            numericUpDownToolStep.Enabled = enabled;
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

            buttonToggleMotion.Enabled = robotConnected;
            buttonToggleMotion.Text = robotReady ? "Disable Motion" : "Enable Motion";
            trackBarMotionSpeed.Enabled = robotConnected;
            buttonSelectCsv.Enabled = !acquisitionRunning;
            buttonStartAcquisition.Enabled = robotReady && serialReady;
            buttonSaveCsv.Enabled = measures.Count > 0 && !string.IsNullOrWhiteSpace(csvFile) && !acquisitionRunning;
            buttonOpenSerial.Enabled = !serialReady;
            buttonCloseSerial.Enabled = serialReady;
            buttonRefreshPorts.Enabled = !serialReady;
            comboBoxSerialPort.Enabled = !serialReady;
            comboBoxBaudRate.Enabled = !serialReady;
            comboBoxParity.Enabled = !serialReady;
            comboBoxStopBits.Enabled = !serialReady;
            comboBoxDataBits.Enabled = !serialReady;
            textBoxSensorCommand.Enabled = serialReady;
            buttonSendCommand.Enabled = serialReady;
            buttonRequestForce.Enabled = serialReady;
            buttonStartAcquisition.Text = acquisitionRunning ? "Stop Test" : "Start Test";

            SetMotionControlsEnabled(robotReady);
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
                RefreshRobotTelemetry();
                SetSelfCollisionCheck(xARM.GetSelfCollision());
            }
            else
            {
                StopAcquisition(false);
                ClearRobotTelemetry();
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
            textBoxBase.Text = FormatValues(xARM.GetBase(), 6);
            textBoxTcp.Text = FormatValues(xARM.GetTCP(), 6);
        }

        private static string FormatValues(float[] values, int count)
        {
            return string.Join(" | ", values.Take(count).Select(value => value.ToString("F2", CultureInfo.InvariantCulture)));
        }

        private void ClearRobotTelemetry()
        {
            textBoxJoint.Text = string.Empty;
            textBoxPosition.Text = string.Empty;
            textBoxBase.Text = string.Empty;
            textBoxTcp.Text = string.Empty;
            SetSelfCollisionCheck(false);
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

        private void AppendSensorLog(string line)
        {
            textBoxSensorLog.AppendText("[" + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + "] " + line + Environment.NewLine);
        }

        private void UpdateForceDisplay()
        {
            textBoxCurrentForce.Text = currentForce.ToString("F3", CultureInfo.InvariantCulture);
        }

        private float GetToolStep()
        {
            return (float)numericUpDownToolStep.Value;
        }

        private void ShowRobotError(string action, int code)
        {
            string message = action + " failed (code " + code.ToString(CultureInfo.InvariantCulture) + ").";
            if (code == 9)
                message = action + " failed: robot state is not ready to move (code 9).";

            MessageBox.Show(message, "Robot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            ExecuteRobotCommand(() => xARM.MoveToolRelative(dx, dy, dz, true), action);
        }

        private void MoveJointByStep(int jointIndex, float deltaDegrees)
        {
            if (!IsRobotReady())
                return;

            float[] angles = xARM.GetCurrentJoint();
            float nextValue = angles[jointIndex] + deltaDegrees;
            nextValue = Math.Max(xARM.MinJoint[jointIndex], Math.Min(xARM.MaxJoint[jointIndex], nextValue));
            angles[jointIndex] = nextValue;
            ExecuteRobotCommand(
                () => xARM.MoveJointValues(angles, wait: true),
                "Move J" + (jointIndex + 1).ToString(CultureInfo.InvariantCulture));
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
            UpdateForceDisplay();

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
                ApplyMotionSpeedProfile();
                UpdateRobotStatus(true);
                AppendSensorLog("Robot connected");
                RefreshRobotTelemetry();
                SetSelfCollisionCheck(xARM.GetSelfCollision());
            }
            else
            {
                UpdateRobotStatus(false);
                MessageBox.Show("Unable to connect to the robot.", "Robot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void ButtonApplyCollisionSensitivity_Click(object sender, EventArgs e)
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

        private void ButtonJoint1Minus_Click(object sender, EventArgs e) { MoveJointByStep(0, -JointStepDegrees); }
        private void ButtonJoint1Plus_Click(object sender, EventArgs e) { MoveJointByStep(0, JointStepDegrees); }
        private void ButtonJoint2Minus_Click(object sender, EventArgs e) { MoveJointByStep(1, -JointStepDegrees); }
        private void ButtonJoint2Plus_Click(object sender, EventArgs e) { MoveJointByStep(1, JointStepDegrees); }
        private void ButtonJoint3Minus_Click(object sender, EventArgs e) { MoveJointByStep(2, -JointStepDegrees); }
        private void ButtonJoint3Plus_Click(object sender, EventArgs e) { MoveJointByStep(2, JointStepDegrees); }
        private void ButtonJoint4Minus_Click(object sender, EventArgs e) { MoveJointByStep(3, -JointStepDegrees); }
        private void ButtonJoint4Plus_Click(object sender, EventArgs e) { MoveJointByStep(3, JointStepDegrees); }
        private void ButtonJoint5Minus_Click(object sender, EventArgs e) { MoveJointByStep(4, -JointStepDegrees); }
        private void ButtonJoint5Plus_Click(object sender, EventArgs e) { MoveJointByStep(4, JointStepDegrees); }
        private void ButtonJoint6Minus_Click(object sender, EventArgs e) { MoveJointByStep(5, -JointStepDegrees); }
        private void ButtonJoint6Plus_Click(object sender, EventArgs e) { MoveJointByStep(5, JointStepDegrees); }

        private void ButtonRefreshPorts_Click(object sender, EventArgs e)
        {
            RefreshSerialPorts();
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

        private void ButtonRequestForce_Click(object sender, EventArgs e)
        {
            SendSensorCommand("M");
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
            timerConnection.Stop();

            if (serialPortSensor.IsOpen)
                serialPortSensor.Close();

            if (xARM.IsCreated())
                xARM.EnableMotion(false);
        }
    }
}
