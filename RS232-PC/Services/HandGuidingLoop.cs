using System;

namespace RS232_PC.Services
{
    public sealed class HandGuidingLoop
    {
        private bool _initialized;
        private double _filteredForce;
        private double _startZ;

        public double Alpha { get; set; } = 0.35;

        public double Deadband { get; set; } = 0.10;

        public double Gain { get; set; } = 0.80;

        public double MaxDeltaZPerTick { get; set; } = 0.50;

        public double ZWindow { get; set; } = 20.0;

        public double HardForceThreshold { get; set; } = 5.0;

        public double OutputSign { get; set; } = 1.0;

        public void Reset(double startZ)
        {
            _initialized = false;
            _filteredForce = 0.0;
            _startZ = startZ;
        }

        public HandGuidingResult Compute(double force, double currentZ)
        {
            var alpha = Clamp(Alpha, 0.0, 1.0);
            if (!_initialized)
            {
                _filteredForce = force;
                _initialized = true;
            }
            else
            {
                _filteredForce = (alpha * force) + ((1.0 - alpha) * _filteredForce);
            }

            var error = -_filteredForce;
            var deadband = Math.Max(0.0, Deadband);
            var magnitude = Math.Abs(_filteredForce);
            var usefulEffort = magnitude <= deadband
                ? 0.0
                : Math.Sign(_filteredForce) * (magnitude - deadband);
            var deltaZ = Clamp(OutputSign * Gain * usefulEffort, -MaxDeltaZPerTick, MaxDeltaZPerTick);

            string stopReason = string.Empty;
            if (Math.Abs(force) >= HardForceThreshold)
            {
                stopReason = "Force threshold exceeded.";
            }
            else if (Math.Abs(currentZ - _startZ) >= ZWindow)
            {
                stopReason = "Z safety window exceeded.";
            }

            return new HandGuidingResult
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

    public sealed class HandGuidingResult
    {
        public double FilteredForce { get; set; }

        public double Error { get; set; }

        public double DeltaZ { get; set; }

        public string StopReason { get; set; } = string.Empty;
    }
}
