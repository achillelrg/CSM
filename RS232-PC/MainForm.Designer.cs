namespace RS232_PC
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer runTimer;
        private System.Windows.Forms.Timer legacyRobotTimer;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.runTimer = new System.Windows.Forms.Timer(this.components);
            this.legacyRobotTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // runTimer
            // 
            this.runTimer.Interval = 300;
            this.runTimer.Tick += new System.EventHandler(this.RunTimer_Tick);
            // 
            // legacyRobotTimer
            // 
            this.legacyRobotTimer.Interval = 300;
            this.legacyRobotTimer.Tick += new System.EventHandler(this.LegacyRobotTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 860);
            this.MinimumSize = new System.Drawing.Size(1280, 760);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CSM Robot + Force";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
        }
    }
}
