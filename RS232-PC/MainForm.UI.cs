using System.Drawing;
using System.Windows.Forms;

namespace RS232_PC
{
    public partial class MainForm
    {
        private void BuildUi()
        {
            SuspendLayout();

            var statusStrip = new StatusStrip();
            toolStripSerialStatus = new ToolStripStatusLabel("Serial: closed");
            toolStripRobotStatus = new ToolStripStatusLabel("Robot: disconnected");
            toolStripRunStatus = new ToolStripStatusLabel("Run: idle");
            statusStrip.Items.Add(toolStripSerialStatus);
            statusStrip.Items.Add(new ToolStripStatusLabel { Spring = true });
            statusStrip.Items.Add(toolStripRobotStatus);
            statusStrip.Items.Add(new ToolStripStatusLabel { Spring = true });
            statusStrip.Items.Add(toolStripRunStatus);

            tabControlMain = new TabControl
            {
                Dock = DockStyle.Fill
            };
            tabControlMain.TabPages.Add(BuildConnectionsTab());
            tabControlMain.TabPages.Add(BuildSensorTab());
            tabControlMain.TabPages.Add(BuildRobotTab());
            tabControlMain.TabPages.Add(BuildLegacyRobotTab());
            tabControlMain.TabPages.Add(BuildExperimentsTab());

            Controls.Add(tabControlMain);
            Controls.Add(statusStrip);

            ResumeLayout(true);
        }

        private TabPage BuildConnectionsTab()
        {
            var tabPage = new TabPage("Connexions");
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 230
            };

            var topLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2
            };
            topLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            topLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            topLayout.Controls.Add(BuildSerialConnectionGroup(), 0, 0);
            topLayout.Controls.Add(BuildRobotConnectionGroup(), 1, 0);
            split.Panel1.Controls.Add(topLayout);

            var logGroup = new GroupBox
            {
                Text = "System log",
                Dock = DockStyle.Fill
            };
            textBoxSystemLog = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            logGroup.Controls.Add(textBoxSystemLog);
            split.Panel2.Controls.Add(logGroup);

