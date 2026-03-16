using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xARMForm
{

    public partial class Form1 : Form
    {
        // Robot arm
        readonly Robot xARM;

        public Form1()
        {
            InitializeComponent();
            xARM = new Robot();
            comboBoxSetCollisionSensitivity.SelectedIndex = 3;
            numericUpDownToolStep.Value = 10;
            checkBoxSelfCollision.AutoCheck = false;
            timerCMD.Interval = 300;
            SetRobotControlsEnabled(false);
            StopTimer();
            UpdateConnectionStatus(false);
            timerConnection.Start();
        }

        private bool IsRobotReady()
        {
            return xARM.IsCreated() && xARM.IsConnected() && xARM.IsEnableMotion();
        }

        private void UpdateConnectionStatus(bool connected)
        {
            toolStripStatusLabelConnection.Text = connected ? "Connected" : "Not connected";
        }

        private void SyncRobotUiState()
        {
            bool connected = xARM.IsConnected();
            buttonMotionARM.Enabled = connected;
            buttonMotionARM.Text = xARM.IsEnableMotion() ? "Disable Motion" : "Enable Motion";
            buttonResetARM.Enabled = IsRobotReady();
            SetRobotControlsEnabled(IsRobotReady());
            UpdateConnectionStatus(connected);
            if (!connected)
            {
                StopTimer();
                ClearRobotStatusDisplays();
            }
        }

        private void RefreshConnectionState()
        {
            bool connected = xARM.ProbeConnection();
            SyncRobotUiState();
            if (connected)
                RefreshRobotStatusDisplays();
        }

        private void SetRobotControlsEnabled(bool enabled)
        {
            buttonSetCollisionSensitivity.Enabled = enabled;
            comboBoxSetCollisionSensitivity.Enabled = enabled;
            buttonSelfCollision.Enabled = enabled;
            buttonMoveHome.Enabled = enabled;
            buttonMoveBase.Enabled = enabled;
            buttonMoveAngle.Enabled = enabled;
            buttonMoveToolXPlus.Enabled = enabled;
            buttonMoveToolXMinus.Enabled = enabled;
            buttonMoveToolYPlus.Enabled = enabled;
            buttonMoveToolYMinus.Enabled = enabled;
            buttonMoveToolZPlus.Enabled = enabled;
            buttonMoveToolZMinus.Enabled = enabled;
            numericUpDownToolStep.Enabled = enabled;
            buttonTimer.Enabled = enabled;
        }

        private void StopTimer()
        {
            timerCMD.Stop();
            buttonTimer.Text = "Start Timer";
        }

        private string FormatValues(float[] values, int count)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                    builder.Append(" | ");
                builder.Append(values[i].ToString("F"));
            }
            return builder.ToString();
        }

        private void ClearRobotStatusDisplays()
        {
            textBoxJoint.Text = string.Empty;
            textBoxPosition.Text = string.Empty;
            textBoxBase.Text = string.Empty;
            textBoxTCP.Text = string.Empty;
            textBoxCollitionSensitivity.Text = string.Empty;
            checkBoxSelfCollision.Checked = false;
        }

        private void UpdateJointDisplay()
        {
            float[] joint = xARM.GetCurrentJoint();
            textBoxJoint.Text = FormatValues(joint, 6);
            textBoxJoint.Update();
        }

        private void UpdatePositionDisplay()
        {
            float[] pose = xARM.GetCurrentPosition();
            textBoxPosition.Text = FormatValues(pose, 6);
            textBoxPosition.Update();
        }

        private void UpdateBaseOffsetDisplay()
        {
            float[] frame = xARM.GetBase();
            textBoxBase.Text = FormatValues(frame, 6);
            textBoxBase.Update();
        }

        private void UpdateTcpOffsetDisplay()
        {
            float[] frame = xARM.GetTCP();
            textBoxTCP.Text = FormatValues(frame, 6);
            textBoxTCP.Update();
        }

        private void UpdateCollisionSensitivityDisplay()
        {
            int sens = xARM.GetCollisionSensitivity();
            textBoxCollitionSensitivity.Text = sens.ToString();
            textBoxCollitionSensitivity.Update();
            comboBoxSetCollisionSensitivity.SelectedIndex = sens;
        }

        private void RefreshMotionData()
        {
            if (!xARM.IsConnected())
            {
                textBoxJoint.Text = string.Empty;
                textBoxPosition.Text = string.Empty;
                return;
            }

            UpdateJointDisplay();
            UpdatePositionDisplay();
        }

        private void RefreshRobotStatusDisplays()
        {
            if (!xARM.IsConnected())
            {
                ClearRobotStatusDisplays();
                return;
            }

            RefreshMotionData();
            UpdateBaseOffsetDisplay();
            UpdateTcpOffsetDisplay();
            UpdateCollisionSensitivityDisplay();
            checkBoxSelfCollision.Checked = xARM.GetSelfCollision();
        }

        private void ShowCommandError(string action, int errorCode)
        {
            string message = $"{action} failed (code {errorCode}).";
            if (errorCode == 9)
                message = $"{action} failed: robot state is not ready to move (code 9).";

            MessageBox.Show(
                message,
                "xARM",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            SyncRobotUiState();
        }

        private void ExecuteMoveCommand(Func<int> command, string action)
        {
            if (!IsRobotReady())
                return;

            int ret = command();
            if (ret == 9)
            {
                int recoverRet = xARM.PrepareMotion();
                if (recoverRet == 0)
                    ret = command();
                else
                    ret = recoverRet;
            }
            if (ret == 0)
                RefreshMotionData();
            else
                ShowCommandError(action, ret);

            SyncRobotUiState();
        }

        private void MoveToolByStep(float dx, float dy, float dz, string action)
        {
            if (!IsRobotReady())
                return;

            int ret = xARM.MoveToolRelative(dx, dy, dz, true);
            if (ret == 9)
            {
                int recoverRet = xARM.PrepareMotion();
                if (recoverRet == 0)
                    ret = xARM.MoveToolRelative(dx, dy, dz, true);
                else
                    ret = recoverRet;
            }
            if (ret == 0)
                RefreshMotionData();
            else
                ShowCommandError(action, ret);

            SyncRobotUiState();
        }

        private void ButtonCreateARM_Click(object sender, EventArgs e)
        {
            string IP = textBoxIPAdress.Text;
            xARM.Create(IP);
            if (xARM.IsConnected())
            {
                // Update GUI
                buttonMotionARM.Enabled = true;
                buttonResetARM.Enabled = false;
                SetRobotControlsEnabled(false);
                StopTimer();
                _ = xARM.GetVersion();
                RefreshRobotStatusDisplays();
            }
            SyncRobotUiState();
        }

        private void ButtonMotionARM_Click(object sender, EventArgs e)
        {
            int ret;
            if (xARM.IsEnableMotion())
            {
                ret = xARM.EnableMotion(false);
                if (ret == 0)
                {
                    StopTimer();
                    SetRobotControlsEnabled(false);
                    buttonResetARM.Enabled = false;
                }
                else
                    ShowCommandError("Disable motion", ret);
            }
            else
            {
                ret = xARM.EnableMotion(true);
                if (ret == 0)
                {
                    xARM.SetMode(0);
                    xARM.SetState(0);
                    buttonResetARM.Enabled = true;
                    SetRobotControlsEnabled(true);
                    RefreshRobotStatusDisplays();
                }
                else
                {
                    buttonResetARM.Enabled = false;
                    SetRobotControlsEnabled(false);
                    ShowCommandError("Enable motion", ret);
                }
            }
            SyncRobotUiState();
        }

        private void ButtonResetARM_Click(object sender, EventArgs e)
        {
            if (!IsRobotReady())
                return;

            StopTimer();
            xARM.Reset();
            SetRobotControlsEnabled(true);
            RefreshRobotStatusDisplays();
            SyncRobotUiState();
        }

        private void ButtonMoveBase_Click(object sender, EventArgs e)
        {
            float[] pose1 = { 300, 0, 200, 180, 0, 0 };
            ExecuteMoveCommand(() => xARM.MoveBase(pose1, true), "Move To Preset Pose");
        }

        private void ButtonMoveAngle_Click(object sender, EventArgs e)
        {
            if (!IsRobotReady())
                return;

            float[] angles = new float[7];
            angles = xARM.GetCurrentJoint();
            angles[0] += 10.0F;
            ExecuteMoveCommand(() => xARM.MoveJointValues(angles), "Move Angle");
        }

        private void ButtonMoveHome_Click(object sender, EventArgs e)
        {
            ExecuteMoveCommand(() => xARM.MoveHome(0, 0, 0, true), "Move Home");
        }

        private void ButtonSetCollisionSensitivity_Click(object sender, EventArgs e)
        {
            int sens;
            int ret = -1;
            if (xARM.IsCreated())
            {
                sens = comboBoxSetCollisionSensitivity.SelectedIndex;
                textBoxCollitionSensitivity.Text = sens.ToString();
                textBoxCollitionSensitivity.Update();
                ret = xARM.SetCollisionSensitivity(sens);
                if (ret == 0)
                {
                    int prepareRet = xARM.PrepareMotion();
                    if (prepareRet != 0)
                        ShowCommandError("Re-arm after collision sensitivity change", prepareRet);
                }
                else
                    ShowCommandError("Set Collision Sensitivity", ret);
            }
            SyncRobotUiState();
        }


        private void ButtonSelfCollision_Click(object sender, EventArgs e)
        {
            if (xARM.IsCreated())
            {
                if (xARM.GetSelfCollision())
                {
                    int ret = xARM.SetSelfCollision(false);
                    if (ret == 0)
                    {
                        xARM.PrepareMotion();
                        checkBoxSelfCollision.Checked = false;
                    }
                    else
                        ShowCommandError("Disable Self Collision", ret);
                }
                else
                {
                    int ret = xARM.SetSelfCollision(true);
                    if (ret == 0)
                    {
                        xARM.PrepareMotion();
                        checkBoxSelfCollision.Checked = true;
                    }
                    else
                        ShowCommandError("Enable Self Collision", ret);
                }
            }
            SyncRobotUiState();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopTimer();
            if (xARM.IsCreated())
            {
                xARM.EnableMotion(false);
            }
        }

        private void timerCMD_Tick(object sender, EventArgs e)
        {
            if (!IsRobotReady())
            {
                StopTimer();
                SyncRobotUiState();
                return;
            }

            int ret = xARM.MoveToolRelative(0.0F, 0.0F, 0.2F, true);
            if (ret == 0)
                RefreshMotionData();
            else
            {
                StopTimer();
                ShowCommandError("Timer Move Tool +Z", ret);
            }

            SyncRobotUiState();
        }

        private void ButtonTimer_Click(object sender, EventArgs e)
        {
            if (!IsRobotReady())
            {
                StopTimer();
                SyncRobotUiState();
                return;
            }

            if (buttonTimer.Text == "Start Timer")
            {
                timerCMD.Start();
                buttonTimer.Text = "Stop Timer";
            }
            else
            {
                StopTimer();
            }
        }

        private void timerConnection_Tick(object sender, EventArgs e)
        {
            RefreshConnectionState();
        }

        private float GetToolStep()
        {
            return (float)numericUpDownToolStep.Value;
        }

        private void ButtonMoveToolXPlus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(GetToolStep(), 0.0F, 0.0F, "Move Tool +X");
        }

        private void ButtonMoveToolXMinus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(-GetToolStep(), 0.0F, 0.0F, "Move Tool -X");
        }

        private void ButtonMoveToolYPlus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(0.0F, GetToolStep(), 0.0F, "Move Tool +Y");
        }

        private void ButtonMoveToolYMinus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(0.0F, -GetToolStep(), 0.0F, "Move Tool -Y");
        }

        private void ButtonMoveToolZPlus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(0.0F, 0.0F, GetToolStep(), "Move Tool +Z");
        }

        private void ButtonMoveToolZMinus_Click(object sender, EventArgs e)
        {
            MoveToolByStep(0.0F, 0.0F, -GetToolStep(), "Move Tool -Z");
        }
    }
}
