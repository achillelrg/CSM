using System;
using System.Linq;
using xARMForm;

namespace RS232_PC.Services
{
    public sealed class XArmRobotService : IDisposable
    {
        public const int AxisCount = Robot.AxisCount;

        private readonly Robot _robot = new Robot();

        public bool IsConnected => _robot.IsCreated();

        public bool MotionEnabled => _robot.IsEnableMotion();

        public string TargetIp { get; private set; } = string.Empty;

        public string Version => IsConnected ? _robot.GetVersion() : string.Empty;

        public bool Connect(string ipAddress, out string message)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                message = "Robot IP address is required.";
                return false;
            }

            if (IsConnected)
            {
                message = "Robot controller already connected.";
                return true;
            }

            try
            {
                var created = _robot.Create(ipAddress.Trim());
                TargetIp = ipAddress.Trim();
                message = created ? "Robot connected." : "Robot connection failed.";
                return created;
            }
            catch (Exception ex)
            {
                message = "Robot connection failed: " + ex.Message;
                return false;
            }
        }

        public void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }

            try
            {
                if (MotionEnabled)
                {
                    _robot.EnableMotion(false);
                }
            }
            catch
            {
                // Best effort shutdown.
            }

            _robot.Disconnect();
            TargetIp = string.Empty;
        }

        public bool EnableMotion(out string message)
        {
            if (!IsConnected)
            {
                message = "Robot is not connected.";
                return false;
            }

            try
            {
                var ret = _robot.EnableMotion(true);
                if (ret != 0)
                {
                    message = "Unable to enable motion (" + ret + ").";
                    return false;
                }

                _robot.SetMode(0);
                ret = _robot.SetState(0);
                if (ret != 0)
                {
                    message = "Unable to start motion state (" + ret + ").";
                    return false;
                }

                message = "Robot motion enabled.";
                return true;
            }
            catch (Exception ex)
            {
                message = "Unable to enable motion: " + ex.Message;
                return false;
            }
        }

        public void DisableMotion()
        {
            if (IsConnected)
            {
                _robot.EnableMotion(false);
            }
        }

        public bool Reset(out string message)
        {
            if (!IsConnected)
            {
                message = "Robot is not connected.";
                return false;
            }

            try
            {
                _robot.Reset();
                message = "Robot reset sent.";
                return true;
            }
            catch (Exception ex)
            {
                message = "Robot reset failed: " + ex.Message;
                return false;
            }
        }

        public bool EmergencyStop(out string message)
        {
            if (!IsConnected)
            {
                message = "Robot is not connected.";
                return false;
            }

            try
            {
                _robot.EmergencyStop();
                message = "Emergency stop sent.";
                return true;
            }
            catch (Exception ex)
            {
                message = "Emergency stop failed: " + ex.Message;
                return false;
            }
        }

        public float[] GetCurrentPosition()
        {
            return IsConnected ? _robot.GetCurrentPosition() : new float[6];
        }

        public float[] GetCurrentJoints()
        {
            return IsConnected ? _robot.GetCurrentJoint().Take(AxisCount).ToArray() : new float[AxisCount];
        }

        public bool MoveToolRelative(double deltaX, double deltaY, double deltaZ, bool wait, out string message)
        {
            if (!EnsureMotion(out message))
            {
                return false;
            }

            var deltaPose = new[]
            {
                (float)deltaX,
                (float)deltaY,
                (float)deltaZ,
                0.0f,
                0.0f,
                0.0f
            };

            var ret = _robot.MoveTool(deltaPose, wait);
            message = ret == 0 ? "MoveTool command sent." : "MoveTool failed (" + ret + ").";
            return ret == 0;
        }

        public bool MoveHome(out string message)
        {
            if (!EnsureMotion(out message))
            {
                return false;
            }

            var ret = _robot.MoveHome(0.0f, 0.0f, 0.0f, true);
            message = ret == 0 ? "MoveHome command sent." : "MoveHome failed (" + ret + ").";
            return ret == 0;
        }

        private bool EnsureMotion(out string message)
        {
            if (!IsConnected)
            {
                message = "Robot is not connected.";
                return false;
            }

            if (!MotionEnabled)
            {
                message = "Robot motion is not enabled.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
