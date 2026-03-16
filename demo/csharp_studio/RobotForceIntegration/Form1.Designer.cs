namespace RobotForceIntegration
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBoxRobot = CreateGroupBox("Robot", 12, 12, 350, 240);
            this.labelRobotIp = CreateLabel("Robot IP", 15, 20, 70, 16);
            this.textBoxRobotIp = CreateTextBox(90, 17, 233, 20);
            this.buttonConnectRobot = CreateButton("Connect", 18, 48, 95, 23, this.ButtonConnectRobot_Click);
            this.buttonToggleMotion = CreateButton("Enable Motion", 129, 48, 120, 23, this.ButtonToggleMotion_Click);
            this.labelCollisionTarget = CreateLabel("Collision sensitivity", 15, 91, 120, 16);
            this.comboBoxCollisionSensitivity = CreateComboBox(18, 111, 120, 21, true);
            this.comboBoxCollisionSensitivity.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5" });
            this.buttonApplyCollisionSensitivity = CreateButton("Apply", 154, 109, 95, 23, this.ButtonApplyCollisionSensitivity_Click);
            this.checkBoxSelfCollision = CreateCheckBox("Self collision", 18, 148, true);
            this.checkBoxSelfCollision.CheckedChanged += new System.EventHandler(this.checkBoxSelfCollision_CheckedChanged);
            this.labelMotionSpeed = CreateLabel("Motion speed", 15, 184, 90, 16);
            this.trackBarMotionSpeed = CreateTrackBar(18, 204, 231, 45, 1, 100, 50);
            this.trackBarMotionSpeed.Scroll += new System.EventHandler(this.trackBarMotionSpeed_Scroll);
            this.labelMotionSpeedValue = CreateLabel("50%", 262, 204, 61, 20);
            this.groupBoxRobot.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.labelRobotIp, this.textBoxRobotIp, this.buttonConnectRobot, this.buttonToggleMotion,
                this.labelCollisionTarget, this.comboBoxCollisionSensitivity, this.buttonApplyCollisionSensitivity,
                this.checkBoxSelfCollision, this.labelMotionSpeed, this.trackBarMotionSpeed, this.labelMotionSpeedValue
            });

            this.groupBoxRobotState = CreateGroupBox("Robot State", 378, 12, 790, 137);
            this.labelJoint = CreateLabel("Joint", 15, 22, 70, 16);
            this.textBoxJoint = CreateTextBox(101, 19, 672, 20, true);
            this.labelPosition = CreateLabel("Position", 15, 51, 70, 16);
            this.textBoxPosition = CreateTextBox(101, 48, 672, 20, true);
            this.labelBase = CreateLabel("Base", 15, 80, 70, 16);
            this.textBoxBase = CreateTextBox(101, 77, 672, 20, true);
            this.labelTcp = CreateLabel("TCP", 15, 109, 70, 16);
            this.textBoxTcp = CreateTextBox(101, 106, 672, 20, true);
            this.groupBoxRobotState.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.labelJoint, this.textBoxJoint, this.labelPosition, this.textBoxPosition,
                this.labelBase, this.textBoxBase, this.labelTcp, this.textBoxTcp
            });

            this.groupBoxTool = CreateGroupBox("Tool Motion", 378, 155, 383, 207);
            this.labelToolStep = CreateLabel("Step TOOL (mm)", 15, 31, 100, 16);
            this.numericUpDownToolStep = CreateNumeric(124, 29, 90, 20, 0, 1, 100, 10);
            this.buttonToolXMinus = CreateButton("-X", 18, 68, 90, 28, this.ButtonToolXMinus_Click);
            this.buttonToolXPlus = CreateButton("+X", 124, 68, 90, 28, this.ButtonToolXPlus_Click);
            this.buttonToolYMinus = CreateButton("-Y", 18, 102, 90, 28, this.ButtonToolYMinus_Click);
            this.buttonToolYPlus = CreateButton("+Y", 124, 102, 90, 28, this.ButtonToolYPlus_Click);
            this.buttonToolZMinus = CreateButton("-Z", 18, 136, 90, 28, this.ButtonToolZMinus_Click);
            this.buttonToolZPlus = CreateButton("+Z", 124, 136, 90, 28, this.ButtonToolZPlus_Click);
            this.buttonMoveHome = CreateButton("Go Home", 232, 43, 120, 23, this.ButtonMoveHome_Click);
            this.buttonMovePreset = CreateButton("Move To Preset", 232, 79, 120, 23, this.ButtonMovePreset_Click);
            this.buttonSetPreset = CreateButton("Set Preset", 232, 115, 120, 23, this.ButtonSetPreset_Click);
            this.groupBoxTool.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.labelToolStep, this.numericUpDownToolStep, this.buttonToolXMinus, this.buttonToolXPlus,
                this.buttonToolYMinus, this.buttonToolYPlus, this.buttonToolZMinus, this.buttonToolZPlus,
                this.buttonMoveHome, this.buttonMovePreset, this.buttonSetPreset
            });

            this.groupBoxJoints = CreateGroupBox("Joint Motion", 777, 155, 391, 207);
            this.labelJointStep = CreateLabel("Step: 10 deg", 18, 28, 120, 16);
            this.buttonJoint1Minus = CreateButton("J1 -", 18, 58, 80, 28, this.ButtonJoint1Minus_Click);
            this.buttonJoint1Plus = CreateButton("J1 +", 106, 58, 80, 28, this.ButtonJoint1Plus_Click);
            this.buttonJoint2Minus = CreateButton("J2 -", 204, 58, 80, 28, this.ButtonJoint2Minus_Click);
            this.buttonJoint2Plus = CreateButton("J2 +", 292, 58, 80, 28, this.ButtonJoint2Plus_Click);
            this.buttonJoint3Minus = CreateButton("J3 -", 18, 94, 80, 28, this.ButtonJoint3Minus_Click);
            this.buttonJoint3Plus = CreateButton("J3 +", 106, 94, 80, 28, this.ButtonJoint3Plus_Click);
            this.buttonJoint4Minus = CreateButton("J4 -", 204, 94, 80, 28, this.ButtonJoint4Minus_Click);
            this.buttonJoint4Plus = CreateButton("J4 +", 292, 94, 80, 28, this.ButtonJoint4Plus_Click);
            this.buttonJoint5Minus = CreateButton("J5 -", 18, 130, 80, 28, this.ButtonJoint5Minus_Click);
            this.buttonJoint5Plus = CreateButton("J5 +", 106, 130, 80, 28, this.ButtonJoint5Plus_Click);
            this.buttonJoint6Minus = CreateButton("J6 -", 204, 130, 80, 28, this.ButtonJoint6Minus_Click);
            this.buttonJoint6Plus = CreateButton("J6 +", 292, 130, 80, 28, this.ButtonJoint6Plus_Click);
            this.groupBoxJoints.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.labelJointStep, this.buttonJoint1Minus, this.buttonJoint1Plus, this.buttonJoint2Minus, this.buttonJoint2Plus,
                this.buttonJoint3Minus, this.buttonJoint3Plus, this.buttonJoint4Minus, this.buttonJoint4Plus,
                this.buttonJoint5Minus, this.buttonJoint5Plus, this.buttonJoint6Minus, this.buttonJoint6Plus
            });

            this.groupBoxSensor = CreateGroupBox("Sensor", 12, 268, 350, 290);
            this.labelSerialPort = CreateLabel("Serial port", 15, 28, 70, 16);
            this.comboBoxSerialPort = CreateComboBox(129, 25, 194, 21, false);
            this.labelBaudRate = CreateLabel("Baud rate", 15, 55, 70, 16);
            this.comboBoxBaudRate = CreateComboBox(129, 52, 194, 21, true);
            this.comboBoxBaudRate.Items.AddRange(new object[] { "9600", "19200", "57600", "115200" });
            this.labelParity = CreateLabel("Parity", 15, 82, 70, 16);
            this.comboBoxParity = CreateComboBox(129, 79, 194, 21, true);
            this.comboBoxParity.Items.AddRange(new object[] { "None", "Even", "Odd" });
            this.labelStopBits = CreateLabel("Stop bits", 15, 109, 70, 16);
            this.comboBoxStopBits = CreateComboBox(129, 106, 194, 21, true);
            this.comboBoxStopBits.Items.AddRange(new object[] { "One", "Two" });
            this.labelDataBits = CreateLabel("Data bits", 15, 136, 70, 16);
            this.comboBoxDataBits = CreateComboBox(129, 133, 194, 21, true);
            this.comboBoxDataBits.Items.AddRange(new object[] { "7", "8" });
            this.buttonRefreshPorts = CreateButton("Refresh Ports", 18, 169, 95, 23, this.ButtonRefreshPorts_Click);
            this.buttonOpenSerial = CreateButton("Open", 129, 169, 95, 23, this.ButtonOpenSerial_Click);
            this.buttonCloseSerial = CreateButton("Close", 240, 169, 83, 23, this.ButtonCloseSerial_Click);
            this.labelSensorCommand = CreateLabel("Command", 15, 205, 70, 16);
            this.textBoxSensorCommand = CreateTextBox(129, 202, 194, 20);
            this.buttonSendCommand = CreateButton("Send", 129, 228, 95, 23, this.ButtonSendCommand_Click);
            this.buttonRequestForce = CreateButton("Read Force", 240, 228, 83, 23, this.ButtonRequestForce_Click);
            this.labelCurrentForce = CreateLabel("Current Force", 15, 259, 85, 16);
            this.textBoxCurrentForce = CreateTextBox(129, 256, 194, 20, true);
            this.groupBoxSensor.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.labelSerialPort, this.comboBoxSerialPort, this.labelBaudRate, this.comboBoxBaudRate,
                this.labelParity, this.comboBoxParity, this.labelStopBits, this.comboBoxStopBits,
                this.labelDataBits, this.comboBoxDataBits, this.buttonRefreshPorts, this.buttonOpenSerial,
                this.buttonCloseSerial, this.labelSensorCommand, this.textBoxSensorCommand,
                this.buttonSendCommand, this.buttonRequestForce, this.labelCurrentForce, this.textBoxCurrentForce
            });

            this.groupBoxAcquisition = CreateGroupBox("Acquisition", 12, 574, 350, 128);
            this.labelCsvPath = CreateLabel("CSV file", 15, 28, 70, 16);
            this.textBoxCsvPath = CreateTextBox(18, 48, 305, 20, true);
            this.labelAcquisitionInfo = CreateLabel("Timer 300 ms, delta Z = TOOL step", 18, 78, 220, 16);
            this.buttonSelectCsv = CreateButton("Select CSV", 18, 95, 95, 23, this.ButtonSelectCsv_Click);
            this.buttonStartAcquisition = CreateButton("Start Test", 129, 95, 95, 23, this.ButtonStartAcquisition_Click);
            this.buttonSaveCsv = CreateButton("Save CSV", 240, 95, 83, 23, this.ButtonSaveCsv_Click);
            this.groupBoxAcquisition.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.labelCsvPath, this.textBoxCsvPath, this.labelAcquisitionInfo,
                this.buttonSelectCsv, this.buttonStartAcquisition, this.buttonSaveCsv
            });

            this.groupBoxLog = CreateGroupBox("Event Log", 378, 368, 790, 334);
            this.groupBoxLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.buttonClearLog = CreateButton("Clear", 691, 19, 82, 23, this.ButtonClearLog_Click);
            this.buttonClearLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.textBoxSensorLog = CreateTextBox(18, 48, 755, 270, true);
            this.textBoxSensorLog.Multiline = true;
            this.textBoxSensorLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSensorLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.groupBoxLog.Controls.AddRange(new System.Windows.Forms.Control[] { this.buttonClearLog, this.textBoxSensorLog });

            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelRobot = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSensor = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.toolStripStatusLabelRobot, this.toolStripStatusLabelSensor });
            this.statusStripMain.Location = new System.Drawing.Point(0, 715);
            this.statusStripMain.Size = new System.Drawing.Size(1180, 22);
            this.toolStripStatusLabelRobot.Text = "Robot: not connected";
            this.toolStripStatusLabelSensor.Text = "Sensor: serial closed";

            this.timerCMD = new System.Windows.Forms.Timer(this.components);
            this.timerCMD.Interval = 300;
            this.timerCMD.Tick += new System.EventHandler(this.timerCMD_Tick);
            this.timerConnection = new System.Windows.Forms.Timer(this.components);
            this.timerConnection.Interval = 1000;
            this.timerConnection.Tick += new System.EventHandler(this.timerConnection_Tick);
            this.serialPortSensor = new System.IO.Ports.SerialPort(this.components);
            this.serialPortSensor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortSensor_DataReceived);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 737);
            this.MinimumSize = new System.Drawing.Size(1196, 776);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Robot + Force Integration";
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.groupBoxRobot, this.groupBoxRobotState, this.groupBoxTool, this.groupBoxJoints,
                this.groupBoxSensor, this.groupBoxAcquisition, this.groupBoxLog, this.statusStripMain
            });
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        }

        private static System.Windows.Forms.GroupBox CreateGroupBox(string text, int x, int y, int w, int h)
        {
            return new System.Windows.Forms.GroupBox
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, h)
            };
        }

        private static System.Windows.Forms.Label CreateLabel(string text, int x, int y, int w, int h)
        {
            return new System.Windows.Forms.Label
            {
                AutoSize = false,
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, h)
            };
        }

        private static System.Windows.Forms.TextBox CreateTextBox(int x, int y, int w, int h, bool readOnly = false)
        {
            return new System.Windows.Forms.TextBox
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, h),
                ReadOnly = readOnly
            };
        }

        private static System.Windows.Forms.Button CreateButton(string text, int x, int y, int w, int h, System.EventHandler handler)
        {
            System.Windows.Forms.Button button = new System.Windows.Forms.Button
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, h),
                UseVisualStyleBackColor = true
            };
            button.Click += handler;
            return button;
        }

        private static System.Windows.Forms.ComboBox CreateComboBox(int x, int y, int w, int h, bool dropDownList)
        {
            return new System.Windows.Forms.ComboBox
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, h),
                DropDownStyle = dropDownList ? System.Windows.Forms.ComboBoxStyle.DropDownList : System.Windows.Forms.ComboBoxStyle.DropDown
            };
        }

        private static System.Windows.Forms.NumericUpDown CreateNumeric(int x, int y, int w, int h, decimal decimals, decimal minimum, decimal maximum, decimal value)
        {
            return new System.Windows.Forms.NumericUpDown
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, h),
                DecimalPlaces = (int)decimals,
                Minimum = minimum,
                Maximum = maximum,
                Value = value
            };
        }

        private static System.Windows.Forms.TrackBar CreateTrackBar(int x, int y, int w, int h, int minimum, int maximum, int value)
        {
            return new System.Windows.Forms.TrackBar
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(w, h),
                Minimum = minimum,
                Maximum = maximum,
                TickFrequency = 10,
                Value = value
            };
        }

        private static System.Windows.Forms.CheckBox CreateCheckBox(string text, int x, int y, bool autoCheck)
        {
            return new System.Windows.Forms.CheckBox
            {
                AutoCheck = autoCheck,
                AutoSize = true,
                Text = text,
                Location = new System.Drawing.Point(x, y)
            };
        }

        private System.Windows.Forms.GroupBox groupBoxRobot;
        private System.Windows.Forms.Label labelRobotIp;
        private System.Windows.Forms.TextBox textBoxRobotIp;
        private System.Windows.Forms.Button buttonConnectRobot;
        private System.Windows.Forms.Button buttonToggleMotion;
        private System.Windows.Forms.Label labelCollisionTarget;
        private System.Windows.Forms.ComboBox comboBoxCollisionSensitivity;
        private System.Windows.Forms.Button buttonApplyCollisionSensitivity;
        private System.Windows.Forms.CheckBox checkBoxSelfCollision;
        private System.Windows.Forms.Label labelMotionSpeed;
        private System.Windows.Forms.TrackBar trackBarMotionSpeed;
        private System.Windows.Forms.Label labelMotionSpeedValue;
        private System.Windows.Forms.GroupBox groupBoxRobotState;
        private System.Windows.Forms.Label labelJoint;
        private System.Windows.Forms.TextBox textBoxJoint;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.Label labelBase;
        private System.Windows.Forms.TextBox textBoxBase;
        private System.Windows.Forms.Label labelTcp;
        private System.Windows.Forms.TextBox textBoxTcp;
        private System.Windows.Forms.GroupBox groupBoxTool;
        private System.Windows.Forms.Label labelToolStep;
        private System.Windows.Forms.NumericUpDown numericUpDownToolStep;
        private System.Windows.Forms.Button buttonToolXMinus;
        private System.Windows.Forms.Button buttonToolXPlus;
        private System.Windows.Forms.Button buttonToolYMinus;
        private System.Windows.Forms.Button buttonToolYPlus;
        private System.Windows.Forms.Button buttonToolZMinus;
        private System.Windows.Forms.Button buttonToolZPlus;
        private System.Windows.Forms.Button buttonMoveHome;
        private System.Windows.Forms.Button buttonMovePreset;
        private System.Windows.Forms.Button buttonSetPreset;
        private System.Windows.Forms.GroupBox groupBoxJoints;
        private System.Windows.Forms.Label labelJointStep;
        private System.Windows.Forms.Button buttonJoint1Minus;
        private System.Windows.Forms.Button buttonJoint1Plus;
        private System.Windows.Forms.Button buttonJoint2Minus;
        private System.Windows.Forms.Button buttonJoint2Plus;
        private System.Windows.Forms.Button buttonJoint3Minus;
        private System.Windows.Forms.Button buttonJoint3Plus;
        private System.Windows.Forms.Button buttonJoint4Minus;
        private System.Windows.Forms.Button buttonJoint4Plus;
        private System.Windows.Forms.Button buttonJoint5Minus;
        private System.Windows.Forms.Button buttonJoint5Plus;
        private System.Windows.Forms.Button buttonJoint6Minus;
        private System.Windows.Forms.Button buttonJoint6Plus;
        private System.Windows.Forms.GroupBox groupBoxSensor;
        private System.Windows.Forms.Label labelSerialPort;
        private System.Windows.Forms.ComboBox comboBoxSerialPort;
        private System.Windows.Forms.Label labelBaudRate;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Label labelParity;
        private System.Windows.Forms.ComboBox comboBoxParity;
        private System.Windows.Forms.Label labelStopBits;
        private System.Windows.Forms.ComboBox comboBoxStopBits;
        private System.Windows.Forms.Label labelDataBits;
        private System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.Button buttonRefreshPorts;
        private System.Windows.Forms.Button buttonOpenSerial;
        private System.Windows.Forms.Button buttonCloseSerial;
        private System.Windows.Forms.Label labelSensorCommand;
        private System.Windows.Forms.TextBox textBoxSensorCommand;
        private System.Windows.Forms.Button buttonSendCommand;
        private System.Windows.Forms.Button buttonRequestForce;
        private System.Windows.Forms.Label labelCurrentForce;
        private System.Windows.Forms.TextBox textBoxCurrentForce;
        private System.Windows.Forms.GroupBox groupBoxAcquisition;
        private System.Windows.Forms.Label labelCsvPath;
        private System.Windows.Forms.TextBox textBoxCsvPath;
        private System.Windows.Forms.Label labelAcquisitionInfo;
        private System.Windows.Forms.Button buttonSelectCsv;
        private System.Windows.Forms.Button buttonStartAcquisition;
        private System.Windows.Forms.Button buttonSaveCsv;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.TextBox textBoxSensorLog;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRobot;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSensor;
        private System.Windows.Forms.Timer timerCMD;
        private System.Windows.Forms.Timer timerConnection;
        private System.IO.Ports.SerialPort serialPortSensor;
    }
}
