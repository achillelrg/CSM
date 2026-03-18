using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
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

        private enum FailureCategory
        {
            None,
            NativeLibraryMissing,
            NativeEntryPointMissing,
            NativeArchitectureMismatch,
            NativeInteropFailure,
            ApiCallFailed,
        }

        protected
            int arm;
            bool xArmCreatedFlag;
            bool ConnectedFlag;
            bool EnableMotionFlag;
            bool SelfCollisionFlag;
            bool SafetyFlag;
            int CollisionSensitivity;
            int TeachSensitivity;

        private FailureCategory lastFailureCategory;
        private string lastFailureMessage;
        private string lastFailureContext;
        private int? lastFailureCode;

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
            SafetyFlag = false;
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
            ClearLastFailure();
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

        private void ClearLastFailure()
        {
            lastFailureCategory = FailureCategory.None;
            lastFailureMessage = string.Empty;
            lastFailureContext = string.Empty;
            lastFailureCode = null;
        }

        private void SetApiFailure(string context, int code, string details = null)
        {
            lastFailureCategory = FailureCategory.ApiCallFailed;
            lastFailureContext = context;
            lastFailureCode = code;
            lastFailureMessage = context + " failed (code " + code.ToString() + ").";
            if (!string.IsNullOrWhiteSpace(details))
                lastFailureMessage += " " + details;
        }

        private void SetInteropFailure(FailureCategory category, string context, Exception ex)
        {
            lastFailureCategory = category;
            lastFailureContext = context;
            lastFailureCode = null;
            lastFailureMessage = context + " failed: " + ex.Message;
        }

        private bool HasInteropFailure()
        {
            return lastFailureCategory == FailureCategory.NativeLibraryMissing
                || lastFailureCategory == FailureCategory.NativeEntryPointMissing
                || lastFailureCategory == FailureCategory.NativeArchitectureMismatch
                || lastFailureCategory == FailureCategory.NativeInteropFailure;
        }

        private bool EnsureSuccess(int code, string context, string details = null)
        {
            if (code == 0)
                return true;

            SetApiFailure(context, code, details);
            return false;
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private int SafeApiCall(Func<int> apiCall, string context = "xArm API call")
        {
            try
            {
                int ret = apiCall();
                if (ret == API_NOT_CONNECTED)
                {
                    MarkDisconnected();
                    SetApiFailure(context, ret, "The xArm SDK reported that the robot is not connected.");
                }
                return ret;
            }
            catch (DllNotFoundException ex)
            {
                MarkDisconnected(true);
                SetInteropFailure(
                    FailureCategory.NativeLibraryMissing,
                    context,
                    ex);
                return API_NOT_CONNECTED;
            }
            catch (EntryPointNotFoundException ex)
            {
                MarkDisconnected(true);
                SetInteropFailure(
                    FailureCategory.NativeEntryPointMissing,
                    context,
                    ex);
                return API_NOT_CONNECTED;
            }
            catch (BadImageFormatException ex)
            {
                MarkDisconnected(true);
                SetInteropFailure(
                    FailureCategory.NativeArchitectureMismatch,
                    context,
                    ex);
                return API_NOT_CONNECTED;
            }
            catch (SEHException ex)
            {
                MarkDisconnected(true);
                SetInteropFailure(
                    FailureCategory.NativeInteropFailure,
                    context,
                    ex);
                return API_NOT_CONNECTED;
            }
            catch
            {
                MarkDisconnected();
                SetInteropFailure(
                    FailureCategory.NativeInteropFailure,
                    context,
                    new InvalidOperationException("Unexpected native interop failure."));
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

            ret = SafeApiCall(() => XArmAPI.clean_warn(), "clean_warn");
            if (!EnsureSuccess(ret, "Robot initialization: clean warnings")) return false;

            ret = SafeApiCall(() => XArmAPI.clean_error(), "clean_error");
            if (!EnsureSuccess(ret, "Robot initialization: clean errors")) return false;

            ret = SafeApiCall(() => XArmAPI.get_world_offset(BASE), "get_world_offset");
            // Sur certains firmwares, get_world_offset renvoie Code 1. On ignore cette erreur non bloquante.
            if (ret != 0 && ret != 1)
                if (!EnsureSuccess(ret, "Robot initialization: read world offset")) return false;

            ret = SafeApiCall(() => XArmAPI.get_tcp_offset(TCP), "get_tcp_offset");
            if (ret != 0 && ret != 1)
                if (!EnsureSuccess(ret, "Robot initialization: read TCP offset")) return false;

            ret = SafeApiCall(() => XArmAPI.get_position(CurrentPosition), "get_position");
            if (!EnsureSuccess(ret, "Robot initialization: read position")) return false;

            ret = SafeApiCall(() => XArmAPI.get_servo_angle(CurrentJoint), "get_servo_angle");
            if (!EnsureSuccess(ret, "Robot initialization: read joint angles")) return false;

            CollisionSensitivity = 3;
            ret = SafeApiCall(() => XArmAPI.set_collision_sensitivity(CollisionSensitivity), "set_collision_sensitivity");
            if (!EnsureSuccess(ret, "Robot initialization: set collision sensitivity")) return false;

            TeachSensitivity = 3;
            ret = SafeApiCall(() => XArmAPI.set_teach_sensitivity(TeachSensitivity), "set_teach_sensitivity");
            if (!EnsureSuccess(ret, "Robot initialization: set teach sensitivity")) return false;

            SelfCollisionFlag = false;
            ret = SafeApiCall(() => XArmAPI.set_self_collision_detection(SelfCollisionFlag), "set_self_collision_detection");
            if (ret != 0)
                SetApiFailure("Robot initialization: configure self collision detection", ret);
            return ret == 0;
        }

        public bool Create(string IP)
        {
            int ret;
            ClearLastFailure();

            if (string.IsNullOrWhiteSpace(IP))
            {
                lastFailureCategory = FailureCategory.ApiCallFailed;
                lastFailureMessage = "Robot IP is empty.";
                return false;
            }

            if (arm == -1)
            {
                arm = SafeApiCall(() => XArmAPI.create_instance(IP, false), "create_instance");
                if (HasInteropFailure())
                    return false;

                if (arm != -1)
                {
                    ret = SafeApiCall(() => XArmAPI.switch_xarm(arm), "switch_xarm");
                    if (HasInteropFailure())
                        return false;

                    xArmCreatedFlag = true;
                    ConnectedFlag = ret == 0 && InitializeConnectedArm();
                    if (ret != 0)
                        SetApiFailure("xArm SDK switch instance", ret);
                    else if (!ConnectedFlag && string.IsNullOrWhiteSpace(lastFailureMessage))
                        lastFailureMessage = "The xArm SDK created an instance but failed during robot initialization.";
                }
                else
                {
                    xArmCreatedFlag = false;
                    ConnectedFlag = false;
                    if (string.IsNullOrWhiteSpace(lastFailureMessage))
                        lastFailureMessage = "The xArm SDK could not create a robot instance. Verify that xarm.dll is present and that the robot API service is reachable.";
                }
            }
            else if (!ConnectedFlag)
            {
                ret = SafeApiCall(() => XArmAPI.switch_xarm(arm), "switch_xarm");
                if (HasInteropFailure())
                    return false;

                if (ret == 0)
                {
                    ret = SafeApiCall(() => XArmAPI.robot_connect(IP), "robot_connect");
                    if (HasInteropFailure())
                        return false;

                    if (ret == 0)
                    {
                        xArmCreatedFlag = true;
                        ConnectedFlag = InitializeConnectedArm();
                        if (!ConnectedFlag && string.IsNullOrWhiteSpace(lastFailureMessage))
                            lastFailureMessage = "The xArm SDK reconnected to the robot but initialization failed.";
                    }
                    else
                        SetApiFailure("Robot network connection", ret);
                }
                else
                    SetApiFailure("xArm SDK switch instance", ret);
            }

            return xArmCreatedFlag && ConnectedFlag;
        }

        public string GetLastFailureMessage()
        {
            if (!string.IsNullOrWhiteSpace(lastFailureMessage))
                return lastFailureMessage;

            return "Unknown robot connection failure.";
        }

        public string GetVersion()
        {
            string s = "Test";
            byte[] version = new byte[40];
            int ret = SafeApiCall(() => XArmAPI.get_version(version), "get_version");
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

            int ret = SafeApiCall(() => XArmAPI.get_state(ref state), "get_state");
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

        public int StopCurrentMotion()
        {
            if (xArmCreatedFlag && ConnectedFlag)
                return SafeApiCall(() => XArmAPI.set_state(4));
            return -1;
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

        public int PrepareCartesianVelocityMotion()
        {
            int ret = -1;
            if (xArmCreatedFlag && ConnectedFlag)
            {
                ret = SafeApiCall(() => XArmAPI.set_mode(5));
                if (ret == 0)
                    ret = SafeApiCall(() => XArmAPI.set_state(0));
                if (ret == 0)
                    SafeApiCall(() => XArmAPI.set_cartesian_velo_continuous(true));
            }
            return ret;
        }

        public void SetMotionScale(int percent)
        {
            int clampedPercent = Math.Max(1, Math.Min(100, percent));
            JointSpeed = clampedPercent;
            JointAcceleration = clampedPercent * 18.0F;
            LinearSpeed = clampedPercent * 2.0F;
            LinearAcceleration = clampedPercent * 80.0F;

            if (xArmCreatedFlag && ConnectedFlag)
            {
                SafeApiCall(() => XArmAPI.set_joint_maxacc(JointAcceleration));
                SafeApiCall(() => XArmAPI.set_tcp_maxacc(LinearAcceleration));
            }
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

        public int SetCartesianVelocity(float vx, float vy, float vz, float vrx = 0.0F, float vry = 0.0F, float vrz = 0.0F, bool isToolCoord = false, float duration = -1.0F)
        {
            float[] speeds = { vx, vy, vz, vrx, vry, vrz };
            return SafeApiCall(() => XArmAPI.vc_set_cartesian_velocity(speeds, isToolCoord, duration));
        }

        public int SetCartesianVelocityContinuous(bool onOff)
        {
            if (xArmCreatedFlag && ConnectedFlag)
                return SafeApiCall(() => XArmAPI.set_cartesian_velo_continuous(onOff));
            return -1;
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
