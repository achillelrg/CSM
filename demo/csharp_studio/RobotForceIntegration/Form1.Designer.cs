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
            this.groupBoxRobot = new System.Windows.Forms.GroupBox();
            this.labelMotionSpeedValue = new System.Windows.Forms.Label();
            this.trackBarMotionSpeed = new System.Windows.Forms.TrackBar();
            this.labelMotionSpeed = new System.Windows.Forms.Label();
            this.checkBoxSelfCollision = new System.Windows.Forms.CheckBox();
            this.comboBoxCollisionSensitivity = new System.Windows.Forms.ComboBox();
            this.labelCollisionTarget = new System.Windows.Forms.Label();
            this.buttonToggleMotion = new System.Windows.Forms.Button();
            this.buttonConnectRobot = new System.Windows.Forms.Button();
            this.textBoxRobotIp = new System.Windows.Forms.TextBox();
            this.labelRobotIp = new System.Windows.Forms.Label();
            this.groupBoxRobotState = new System.Windows.Forms.GroupBox();
            this.textBoxPosition = new System.Windows.Forms.TextBox();
            this.labelPosition = new System.Windows.Forms.Label();
            this.textBoxJoint = new System.Windows.Forms.TextBox();
            this.labelJoint = new System.Windows.Forms.Label();
            this.groupBoxTool = new System.Windows.Forms.GroupBox();
            this.buttonSetPreset = new System.Windows.Forms.Button();
            this.buttonMovePreset = new System.Windows.Forms.Button();
            this.buttonMoveHome = new System.Windows.Forms.Button();
            this.buttonToolZPlus = new System.Windows.Forms.Button();
            this.buttonToolZMinus = new System.Windows.Forms.Button();
            this.buttonToolYPlus = new System.Windows.Forms.Button();
            this.buttonToolYMinus = new System.Windows.Forms.Button();
            this.buttonToolXPlus = new System.Windows.Forms.Button();
            this.buttonToolXMinus = new System.Windows.Forms.Button();
            this.labelToolStepValue = new System.Windows.Forms.Label();
            this.trackBarToolStep = new System.Windows.Forms.TrackBar();
            this.labelToolStep = new System.Windows.Forms.Label();
            this.groupBoxJoints = new System.Windows.Forms.GroupBox();
            this.buttonJoint6Plus = new System.Windows.Forms.Button();
            this.buttonJoint6Minus = new System.Windows.Forms.Button();
            this.buttonJoint5Plus = new System.Windows.Forms.Button();
            this.buttonJoint5Minus = new System.Windows.Forms.Button();
            this.buttonJoint4Plus = new System.Windows.Forms.Button();
            this.buttonJoint4Minus = new System.Windows.Forms.Button();
            this.buttonJoint3Plus = new System.Windows.Forms.Button();
            this.buttonJoint3Minus = new System.Windows.Forms.Button();
            this.buttonJoint2Plus = new System.Windows.Forms.Button();
            this.buttonJoint2Minus = new System.Windows.Forms.Button();
            this.buttonJoint1Plus = new System.Windows.Forms.Button();
            this.buttonJoint1Minus = new System.Windows.Forms.Button();
            this.labelJointStepValue = new System.Windows.Forms.Label();
            this.trackBarJointStep = new System.Windows.Forms.TrackBar();
            this.labelJointStep = new System.Windows.Forms.Label();
            this.groupBoxSensor = new System.Windows.Forms.GroupBox();
            this.buttonTare = new System.Windows.Forms.Button();
            this.buttonSendCommand = new System.Windows.Forms.Button();
            this.textBoxSensorCommand = new System.Windows.Forms.TextBox();
            this.labelSensorCommand = new System.Windows.Forms.Label();
            this.buttonCloseSerial = new System.Windows.Forms.Button();
            this.buttonOpenSerial = new System.Windows.Forms.Button();
            this.buttonSensorConfiguration = new System.Windows.Forms.Button();
            this.comboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.labelDataBits = new System.Windows.Forms.Label();
            this.comboBoxStopBits = new System.Windows.Forms.ComboBox();
            this.labelStopBits = new System.Windows.Forms.Label();
            this.comboBoxParity = new System.Windows.Forms.ComboBox();
            this.labelParity = new System.Windows.Forms.Label();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.labelBaudRate = new System.Windows.Forms.Label();
            this.comboBoxSerialPort = new System.Windows.Forms.ComboBox();
            this.labelSerialPort = new System.Windows.Forms.Label();
            this.groupBoxAcquisition = new System.Windows.Forms.GroupBox();
            this.buttonSaveCsv = new System.Windows.Forms.Button();
            this.buttonStartAcquisition = new System.Windows.Forms.Button();
            this.buttonSelectCsv = new System.Windows.Forms.Button();
            this.textBoxCsvPath = new System.Windows.Forms.TextBox();
            this.labelCsvPath = new System.Windows.Forms.Label();
            this.groupBoxDodge = new System.Windows.Forms.GroupBox();
            this.labelDodgeThresholdValue = new System.Windows.Forms.Label();
            this.trackBarDodgeThreshold = new System.Windows.Forms.TrackBar();
            this.labelDodgeThreshold = new System.Windows.Forms.Label();
            this.checkBoxDodgeEnabled = new System.Windows.Forms.CheckBox();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.buttonClearLog = new System.Windows.Forms.Button();
            this.textBoxSensorLog = new System.Windows.Forms.TextBox();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelRobot = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSensor = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerCMD = new System.Windows.Forms.Timer(this.components);
            this.timerConnection = new System.Windows.Forms.Timer(this.components);
            this.timerDodge = new System.Windows.Forms.Timer(this.components);
            this.serialPortSensor = new System.IO.Ports.SerialPort(this.components);
            this.groupBoxForceControl = new System.Windows.Forms.GroupBox();
            this.checkBoxForceControl = new System.Windows.Forms.CheckBox();
            this.labelTargetForce = new System.Windows.Forms.Label();
            this.numericUpDownTargetForce = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDeadband = new System.Windows.Forms.NumericUpDown();
            this.labelDeadband = new System.Windows.Forms.Label();
            this.labelKp = new System.Windows.Forms.Label();
            this.numericUpDownKp = new System.Windows.Forms.NumericUpDown();
            this.labelKi = new System.Windows.Forms.Label();
            this.numericUpDownKi = new System.Windows.Forms.NumericUpDown();
            this.labelKd = new System.Windows.Forms.Label();
            this.numericUpDownKd = new System.Windows.Forms.NumericUpDown();
            this.labelMaxVelocity = new System.Windows.Forms.Label();
            this.numericUpDownMaxVelocity = new System.Windows.Forms.NumericUpDown();
            this.timerForceControl = new System.Windows.Forms.Timer(this.components);
            this.checkBoxInvertForce = new System.Windows.Forms.CheckBox();
            this.groupBoxForceControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTargetForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDeadband)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxVelocity)).BeginInit();

            

            // 
            // numericUpDownDeadband
            // 
            this.numericUpDownDeadband.DecimalPlaces = 1;
            this.numericUpDownDeadband.Location = new System.Drawing.Point(180, 45);
            this.numericUpDownDeadband.Name = "numericUpDownDeadband";
            this.numericUpDownDeadband.Size = new System.Drawing.Size(70, 24);
            this.numericUpDownDeadband.TabIndex = 22;
            this.numericUpDownDeadband.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownDeadband.ValueChanged += new System.EventHandler(this.numericUpDownDeadband_ValueChanged);
            // 
            // labelDeadband
            // 
            this.labelDeadband.AutoSize = true;
            this.labelDeadband.Location = new System.Drawing.Point(180, 25);
            this.labelDeadband.Name = "labelDeadband";
            this.labelDeadband.Size = new System.Drawing.Size(95, 18);
            this.labelDeadband.TabIndex = 23;
            this.labelDeadband.Text = "Deadband (N):";
            // 
            // groupBoxForceControl
            // 
            this.groupBoxForceControl.Controls.Add(this.checkBoxForceControl);
            this.groupBoxForceControl.Controls.Add(this.numericUpDownDeadband);
            this.groupBoxForceControl.Controls.Add(this.labelDeadband);
            this.groupBoxForceControl.Controls.Add(this.labelTargetForce);
            this.groupBoxForceControl.Controls.Add(this.numericUpDownTargetForce);
            this.groupBoxForceControl.Controls.Add(this.labelKp);
            this.groupBoxForceControl.Controls.Add(this.numericUpDownKp);
            this.groupBoxForceControl.Controls.Add(this.labelKi);
            this.groupBoxForceControl.Controls.Add(this.numericUpDownKi);
            this.groupBoxForceControl.Controls.Add(this.labelKd);
            this.groupBoxForceControl.Controls.Add(this.numericUpDownKd);
            this.groupBoxForceControl.Controls.Add(this.labelMaxVelocity);
            this.groupBoxForceControl.Controls.Add(this.numericUpDownMaxVelocity);
            this.groupBoxForceControl.Controls.Add(this.checkBoxInvertForce);
            
            // 
            // checkBoxInvertForce
            // 
            this.checkBoxInvertForce.AutoSize = true;
            this.checkBoxInvertForce.Location = new System.Drawing.Point(600, 45);
            this.checkBoxInvertForce.Name = "checkBoxInvertForce";
            this.checkBoxInvertForce.Size = new System.Drawing.Size(95, 20);
            this.checkBoxInvertForce.TabIndex = 11;
            this.checkBoxInvertForce.Text = "Invert Dir.";
            this.checkBoxInvertForce.UseVisualStyleBackColor = true;
            this.groupBoxForceControl.Location = new System.Drawing.Point(16, 580);
            this.groupBoxForceControl.Name = "groupBoxForceControl";
            this.groupBoxForceControl.Size = new System.Drawing.Size(758, 100);
            this.groupBoxForceControl.TabIndex = 8;
            this.groupBoxForceControl.TabStop = false;
            this.groupBoxForceControl.Text = "Force Control (PID)";
            // 
            // checkBoxForceControl
            // 
            this.checkBoxForceControl.AutoSize = false;
            this.checkBoxForceControl.Location = new System.Drawing.Point(15, 25);
            this.checkBoxForceControl.Name = "checkBoxForceControl";
            this.checkBoxForceControl.Size = new System.Drawing.Size(70, 40);
            this.checkBoxForceControl.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxForceControl.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxForceControl.TabIndex = 0;
            this.checkBoxForceControl.Text = "Enable PID";
            this.checkBoxForceControl.UseVisualStyleBackColor = true;
            this.checkBoxForceControl.CheckedChanged += new System.EventHandler(this.checkBoxForceControl_CheckedChanged);
            // 
            // labelTargetForce
            // 
            this.labelTargetForce.AutoSize = true;
            this.labelTargetForce.Location = new System.Drawing.Point(90, 25);
            this.labelTargetForce.Name = "labelTargetForce";
            this.labelTargetForce.Size = new System.Drawing.Size(100, 16);
            this.labelTargetForce.TabIndex = 1;
            this.labelTargetForce.Text = "Target Force (N)";
            // 
            // numericUpDownTargetForce
            // 
            this.numericUpDownTargetForce.DecimalPlaces = 2;
            this.numericUpDownTargetForce.Location = new System.Drawing.Point(90, 45);
            this.numericUpDownTargetForce.Minimum = new decimal(new int[] { 100, 0, 0, -2147483648 });
            this.numericUpDownTargetForce.Name = "numericUpDownTargetForce";
            this.numericUpDownTargetForce.Size = new System.Drawing.Size(80, 22);
            this.numericUpDownTargetForce.TabIndex = 2;
            this.numericUpDownTargetForce.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // labelKp
            // 
            this.labelKp.AutoSize = true;
            this.labelKp.Location = new System.Drawing.Point(280, 25);
            this.labelKp.Name = "labelKp";
            this.labelKp.Size = new System.Drawing.Size(23, 16);
            this.labelKp.TabIndex = 3;
            this.labelKp.Text = "Kp";
            // 
            // numericUpDownKp
            // 
            this.numericUpDownKp.DecimalPlaces = 3;
            this.numericUpDownKp.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            this.numericUpDownKp.Location = new System.Drawing.Point(280, 45);
            this.numericUpDownKp.Name = "numericUpDownKp";
            this.numericUpDownKp.Size = new System.Drawing.Size(70, 22);
            this.numericUpDownKp.TabIndex = 4;
            this.numericUpDownKp.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // labelKi
            // 
            this.labelKi.AutoSize = true;
            this.labelKi.Location = new System.Drawing.Point(360, 25);
            this.labelKi.Name = "labelKi";
            this.labelKi.Size = new System.Drawing.Size(18, 16);
            this.labelKi.TabIndex = 5;
            this.labelKi.Text = "Ki";
            // 
            // numericUpDownKi
            // 
            this.numericUpDownKi.DecimalPlaces = 3;
            this.numericUpDownKi.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            this.numericUpDownKi.Location = new System.Drawing.Point(360, 45);
            this.numericUpDownKi.Name = "numericUpDownKi";
            this.numericUpDownKi.Size = new System.Drawing.Size(70, 22);
            this.numericUpDownKi.TabIndex = 6;
            this.numericUpDownKi.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            // 
            // labelKd
            // 
            this.labelKd.AutoSize = true;
            this.labelKd.Location = new System.Drawing.Point(440, 25);
            this.labelKd.Name = "labelKd";
            this.labelKd.Size = new System.Drawing.Size(23, 16);
            this.labelKd.TabIndex = 7;
            this.labelKd.Text = "Kd";
            // 
            // numericUpDownKd
            // 
            this.numericUpDownKd.DecimalPlaces = 3;
            this.numericUpDownKd.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            this.numericUpDownKd.Location = new System.Drawing.Point(440, 45);
            this.numericUpDownKd.Name = "numericUpDownKd";
            this.numericUpDownKd.Size = new System.Drawing.Size(70, 22);
            this.numericUpDownKd.TabIndex = 8;
            this.numericUpDownKd.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // labelMaxVelocity
            // 
            this.labelMaxVelocity.AutoSize = true;
            this.labelMaxVelocity.Location = new System.Drawing.Point(520, 25);
            this.labelMaxVelocity.Name = "labelMaxVelocity";
            this.labelMaxVelocity.Size = new System.Drawing.Size(94, 16);
            this.labelMaxVelocity.TabIndex = 9;
            this.labelMaxVelocity.Text = "Max Vel (mm/s)";
            // 
            // numericUpDownMaxVelocity
            // 
            this.numericUpDownMaxVelocity.Location = new System.Drawing.Point(520, 45);
            this.numericUpDownMaxVelocity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericUpDownMaxVelocity.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            this.numericUpDownMaxVelocity.Name = "numericUpDownMaxVelocity";
            this.numericUpDownMaxVelocity.Size = new System.Drawing.Size(70, 22);
            this.numericUpDownMaxVelocity.TabIndex = 10;
            this.numericUpDownMaxVelocity.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // timerForceControl
            // 
            this.timerForceControl.Interval = 50;
            this.timerForceControl.Tick += new System.EventHandler(this.timerForceControl_Tick);

            this.groupBoxRobot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMotionSpeed)).BeginInit();
            this.groupBoxRobotState.SuspendLayout();
            this.groupBoxTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarToolStep)).BeginInit();
            this.groupBoxJoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJointStep)).BeginInit();
            this.groupBoxSensor.SuspendLayout();
            this.groupBoxAcquisition.SuspendLayout();
            this.groupBoxDodge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDodgeThreshold)).BeginInit();
            this.groupBoxLog.SuspendLayout();
            this.groupBoxChart = new System.Windows.Forms.GroupBox();
            this.chartForceVelocity = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartForceVelocity)).BeginInit();
            this.groupBoxChart.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxRobot
            // 
            this.groupBoxRobot.Controls.Add(this.labelMotionSpeedValue);
            this.groupBoxRobot.Controls.Add(this.trackBarMotionSpeed);
            this.groupBoxRobot.Controls.Add(this.labelMotionSpeed);
            this.groupBoxRobot.Controls.Add(this.checkBoxSelfCollision);
            this.groupBoxRobot.Controls.Add(this.comboBoxCollisionSensitivity);
            this.groupBoxRobot.Controls.Add(this.labelCollisionTarget);
            this.groupBoxRobot.Controls.Add(this.buttonToggleMotion);
            this.groupBoxRobot.Controls.Add(this.buttonConnectRobot);
            this.groupBoxRobot.Controls.Add(this.textBoxRobotIp);
            this.groupBoxRobot.Controls.Add(this.labelRobotIp);
            this.groupBoxRobot.Location = new System.Drawing.Point(16, 15);
            this.groupBoxRobot.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxRobot.Name = "groupBoxRobot";
            this.groupBoxRobot.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxRobot.Size = new System.Drawing.Size(350, 260);
            this.groupBoxRobot.TabIndex = 0;
            this.groupBoxRobot.TabStop = false;
            this.groupBoxRobot.Text = "Robot";
            // 
            // labelMotionSpeedValue
            // 
            this.labelMotionSpeedValue.Location = new System.Drawing.Point(293, 166);
            this.labelMotionSpeedValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMotionSpeedValue.Name = "labelMotionSpeedValue";
            this.labelMotionSpeedValue.Size = new System.Drawing.Size(40, 25);
            this.labelMotionSpeedValue.TabIndex = 10;
            this.labelMotionSpeedValue.Text = "50%";
            // 
            // trackBarMotionSpeed
            // 
            this.trackBarMotionSpeed.Location = new System.Drawing.Point(24, 162);
            this.trackBarMotionSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarMotionSpeed.Maximum = 100;
            this.trackBarMotionSpeed.Minimum = 1;
            this.trackBarMotionSpeed.Name = "trackBarMotionSpeed";
            this.trackBarMotionSpeed.Size = new System.Drawing.Size(275, 56);
            this.trackBarMotionSpeed.TabIndex = 9;
            this.trackBarMotionSpeed.TickFrequency = 10;
            this.trackBarMotionSpeed.Value = 50;
            this.trackBarMotionSpeed.Scroll += new System.EventHandler(this.trackBarMotionSpeed_Scroll);
            // 
            // labelMotionSpeed
            // 
            this.labelMotionSpeed.Location = new System.Drawing.Point(21, 141);
            this.labelMotionSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMotionSpeed.Name = "labelMotionSpeed";
            this.labelMotionSpeed.Size = new System.Drawing.Size(120, 20);
            this.labelMotionSpeed.TabIndex = 8;
            this.labelMotionSpeed.Text = "Motion speed";
            // 
            // checkBoxSelfCollision
            // 
            this.checkBoxSelfCollision.AutoSize = true;
            this.checkBoxSelfCollision.Location = new System.Drawing.Point(23, 224);
            this.checkBoxSelfCollision.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxSelfCollision.Name = "checkBoxSelfCollision";
            this.checkBoxSelfCollision.Size = new System.Drawing.Size(104, 20);
            this.checkBoxSelfCollision.TabIndex = 7;
            this.checkBoxSelfCollision.Text = "Self collision";
            this.checkBoxSelfCollision.UseVisualStyleBackColor = true;
            this.checkBoxSelfCollision.CheckedChanged += new System.EventHandler(this.checkBoxSelfCollision_CheckedChanged);
            // 
            // comboBoxCollisionSensitivity
            // 
            this.comboBoxCollisionSensitivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCollisionSensitivity.FormattingEnabled = true;
            this.comboBoxCollisionSensitivity.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboBoxCollisionSensitivity.Location = new System.Drawing.Point(151, 102);
            this.comboBoxCollisionSensitivity.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxCollisionSensitivity.Name = "comboBoxCollisionSensitivity";
            this.comboBoxCollisionSensitivity.Size = new System.Drawing.Size(181, 24);
            this.comboBoxCollisionSensitivity.TabIndex = 5;
            this.comboBoxCollisionSensitivity.SelectedIndexChanged += new System.EventHandler(this.comboBoxCollisionSensitivity_SelectedIndexChanged);
            // 
            // labelCollisionTarget
            // 
            this.labelCollisionTarget.Location = new System.Drawing.Point(21, 105);
            this.labelCollisionTarget.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCollisionTarget.Name = "labelCollisionTarget";
            this.labelCollisionTarget.Size = new System.Drawing.Size(160, 20);
            this.labelCollisionTarget.TabIndex = 4;
            this.labelCollisionTarget.Text = "Collision sensitivity";
            // 
            // buttonToggleMotion
            // 
            this.buttonToggleMotion.Location = new System.Drawing.Point(182, 59);
            this.buttonToggleMotion.Margin = new System.Windows.Forms.Padding(4);
            this.buttonToggleMotion.Name = "buttonToggleMotion";
            this.buttonToggleMotion.Size = new System.Drawing.Size(150, 28);
            this.buttonToggleMotion.TabIndex = 3;
            this.buttonToggleMotion.Text = "Enable Motion";
            this.buttonToggleMotion.UseVisualStyleBackColor = true;
            this.buttonToggleMotion.Click += new System.EventHandler(this.ButtonToggleMotion_Click);
            // 
            // buttonConnectRobot
            // 
            this.buttonConnectRobot.Location = new System.Drawing.Point(23, 59);
            this.buttonConnectRobot.Margin = new System.Windows.Forms.Padding(4);
            this.buttonConnectRobot.Name = "buttonConnectRobot";
            this.buttonConnectRobot.Size = new System.Drawing.Size(150, 28);
            this.buttonConnectRobot.TabIndex = 2;
            this.buttonConnectRobot.Text = "Connect";
            this.buttonConnectRobot.UseVisualStyleBackColor = true;
            this.buttonConnectRobot.Click += new System.EventHandler(this.ButtonConnectRobot_Click);
            // 
            // textBoxRobotIp
            // 
            this.textBoxRobotIp.Location = new System.Drawing.Point(120, 21);
            this.textBoxRobotIp.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxRobotIp.Name = "textBoxRobotIp";
            this.textBoxRobotIp.Size = new System.Drawing.Size(212, 22);
            this.textBoxRobotIp.TabIndex = 1;
            // 
            // labelRobotIp
            // 
            this.labelRobotIp.Location = new System.Drawing.Point(20, 25);
            this.labelRobotIp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRobotIp.Name = "labelRobotIp";
            this.labelRobotIp.Size = new System.Drawing.Size(93, 20);
            this.labelRobotIp.TabIndex = 0;
            this.labelRobotIp.Text = "Robot IP";
            // 
            // groupBoxRobotState
            // 
            this.groupBoxRobotState.Controls.Add(this.textBoxPosition);
            this.groupBoxRobotState.Controls.Add(this.labelPosition);
            this.groupBoxRobotState.Controls.Add(this.textBoxJoint);
            this.groupBoxRobotState.Controls.Add(this.labelJoint);
            this.groupBoxRobotState.Location = new System.Drawing.Point(782, 15);
            this.groupBoxRobotState.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxRobotState.Name = "groupBoxRobotState";
            this.groupBoxRobotState.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxRobotState.Size = new System.Drawing.Size(450, 95);
            this.groupBoxRobotState.TabIndex = 1;
            this.groupBoxRobotState.TabStop = false;
            this.groupBoxRobotState.Text = "Robot State";
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.Location = new System.Drawing.Point(101, 59);
            this.textBoxPosition.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.ReadOnly = true;
            this.textBoxPosition.Size = new System.Drawing.Size(325, 22);
            this.textBoxPosition.TabIndex = 3;
            // 
            // labelPosition
            // 
            this.labelPosition.Location = new System.Drawing.Point(24, 63);
            this.labelPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(93, 20);
            this.labelPosition.TabIndex = 2;
            this.labelPosition.Text = "Position";
            // 
            // textBoxJoint
            // 
            this.textBoxJoint.Location = new System.Drawing.Point(101, 27);
            this.textBoxJoint.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxJoint.Name = "textBoxJoint";
            this.textBoxJoint.ReadOnly = true;
            this.textBoxJoint.Size = new System.Drawing.Size(325, 22);
            this.textBoxJoint.TabIndex = 1;
            // 
            // labelJoint
            // 
            this.labelJoint.Location = new System.Drawing.Point(24, 31);
            this.labelJoint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelJoint.Name = "labelJoint";
            this.labelJoint.Size = new System.Drawing.Size(93, 20);
            this.labelJoint.TabIndex = 0;
            this.labelJoint.Text = "Joint";
            // 
            // groupBoxTool
            // 
            this.groupBoxTool.Controls.Add(this.buttonSetPreset);
            this.groupBoxTool.Controls.Add(this.buttonMovePreset);
            this.groupBoxTool.Controls.Add(this.buttonMoveHome);
            this.groupBoxTool.Controls.Add(this.buttonToolZPlus);
            this.groupBoxTool.Controls.Add(this.buttonToolZMinus);
            this.groupBoxTool.Controls.Add(this.buttonToolYPlus);
            this.groupBoxTool.Controls.Add(this.buttonToolYMinus);
            this.groupBoxTool.Controls.Add(this.buttonToolXPlus);
            this.groupBoxTool.Controls.Add(this.buttonToolXMinus);
            this.groupBoxTool.Controls.Add(this.labelToolStepValue);
            this.groupBoxTool.Controls.Add(this.trackBarToolStep);
            this.groupBoxTool.Controls.Add(this.labelToolStep);
            this.groupBoxTool.Location = new System.Drawing.Point(374, 15);
            this.groupBoxTool.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxTool.Name = "groupBoxTool";
            this.groupBoxTool.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxTool.Size = new System.Drawing.Size(400, 218);
            this.groupBoxTool.TabIndex = 2;
            this.groupBoxTool.TabStop = false;
            this.groupBoxTool.Text = "Tool Motion";
            // 
            // buttonSetPreset
            // 
            this.buttonSetPreset.Location = new System.Drawing.Point(258, 160);
            this.buttonSetPreset.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSetPreset.Name = "buttonSetPreset";
            this.buttonSetPreset.Size = new System.Drawing.Size(120, 28);
            this.buttonSetPreset.TabIndex = 10;
            this.buttonSetPreset.Text = "Set Preset";
            this.buttonSetPreset.UseVisualStyleBackColor = true;
            this.buttonSetPreset.Click += new System.EventHandler(this.ButtonSetPreset_Click);
            // 
            // buttonMovePreset
            // 
            this.buttonMovePreset.Location = new System.Drawing.Point(258, 119);
            this.buttonMovePreset.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMovePreset.Name = "buttonMovePreset";
            this.buttonMovePreset.Size = new System.Drawing.Size(120, 28);
            this.buttonMovePreset.TabIndex = 9;
            this.buttonMovePreset.Text = "Move To Preset";
            this.buttonMovePreset.UseVisualStyleBackColor = true;
            this.buttonMovePreset.Click += new System.EventHandler(this.ButtonMovePreset_Click);
            // 
            // buttonMoveHome
            // 
            this.buttonMoveHome.Location = new System.Drawing.Point(258, 78);
            this.buttonMoveHome.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMoveHome.Name = "buttonMoveHome";
            this.buttonMoveHome.Size = new System.Drawing.Size(120, 28);
            this.buttonMoveHome.TabIndex = 8;
            this.buttonMoveHome.Text = "Go Home";
            this.buttonMoveHome.UseVisualStyleBackColor = true;
            this.buttonMoveHome.Click += new System.EventHandler(this.ButtonMoveHome_Click);
            // 
            // buttonToolZPlus
            // 
            this.buttonToolZPlus.Location = new System.Drawing.Point(112, 157);
            this.buttonToolZPlus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonToolZPlus.Name = "buttonToolZPlus";
            this.buttonToolZPlus.Size = new System.Drawing.Size(80, 34);
            this.buttonToolZPlus.TabIndex = 7;
            this.buttonToolZPlus.Text = "+Z";
            this.buttonToolZPlus.UseVisualStyleBackColor = true;
            this.buttonToolZPlus.Click += new System.EventHandler(this.ButtonToolZPlus_Click);
            // 
            // buttonToolZMinus
            // 
            this.buttonToolZMinus.Location = new System.Drawing.Point(24, 157);
            this.buttonToolZMinus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonToolZMinus.Name = "buttonToolZMinus";
            this.buttonToolZMinus.Size = new System.Drawing.Size(80, 34);
            this.buttonToolZMinus.TabIndex = 6;
            this.buttonToolZMinus.Text = "-Z";
            this.buttonToolZMinus.UseVisualStyleBackColor = true;
            this.buttonToolZMinus.Click += new System.EventHandler(this.ButtonToolZMinus_Click);
            // 
            // buttonToolYPlus
            // 
            this.buttonToolYPlus.Location = new System.Drawing.Point(112, 116);
            this.buttonToolYPlus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonToolYPlus.Name = "buttonToolYPlus";
            this.buttonToolYPlus.Size = new System.Drawing.Size(80, 34);
            this.buttonToolYPlus.TabIndex = 5;
            this.buttonToolYPlus.Text = "+Y";
            this.buttonToolYPlus.UseVisualStyleBackColor = true;
            this.buttonToolYPlus.Click += new System.EventHandler(this.ButtonToolYPlus_Click);
            // 
            // buttonToolYMinus
            // 
            this.buttonToolYMinus.Location = new System.Drawing.Point(24, 116);
            this.buttonToolYMinus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonToolYMinus.Name = "buttonToolYMinus";
            this.buttonToolYMinus.Size = new System.Drawing.Size(80, 34);
            this.buttonToolYMinus.TabIndex = 4;
            this.buttonToolYMinus.Text = "-Y";
            this.buttonToolYMinus.UseVisualStyleBackColor = true;
            this.buttonToolYMinus.Click += new System.EventHandler(this.ButtonToolYMinus_Click);
            // 
            // buttonToolXPlus
            // 
            this.buttonToolXPlus.Location = new System.Drawing.Point(112, 75);
            this.buttonToolXPlus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonToolXPlus.Name = "buttonToolXPlus";
            this.buttonToolXPlus.Size = new System.Drawing.Size(80, 34);
            this.buttonToolXPlus.TabIndex = 3;
            this.buttonToolXPlus.Text = "+X";
            this.buttonToolXPlus.UseVisualStyleBackColor = true;
            this.buttonToolXPlus.Click += new System.EventHandler(this.ButtonToolXPlus_Click);
            // 
            // buttonToolXMinus
            // 
            this.buttonToolXMinus.Location = new System.Drawing.Point(24, 75);
            this.buttonToolXMinus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonToolXMinus.Name = "buttonToolXMinus";
            this.buttonToolXMinus.Size = new System.Drawing.Size(80, 34);
            this.buttonToolXMinus.TabIndex = 2;
            this.buttonToolXMinus.Text = "-X";
            this.buttonToolXMinus.UseVisualStyleBackColor = true;
            this.buttonToolXMinus.Click += new System.EventHandler(this.ButtonToolXMinus_Click);
            // 
            // labelToolStepValue
            // 
            this.labelToolStepValue.Location = new System.Drawing.Point(322, 38);
            this.labelToolStepValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelToolStepValue.Name = "labelToolStepValue";
            this.labelToolStepValue.Size = new System.Drawing.Size(70, 20);
            this.labelToolStepValue.TabIndex = 2;
            this.labelToolStepValue.Text = "10.0 mm";
            // 
            // trackBarToolStep
            // 
            this.trackBarToolStep.Location = new System.Drawing.Point(112, 27);
            this.trackBarToolStep.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarToolStep.Maximum = 100;
            this.trackBarToolStep.Name = "trackBarToolStep";
            this.trackBarToolStep.Size = new System.Drawing.Size(202, 56);
            this.trackBarToolStep.TabIndex = 1;
            this.trackBarToolStep.TickFrequency = 10;
            this.trackBarToolStep.Value = 67;
            this.trackBarToolStep.Scroll += new System.EventHandler(this.trackBarToolStep_Scroll);
            // 
            // labelToolStep
            // 
            this.labelToolStep.Location = new System.Drawing.Point(20, 38);
            this.labelToolStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelToolStep.Name = "labelToolStep";
            this.labelToolStep.Size = new System.Drawing.Size(84, 20);
            this.labelToolStep.TabIndex = 0;
            this.labelToolStep.Text = "Step TOOL";
            // 
            // groupBoxJoints
            // 
            this.groupBoxJoints.Controls.Add(this.buttonJoint6Plus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint6Minus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint5Plus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint5Minus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint4Plus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint4Minus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint3Plus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint3Minus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint2Plus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint2Minus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint1Plus);
            this.groupBoxJoints.Controls.Add(this.buttonJoint1Minus);
            this.groupBoxJoints.Controls.Add(this.labelJointStepValue);
            this.groupBoxJoints.Controls.Add(this.trackBarJointStep);
            this.groupBoxJoints.Controls.Add(this.labelJointStep);
            this.groupBoxJoints.Location = new System.Drawing.Point(374, 239);
            this.groupBoxJoints.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxJoints.Name = "groupBoxJoints";
            this.groupBoxJoints.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxJoints.Size = new System.Drawing.Size(400, 195);
            this.groupBoxJoints.TabIndex = 3;
            this.groupBoxJoints.TabStop = false;
            this.groupBoxJoints.Text = "Joint Motion";
            // 
            // buttonJoint6Plus
            // 
            this.buttonJoint6Plus.Location = new System.Drawing.Point(298, 142);
            this.buttonJoint6Plus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint6Plus.Name = "buttonJoint6Plus";
            this.buttonJoint6Plus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint6Plus.TabIndex = 12;
            this.buttonJoint6Plus.Text = "J6 +";
            this.buttonJoint6Plus.UseVisualStyleBackColor = true;
            this.buttonJoint6Plus.Click += new System.EventHandler(this.ButtonJoint6Plus_Click);
            // 
            // buttonJoint6Minus
            // 
            this.buttonJoint6Minus.Location = new System.Drawing.Point(210, 142);
            this.buttonJoint6Minus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint6Minus.Name = "buttonJoint6Minus";
            this.buttonJoint6Minus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint6Minus.TabIndex = 11;
            this.buttonJoint6Minus.Text = "J6 -";
            this.buttonJoint6Minus.UseVisualStyleBackColor = true;
            this.buttonJoint6Minus.Click += new System.EventHandler(this.ButtonJoint6Minus_Click);
            // 
            // buttonJoint5Plus
            // 
            this.buttonJoint5Plus.Location = new System.Drawing.Point(104, 142);
            this.buttonJoint5Plus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint5Plus.Name = "buttonJoint5Plus";
            this.buttonJoint5Plus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint5Plus.TabIndex = 10;
            this.buttonJoint5Plus.Text = "J5 +";
            this.buttonJoint5Plus.UseVisualStyleBackColor = true;
            this.buttonJoint5Plus.Click += new System.EventHandler(this.ButtonJoint5Plus_Click);
            // 
            // buttonJoint5Minus
            // 
            this.buttonJoint5Minus.Location = new System.Drawing.Point(16, 142);
            this.buttonJoint5Minus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint5Minus.Name = "buttonJoint5Minus";
            this.buttonJoint5Minus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint5Minus.TabIndex = 9;
            this.buttonJoint5Minus.Text = "J5 -";
            this.buttonJoint5Minus.UseVisualStyleBackColor = true;
            this.buttonJoint5Minus.Click += new System.EventHandler(this.ButtonJoint5Minus_Click);
            // 
            // buttonJoint4Plus
            // 
            this.buttonJoint4Plus.Location = new System.Drawing.Point(298, 107);
            this.buttonJoint4Plus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint4Plus.Name = "buttonJoint4Plus";
            this.buttonJoint4Plus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint4Plus.TabIndex = 8;
            this.buttonJoint4Plus.Text = "J4 +";
            this.buttonJoint4Plus.UseVisualStyleBackColor = true;
            this.buttonJoint4Plus.Click += new System.EventHandler(this.ButtonJoint4Plus_Click);
            // 
            // buttonJoint4Minus
            // 
            this.buttonJoint4Minus.Location = new System.Drawing.Point(210, 107);
            this.buttonJoint4Minus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint4Minus.Name = "buttonJoint4Minus";
            this.buttonJoint4Minus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint4Minus.TabIndex = 7;
            this.buttonJoint4Minus.Text = "J4 -";
            this.buttonJoint4Minus.UseVisualStyleBackColor = true;
            this.buttonJoint4Minus.Click += new System.EventHandler(this.ButtonJoint4Minus_Click);
            // 
            // buttonJoint3Plus
            // 
            this.buttonJoint3Plus.Location = new System.Drawing.Point(104, 107);
            this.buttonJoint3Plus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint3Plus.Name = "buttonJoint3Plus";
            this.buttonJoint3Plus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint3Plus.TabIndex = 6;
            this.buttonJoint3Plus.Text = "J3 +";
            this.buttonJoint3Plus.UseVisualStyleBackColor = true;
            this.buttonJoint3Plus.Click += new System.EventHandler(this.ButtonJoint3Plus_Click);
            // 
            // buttonJoint3Minus
            // 
            this.buttonJoint3Minus.Location = new System.Drawing.Point(16, 107);
            this.buttonJoint3Minus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint3Minus.Name = "buttonJoint3Minus";
            this.buttonJoint3Minus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint3Minus.TabIndex = 5;
            this.buttonJoint3Minus.Text = "J3 -";
            this.buttonJoint3Minus.UseVisualStyleBackColor = true;
            this.buttonJoint3Minus.Click += new System.EventHandler(this.ButtonJoint3Minus_Click);
            // 
            // buttonJoint2Plus
            // 
            this.buttonJoint2Plus.Location = new System.Drawing.Point(298, 72);
            this.buttonJoint2Plus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint2Plus.Name = "buttonJoint2Plus";
            this.buttonJoint2Plus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint2Plus.TabIndex = 4;
            this.buttonJoint2Plus.Text = "J2 +";
            this.buttonJoint2Plus.UseVisualStyleBackColor = true;
            this.buttonJoint2Plus.Click += new System.EventHandler(this.ButtonJoint2Plus_Click);
            // 
            // buttonJoint2Minus
            // 
            this.buttonJoint2Minus.Location = new System.Drawing.Point(210, 72);
            this.buttonJoint2Minus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint2Minus.Name = "buttonJoint2Minus";
            this.buttonJoint2Minus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint2Minus.TabIndex = 3;
            this.buttonJoint2Minus.Text = "J2 -";
            this.buttonJoint2Minus.UseVisualStyleBackColor = true;
            this.buttonJoint2Minus.Click += new System.EventHandler(this.ButtonJoint2Minus_Click);
            // 
            // buttonJoint1Plus
            // 
            this.buttonJoint1Plus.Location = new System.Drawing.Point(104, 72);
            this.buttonJoint1Plus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint1Plus.Name = "buttonJoint1Plus";
            this.buttonJoint1Plus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint1Plus.TabIndex = 2;
            this.buttonJoint1Plus.Text = "J1 +";
            this.buttonJoint1Plus.UseVisualStyleBackColor = true;
            this.buttonJoint1Plus.Click += new System.EventHandler(this.ButtonJoint1Plus_Click);
            // 
            // buttonJoint1Minus
            // 
            this.buttonJoint1Minus.Location = new System.Drawing.Point(16, 72);
            this.buttonJoint1Minus.Margin = new System.Windows.Forms.Padding(4);
            this.buttonJoint1Minus.Name = "buttonJoint1Minus";
            this.buttonJoint1Minus.Size = new System.Drawing.Size(80, 34);
            this.buttonJoint1Minus.TabIndex = 1;
            this.buttonJoint1Minus.Text = "J1 -";
            this.buttonJoint1Minus.UseVisualStyleBackColor = true;
            this.buttonJoint1Minus.Click += new System.EventHandler(this.ButtonJoint1Minus_Click);
            // 
            // labelJointStepValue
            // 
            this.labelJointStepValue.Location = new System.Drawing.Point(322, 34);
            this.labelJointStepValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelJointStepValue.Name = "labelJointStepValue";
            this.labelJointStepValue.Size = new System.Drawing.Size(70, 20);
            this.labelJointStepValue.TabIndex = 14;
            this.labelJointStepValue.Text = "10.0 deg";
            // 
            // trackBarJointStep
            // 
            this.trackBarJointStep.Location = new System.Drawing.Point(104, 23);
            this.trackBarJointStep.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarJointStep.Maximum = 20;
            this.trackBarJointStep.Minimum = 1;
            this.trackBarJointStep.Name = "trackBarJointStep";
            this.trackBarJointStep.Size = new System.Drawing.Size(210, 56);
            this.trackBarJointStep.TabIndex = 13;
            this.trackBarJointStep.Value = 10;
            this.trackBarJointStep.Scroll += new System.EventHandler(this.trackBarJointStep_Scroll);
            // 
            // labelJointStep
            // 
            this.labelJointStep.Location = new System.Drawing.Point(24, 34);
            this.labelJointStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelJointStep.Name = "labelJointStep";
            this.labelJointStep.Size = new System.Drawing.Size(72, 20);
            this.labelJointStep.TabIndex = 0;
            this.labelJointStep.Text = "Joint step";
            // 
            // groupBoxSensor
            // 
            this.groupBoxSensor.Controls.Add(this.buttonTare);
            this.groupBoxSensor.Controls.Add(this.buttonSendCommand);
            this.groupBoxSensor.Controls.Add(this.textBoxSensorCommand);
            this.groupBoxSensor.Controls.Add(this.labelSensorCommand);
            this.groupBoxSensor.Controls.Add(this.buttonCloseSerial);
            this.groupBoxSensor.Controls.Add(this.buttonOpenSerial);
            this.groupBoxSensor.Controls.Add(this.buttonSensorConfiguration);
            this.groupBoxSensor.Location = new System.Drawing.Point(16, 281);
            this.groupBoxSensor.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxSensor.Name = "groupBoxSensor";
            this.groupBoxSensor.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxSensor.Size = new System.Drawing.Size(350, 151);
            this.groupBoxSensor.TabIndex = 4;
            this.groupBoxSensor.TabStop = false;
            this.groupBoxSensor.Text = "Sensor";
            // 
            // buttonTare
            // 
            this.buttonTare.Location = new System.Drawing.Point(182, 101);
            this.buttonTare.Margin = new System.Windows.Forms.Padding(4);
            this.buttonTare.Name = "buttonTare";
            this.buttonTare.Size = new System.Drawing.Size(150, 28);
            this.buttonTare.TabIndex = 15;
            this.buttonTare.Text = "Tare";
            this.buttonTare.UseVisualStyleBackColor = true;
            this.buttonTare.Click += new System.EventHandler(this.ButtonTare_Click);
            // 
            // buttonSendCommand
            // 
            this.buttonSendCommand.Location = new System.Drawing.Point(23, 101);
            this.buttonSendCommand.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSendCommand.Name = "buttonSendCommand";
            this.buttonSendCommand.Size = new System.Drawing.Size(150, 28);
            this.buttonSendCommand.TabIndex = 14;
            this.buttonSendCommand.Text = "Send";
            this.buttonSendCommand.UseVisualStyleBackColor = true;
            this.buttonSendCommand.Click += new System.EventHandler(this.ButtonSendCommand_Click);
            // 
            // textBoxSensorCommand
            // 
            this.textBoxSensorCommand.Location = new System.Drawing.Point(121, 69);
            this.textBoxSensorCommand.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSensorCommand.Name = "textBoxSensorCommand";
            this.textBoxSensorCommand.Size = new System.Drawing.Size(211, 22);
            this.textBoxSensorCommand.TabIndex = 14;
            // 
            // labelSensorCommand
            // 
            this.labelSensorCommand.Location = new System.Drawing.Point(20, 72);
            this.labelSensorCommand.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSensorCommand.Name = "labelSensorCommand";
            this.labelSensorCommand.Size = new System.Drawing.Size(93, 20);
            this.labelSensorCommand.TabIndex = 13;
            this.labelSensorCommand.Text = "Command";
            // 
            // buttonCloseSerial
            // 
            this.buttonCloseSerial.Location = new System.Drawing.Point(257, 28);
            this.buttonCloseSerial.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCloseSerial.Name = "buttonCloseSerial";
            this.buttonCloseSerial.Size = new System.Drawing.Size(75, 28);
            this.buttonCloseSerial.TabIndex = 12;
            this.buttonCloseSerial.Text = "Close";
            this.buttonCloseSerial.UseVisualStyleBackColor = true;
            this.buttonCloseSerial.Click += new System.EventHandler(this.ButtonCloseSerial_Click);
            // 
            // buttonOpenSerial
            // 
            this.buttonOpenSerial.Location = new System.Drawing.Point(174, 28);
            this.buttonOpenSerial.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenSerial.Name = "buttonOpenSerial";
            this.buttonOpenSerial.Size = new System.Drawing.Size(75, 28);
            this.buttonOpenSerial.TabIndex = 11;
            this.buttonOpenSerial.Text = "Open";
            this.buttonOpenSerial.UseVisualStyleBackColor = true;
            this.buttonOpenSerial.Click += new System.EventHandler(this.ButtonOpenSerial_Click);
            // 
            // buttonSensorConfiguration
            // 
            this.buttonSensorConfiguration.Location = new System.Drawing.Point(15, 28);
            this.buttonSensorConfiguration.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSensorConfiguration.Name = "buttonSensorConfiguration";
            this.buttonSensorConfiguration.Size = new System.Drawing.Size(151, 28);
            this.buttonSensorConfiguration.TabIndex = 10;
            this.buttonSensorConfiguration.Text = "Configuration";
            this.buttonSensorConfiguration.UseVisualStyleBackColor = true;
            this.buttonSensorConfiguration.Click += new System.EventHandler(this.ButtonSensorConfiguration_Click);
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Items.AddRange(new object[] {
            "7",
            "8"});
            this.comboBoxDataBits.Location = new System.Drawing.Point(120, 167);
            this.comboBoxDataBits.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(212, 24);
            this.comboBoxDataBits.TabIndex = 9;
            // 
            // labelDataBits
            // 
            this.labelDataBits.Location = new System.Drawing.Point(20, 167);
            this.labelDataBits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDataBits.Name = "labelDataBits";
            this.labelDataBits.Size = new System.Drawing.Size(93, 20);
            this.labelDataBits.TabIndex = 8;
            this.labelDataBits.Text = "Data bits";
            // 
            // comboBoxStopBits
            // 
            this.comboBoxStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStopBits.FormattingEnabled = true;
            this.comboBoxStopBits.Items.AddRange(new object[] {
            "One",
            "Two"});
            this.comboBoxStopBits.Location = new System.Drawing.Point(120, 131);
            this.comboBoxStopBits.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxStopBits.Name = "comboBoxStopBits";
            this.comboBoxStopBits.Size = new System.Drawing.Size(212, 24);
            this.comboBoxStopBits.TabIndex = 7;
            // 
            // labelStopBits
            // 
            this.labelStopBits.Location = new System.Drawing.Point(20, 134);
            this.labelStopBits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStopBits.Name = "labelStopBits";
            this.labelStopBits.Size = new System.Drawing.Size(93, 20);
            this.labelStopBits.TabIndex = 6;
            this.labelStopBits.Text = "Stop bits";
            // 
            // comboBoxParity
            // 
            this.comboBoxParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParity.FormattingEnabled = true;
            this.comboBoxParity.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd"});
            this.comboBoxParity.Location = new System.Drawing.Point(120, 99);
            this.comboBoxParity.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxParity.Name = "comboBoxParity";
            this.comboBoxParity.Size = new System.Drawing.Size(212, 24);
            this.comboBoxParity.TabIndex = 5;
            // 
            // labelParity
            // 
            this.labelParity.Location = new System.Drawing.Point(20, 101);
            this.labelParity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelParity.Name = "labelParity";
            this.labelParity.Size = new System.Drawing.Size(93, 20);
            this.labelParity.TabIndex = 4;
            this.labelParity.Text = "Parity";
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "57600",
            "115200"});
            this.comboBoxBaudRate.Location = new System.Drawing.Point(121, 64);
            this.comboBoxBaudRate.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(211, 24);
            this.comboBoxBaudRate.TabIndex = 3;
            // 
            // labelBaudRate
            // 
            this.labelBaudRate.Location = new System.Drawing.Point(20, 68);
            this.labelBaudRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBaudRate.Name = "labelBaudRate";
            this.labelBaudRate.Size = new System.Drawing.Size(93, 20);
            this.labelBaudRate.TabIndex = 2;
            this.labelBaudRate.Text = "Baud rate";
            // 
            // comboBoxSerialPort
            // 
            this.comboBoxSerialPort.FormattingEnabled = true;
            this.comboBoxSerialPort.Location = new System.Drawing.Point(120, 30);
            this.comboBoxSerialPort.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxSerialPort.Name = "comboBoxSerialPort";
            this.comboBoxSerialPort.Size = new System.Drawing.Size(212, 24);
            this.comboBoxSerialPort.TabIndex = 1;
            // 
            // labelSerialPort
            // 
            this.labelSerialPort.Location = new System.Drawing.Point(20, 34);
            this.labelSerialPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSerialPort.Name = "labelSerialPort";
            this.labelSerialPort.Size = new System.Drawing.Size(93, 20);
            this.labelSerialPort.TabIndex = 0;
            this.labelSerialPort.Text = "Serial port";
            // 
            // groupBoxAcquisition
            // 
            this.groupBoxAcquisition.Controls.Add(this.buttonSaveCsv);
            this.groupBoxAcquisition.Controls.Add(this.buttonStartAcquisition);
            this.groupBoxAcquisition.Controls.Add(this.buttonSelectCsv);
            this.groupBoxAcquisition.Controls.Add(this.textBoxCsvPath);
            this.groupBoxAcquisition.Controls.Add(this.labelCsvPath);
            this.groupBoxAcquisition.Location = new System.Drawing.Point(16, 436);
            this.groupBoxAcquisition.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxAcquisition.Name = "groupBoxAcquisition";
            this.groupBoxAcquisition.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxAcquisition.Size = new System.Drawing.Size(350, 138);
            this.groupBoxAcquisition.TabIndex = 5;
            this.groupBoxAcquisition.TabStop = false;
            this.groupBoxAcquisition.Text = "Acquisition";
            // 
            // buttonSaveCsv
            // 
            this.buttonSaveCsv.Location = new System.Drawing.Point(183, 61);
            this.buttonSaveCsv.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSaveCsv.Name = "buttonSaveCsv";
            this.buttonSaveCsv.Size = new System.Drawing.Size(150, 28);
            this.buttonSaveCsv.TabIndex = 5;
            this.buttonSaveCsv.Text = "Save CSV";
            this.buttonSaveCsv.UseVisualStyleBackColor = true;
            this.buttonSaveCsv.Click += new System.EventHandler(this.ButtonSaveCsv_Click);
            // 
            // buttonStartAcquisition
            // 
            this.buttonStartAcquisition.Location = new System.Drawing.Point(23, 97);
            this.buttonStartAcquisition.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStartAcquisition.Name = "buttonStartAcquisition";
            this.buttonStartAcquisition.Size = new System.Drawing.Size(309, 28);
            this.buttonStartAcquisition.TabIndex = 4;
            this.buttonStartAcquisition.Text = "Start Test";
            this.buttonStartAcquisition.UseVisualStyleBackColor = true;
            this.buttonStartAcquisition.Click += new System.EventHandler(this.ButtonStartAcquisition_Click);
            // 
            // buttonSelectCsv
            // 
            this.buttonSelectCsv.Location = new System.Drawing.Point(23, 61);
            this.buttonSelectCsv.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectCsv.Name = "buttonSelectCsv";
            this.buttonSelectCsv.Size = new System.Drawing.Size(150, 28);
            this.buttonSelectCsv.TabIndex = 3;
            this.buttonSelectCsv.Text = "Select CSV";
            this.buttonSelectCsv.UseVisualStyleBackColor = true;
            this.buttonSelectCsv.Click += new System.EventHandler(this.ButtonSelectCsv_Click);
            // 
            // textBoxCsvPath
            // 
            this.textBoxCsvPath.Location = new System.Drawing.Point(94, 31);
            this.textBoxCsvPath.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCsvPath.Name = "textBoxCsvPath";
            this.textBoxCsvPath.ReadOnly = true;
            this.textBoxCsvPath.Size = new System.Drawing.Size(238, 22);
            this.textBoxCsvPath.TabIndex = 1;
            // 
            // labelCsvPath
            // 
            this.labelCsvPath.Location = new System.Drawing.Point(20, 34);
            this.labelCsvPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCsvPath.Name = "labelCsvPath";
            this.labelCsvPath.Size = new System.Drawing.Size(93, 20);
            this.labelCsvPath.TabIndex = 0;
            this.labelCsvPath.Text = "CSV file";
            // 
            // groupBoxDodge
            // 
            this.groupBoxDodge.Controls.Add(this.labelDodgeThresholdValue);
            this.groupBoxDodge.Controls.Add(this.trackBarDodgeThreshold);
            this.groupBoxDodge.Controls.Add(this.labelDodgeThreshold);
            this.groupBoxDodge.Controls.Add(this.checkBoxDodgeEnabled);
            this.groupBoxDodge.Location = new System.Drawing.Point(374, 438);
            this.groupBoxDodge.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxDodge.Name = "groupBoxDodge";
            this.groupBoxDodge.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxDodge.Size = new System.Drawing.Size(400, 136);
            this.groupBoxDodge.TabIndex = 5;
            this.groupBoxDodge.TabStop = false;
            this.groupBoxDodge.Text = "Dodge";
            // 
            // labelDodgeThresholdValue
            // 
            this.labelDodgeThresholdValue.Location = new System.Drawing.Point(283, 69);
            this.labelDodgeThresholdValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDodgeThresholdValue.Name = "labelDodgeThresholdValue";
            this.labelDodgeThresholdValue.Size = new System.Drawing.Size(70, 20);
            this.labelDodgeThresholdValue.TabIndex = 3;
            this.labelDodgeThresholdValue.Text = "0.100 kg";
            // 
            // trackBarDodgeThreshold
            // 
            this.trackBarDodgeThreshold.Location = new System.Drawing.Point(99, 69);
            this.trackBarDodgeThreshold.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarDodgeThreshold.Maximum = 100;
            this.trackBarDodgeThreshold.Name = "trackBarDodgeThreshold";
            this.trackBarDodgeThreshold.Size = new System.Drawing.Size(184, 56);
            this.trackBarDodgeThreshold.TabIndex = 2;
            this.trackBarDodgeThreshold.TickFrequency = 10;
            this.trackBarDodgeThreshold.Scroll += new System.EventHandler(this.trackBarDodgeThreshold_Scroll);
            // 
            // labelDodgeThreshold
            // 
            this.labelDodgeThreshold.Location = new System.Drawing.Point(20, 82);
            this.labelDodgeThreshold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDodgeThreshold.Name = "labelDodgeThreshold";
            this.labelDodgeThreshold.Size = new System.Drawing.Size(71, 20);
            this.labelDodgeThreshold.TabIndex = 1;
            this.labelDodgeThreshold.Text = "Sensibility";
            // 
            // checkBoxDodgeEnabled
            // 
            this.checkBoxDodgeEnabled.AutoSize = true;
            this.checkBoxDodgeEnabled.Location = new System.Drawing.Point(24, 32);
            this.checkBoxDodgeEnabled.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxDodgeEnabled.Name = "checkBoxDodgeEnabled";
            this.checkBoxDodgeEnabled.Size = new System.Drawing.Size(91, 20);
            this.checkBoxDodgeEnabled.TabIndex = 0;
            this.checkBoxDodgeEnabled.Text = "Dodge On";
            this.checkBoxDodgeEnabled.UseVisualStyleBackColor = true;
            this.checkBoxDodgeEnabled.CheckedChanged += new System.EventHandler(this.checkBoxDodgeEnabled_CheckedChanged);
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Controls.Add(this.buttonClearLog);
            this.groupBoxLog.Controls.Add(this.textBoxSensorLog);
            this.groupBoxLog.Location = new System.Drawing.Point(782, 117);
            this.groupBoxLog.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxLog.Size = new System.Drawing.Size(450, 200);
            this.groupBoxLog.TabIndex = 6;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "Event Log";
            // 
            // buttonClearLog
            // 
            this.buttonClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearLog.Location = new System.Drawing.Point(322, 23);
            this.buttonClearLog.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClearLog.Name = "buttonClearLog";
            this.buttonClearLog.Size = new System.Drawing.Size(109, 28);
            this.buttonClearLog.TabIndex = 1;
            this.buttonClearLog.Text = "Clear";
            this.buttonClearLog.UseVisualStyleBackColor = true;
            this.buttonClearLog.Click += new System.EventHandler(this.ButtonClearLog_Click);
            // 
            // textBoxSensorLog
            // 
            this.textBoxSensorLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSensorLog.Location = new System.Drawing.Point(15, 61);
            this.textBoxSensorLog.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSensorLog.Multiline = true;
            this.textBoxSensorLog.Name = "textBoxSensorLog";
            this.textBoxSensorLog.ReadOnly = true;
            this.textBoxSensorLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSensorLog.Size = new System.Drawing.Size(430, 150);
            this.textBoxSensorLog.TabIndex = 0;
            // 
            // statusStripMain
            // 
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelRobot,
            this.toolStripStatusLabelSensor});
            this.statusStripMain.Location = new System.Drawing.Point(0, 704);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStripMain.Size = new System.Drawing.Size(1242, 26);
            this.statusStripMain.TabIndex = 7;
            // 
            // toolStripStatusLabelRobot
            // 
            this.toolStripStatusLabelRobot.Name = "toolStripStatusLabelRobot";
            this.toolStripStatusLabelRobot.Size = new System.Drawing.Size(152, 20);
            this.toolStripStatusLabelRobot.Text = "Robot: not connected";
            // 
            // toolStripStatusLabelSensor
            // 
            this.toolStripStatusLabelSensor.Name = "toolStripStatusLabelSensor";
            this.toolStripStatusLabelSensor.Size = new System.Drawing.Size(142, 20);
            this.toolStripStatusLabelSensor.Text = "Sensor: serial closed";
            // 
            // timerCMD
            // 
            this.timerCMD.Interval = 300;
            this.timerCMD.Tick += new System.EventHandler(this.timerCMD_Tick);
            // 
            // timerConnection
            // 
            this.timerConnection.Interval = 1000;
            this.timerConnection.Tick += new System.EventHandler(this.timerConnection_Tick);
            // 
            // timerDodge
            // 
            this.timerDodge.Interval = 350;
            this.timerDodge.Tick += new System.EventHandler(this.timerDodge_Tick);
            // 
            // serialPortSensor
            // 
            this.serialPortSensor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortSensor_DataReceived);

            // 
            // groupBoxChart
            // 
            this.groupBoxChart.Controls.Add(this.chartForceVelocity);
            this.groupBoxChart.Location = new System.Drawing.Point(782, 323);
            this.groupBoxChart.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxChart.Name = "groupBoxChart";
            this.groupBoxChart.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxChart.Size = new System.Drawing.Size(450, 480);
            this.groupBoxChart.TabIndex = 12;
            this.groupBoxChart.TabStop = false;
            this.groupBoxChart.Text = "Live Data (Force / Velocity)";
            // 
            // chartForceVelocity
            // 
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            chartArea1.Name = "ChartArea1";
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisY.Title = "Force (N)";
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.AxisY2.Title = "Velocity (mm/s)";
            
            chartArea1.AxisY.Minimum = -100;
            chartArea1.AxisY.Maximum = 100;
            chartArea1.AxisY2.Minimum = -100;
            chartArea1.AxisY2.Maximum = 100;

            this.chartForceVelocity.ChartAreas.Add(chartArea1);
            this.chartForceVelocity.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            this.chartForceVelocity.Legends.Add(legend1);
            this.chartForceVelocity.Location = new System.Drawing.Point(4, 19);
            this.chartForceVelocity.Name = "chartForceVelocity";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "Force (N)";
            series1.BorderWidth = 2;
            series1.Color = System.Drawing.Color.Red;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "Velocity Z (%)";
            series2.BorderWidth = 2;
            series2.Color = System.Drawing.Color.Blue;
            series2.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this.chartForceVelocity.Series.Add(series1);
            this.chartForceVelocity.Series.Add(series2);
            this.chartForceVelocity.Size = new System.Drawing.Size(442, 455);
            this.chartForceVelocity.TabIndex = 0;
            this.chartForceVelocity.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 730);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.groupBoxForceControl);

            this.Controls.Add(this.groupBoxDodge);
            this.Controls.Add(this.groupBoxChart);
            this.Controls.Add(this.groupBoxAcquisition);
            this.Controls.Add(this.groupBoxSensor);
            this.Controls.Add(this.groupBoxJoints);
            this.Controls.Add(this.groupBoxTool);
            this.Controls.Add(this.groupBoxRobotState);
            this.Controls.Add(this.groupBoxRobot);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1260, 775);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Robot + Force Integration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBoxRobot.ResumeLayout(false);
            this.groupBoxRobot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMotionSpeed)).EndInit();
            this.groupBoxRobotState.ResumeLayout(false);
            this.groupBoxRobotState.PerformLayout();
            this.groupBoxTool.ResumeLayout(false);
            this.groupBoxTool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarToolStep)).EndInit();
            this.groupBoxJoints.ResumeLayout(false);
            this.groupBoxJoints.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarJointStep)).EndInit();
            this.groupBoxSensor.ResumeLayout(false);
            this.groupBoxSensor.PerformLayout();
            this.groupBoxAcquisition.ResumeLayout(false);
            this.groupBoxAcquisition.PerformLayout();
            this.groupBoxDodge.ResumeLayout(false);
            this.groupBoxDodge.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDodgeThreshold)).EndInit();
            this.groupBoxLog.ResumeLayout(false);
            this.groupBoxLog.PerformLayout();
            this.groupBoxChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartForceVelocity)).EndInit();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            
            this.groupBoxForceControl.ResumeLayout(false);
            this.groupBoxForceControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTargetForce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDeadband)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownKd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxVelocity)).EndInit();

            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.GroupBox groupBoxRobot;
        private System.Windows.Forms.Label labelMotionSpeedValue;
        private System.Windows.Forms.TrackBar trackBarMotionSpeed;
        private System.Windows.Forms.Label labelMotionSpeed;
        private System.Windows.Forms.CheckBox checkBoxSelfCollision;
        private System.Windows.Forms.ComboBox comboBoxCollisionSensitivity;
        private System.Windows.Forms.Label labelCollisionTarget;
        private System.Windows.Forms.Button buttonToggleMotion;
        private System.Windows.Forms.Button buttonConnectRobot;
        private System.Windows.Forms.TextBox textBoxRobotIp;
        private System.Windows.Forms.Label labelRobotIp;
        private System.Windows.Forms.GroupBox groupBoxRobotState;
        private System.Windows.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.TextBox textBoxJoint;
        private System.Windows.Forms.Label labelJoint;
        private System.Windows.Forms.GroupBox groupBoxTool;
        private System.Windows.Forms.Button buttonSetPreset;
        private System.Windows.Forms.Button buttonMovePreset;
        private System.Windows.Forms.Button buttonMoveHome;
        private System.Windows.Forms.Button buttonToolZPlus;
        private System.Windows.Forms.Button buttonToolZMinus;
        private System.Windows.Forms.Button buttonToolYPlus;
        private System.Windows.Forms.Button buttonToolYMinus;
        private System.Windows.Forms.Button buttonToolXPlus;
        private System.Windows.Forms.Button buttonToolXMinus;
        private System.Windows.Forms.Label labelToolStepValue;
        private System.Windows.Forms.TrackBar trackBarToolStep;
        private System.Windows.Forms.Label labelToolStep;
        private System.Windows.Forms.GroupBox groupBoxJoints;
        private System.Windows.Forms.Label labelJointStepValue;
        private System.Windows.Forms.TrackBar trackBarJointStep;
        private System.Windows.Forms.Button buttonJoint6Plus;
        private System.Windows.Forms.Button buttonJoint6Minus;
        private System.Windows.Forms.Button buttonJoint5Plus;
        private System.Windows.Forms.Button buttonJoint5Minus;
        private System.Windows.Forms.Button buttonJoint4Plus;
        private System.Windows.Forms.Button buttonJoint4Minus;
        private System.Windows.Forms.Button buttonJoint3Plus;
        private System.Windows.Forms.Button buttonJoint3Minus;
        private System.Windows.Forms.Button buttonJoint2Plus;
        private System.Windows.Forms.Button buttonJoint2Minus;
        private System.Windows.Forms.Button buttonJoint1Plus;
        private System.Windows.Forms.Button buttonJoint1Minus;
        private System.Windows.Forms.Label labelJointStep;
        private System.Windows.Forms.GroupBox groupBoxSensor;
        private System.Windows.Forms.Button buttonTare;
        private System.Windows.Forms.Button buttonSendCommand;
        private System.Windows.Forms.TextBox textBoxSensorCommand;
        private System.Windows.Forms.Label labelSensorCommand;
        private System.Windows.Forms.Button buttonCloseSerial;
        private System.Windows.Forms.Button buttonOpenSerial;
        private System.Windows.Forms.Button buttonSensorConfiguration;
        private System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.Label labelDataBits;
        private System.Windows.Forms.ComboBox comboBoxStopBits;
        private System.Windows.Forms.Label labelStopBits;
        private System.Windows.Forms.ComboBox comboBoxParity;
        private System.Windows.Forms.Label labelParity;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Label labelBaudRate;
        private System.Windows.Forms.ComboBox comboBoxSerialPort;
        private System.Windows.Forms.Label labelSerialPort;
        private System.Windows.Forms.GroupBox groupBoxAcquisition;
        private System.Windows.Forms.Button buttonSaveCsv;
        private System.Windows.Forms.Button buttonStartAcquisition;
        private System.Windows.Forms.Button buttonSelectCsv;
        private System.Windows.Forms.TextBox textBoxCsvPath;
        private System.Windows.Forms.Label labelCsvPath;
        private System.Windows.Forms.GroupBox groupBoxDodge;
        private System.Windows.Forms.Label labelDodgeThresholdValue;
        private System.Windows.Forms.TrackBar trackBarDodgeThreshold;
        private System.Windows.Forms.Label labelDodgeThreshold;
        private System.Windows.Forms.CheckBox checkBoxDodgeEnabled;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.Button buttonClearLog;
        private System.Windows.Forms.TextBox textBoxSensorLog;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRobot;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSensor;
        private System.Windows.Forms.Timer timerCMD;
        private System.Windows.Forms.Timer timerConnection;
        private System.Windows.Forms.Timer timerDodge;
        private System.IO.Ports.SerialPort serialPortSensor;
        private System.Windows.Forms.GroupBox groupBoxForceControl;
        private System.Windows.Forms.CheckBox checkBoxForceControl;
        private System.Windows.Forms.NumericUpDown numericUpDownDeadband;
        private System.Windows.Forms.Label labelDeadband;
        private System.Windows.Forms.Label labelTargetForce;
        private System.Windows.Forms.NumericUpDown numericUpDownTargetForce;
        private System.Windows.Forms.Label labelKp;
        private System.Windows.Forms.NumericUpDown numericUpDownKp;
        private System.Windows.Forms.Label labelKi;
        private System.Windows.Forms.NumericUpDown numericUpDownKi;
        private System.Windows.Forms.Label labelKd;
        private System.Windows.Forms.NumericUpDown numericUpDownKd;
        private System.Windows.Forms.Label labelMaxVelocity;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxVelocity;
        private System.Windows.Forms.Timer timerForceControl;
        private System.Windows.Forms.CheckBox checkBoxInvertForce;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartForceVelocity;
        private System.Windows.Forms.GroupBox groupBoxChart;
    }
}
