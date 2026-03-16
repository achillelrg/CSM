using System;

namespace RS232_PC.Services
{
    public sealed class ForceControlLoop
    {
        private bool _initialized;
        private double _filteredForce;
        private double _previousFilteredForce;
        private double _integral;
        private double _startZ;

        public double Alpha { get; set; } = 0.35;

        public double Kp { get; set; } = 0.25;

        public double Ki { get; set; } = 0.05;

        public double Kd { get; set; } = 0.02;

        public double IntegralLimit { get; set; } = 100.0;

        public double Setpoint { get; set; }

        public double MaxDeltaZPerTick { get; set; } = 0.5;

        public double ZWindow { get; set; } = 10.0;

        public double HardForceThreshold { get; set; } = 50.0;

        public double OutputSign { get; set; } = 1.0;

        public void Reset(double startZ)
        {
            _initialized = false;
            _filteredForce = 0.0;
            _previousFilteredForce = 0.0;
            _integral = 0.0;
            _startZ = startZ;
        }

        public ForceControlResult Compute(double force, double currentZ, double dtSeconds)
        {
            var alpha = Clamp(Alpha, 0.0, 1.0);
            if (!_initialized)
            {
                _filteredForce = force;
                _previousFilteredForce = force;
                _initialized = true;
            }
            else
            {
                _filteredForce = (alpha * force) + ((1.0 - alpha) * _filteredForce);
            }

            var error = Setpoint - _filteredForce;
            _integral += error * Math.Max(dtSeconds, 1e-3);
            _integral = Clamp(_integral, -IntegralLimit, IntegralLimit);

            var derivative = -(_filteredForce - _previousFilteredForce) / Math.Max(dtSeconds, 1e-3);
            var output = OutputSign * ((Kp * error) + (Ki * _integral) + (Kd * derivative));
            var deltaZ = Clamp(output, -MaxDeltaZPerTick, MaxDeltaZPerTick);

            _previousFilteredForce = _filteredForce;

            string stopReason = string.Empty;
            if (Math.Abs(force) >= HardForceThreshold)
            {
                stopReason = "Force threshold exceeded.";
            }
            else if (Math.Abs(currentZ - _startZ) >= ZWindow)
            {
                stopReason = "Z safety window exceeded.";
            }

            return new ForceControlResult
            {
                FilteredForce = _filteredForce,
                Error = error,
                DeltaZ = deltaZ,
                StopReason = stopReason
            };
        }

        private static double Clamp(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }

    public sealed class ForceControlResult
    {
        public double FilteredForce { get; set; }

        public double Error { get; set; }

        public double DeltaZ { get; set; }

        public string StopReason { get; set; } = string.Empty;
    }
}
