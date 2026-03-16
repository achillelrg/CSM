using System;

namespace RS232_PC.Models
{
    public sealed class ForceReading
    {
        public DateTime Timestamp { get; set; }

        public double Force { get; set; }

        public string Unit { get; set; } = string.Empty;

        public string RawLine { get; set; } = string.Empty;
    }
}
