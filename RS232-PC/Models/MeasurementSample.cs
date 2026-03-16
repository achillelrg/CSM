using System;

namespace RS232_PC.Models
{
    public sealed class MeasurementSample
    {
        public DateTime Timestamp { get; set; }

        public string Mode { get; set; } = string.Empty;

        public double Force { get; set; }

        public string Unit { get; set; } = string.Empty;

        public double? FilteredForce { get; set; }

        public double? Setpoint { get; set; }

        public double? Error { get; set; }

        public double? DeltaZ { get; set; }

        public double? PositionZ { get; set; }

        public string RawLine { get; set; } = string.Empty;
    }
}
