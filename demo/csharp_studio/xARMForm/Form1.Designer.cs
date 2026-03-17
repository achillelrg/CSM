namespace xARMForm
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxIPAdress = new System.Windows.Forms.TextBox();
            this.labelIPAdress = new System.Windows.Forms.Label();
            this.buttonCreateARM = new System.Windows.Forms.Button();
            this.labelJoint = new System.Windows.Forms.Label();
            this.textBoxJoint = new System.Windows.Forms.TextBox();
            this.buttonResetARM = new System.Windows.Forms.Button();
            this.textBoxPosition = new System.Windows.Forms.TextBox();
            this.labelPosition = new System.Windows.Forms.Label();
            this.textBoxBase = new System.Windows.Forms.TextBox();
            this.labelBase = new System.Windows.Forms.Label();
            this.textBoxTCP = new System.Windows.Forms.TextBox();
            this.labelTCP = new System.Windows.Forms.Label();
            this.buttonMoveBase = new System.Windows.Forms.Button();
            this.buttonMoveAngle = new System.Windows.Forms.Button();
            this.buttonMoveHome = new System.Windows.Forms.Button();
            this.buttonMotionARM = new System.Windows.Forms.Button();
            this.labelCollisionSensitivity = new System.Windows.Forms.Label();
            this.textBoxCollitionSensitivity = new System.Windows.Forms.TextBox();
            this.timerCMD = new System.Windows.Forms.Timer(this.components);
            this.buttonTimer = new System.Windows.Forms.Button();
            this.buttonSelfCollision = new System.Windows.Forms.Button();
            this.checkBoxSelfCollision = new System.Windows.Forms.CheckBox();
            this.buttonSetCollisionSensitivity = new System.Windows.Forms.Button();
            this.comboBoxSetCollisionSensitivity = new System.Windows.Forms.ComboBox();
            this.groupBoxMoveTool = new System.Windows.Forms.GroupBox();
            this.buttonMoveToolZMinus = new System.Windows.Forms.Button();
            this.buttonMoveToolZPlus = new System.Windows.Forms.Button();
            this.buttonMoveToolYMinus = new System.Windows.Forms.Button();
            this.buttonMoveToolYPlus = new System.Windows.Forms.Button();
            this.buttonMoveToolXMinus = new System.Windows.Forms.Button();
            this.buttonMoveToolXPlus = new System.Windows.Forms.Button();
            this.labelToolStep = new System.Windows.Forms.Label();
            this.numericUpDownToolStep = new System.Windows.Forms.NumericUpDown();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelConnection = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerConnection = new System.Windows.Forms.Timer(this.components);
            this.groupBoxMoveTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownToolStep)).BeginInit();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxIPAdress
            // 
            this.textBoxIPAdress.Location = new System.Drawing.Point(125, 52);
            this.textBoxIPAdress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxIPAdress.Name = "textBoxIPAdress";
            this.textBoxIPAdress.Size = new System.Drawing.Size(132, 22);
            this.textBoxIPAdress.TabIndex = 0;
            this.textBoxIPAdress.Text = "192.168.1.214";
            // 
            // labelIPAdress
            // 
            this.labelIPAdress.AutoSize = true;
            this.labelIPAdress.Location = new System.Drawing.Point(55, 57);
            this.labelIPAdress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIPAdress.Name = "labelIPAdress";
            this.labelIPAdress.Size = new System.Drawing.Size(65, 16);
            this.labelIPAdress.TabIndex = 1;
            this.labelIPAdress.Text = "IP Adress";
            // 
            // buttonCreateARM
            // 
            this.buttonCreateARM.Location = new System.Drawing.Point(59, 102);
            this.buttonCreateARM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCreateARM.Name = "buttonCreateARM";
            this.buttonCreateARM.Size = new System.Drawing.Size(137, 28);
            this.buttonCreateARM.TabIndex = 2;
            this.buttonCreateARM.Text = "Connect Robot";
            this.buttonCreateARM.UseVisualStyleBackColor = true;
            this.buttonCreateARM.Click += new System.EventHandler(this.ButtonCreateARM_Click);
            // 
            // labelJoint
            // 
            this.labelJoint.AutoSize = true;
            this.labelJoint.Location = new System.Drawing.Point(376, 63);
            this.labelJoint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelJoint.Name = "labelJoint";
            this.labelJoint.Size = new System.Drawing.Size(80, 16);
            this.labelJoint.TabIndex = 3;
            this.labelJoint.Text = "Joint Angles";
            // 
            // textBoxJoint
            // 
            this.textBoxJoint.Location = new System.Drawing.Point(504, 59);
            this.textBoxJoint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxJoint.Name = "textBoxJoint";
            this.textBoxJoint.ReadOnly = true;
            this.textBoxJoint.Size = new System.Drawing.Size(312, 22);
            this.textBoxJoint.TabIndex = 4;
            // 
            // buttonResetARM
            // 
            this.buttonResetARM.Enabled = false;
            this.buttonResetARM.Location = new System.Drawing.Point(59, 174);
            this.buttonResetARM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonResetARM.Name = "buttonResetARM";
            this.buttonResetARM.Size = new System.Drawing.Size(137, 28);
            this.buttonResetARM.TabIndex = 5;
            this.buttonResetARM.Text = "Reset / Re-arm";
            this.buttonResetARM.UseVisualStyleBackColor = true;
            this.buttonResetARM.Click += new System.EventHandler(this.ButtonResetARM_Click);
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.Location = new System.Drawing.Point(504, 95);
            this.textBoxPosition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.ReadOnly = true;
            this.textBoxPosition.Size = new System.Drawing.Size(312, 22);
            this.textBoxPosition.TabIndex = 7;
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(376, 98);
            this.labelPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(100, 16);
            this.labelPosition.TabIndex = 6;
            this.labelPosition.Text = "Current Position";
            // 
            // textBoxBase
            // 
            this.textBoxBase.Location = new System.Drawing.Point(504, 130);
            this.textBoxBase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxBase.Name = "textBoxBase";
            this.textBoxBase.ReadOnly = true;
            this.textBoxBase.Size = new System.Drawing.Size(312, 22);
            this.textBoxBase.TabIndex = 9;
            // 
            // labelBase
            // 
            this.labelBase.AutoSize = true;
            this.labelBase.Location = new System.Drawing.Point(376, 134);
            this.labelBase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBase.Name = "labelBase";
            this.labelBase.Size = new System.Drawing.Size(76, 16);
            this.labelBase.TabIndex = 8;
            this.labelBase.Text = "Base Offset";
            // 
            // textBoxTCP
            // 
            this.textBoxTCP.Location = new System.Drawing.Point(504, 166);
            this.textBoxTCP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxTCP.Name = "textBoxTCP";
            this.textBoxTCP.ReadOnly = true;
            this.textBoxTCP.Size = new System.Drawing.Size(312, 22);
            this.textBoxTCP.TabIndex = 11;
            // 
            // labelTCP
            // 
            this.labelTCP.AutoSize = true;
            this.labelTCP.Location = new System.Drawing.Point(376, 170);
            this.labelTCP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTCP.Name = "labelTCP";
            this.labelTCP.Size = new System.Drawing.Size(71, 16);
            this.labelTCP.TabIndex = 10;
            this.labelTCP.Text = "TCP Offset";
            // 
            // buttonMoveBase
            // 
            this.buttonMoveBase.Enabled = false;
            this.buttonMoveBase.Location = new System.Drawing.Point(379, 255);
            this.buttonMoveBase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMoveBase.Name = "buttonMoveBase";
            this.buttonMoveBase.Size = new System.Drawing.Size(120, 28);
            this.buttonMoveBase.TabIndex = 12;
            this.buttonMoveBase.Text = "Move To Preset";
            this.buttonMoveBase.UseVisualStyleBackColor = true;
            this.buttonMoveBase.Click += new System.EventHandler(this.ButtonMoveBase_Click);
            // 
            // buttonMoveAngle
            // 
            this.buttonMoveAngle.Enabled = false;
            this.buttonMoveAngle.Location = new System.Drawing.Point(379, 291);
            this.buttonMoveAngle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMoveAngle.Name = "buttonMoveAngle";
            this.buttonMoveAngle.Size = new System.Drawing.Size(120, 28);
            this.buttonMoveAngle.TabIndex = 13;
            this.buttonMoveAngle.Text = "Joint 1 +10 deg";
            this.buttonMoveAngle.UseVisualStyleBackColor = true;
            this.buttonMoveAngle.Click += new System.EventHandler(this.ButtonMoveAngle_Click);
            // 
            // buttonMoveHome
            // 
            this.buttonMoveHome.Enabled = false;
            this.buttonMoveHome.Location = new System.Drawing.Point(379, 220);
            this.buttonMoveHome.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMoveHome.Name = "buttonMoveHome";
            this.buttonMoveHome.Size = new System.Drawing.Size(120, 28);
            this.buttonMoveHome.TabIndex = 15;
            this.buttonMoveHome.Text = "Go Home";
            this.buttonMoveHome.UseVisualStyleBackColor = true;
            this.buttonMoveHome.Click += new System.EventHandler(this.ButtonMoveHome_Click);
            // 
            // buttonMotionARM
            // 
            this.buttonMotionARM.Enabled = false;
            this.buttonMotionARM.Location = new System.Drawing.Point(59, 138);
            this.buttonMotionARM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMotionARM.Name = "buttonMotionARM";
            this.buttonMotionARM.Size = new System.Drawing.Size(137, 28);
            this.buttonMotionARM.TabIndex = 16;
            this.buttonMotionARM.Text = "Enable Motion";
            this.buttonMotionARM.UseVisualStyleBackColor = true;
            this.buttonMotionARM.Click += new System.EventHandler(this.ButtonMotionARM_Click);
            // 
            // labelCollisionSensitivity
            // 
            this.labelCollisionSensitivity.AutoSize = true;
            this.labelCollisionSensitivity.Location = new System.Drawing.Point(55, 220);
            this.labelCollisionSensitivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCollisionSensitivity.Name = "labelCollisionSensitivity";
            this.labelCollisionSensitivity.Size = new System.Drawing.Size(112, 16);
            this.labelCollisionSensitivity.TabIndex = 17;
            this.labelCollisionSensitivity.Text = "Current Sensitivity";
            // 
            // textBoxCollitionSensitivity
            // 
            this.textBoxCollitionSensitivity.Location = new System.Drawing.Point(237, 214);
            this.textBoxCollitionSensitivity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCollitionSensitivity.Name = "textBoxCollitionSensitivity";
            this.textBoxCollitionSensitivity.ReadOnly = true;
            this.textBoxCollitionSensitivity.Size = new System.Drawing.Size(89, 22);
            this.textBoxCollitionSensitivity.TabIndex = 18;
            // 
            // timerCMD
            // 
            this.timerCMD.Tick += new System.EventHandler(this.timerCMD_Tick);
            // 
            // buttonTimer
            // 
            this.buttonTimer.Enabled = false;
            this.buttonTimer.Location = new System.Drawing.Point(379, 327);
            this.buttonTimer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTimer.Name = "buttonTimer";
            this.buttonTimer.Size = new System.Drawing.Size(120, 28);
            this.buttonTimer.TabIndex = 19;
            this.buttonTimer.Text = "Start Timer";
            this.buttonTimer.UseVisualStyleBackColor = true;
            this.buttonTimer.Click += new System.EventHandler(this.ButtonTimer_Click);
            // 
            // buttonSelfCollision
            // 
            this.buttonSelfCollision.Enabled = false;
            this.buttonSelfCollision.Location = new System.Drawing.Point(59, 283);
            this.buttonSelfCollision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSelfCollision.Name = "buttonSelfCollision";
            this.buttonSelfCollision.Size = new System.Drawing.Size(171, 28);
            this.buttonSelfCollision.TabIndex = 20;
            this.buttonSelfCollision.Text = "Toggle Self Collision";
            this.buttonSelfCollision.UseVisualStyleBackColor = true;
            this.buttonSelfCollision.Click += new System.EventHandler(this.ButtonSelfCollision_Click);
            // 
            // checkBoxSelfCollision
            // 
            this.checkBoxSelfCollision.AutoSize = true;
            this.checkBoxSelfCollision.Location = new System.Drawing.Point(237, 288);
            this.checkBoxSelfCollision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxSelfCollision.Name = "checkBoxSelfCollision";
            this.checkBoxSelfCollision.Size = new System.Drawing.Size(106, 20);
            this.checkBoxSelfCollision.TabIndex = 21;
            this.checkBoxSelfCollision.Text = "Self Collision";
            this.checkBoxSelfCollision.UseVisualStyleBackColor = true;
            // 
            // buttonSetCollisionSensitivity
            // 
            this.buttonSetCollisionSensitivity.Enabled = false;
            this.buttonSetCollisionSensitivity.Location = new System.Drawing.Point(59, 247);
            this.buttonSetCollisionSensitivity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSetCollisionSensitivity.Name = "buttonSetCollisionSensitivity";
            this.buttonSetCollisionSensitivity.Size = new System.Drawing.Size(171, 28);
            this.buttonSetCollisionSensitivity.TabIndex = 22;
            this.buttonSetCollisionSensitivity.Text = "Apply Sensitivity";
            this.buttonSetCollisionSensitivity.UseVisualStyleBackColor = true;
            this.buttonSetCollisionSensitivity.Click += new System.EventHandler(this.ButtonSetCollisionSensitivity_Click);
            // 
            // comboBoxSetCollisionSensitivity
            // 
            this.comboBoxSetCollisionSensitivity.FormattingEnabled = true;
            this.comboBoxSetCollisionSensitivity.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboBoxSetCollisionSensitivity.Location = new System.Drawing.Point(237, 250);
            this.comboBoxSetCollisionSensitivity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxSetCollisionSensitivity.Name = "comboBoxSetCollisionSensitivity";
            this.comboBoxSetCollisionSensitivity.Size = new System.Drawing.Size(89, 24);
            this.comboBoxSetCollisionSensitivity.TabIndex = 23;
            // 
            // groupBoxMoveTool
            // 
            this.groupBoxMoveTool.Controls.Add(this.buttonMoveToolZMinus);
            this.groupBoxMoveTool.Controls.Add(this.buttonMoveToolZPlus);
            this.groupBoxMoveTool.Controls.Add(this.buttonMoveToolYMinus);
            this.groupBoxMoveTool.Controls.Add(this.buttonMoveToolYPlus);
            this.groupBoxMoveTool.Controls.Add(this.buttonMoveToolXMinus);
            this.groupBoxMoveTool.Controls.Add(this.buttonMoveToolXPlus);
            this.groupBoxMoveTool.Controls.Add(this.labelToolStep);
            this.groupBoxMoveTool.Controls.Add(this.numericUpDownToolStep);
            this.groupBoxMoveTool.Location = new System.Drawing.Point(592, 212);
            this.groupBoxMoveTool.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxMoveTool.Name = "groupBoxMoveTool";
            this.groupBoxMoveTool.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxMoveTool.Size = new System.Drawing.Size(224, 178);
            this.groupBoxMoveTool.TabIndex = 24;
            this.groupBoxMoveTool.TabStop = false;
            this.groupBoxMoveTool.Text = "Move Tool";
            // 
            // buttonMoveToolZMinus
            // 
            this.buttonMoveToolZMinus.Enabled = false;
            this.buttonMoveToolZMinus.Location = new System.Drawing.Point(21, 133);
            this.buttonMoveToolZMinus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMoveToolZMinus.Name = "buttonMoveToolZMinus";
            this.buttonMoveToolZMinus.Size = new System.Drawing.Size(80, 28);
            this.buttonMoveToolZMinus.TabIndex = 7;
            this.buttonMoveToolZMinus.Text = "-Z";
            this.buttonMoveToolZMinus.UseVisualStyleBackColor = true;
            this.buttonMoveToolZMinus.Click += new System.EventHandler(this.ButtonMoveToolZMinus_Click);
            // 
            // buttonMoveToolZPlus
            // 
            this.buttonMoveToolZPlus.Enabled = false;
            this.buttonMoveToolZPlus.Location = new System.Drawing.Point(123, 133);
            this.buttonMoveToolZPlus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMoveToolZPlus.Name = "buttonMoveToolZPlus";
            this.buttonMoveToolZPlus.Size = new System.Drawing.Size(80, 28);
            this.buttonMoveToolZPlus.TabIndex = 6;
            this.buttonMoveToolZPlus.Text = "+Z";
            this.buttonMoveToolZPlus.UseVisualStyleBackColor = true;
            this.buttonMoveToolZPlus.Click += new System.EventHandler(this.ButtonMoveToolZPlus_Click);
            // 
            // buttonMoveToolYMinus
            // 
            this.buttonMoveToolYMinus.Enabled = false;
            this.buttonMoveToolYMinus.Location = new System.Drawing.Point(21, 97);
            this.buttonMoveToolYMinus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMoveToolYMinus.Name = "buttonMoveToolYMinus";
            this.buttonMoveToolYMinus.Size = new System.Drawing.Size(80, 28);
            this.buttonMoveToolYMinus.TabIndex = 5;
            this.buttonMoveToolYMinus.Text = "-Y";
            this.buttonMoveToolYMinus.UseVisualStyleBackColor = true;
            this.buttonMoveToolYMinus.Click += new System.EventHandler(this.ButtonMoveToolYMinus_Click);
            // 
            // buttonMoveToolYPlus
            // 
            this.buttonMoveToolYPlus.Enabled = false;
            this.buttonMoveToolYPlus.Location = new System.Drawing.Point(123, 97);
            this.buttonMoveToolYPlus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMoveToolYPlus.Name = "buttonMoveToolYPlus";
            this.buttonMoveToolYPlus.Size = new System.Drawing.Size(80, 28);
            this.buttonMoveToolYPlus.TabIndex = 4;
            this.buttonMoveToolYPlus.Text = "+Y";
            this.buttonMoveToolYPlus.UseVisualStyleBackColor = true;
            this.buttonMoveToolYPlus.Click += new System.EventHandler(this.ButtonMoveToolYPlus_Click);
            // 
            // buttonMoveToolXMinus
            // 
            this.buttonMoveToolXMinus.Enabled = false;
            this.buttonMoveToolXMinus.Location = new System.Drawing.Point(21, 62);
            this.buttonMoveToolXMinus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMoveToolXMinus.Name = "buttonMoveToolXMinus";
            this.buttonMoveToolXMinus.Size = new System.Drawing.Size(80, 28);
            this.buttonMoveToolXMinus.TabIndex = 3;
            this.buttonMoveToolXMinus.Text = "-X";
            this.buttonMoveToolXMinus.UseVisualStyleBackColor = true;
            this.buttonMoveToolXMinus.Click += new System.EventHandler(this.ButtonMoveToolXMinus_Click);
            // 
            // buttonMoveToolXPlus
            // 
            this.buttonMoveToolXPlus.Enabled = false;
            this.buttonMoveToolXPlus.Location = new System.Drawing.Point(123, 62);
            this.buttonMoveToolXPlus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonMoveToolXPlus.Name = "buttonMoveToolXPlus";
            this.buttonMoveToolXPlus.Size = new System.Drawing.Size(80, 28);
            this.buttonMoveToolXPlus.TabIndex = 2;
            this.buttonMoveToolXPlus.Text = "+X";
            this.buttonMoveToolXPlus.UseVisualStyleBackColor = true;
            this.buttonMoveToolXPlus.Click += new System.EventHandler(this.ButtonMoveToolXPlus_Click);
            // 
            // labelToolStep
            // 
            this.labelToolStep.AutoSize = true;
            this.labelToolStep.Location = new System.Drawing.Point(17, 34);
            this.labelToolStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelToolStep.Name = "labelToolStep";
            this.labelToolStep.Size = new System.Drawing.Size(68, 16);
            this.labelToolStep.TabIndex = 1;
            this.labelToolStep.Text = "Step (mm)";
            // 
            // numericUpDownToolStep
            // 
            this.numericUpDownToolStep.Enabled = false;
            this.numericUpDownToolStep.Location = new System.Drawing.Point(123, 30);
            this.numericUpDownToolStep.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownToolStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownToolStep.Name = "numericUpDownToolStep";
            this.numericUpDownToolStep.Size = new System.Drawing.Size(80, 22);
            this.numericUpDownToolStep.TabIndex = 0;
            this.numericUpDownToolStep.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // statusStripMain
            // 
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelTitle,
            this.toolStripStatusLabelConnection});
            this.statusStripMain.Location = new System.Drawing.Point(0, 429);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStripMain.Size = new System.Drawing.Size(870, 26);
            this.statusStripMain.TabIndex = 25;
            this.statusStripMain.Text = "statusStripMain";
            // 
            // toolStripStatusLabelTitle
            // 
            this.toolStripStatusLabelTitle.Name = "toolStripStatusLabelTitle";
            this.toolStripStatusLabelTitle.Size = new System.Drawing.Size(95, 20);
            this.toolStripStatusLabelTitle.Text = "Robot status:";
            // 
            // toolStripStatusLabelConnection
            // 
            this.toolStripStatusLabelConnection.Name = "toolStripStatusLabelConnection";
            this.toolStripStatusLabelConnection.Size = new System.Drawing.Size(107, 20);
            this.toolStripStatusLabelConnection.Text = "Not connected";
            // 
            // timerConnection
            // 
            this.timerConnection.Enabled = true;
            this.timerConnection.Interval = 1000;
            this.timerConnection.Tick += new System.EventHandler(this.timerConnection_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 455);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.groupBoxMoveTool);
            this.Controls.Add(this.comboBoxSetCollisionSensitivity);
            this.Controls.Add(this.buttonSetCollisionSensitivity);
            this.Controls.Add(this.checkBoxSelfCollision);
            this.Controls.Add(this.buttonSelfCollision);
            this.Controls.Add(this.buttonTimer);
            this.Controls.Add(this.textBoxCollitionSensitivity);
            this.Controls.Add(this.labelCollisionSensitivity);
            this.Controls.Add(this.buttonMotionARM);
            this.Controls.Add(this.buttonMoveHome);
            this.Controls.Add(this.buttonMoveAngle);
            this.Controls.Add(this.buttonMoveBase);
            this.Controls.Add(this.textBoxTCP);
            this.Controls.Add(this.labelTCP);
            this.Controls.Add(this.textBoxBase);
            this.Controls.Add(this.labelBase);
            this.Controls.Add(this.textBoxPosition);
            this.Controls.Add(this.labelPosition);
            this.Controls.Add(this.buttonResetARM);
            this.Controls.Add(this.textBoxJoint);
            this.Controls.Add(this.labelJoint);
            this.Controls.Add(this.buttonCreateARM);
            this.Controls.Add(this.labelIPAdress);
            this.Controls.Add(this.textBoxIPAdress);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "xARM Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBoxMoveTool.ResumeLayout(false);
            this.groupBoxMoveTool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownToolStep)).EndInit();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxIPAdress;
        private System.Windows.Forms.Label labelIPAdress;
        private System.Windows.Forms.Button buttonCreateARM;
        private System.Windows.Forms.Label labelJoint;
        private System.Windows.Forms.TextBox textBoxJoint;
        private System.Windows.Forms.Button buttonResetARM;
        private System.Windows.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.TextBox textBoxBase;
        private System.Windows.Forms.Label labelBase;
        private System.Windows.Forms.TextBox textBoxTCP;
        private System.Windows.Forms.Label labelTCP;
        private System.Windows.Forms.Button buttonMoveBase;
        private System.Windows.Forms.Button buttonMoveAngle;
        private System.Windows.Forms.Button buttonMoveHome;
        private System.Windows.Forms.Button buttonMotionARM;
        private System.Windows.Forms.Label labelCollisionSensitivity;
        private System.Windows.Forms.TextBox textBoxCollitionSensitivity;
        private System.Windows.Forms.Timer timerCMD;
        private System.Windows.Forms.Button buttonTimer;
        private System.Windows.Forms.Button buttonSelfCollision;
        private System.Windows.Forms.CheckBox checkBoxSelfCollision;
        private System.Windows.Forms.Button buttonSetCollisionSensitivity;
        private System.Windows.Forms.ComboBox comboBoxSetCollisionSensitivity;
        private System.Windows.Forms.GroupBox groupBoxMoveTool;
        private System.Windows.Forms.Button buttonMoveToolZMinus;
        private System.Windows.Forms.Button buttonMoveToolZPlus;
        private System.Windows.Forms.Button buttonMoveToolYMinus;
        private System.Windows.Forms.Button buttonMoveToolYPlus;
        private System.Windows.Forms.Button buttonMoveToolXMinus;
        private System.Windows.Forms.Button buttonMoveToolXPlus;
        private System.Windows.Forms.Label labelToolStep;
        private System.Windows.Forms.NumericUpDown numericUpDownToolStep;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelTitle;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConnection;
        private System.Windows.Forms.Timer timerConnection;
    }
}

