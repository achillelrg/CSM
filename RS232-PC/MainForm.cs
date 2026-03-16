using System;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RS232_PC.Models;
using RS232_PC.Services;

namespace RS232_PC
{
    public partial class MainForm : Form
    {
        private enum RunMode
        {
            Idle,
            MechanicalTest,
            ForceControl,
            HandGuiding
        }

        private const int MeasurementTimeoutMilliseconds = 800;

        private readonly SerialForceSensorService _sensorService;
        private readonly XArmRobotService _robotService;
        private readonly MeasurementSession _session;
        private readonly CsvExportService _csvExportService;
        private readonly ForceControlLoop _forceControlLoop;
        private readonly HandGuidingLoop _handGuidingLoop;

        private RunMode _runMode;
        private bool _tickInProgress;
        private bool _runSetupInProgress;
        private string _csvFilePath = string.Empty;
        private double _sessionStartZ;
        private string _activeUnit = "Kg";

        private TabControl tabControlMain;
        private ComboBox comboSerialPorts;
        private Label labelSerialState;
        private Button buttonRefreshPorts;
        private Button buttonOpenSerial;
        private Button buttonCloseSerial;
        private TextBox textBoxRobotIp;
        private Label labelRobotState;
        private Label labelRobotVersion;
        private Button buttonConnectRobot;
        private Button buttonDisconnectRobot;
        private Button buttonEnableMotion;
        private Button buttonDisableMotion;
        private TextBox textBoxSystemLog;
        private Label labelLastForceValue;
        private Label labelLastUnitValue;
        private TextBox textBoxLastRaw;
        private TextBox textBoxSensorLog;
        private Button buttonMeasure;
        private Button buttonCalibrationStart;
        private Button buttonCalibrationStop;
        private Button buttonTare;
        private Button buttonUnitToggle;
        private TextBox textBoxCustomSensorCommand;
        private Button buttonSendCustomSensorCommand;
        private TextBox textBoxRobotPose;
        private TextBox textBoxRobotJoints;
        private Button buttonRefreshRobotState;
        private Button buttonMoveHome;
        private Button buttonResetRobot;
        private Button buttonEmergencyStop;
        private NumericUpDown numericJogStep;
        private NumericUpDown numericTimerInterval;
        private NumericUpDown numericMechanicalStepZ;
        private NumericUpDown numericForceThreshold;
        private NumericUpDown numericZWindow;
        private NumericUpDown numericForceSetpoint;
        private NumericUpDown numericKp;
        private NumericUpDown numericKi;
        private NumericUpDown numericKd;
        private NumericUpDown numericAlpha;
        private NumericUpDown numericMaxDeltaZ;
        private CheckBox checkBoxInvertControl;
        private NumericUpDown numericHandGuidingTimerInterval;
        private NumericUpDown numericHandGuidingDeadband;
        private NumericUpDown numericHandGuidingGain;
        private NumericUpDown numericHandGuidingMaxDeltaZ;
        private NumericUpDown numericHandGuidingForceThreshold;
        private NumericUpDown numericHandGuidingZWindow;
        private CheckBox checkBoxHandGuidingAutoTare;
        private CheckBox checkBoxHandGuidingInvertControl;
        private TextBox textBoxCsvPath;
        private Button buttonBrowseCsv;
        private Button buttonExportCsv;
        private Button buttonStartMechanical;
        private Button buttonStartForceControl;
        private Button buttonStartHandGuiding;
        private Button buttonStopRun;
        private Button buttonClearSession;
        private Label labelRunState;
        private Label labelSampleCount;
        private DataGridView dataGridViewSamples;
        private ToolStripStatusLabel toolStripSerialStatus;
        private ToolStripStatusLabel toolStripRobotStatus;
        private ToolStripStatusLabel toolStripRunStatus;
        private TextBox textBoxLegacyRobotIp;
        private Button buttonLegacyCreateArm;
        private Button buttonLegacyMotionArm;
        private Button buttonLegacyResetArm;
        private Button buttonLegacyGetJoint;
        private TextBox textBoxLegacyJoint;
        private Button buttonLegacyGetPosition;
        private TextBox textBoxLegacyPosition;
        private Button buttonLegacyGetBase;
        private TextBox textBoxLegacyBase;
        private Button buttonLegacyGetTcp;
        private TextBox textBoxLegacyTcp;
        private Button buttonLegacyMoveHome;
        private Button buttonLegacyMoveBase;
        private Button buttonLegacyMoveTcp;
        private Button buttonLegacyMoveAngle;
        private Button buttonLegacyGetCollisionSensitivity;
        private TextBox textBoxLegacyCollisionSensitivity;
        private Button buttonLegacySetCollisionSensitivity;
        private ComboBox comboBoxLegacyCollisionSensitivity;
        private Button buttonLegacySelfCollision;
        private CheckBox checkBoxLegacySelfCollision;
        private Button buttonLegacyTimer;