            tabPage.Controls.Add(split);
            return tabPage;
        }

        private GroupBox BuildSerialConnectionGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Capteur serie",
                Dock = DockStyle.Fill
            };

            var layout = CreateSettingsTable(5);
            comboSerialPorts = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDown
            };
            AddSettingRow(layout, 0, "Port COM", comboSerialPorts);
            AddSettingRow(layout, 1, "Protocol", new Label
            {
                Text = "115200 baud / 8N1",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });

            labelSerialState = new Label
            {
                Text = "Closed",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            AddSettingRow(layout, 2, "State", labelSerialState);

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            buttonRefreshPorts = new Button { Text = "Refresh ports", AutoSize = true };
            buttonRefreshPorts.Click += ButtonRefreshPorts_Click;
            buttonOpenSerial = new Button { Text = "Open", AutoSize = true };
            buttonOpenSerial.Click += ButtonOpenSerial_Click;
            buttonCloseSerial = new Button { Text = "Close", AutoSize = true };
            buttonCloseSerial.Click += ButtonCloseSerial_Click;
            buttons.Controls.Add(buttonRefreshPorts);
            buttons.Controls.Add(buttonOpenSerial);
            buttons.Controls.Add(buttonCloseSerial);
            AddSettingRow(layout, 3, "Actions", buttons);

            AddSettingRow(layout, 4, "Notes", new Label
            {
                Text = "Use the emulator first, then the real ESP32 sensor.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });

            groupBox.Controls.Add(layout);
            return groupBox;
        }

        private GroupBox BuildRobotConnectionGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Robot xARM",
                Dock = DockStyle.Fill
            };

            var layout = CreateSettingsTable(6);
            textBoxRobotIp = new TextBox
            {
                Dock = DockStyle.Fill,
                Text = "192.168.1.214"
            };
            AddSettingRow(layout, 0, "Robot IP", textBoxRobotIp);

            labelRobotState = new Label
            {
                Text = "Disconnected",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            AddSettingRow(layout, 1, "State", labelRobotState);

            labelRobotVersion = new Label
            {
                Text = "-",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            AddSettingRow(layout, 2, "Version", labelRobotVersion);

            var connectionButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            buttonConnectRobot = new Button { Text = "Connect", AutoSize = true };
            buttonConnectRobot.Click += ButtonConnectRobot_Click;
            buttonDisconnectRobot = new Button { Text = "Disconnect", AutoSize = true };
            buttonDisconnectRobot.Click += ButtonDisconnectRobot_Click;
            connectionButtons.Controls.Add(buttonConnectRobot);
            connectionButtons.Controls.Add(buttonDisconnectRobot);
            AddSettingRow(layout, 3, "Connection", connectionButtons);

            var motionButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            buttonEnableMotion = new Button { Text = "Enable motion", AutoSize = true };
            buttonEnableMotion.Click += ButtonEnableMotion_Click;
            buttonDisableMotion = new Button { Text = "Disable motion", AutoSize = true };
            buttonDisableMotion.Click += ButtonDisableMotion_Click;
            motionButtons.Controls.Add(buttonEnableMotion);
            motionButtons.Controls.Add(buttonDisableMotion);
            AddSettingRow(layout, 4, "Motion", motionButtons);

            AddSettingRow(layout, 5, "Notes", new Label
            {
                Text = "Only one user can control the robot at a time.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });

            groupBox.Controls.Add(layout);
            return groupBox;
        }

        private TabPage BuildSensorTab()
        {
            var tabPage = new TabPage("Capteur");
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 520
            };

            var leftPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2
            };
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            leftPanel.Controls.Add(BuildSensorStatusGroup(), 0, 0);
            leftPanel.Controls.Add(BuildSensorCommandsGroup(), 0, 1);
            split.Panel1.Controls.Add(leftPanel);

            var rawGroup = new GroupBox
            {
                Text = "Raw serial log",
                Dock = DockStyle.Fill
            };
            textBoxSensorLog = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            rawGroup.Controls.Add(textBoxSensorLog);
            split.Panel2.Controls.Add(rawGroup);

            tabPage.Controls.Add(split);
            return tabPage;
        }

        private GroupBox BuildSensorStatusGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Last measurement",
                Dock = DockStyle.Fill
            };

            var layout = CreateSettingsTable(3);
            labelLastForceValue = new Label
            {
                Text = "0.000",
                Dock = DockStyle.Fill,
                Font = new Font(Font.FontFamily, 18F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            AddSettingRow(layout, 0, "Force", labelLastForceValue);

            labelLastUnitValue = new Label
            {
                Text = _activeUnit,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            AddSettingRow(layout, 1, "Unit", labelLastUnitValue);

            textBoxLastRaw = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true
            };
            AddSettingRow(layout, 2, "Raw line", textBoxLastRaw);

            groupBox.Controls.Add(layout);
            return groupBox;
        }

        private GroupBox BuildSensorCommandsGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Sensor commands",
                Dock = DockStyle.Fill
            };

            var outerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3
            };
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var standardButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            buttonMeasure = new Button { Text = "Measure (M)", AutoSize = true };
            buttonMeasure.Click += async (sender, e) => await RequestSingleMeasurementAsync();
            buttonCalibrationStart = new Button { Text = "Calibration (C)", AutoSize = true };
            buttonCalibrationStart.Click += (sender, e) => SendSensorCommand("C");
            buttonCalibrationStop = new Button { Text = "Quit calibration (Q)", AutoSize = true };
            buttonCalibrationStop.Click += (sender, e) => SendSensorCommand("Q");
            buttonTare = new Button { Text = "Tare (T)", AutoSize = true };
            buttonTare.Click += (sender, e) => SendSensorCommand("T");
            buttonUnitToggle = new Button { Text = "Change unit (U)", AutoSize = true };
            buttonUnitToggle.Click += (sender, e) => SendSensorCommand("U");
            standardButtons.Controls.Add(buttonMeasure);
            standardButtons.Controls.Add(buttonCalibrationStart);
            standardButtons.Controls.Add(buttonCalibrationStop);
            standardButtons.Controls.Add(buttonTare);
            standardButtons.Controls.Add(buttonUnitToggle);
            outerLayout.Controls.Add(standardButtons, 0, 0);

            var customCommandPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            textBoxCustomSensorCommand = new TextBox
            {
                Width = 220
            };
            buttonSendCustomSensorCommand = new Button { Text = "Send custom command", AutoSize = true };
            buttonSendCustomSensorCommand.Click += ButtonSendCustomSensorCommand_Click;
            customCommandPanel.Controls.Add(textBoxCustomSensorCommand);
            customCommandPanel.Controls.Add(buttonSendCustomSensorCommand);
            outerLayout.Controls.Add(customCommandPanel, 0, 1);

            outerLayout.Controls.Add(new Label
            {
                Text = "Use M for normal acquisition. Boot messages and other text are logged but ignored for force parsing.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 2);

            groupBox.Controls.Add(outerLayout);
            return groupBox;
        }

        private TabPage BuildRobotTab()
        {
            var tabPage = new TabPage("Robot");
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 650
            };

            split.Panel1.Controls.Add(BuildRobotStateGroup());
            split.Panel2.Controls.Add(BuildRobotJogGroup());
            tabPage.Controls.Add(split);
            return tabPage;
        }

        private TabPage BuildLegacyRobotTab()
        {
            var tabPage = new TabPage("xARM Base");
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 430
            };

            split.Panel1.Controls.Add(BuildLegacyRobotCommandGroup());
            split.Panel2.Controls.Add(BuildLegacyRobotStateGroup());
            tabPage.Controls.Add(split);
            return tabPage;
        }

        private GroupBox BuildLegacyRobotCommandGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Legacy xARM interface",
                Dock = DockStyle.Fill
            };

            var outerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 5,
                ColumnCount = 1,
                Padding = new Padding(8)
            };
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var ipPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };
            ipPanel.Controls.Add(new Label
            {
                Text = "IP address",
                AutoSize = true,
                Margin = new Padding(0, 8, 12, 0)
            });
            textBoxLegacyRobotIp = new TextBox
            {
                Width = 180,
                Text = "192.168.1.214"
            };
            ipPanel.Controls.Add(textBoxLegacyRobotIp);
            outerLayout.Controls.Add(ipPanel, 0, 0);

            var createPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };
            buttonLegacyCreateArm = new Button { Text = "Create xARM", AutoSize = true };
            buttonLegacyCreateArm.Click += ButtonLegacyCreateArm_Click;
            buttonLegacyMotionArm = new Button { Text = "Motion xARM", AutoSize = true };
            buttonLegacyMotionArm.Click += ButtonLegacyMotionArm_Click;
            buttonLegacyResetArm = new Button { Text = "Reset xARM", AutoSize = true };
            buttonLegacyResetArm.Click += ButtonLegacyResetArm_Click;
            createPanel.Controls.Add(buttonLegacyCreateArm);
            createPanel.Controls.Add(buttonLegacyMotionArm);
            createPanel.Controls.Add(buttonLegacyResetArm);
            outerLayout.Controls.Add(createPanel, 0, 1);

            var safetyPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };
            buttonLegacyGetCollisionSensitivity = new Button { Text = "Get Collision Sensitivity", AutoSize = true };
            buttonLegacyGetCollisionSensitivity.Click += ButtonLegacyGetCollisionSensitivity_Click;
            textBoxLegacyCollisionSensitivity = new TextBox
            {
                Width = 60,
                ReadOnly = true
            };
            buttonLegacySetCollisionSensitivity = new Button { Text = "Set Collision Sensitivity", AutoSize = true };
            buttonLegacySetCollisionSensitivity.Click += ButtonLegacySetCollisionSensitivity_Click;
            comboBoxLegacyCollisionSensitivity = new ComboBox
            {
                Width = 60,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBoxLegacyCollisionSensitivity.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5" });
            comboBoxLegacyCollisionSensitivity.SelectedIndex = 3;
            safetyPanel.Controls.Add(buttonLegacyGetCollisionSensitivity);
            safetyPanel.Controls.Add(textBoxLegacyCollisionSensitivity);
            safetyPanel.Controls.Add(buttonLegacySetCollisionSensitivity);
            safetyPanel.Controls.Add(comboBoxLegacyCollisionSensitivity);
            outerLayout.Controls.Add(safetyPanel, 0, 2);

            var selfCollisionPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };
            buttonLegacySelfCollision = new Button { Text = "Self Collision Detection", AutoSize = true };
            buttonLegacySelfCollision.Click += ButtonLegacySelfCollision_Click;
            checkBoxLegacySelfCollision = new CheckBox
            {
                Text = "Self Collision",
                AutoSize = true,
                Enabled = false,
                Margin = new Padding(12, 6, 0, 0)
            };
            buttonLegacyTimer = new Button { Text = "Start Timer", AutoSize = true };
            buttonLegacyTimer.Click += ButtonLegacyTimer_Click;
            selfCollisionPanel.Controls.Add(buttonLegacySelfCollision);
            selfCollisionPanel.Controls.Add(checkBoxLegacySelfCollision);
            selfCollisionPanel.Controls.Add(buttonLegacyTimer);
            outerLayout.Controls.Add(selfCollisionPanel, 0, 3);

            var movePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            buttonLegacyMoveHome = new Button { Text = "Move Home", AutoSize = true };
            buttonLegacyMoveHome.Click += ButtonLegacyMoveHome_Click;
            buttonLegacyMoveBase = new Button { Text = "Move Base", AutoSize = true };
            buttonLegacyMoveBase.Click += ButtonLegacyMoveBase_Click;
            buttonLegacyMoveTcp = new Button { Text = "Move TCP", AutoSize = true };
            buttonLegacyMoveTcp.Click += ButtonLegacyMoveTcp_Click;
            buttonLegacyMoveAngle = new Button { Text = "Move Angle", AutoSize = true };
            buttonLegacyMoveAngle.Click += ButtonLegacyMoveAngle_Click;
            movePanel.Controls.Add(buttonLegacyMoveHome);
            movePanel.Controls.Add(buttonLegacyMoveBase);
            movePanel.Controls.Add(buttonLegacyMoveTcp);
            movePanel.Controls.Add(buttonLegacyMoveAngle);
            outerLayout.Controls.Add(movePanel, 0, 4);

            groupBox.Controls.Add(outerLayout);
            return groupBox;
        }

        private GroupBox BuildLegacyRobotStateGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Legacy robot state",
                Dock = DockStyle.Fill
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(8)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            buttonLegacyGetJoint = new Button { Text = "Get Joint", AutoSize = true };
            buttonLegacyGetJoint.Click += ButtonLegacyGetJoint_Click;
            textBoxLegacyJoint = new TextBox { ReadOnly = true, Dock = DockStyle.Fill };
            layout.Controls.Add(buttonLegacyGetJoint, 0, 0);
            layout.Controls.Add(textBoxLegacyJoint, 1, 0);

            buttonLegacyGetPosition = new Button { Text = "Get Position", AutoSize = true };
            buttonLegacyGetPosition.Click += ButtonLegacyGetPosition_Click;
            textBoxLegacyPosition = new TextBox { ReadOnly = true, Dock = DockStyle.Fill };
            layout.Controls.Add(buttonLegacyGetPosition, 0, 1);
            layout.Controls.Add(textBoxLegacyPosition, 1, 1);

            buttonLegacyGetBase = new Button { Text = "Get Base", AutoSize = true };
            buttonLegacyGetBase.Click += ButtonLegacyGetBase_Click;
            textBoxLegacyBase = new TextBox { ReadOnly = true, Dock = DockStyle.Fill };
            layout.Controls.Add(buttonLegacyGetBase, 0, 2);
            layout.Controls.Add(textBoxLegacyBase, 1, 2);

            buttonLegacyGetTcp = new Button { Text = "Get TCP", AutoSize = true };
            buttonLegacyGetTcp.Click += ButtonLegacyGetTcp_Click;
            textBoxLegacyTcp = new TextBox { ReadOnly = true, Dock = DockStyle.Fill };
            layout.Controls.Add(buttonLegacyGetTcp, 0, 3);
            layout.Controls.Add(textBoxLegacyTcp, 1, 3);

            groupBox.Controls.Add(layout);
            return groupBox;
        }

        private GroupBox BuildRobotStateGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Robot state",
                Dock = DockStyle.Fill
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

            buttonRefreshRobotState = new Button
            {
                Text = "Refresh state",
                Dock = DockStyle.Left,
                AutoSize = true
            };
            buttonRefreshRobotState.Click += ButtonRefreshRobotState_Click;
            layout.Controls.Add(buttonRefreshRobotState, 0, 0);

            textBoxRobotPose = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            layout.Controls.Add(CreateGroupSection("Current pose (X Y Z Rx Ry Rz)", textBoxRobotPose), 0, 1);

            textBoxRobotJoints = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            layout.Controls.Add(CreateGroupSection("Current joints (6 axes)", textBoxRobotJoints), 0, 2);

            var commandButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };
            buttonMoveHome = new Button { Text = "Move home", AutoSize = true };
            buttonMoveHome.Click += ButtonMoveHome_Click;
            buttonResetRobot = new Button { Text = "Reset", AutoSize = true };
            buttonResetRobot.Click += ButtonResetRobot_Click;
            buttonEmergencyStop = new Button { Text = "Emergency stop", AutoSize = true, BackColor = Color.LightCoral };
            buttonEmergencyStop.Click += ButtonEmergencyStop_Click;
            commandButtons.Controls.Add(buttonMoveHome);
            commandButtons.Controls.Add(buttonResetRobot);
            commandButtons.Controls.Add(buttonEmergencyStop);
            layout.Controls.Add(commandButtons, 0, 3);

            groupBox.Controls.Add(layout);
            return groupBox;
        }

        private GroupBox BuildRobotJogGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "TOOL jog",
                Dock = DockStyle.Fill
            };

            var outerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            outerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var stepPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };
            stepPanel.Controls.Add(new Label
            {
                Text = "Jog step (mm)",
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Margin = new Padding(0, 8, 12, 0)
            });
            numericJogStep = new NumericUpDown
            {
                DecimalPlaces = 1,
                Minimum = 0.1M,
                Maximum = 50.0M,
                Increment = 0.5M,
                Value = 10.0M,
                Width = 100
            };
            stepPanel.Controls.Add(numericJogStep);
            outerLayout.Controls.Add(stepPanel, 0, 0);

            var jogTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3
            };
            jogTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            jogTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            jogTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            jogTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            jogTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            jogTable.Controls.Add(CreateAxisButtons("X"), 0, 0);
            jogTable.Controls.Add(CreateAxisButtons("Y"), 1, 0);
            jogTable.Controls.Add(CreateAxisButtons("Z"), 0, 1);
            jogTable.SetColumnSpan(jogTable.GetControlFromPosition(0, 1), 2);
            outerLayout.Controls.Add(jogTable, 0, 1);

            outerLayout.Controls.Add(new Label
            {
                Text = "The jog commands use MoveTool relative moves in the TOOL frame.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 2);

            groupBox.Controls.Add(outerLayout);
            return groupBox;
        }

        private Control CreateAxisButtons(string axis)
        {
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            var negativeButton = new Button
            {
                Text = "-" + axis,
                AutoSize = true
            };
            negativeButton.Click += (sender, e) => JogTool(axis, -GetJogStep());
            var positiveButton = new Button
            {
                Text = "+" + axis,
                AutoSize = true
            };
            positiveButton.Click += (sender, e) => JogTool(axis, GetJogStep());

            panel.Controls.Add(negativeButton);
            panel.Controls.Add(positiveButton);
            return panel;
        }

        private TabPage BuildExperimentsTab()
        {
            var tabPage = new TabPage("Essais");
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 290
            };

            var configLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3
            };
            configLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            configLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            configLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            configLayout.Controls.Add(BuildMechanicalTestGroup(), 0, 0);
            configLayout.Controls.Add(BuildForceControlGroup(), 1, 0);
            configLayout.Controls.Add(BuildSessionGroup(), 2, 0);
            split.Panel1.Controls.Add(configLayout);

            dataGridViewSamples = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                DataSource = _session.Samples,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            };
            split.Panel2.Controls.Add(dataGridViewSamples);

            tabPage.Controls.Add(split);
            return tabPage;
        }

        private GroupBox BuildMechanicalTestGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Mechanical test",
                Dock = DockStyle.Fill
            };

            var layout = CreateSettingsTable(6);
            numericTimerInterval = new NumericUpDown
            {
                Minimum = 100,
                Maximum = 2000,
                Increment = 50,
                Value = 300,
                Width = 120
            };
            AddSettingRow(layout, 0, "Timer (ms)", numericTimerInterval);

            numericMechanicalStepZ = new NumericUpDown
            {
                DecimalPlaces = 2,
                Minimum = -10.0M,
                Maximum = 10.0M,
                Increment = 0.05M,
                Value = 0.20M,
                Width = 120
            };
            AddSettingRow(layout, 1, "Delta Z / tick", numericMechanicalStepZ);

            numericForceThreshold = new NumericUpDown
            {
                DecimalPlaces = 2,
                Minimum = 0.1M,
                Maximum = 500.0M,
                Increment = 1.0M,
                Value = 50.0M,
                Width = 120
            };
            AddSettingRow(layout, 2, "Force threshold", numericForceThreshold);

            numericZWindow = new NumericUpDown
            {
                DecimalPlaces = 2,
                Minimum = 0.10M,
                Maximum = 200.0M,
                Increment = 0.50M,
                Value = 10.0M,
                Width = 120
            };
            AddSettingRow(layout, 3, "Z safety window", numericZWindow);

            buttonStartMechanical = new Button
            {
                Text = "Start mechanical test",
                AutoSize = true
            };
            buttonStartMechanical.Click += ButtonStartMechanical_Click;
            AddSettingRow(layout, 4, "Run", buttonStartMechanical);

            AddSettingRow(layout, 5, "Notes", new Label
            {
                Text = "Acquires M, logs force and current Z, then applies the fixed TOOL delta on Z.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });

            groupBox.Controls.Add(layout);
            return groupBox;
        }

        private GroupBox BuildForceControlGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Force control (PID on Z)",
                Dock = DockStyle.Fill
            };

            var layout = CreateSettingsTable(8);
            numericForceSetpoint = new NumericUpDown
            {
                DecimalPlaces = 2,
                Minimum = -500.0M,
                Maximum = 500.0M,
                Increment = 1.0M,
                Value = 5.0M,
                Width = 120
            };
            AddSettingRow(layout, 0, "Setpoint", numericForceSetpoint);

            numericKp = CreateGainNumeric(0.25M);
            AddSettingRow(layout, 1, "Kp", numericKp);
            numericKi = CreateGainNumeric(0.05M);
            AddSettingRow(layout, 2, "Ki", numericKi);
            numericKd = CreateGainNumeric(0.02M);
            AddSettingRow(layout, 3, "Kd", numericKd);

            numericAlpha = new NumericUpDown
            {
                DecimalPlaces = 2,
                Minimum = 0.00M,
                Maximum = 1.00M,
                Increment = 0.05M,
                Value = 0.35M,
                Width = 120
            };
            AddSettingRow(layout, 4, "Filter alpha", numericAlpha);

            numericMaxDeltaZ = new NumericUpDown
            {
                DecimalPlaces = 2,
                Minimum = 0.01M,
                Maximum = 10.0M,
                Increment = 0.05M,
                Value = 0.50M,
                Width = 120
            };
            AddSettingRow(layout, 5, "Max Delta Z", numericMaxDeltaZ);

            checkBoxInvertControl = new CheckBox
            {
                Text = "Invert Z command",
                AutoSize = true
            };
            AddSettingRow(layout, 6, "Direction", checkBoxInvertControl);

            buttonStartForceControl = new Button
            {
                Text = "Start force control",
                AutoSize = true
            };
            buttonStartForceControl.Click += ButtonStartForceControl_Click;
            AddSettingRow(layout, 7, "Run", buttonStartForceControl);

            groupBox.Controls.Add(layout);
            return groupBox;
        }

        private GroupBox BuildSessionGroup()
        {
            var groupBox = new GroupBox
            {
                Text = "Session / CSV",
                Dock = DockStyle.Fill
            };

            var layout = CreateSettingsTable(7);
            textBoxCsvPath = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true
            };
            AddSettingRow(layout, 0, "CSV file", textBoxCsvPath);

            var fileButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };
            buttonBrowseCsv = new Button { Text = "Select file", AutoSize = true };
            buttonBrowseCsv.Click += ButtonBrowseCsv_Click;
            buttonExportCsv = new Button { Text = "Export CSV", AutoSize = true };
            buttonExportCsv.Click += ButtonExportCsv_Click;
            buttonClearSession = new Button { Text = "Clear session", AutoSize = true };
            buttonClearSession.Click += ButtonClearSession_Click;
            fileButtons.Controls.Add(buttonBrowseCsv);
            fileButtons.Controls.Add(buttonExportCsv);
            fileButtons.Controls.Add(buttonClearSession);
            AddSettingRow(layout, 1, "Actions", fileButtons);

            labelRunState = new Label
            {
                Text = "Idle",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            AddSettingRow(layout, 2, "Run state", labelRunState);

            labelSampleCount = new Label
            {
                Text = "0 sample(s)",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            AddSettingRow(layout, 3, "Samples", labelSampleCount);

            buttonStopRun = new Button
            {
                Text = "Stop run",
                AutoSize = true
            };
            buttonStopRun.Click += ButtonStopRun_Click;
            AddSettingRow(layout, 4, "Control", buttonStopRun);

            AddSettingRow(layout, 5, "Current unit", new Label
            {
                Name = "labelCurrentUnit",
                Text = _activeUnit,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });

            AddSettingRow(layout, 6, "Notes", new Label
            {
                Text = "CSV columns follow MeasurementSample properties 1:1.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            });

            groupBox.Controls.Add(layout);
            return groupBox;
        }
    }
}
