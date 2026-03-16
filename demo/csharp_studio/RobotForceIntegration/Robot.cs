using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace RobotForceIntegration
{
    internal class Robot
    {
        public const float NO_TIMEOUT = -1;
        public const int API_NOT_CONNECTED = -1;

        protected
            int arm;
            bool xArmCreatedFlag;
            bool ConnectedFlag;
            bool EnableMotionFlag;
            bool SelfCollisionFlag;
            bool WaitFlag;
            bool SafetyFlag;
            float TimeOut;
            int CollisionSensitivity;
            int TeachSensitivity;

        public
            float JointSpeed;
            float JointAcceleration;
            float LinearSpeed;
            float LinearAcceleration;
            float[] CurrentPosition = new float[6];
            float[] OriginePosition = new float[6];
            float[] HomeJoint = new float[7];
            float[] TCP = new float[6];
            float[] BASE = new float[6];
            int[] TCPBoundary = new int[6];
            float[] CurrentJoint = new float[7];
            public float[] MinJoint = new float[7];
            public float[] MaxJoint = new float[7];

        public Robot()
        {
            arm = -1;
            xArmCreatedFlag = false;
            ConnectedFlag = false;
            EnableMotionFlag = false;
            SelfCollisionFlag = false;
            WaitFlag = true;
            SafetyFlag = false;
            TimeOut = -1;
            // Init Motion
            JointSpeed = 9.0F;
            JointAcceleration = 1000.0F;
            LinearSpeed = 100.0F;
            LinearAcceleration = 1000.0F;
            // Init Arrays
            int i;
            for (i = 0; i < 6; i++)
            {
                CurrentPosition[i] = 0.0F;
                OriginePosition[i] = 0.0F;
                TCP[i] = 0.0F;
                BASE[i] = 0.0F;
                TCPBoundary[i] = 0;
            }
            for (i = 0; i < 7; i++)
            {
                HomeJoint[i] = 0.0F;
                CurrentJoint[i] = 0.0F;
            }
            MinJoint[0] = -360.0F;
            MaxJoint[0] = +360.0F;
            MinJoint[1] = -118.0F;
            MaxJoint[1] = +120.0F;
            MinJoint[2] = -225.0F;
            MaxJoint[2] = +11.0F;
            MinJoint[3] = -360.0F;
            MaxJoint[3] = +360.0F;
            MinJoint[4] = -97.0F;
            MaxJoint[4] = +180.0F;
            MinJoint[5] = -360.0F;
            MaxJoint[5] = +360.0F;
            MinJoint[6] = 0.0F;
            MaxJoint[6] = 0.0F;
        }

        private void MarkDisconnected(bool resetInstance = false)
        {
            ConnectedFlag = false;
            EnableMotionFlag = false;
            if (resetInstance)
            {
                xArmCreatedFlag = false;
                arm = -1;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private int SafeApiCall(Func<int> apiCall)
        {
            try
            {
                int ret = apiCall();
                if (ret == API_NOT_CONNECTED)
                    MarkDisconnected();
                return ret;
            }
            catch
            {
                MarkDisconnected();
                return API_NOT_CONNECTED;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private bool SafeApiCall(Func<bool> apiCall, bool defaultValue = false)
        {
            try
            {
                return apiCall();
            }
            catch
            {
                MarkDisconnected();
                return defaultValue;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private void SafeApiAction(Action apiAction)
        {
            try
            {
                apiAction();
            }
            catch
            {
                MarkDisconnected();
            }
        }

        private bool InitializeConnectedArm()
        {
            int ret;

            ret = SafeApiCall(() => XArmAPI.clean_warn());
            if (ret != 0) return false;

            ret = SafeApiCall(() => XArmAPI.clean_error());
            if (ret != 0) return false;

            ret = SafeApiCall(() => XArmAPI.get_world_offset(BASE));
            if (ret != 0) return false;

            ret = SafeApiCall(() => XArmAPI.get_tcp_offset(TCP));
            if (ret != 0) return false;

            ret = SafeApiCall(() => XArmAPI.get_position(CurrentPosition));
            if (ret != 0) return false;

            ret = SafeApiCall(() => XArmAPI.get_servo_angle(CurrentJoint));
            if (ret != 0) return false;

            CollisionSensitivity = 3;
            ret = SafeApiCall(() => XArmAPI.set_collision_sensitivity(CollisionSensitivity));
            if (ret != 0) return false;

            TeachSensitivity = 3;
            ret = SafeApiCall(() => XArmAPI.set_teach_sensitivity(TeachSensitivity));
            if (ret != 0) return false;

            SelfCollisionFlag = false;
            ret = SafeApiCall(() => XArmAPI.set_self_collision_detection(SelfCollisionFlag));
            return ret == 0;
        }

        public bool Create(string IP)
        {
            int ret;
            if (arm == -1)
            {
                arm = SafeApiCall(() => XArmAPI.create_instance(IP, false));
                if (arm != -1)
                {
                    ret = SafeApiCall(() => XArmAPI.switch_xarm(arm));
                    xArmCreatedFlag = true;
                    ConnectedFlag = ret == 0 && InitializeConnectedArm();
                }
                else
                {
                    xArmCreatedFlag = false;
                    ConnectedFlag = false;
                }
            }
            else if (!ConnectedFlag)
            {
                ret = SafeApiCall(() => XArmAPI.switch_xarm(arm));
                if (ret == 0)
                {
                    ret = SafeApiCall(() => XArmAPI.robot_connect(IP));
                    if (ret == 0)
                    {
                        xArmCreatedFlag = true;
                        ConnectedFlag = InitializeConnectedArm();
                    }
                }
            }

            return xArmCreatedFlag && ConnectedFlag;
        }

        public string GetVersion()
        {
            string s = "Test";
            byte[] version = new byte[40];
            int ret = SafeApiCall(() => XArmAPI.get_version(version));
            if (ret == 0)
                s = Encoding.ASCII.GetString(version).TrimEnd('\0');
            return s;
        }

        public bool IsCreated()
        {
            return xArmCreatedFlag;
        }

        public bool IsEnableMotion()
        {
            return EnableMotionFlag;
        }

        public bool IsConnected()
        {
            return ConnectedFlag;
        }

        public bool ProbeConnection()
        {
            int state = -1;
            if (!xArmCreatedFlag || arm == -1)
            {
                ConnectedFlag = false;
                EnableMotionFlag = false;
                return false;
            }

            int ret = SafeApiCall(() => XArmAPI.get_state(ref state));
            ConnectedFlag = ret == 0;
            if (!ConnectedFlag)
                EnableMotionFlag = false;
            return ConnectedFlag;
        }

        public void Reset()
        {
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiAction(() => XArmAPI.reset(true));
        }

        //
        public void ClearErrorCode()
        {
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiCall(() => XArmAPI.clean_error());
        }

        public void ClearWarnCode()
        {
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiCall(() => XArmAPI.clean_warn());
        }

        public int SetSelfCollision(bool flag)
        {
            int ret = -1;
            if (xArmCreatedFlag && ConnectedFlag)
            {
                ret = SafeApiCall(() => XArmAPI.set_self_collision_detection(flag));
                if (ret == 0)
                    SelfCollisionFlag = flag;
            }
            return ret;
        }
        public bool GetSelfCollision()
        {
            return SelfCollisionFlag;
        }

        public int SetCollisionSensitivity(int sens)
        {
            int s;
            int ret;
            switch (sens)
            {
                case 0: 
                case 1: 
                case 2: 
                case 3: 
                case 4: 
                case 5: s = sens; break;
                default: s = 3; break;
            }
            ret = SafeApiCall(() => XArmAPI.set_collision_sensitivity(s));
            if (ret == 0)
                CollisionSensitivity = s;
            return ret;
        }

        public int GetCollisionSensitivity()
        {
            int s;
            s = CollisionSensitivity;
            return s;
        }

        public int SetTeachSensitivity(int sens)
        {
            int s;
            int ret;
            switch (sens)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5: s = sens; break;
                default: s = 3; break;
            }
            ret = SafeApiCall(() => XArmAPI.set_teach_sensitivity(s));
            if (ret == 0)
                TeachSensitivity = s;
            return ret;
        }

        public int GetTeachSensitivity()
        {
            int s;
            s = TeachSensitivity;
            return s;
        }

        public void SetSafetyBoundary(int[] boundary)
        {
            int i;
            if (xArmCreatedFlag && ConnectedFlag)
            {
                int ret = SafeApiCall(() => XArmAPI.set_reduced_tcp_boundary(boundary));
                if (ret == 0)
                {
                    for (i = 0; i < 6; i++)
                        TCPBoundary[i] = boundary[i];
                    SafetyFlag = true;
                }
            }
        }

        public int[] GetSafetyBoundary()
        {
            int i;
            int[] boundary = new int[6];
            if (xArmCreatedFlag && SafetyFlag)
            {
                for (i = 0; i < 6; i++)
                     boundary[i] = TCPBoundary[i];
                SafetyFlag = true;
            }
            return boundary;
        }

        public int ActivateSafetyBoundary(bool flag)
        {
            int ret = -1;
            if (xArmCreatedFlag && ConnectedFlag && SafetyFlag)
            {
                ret = SafeApiCall(() => XArmAPI.set_fense_mode(flag));
            }
            return ret;
        }

        public int EnableMotion(bool flag)
        {
            int ret = -1;
            if (xArmCreatedFlag && ConnectedFlag)
            {
                ret = SafeApiCall(() => XArmAPI.motion_enable(flag));
                if (ret == 0)
                    EnableMotionFlag = flag;
            }
            return ret;
        }

        //Mode 0: Position control mode.
        //Mode 1 : Servoj mode.
        //Mode 2: Joint teaching mode.
        //Mode 4 : Cartesian teaching mode, (not yet available).
        public void SetMode(int mode)
        {
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiCall(() => XArmAPI.set_mode(mode));
        }

        public int GetMode()
        {
            int mode = -1;
            if (xArmCreatedFlag && ConnectedFlag)
                mode = SafeApiCall(() => XArmAPI.get_mode());
            return mode;
        }

        //State 0: Start motion.
        //State 3: Paused state.
        //State 4 : Stop state.
        public int SetState(int state)
        {
            int ret = -1;
            if (xArmCreatedFlag && ConnectedFlag)
                ret = SafeApiCall(() => XArmAPI.set_state(state));
            return ret;
        }

        public int GetState()
        {
            int ret = -1;
            int state = -1;
            if (xArmCreatedFlag && ConnectedFlag)
                ret = SafeApiCall(() => XArmAPI.get_state(ref state));
            return ret == 0 ? state : -1;
        }

        public int PrepareMotion()
        {
            int ret = -1;
            if (xArmCreatedFlag && ConnectedFlag)
            {
                ret = SafeApiCall(() => XArmAPI.set_mode(0));
                if (ret == 0)
                    ret = SafeApiCall(() => XArmAPI.set_state(0));
            }
            return ret;
        }

        public void SetMotionScale(int percent)
        {
            int clampedPercent = Math.Max(1, Math.Min(100, percent));
            JointSpeed = clampedPercent;
            JointAcceleration = clampedPercent * 10.0F;
            LinearSpeed = clampedPercent * 2.0F;
            LinearAcceleration = clampedPercent * 20.0F;
        }

        // world_offset Get Set
        public float[] GetBase()
        {
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiCall(() => XArmAPI.get_world_offset(BASE));
            int i;
            float[] Frame = {0,0,0,0,0,0};
            for (i = 0; i < 6; i++)
                Frame[i] = BASE[i];
            return Frame;
        }

        public void SetBase(float[] Frame)
        {
            int i;
            for (i = 0; i < 6; i++)
                BASE[i] = Frame[i]; 
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiCall(() => XArmAPI.set_world_offset(BASE));
        }

        // tcp_offset Get Set
        public float[] GetTCP()
        {
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiCall(() => XArmAPI.get_tcp_offset(TCP));
            int i;
            float[] Frame = { 0, 0, 0, 0, 0, 0 };
            for (i = 0; i < 6; i++)
                Frame[i] = TCP[i];
            return Frame;
        }

        public void SetTCP(float[] Frame)
        {
            int i;
            for (i = 0; i < 6; i++)
                TCP[i] = Frame[i];
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiCall(() => XArmAPI.set_tcp_offset(TCP));
        }

        public float[] GetCurrentJoint()
        {
            float[] angles = new float[7];
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiCall(() => XArmAPI.get_servo_angle(angles));
            int i;
            for (i = 0; i < 7; i++)
                CurrentJoint[i] = angles[i];
            return CurrentJoint;
        }

        public float[] GetCurrentPosition()
        {
            float[] pose = new float[6];
            if (xArmCreatedFlag && ConnectedFlag)
                SafeApiCall(() => XArmAPI.get_position(pose));
            int i;
            for (i = 0; i < 6; i++)
                CurrentPosition[i] = pose[i];
            return CurrentPosition;
        }

        // set_servo_angle(float[] angles, float speed = 0, float acc = 0, float mvtime = 0, bool wait = false, float timeout = NO_TIMEOUT, float radius = -1, bool relative = false);
        public int MoveJointValues(float[] angles, float speed = 0, float acc = 0, float mvtime = 0, bool wait = false, float timeout = NO_TIMEOUT, float radius = -1, bool relative = false)
        {
            if (speed <= 0)
                speed = JointSpeed;
            if (acc <= 0)
                acc = JointAcceleration;
            return SafeApiCall(() => XArmAPI.set_servo_angle(angles, speed, acc, mvtime, wait, timeout, radius, relative));
        }
        
        // move_gohome(float speed = 0, float acc = 0, float mvtime = 0, bool wait = false, float timeout = NO_TIMEOUT);
        public int MoveHome(float speed = 0.0F, float acc = 0.0F, float mvtime = 0.0F, bool wait = false, float timeout = NO_TIMEOUT)
        {
            if (speed <= 0)
                speed = JointSpeed;
            if (acc <= 0)
                acc = JointAcceleration;
            return SafeApiCall(() => XArmAPI.move_gohome(speed, acc, mvtime, wait, timeout));
        }

        //public static int set_position(float[] pose, float radius = -1, bool wait = false, float timeout = NO_TIMEOUT, bool relative = false)
        public int MoveBase(float[] pose, float radius = -1, bool wait = false, float timeout = NO_TIMEOUT, bool relative = false)
        {
            return SafeApiCall(() => XArmAPI.set_position(
                pose,
                radius,
                LinearSpeed,
                LinearAcceleration,
                0.0F,
                wait,
                timeout,
                relative));
        }

        //public static int set_position(float[] pose, bool wait = false, float timeout = NO_TIMEOUT, bool relative = false)
        public int MoveBase(float[] pose, bool wait = false, float timeout = NO_TIMEOUT, bool relative = false)
        {
            return SafeApiCall(() => XArmAPI.set_position(
                pose,
                -1.0F,
                LinearSpeed,
                LinearAcceleration,
                0.0F,
                wait,
                timeout,
                relative));
        }

        //public static int set_tool_position(float[] pose, bool wait = false, float timeout = NO_TIMEOUT)
        public int MoveTool(float[] pose, bool wait = false, float timeout = NO_TIMEOUT)
        {
            return SafeApiCall(() => XArmAPI.set_tool_position(
                pose,
                LinearSpeed,
                LinearAcceleration,
                0.0F,
                wait,
                timeout));
        }

        public int MoveToolRelative(float dx, float dy, float dz, bool wait = true, float timeout = NO_TIMEOUT)
        {
            float[] pose = { dx, dy, dz, 0.0F, 0.0F, 0.0F };
            return MoveTool(pose, wait, timeout);
        }

        // Inverse Kinematics
        public int GetInverseKinematics(float[] pose, float[] angles)
        {
            return SafeApiCall(() => XArmAPI.get_inverse_kinematics(pose, angles));
        }
        // Forward Kinematics
        public int Get_ForwardKinematics(float[] angles, float[] pose)
        {
            return SafeApiCall(() => XArmAPI.get_forward_kinematics(angles, pose));
        }
        
        //public static int set_servo_angle(float[] angles, bool wait = false, float timeout = NO_TIMEOUT, float radius = -1, bool relative = false)


        }
}
