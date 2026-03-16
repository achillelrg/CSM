using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using RS232_PC.Services;

namespace RS232_PC
{
    public partial class MainForm
    {
        private void ButtonLegacyCreateArm_Click(object sender, EventArgs e)
        {
            textBoxRobotIp.Text = textBoxLegacyRobotIp.Text;
            ButtonConnectRobot_Click(sender, e);
            RefreshLegacyRobotState();
        }

        private void ButtonLegacyMotionArm_Click(object sender, EventArgs e)
        {
            if (_robotService.MotionEnabled)
            {
                if (legacyRobotTimer.Enabled)
                {
                    legacyRobotTimer.Stop();
                    buttonLegacyTimer.Text = "Start Timer";
                }

                ButtonDisableMotion_Click(sender, e);
            }
            else
            {
                ButtonEnableMotion_Click(sender, e);
            }

            RefreshLegacyRobotState();
        }

        private void ButtonLegacyResetArm_Click(object sender, EventArgs e)
        {
            ButtonResetRobot_Click(sender, e);
            RefreshLegacyRobotState();
        }

        private void ButtonLegacyGetJoint_Click(object sender, EventArgs e)
        {
            textBoxLegacyJoint.Text = string.Join(" | ",
                _robotService.GetCurrentJoints().Select(value => value.ToString("F3", CultureInfo.InvariantCulture)));
        }

        private void ButtonLegacyGetPosition_Click(object sender, EventArgs e)
        {
            textBoxLegacyPosition.Text = string.Join(" | ",
                _robotService.GetCurrentPosition().Take(6).Select(value => value.ToString("F3", CultureInfo.InvariantCulture)));
        }

        private void ButtonLegacyGetBase_Click(object sender, EventArgs e)
        {
            textBoxLegacyBase.Text = string.Join(" | ",
                _robotService.GetBase().Take(6).Select(value => value.ToString("F3", CultureInfo.InvariantCulture)));
        }

        private void ButtonLegacyGetTcp_Click(object sender, EventArgs e)
        {
            textBoxLegacyTcp.Text = string.Join(" | ",
                _robotService.GetTcp().Take(6).Select(value => value.ToString("F3", CultureInfo.InvariantCulture)));
        }

        private void ButtonLegacyMoveBase_Click(object sender, EventArgs e)
        {
            var pose = new[] { 300.0f, 0.0f, 200.0f, 180.0f, 0.0f, 0.0f };
            if (_robotService.MoveBaseAbsolute(pose, true, out var message))
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

        private void ButtonLegacyMoveTcp_Click(object sender, EventArgs e)
        {
            if (_robotService.MoveToolRelative(0.0, 0.0, 20.0, true, out var message))
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

        private void ButtonLegacyMoveAngle_Click(object sender, EventArgs e)
        {
            var angles = _robotService.GetCurrentJoints().ToArray();
            if (angles.Length == 0)
            {
                ShowWarning("Robot joints are not available.");
                return;
            }

            angles[0] += 10.0f;
            if (_robotService.MoveJointValues(angles, out var message))
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

        private void ButtonLegacyMoveHome_Click(object sender, EventArgs e)
        {
            ButtonMoveHome_Click(sender, e);
            RefreshLegacyRobotState();
        }

        private void ButtonLegacyGetCollisionSensitivity_Click(object sender, EventArgs e)
        {
            if (!_robotService.IsConnected)
            {
                ShowWarning("Robot is not connected.");
                return;
            }

            var sensitivity = _robotService.GetCollisionSensitivity();
            textBoxLegacyCollisionSensitivity.Text = sensitivity.ToString(CultureInfo.InvariantCulture);
            comboBoxLegacyCollisionSensitivity.SelectedIndex = Math.Max(0, Math.Min(5, sensitivity));
        }

        private void ButtonLegacySetCollisionSensitivity_Click(object sender, EventArgs e)
        {
            var sensitivity = comboBoxLegacyCollisionSensitivity.SelectedIndex;
            if (sensitivity < 0)
            {
                sensitivity = 3;
            }

            if (_robotService.SetCollisionSensitivity(sensitivity, out var message))
            {
                AppendSystemLog(message);
                ButtonLegacyGetCollisionSensitivity_Click(sender, e);
            }
            else
            {
                ShowWarning(message);
            }
        }

        private void ButtonLegacySelfCollision_Click(object sender, EventArgs e)
        {
            var enabled = !_robotService.GetSelfCollision();
            if (_robotService.SetSelfCollision(enabled, out var message))
            {
                AppendSystemLog(message);
                checkBoxLegacySelfCollision.Checked = enabled;
            }
            else
            {
                ShowWarning(message);
            }
        }

        private void ButtonLegacyTimer_Click(object sender, EventArgs e)
        {
            if (_runMode != RunMode.Idle)
            {
                ShowWarning("Stop the current TP run before using the legacy xARM timer.");
                return;
            }

            if (!_robotService.IsConnected || !_robotService.MotionEnabled)
            {
                ShowWarning("Robot must be connected and motion-enabled before starting the legacy timer.");
                return;
            }

            if (legacyRobotTimer.Enabled)
            {
                legacyRobotTimer.Stop();
                buttonLegacyTimer.Text = "Start Timer";
                AppendSystemLog("Legacy xARM timer stopped.");
            }
            else
            {
                legacyRobotTimer.Start();
                buttonLegacyTimer.Text = "Stop Timer";
                AppendSystemLog("Legacy xARM timer started.");
            }
        }

        private void LegacyRobotTimer_Tick(object sender, EventArgs e)
        {
            if (!_robotService.IsConnected || !_robotService.MotionEnabled)
            {
                legacyRobotTimer.Stop();
                buttonLegacyTimer.Text = "Start Timer";
                return;
            }

            if (_robotService.MoveToolRelative(0.0, 0.0, 0.2, true, out var message))
            {
                AppendSystemLog(message);
                RefreshRobotState(false);
                RefreshLegacyRobotState();
            }
            else
            {
                legacyRobotTimer.Stop();
                buttonLegacyTimer.Text = "Start Timer";
                ShowWarning(message);
            }
        }

        private void RefreshLegacyRobotState()
        {
            if (textBoxLegacyRobotIp != null)
            {
                textBoxLegacyRobotIp.Text = textBoxRobotIp.Text;
            }

            if (!_robotService.IsConnected)
            {
                if (textBoxLegacyJoint != null) textBoxLegacyJoint.Text = string.Empty;
                if (textBoxLegacyPosition != null) textBoxLegacyPosition.Text = string.Empty;
                if (textBoxLegacyBase != null) textBoxLegacyBase.Text = string.Empty;
                if (textBoxLegacyTcp != null) textBoxLegacyTcp.Text = string.Empty;
                if (textBoxLegacyCollisionSensitivity != null) textBoxLegacyCollisionSensitivity.Text = string.Empty;
                if (checkBoxLegacySelfCollision != null) checkBoxLegacySelfCollision.Checked = false;
                return;
            }

            ButtonLegacyGetJoint_Click(this, EventArgs.Empty);
            ButtonLegacyGetPosition_Click(this, EventArgs.Empty);
            ButtonLegacyGetBase_Click(this, EventArgs.Empty);
            ButtonLegacyGetTcp_Click(this, EventArgs.Empty);
            ButtonLegacyGetCollisionSensitivity_Click(this, EventArgs.Empty);
            checkBoxLegacySelfCollision.Checked = _robotService.GetSelfCollision();
        }
    }
}