        public MainForm()
        {
            InitializeComponent();

            _sensorService = new SerialForceSensorService();
            _robotService = new XArmRobotService();
            _session = new MeasurementSession();
            _csvExportService = new CsvExportService();
            _forceControlLoop = new ForceControlLoop();
            _handGuidingLoop = new HandGuidingLoop();

            BuildUi();
            WireServices();
            RefreshSerialPortList();
            UpdateUiState();
        }

        private void WireServices()
        {
            _sensorService.RawLineReceived += SensorService_RawLineReceived;
            _sensorService.MeasurementReceived += SensorService_MeasurementReceived;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            runTimer.Stop();
            legacyRobotTimer.Stop();
            _robotService.Dispose();
            _sensorService.Dispose();
        }

        private void AppendSystemLog(string message)
        {
            textBoxSystemLog.AppendText(
                "[" + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + "] " + message + Environment.NewLine);
        }

        private void BeginInvokeSafe(Action action)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(this, message, "CSM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowInformation(string message, string title)
        {
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static NumericUpDown CreateGainNumeric(decimal initialValue)
        {
            return new NumericUpDown
            {
                DecimalPlaces = 3,
                Minimum = 0.000M,
                Maximum = 100.0M,
                Increment = 0.005M,
                Value = initialValue,
                Width = 120
            };
        }

        private static bool IsKilogramUnit(string unit)
        {
            return string.Equals((unit ?? string.Empty).Trim(), "Kg", StringComparison.OrdinalIgnoreCase);
        }

        private static GroupBox CreateGroupSection(string title, Control content)
        {
            var groupBox = new GroupBox
            {
                Text = title,
                Dock = DockStyle.Fill
            };
            content.Dock = DockStyle.Fill;
            groupBox.Controls.Add(content);
            return groupBox;
        }

        private static TableLayoutPanel CreateSettingsTable(int rowCount)
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = rowCount,
                Padding = new Padding(8)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            for (var row = 0; row < rowCount; row++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            }

            return layout;
        }

        private static void AddSettingRow(TableLayoutPanel layout, int rowIndex, string labelText, Control control)
        {
            var label = new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            control.Dock = DockStyle.Fill;
            layout.Controls.Add(label, 0, rowIndex);
            layout.Controls.Add(control, 1, rowIndex);
        }

        private static Control FindControlByName(Control root, string name)
        {
            if (root.Name == name)
            {
                return root;
            }

            foreach (Control child in root.Controls)
            {
                var result = FindControlByName(child, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private double GetJogStep()
        {
            return (double)numericJogStep.Value;
        }

        private void UpdateCurrentUnitLabel()
        {
            var currentUnitLabel = FindControlByName(tabControlMain, "labelCurrentUnit") as Label;
            if (currentUnitLabel != null)
            {
                currentUnitLabel.Text = _activeUnit;
            }
        }
    }
}
