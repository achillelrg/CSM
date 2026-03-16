using System;
using System.ComponentModel;

namespace RS232_PC.Models
{
    public sealed class MeasurementSession
    {
        public BindingList<MeasurementSample> Samples { get; } = new BindingList<MeasurementSample>();

        public string CurrentMode { get; private set; } = "Idle";

        public DateTime? StartedAt { get; private set; }

        public int Count => Samples.Count;

        public void Begin(string mode)
        {
            Samples.Clear();
            CurrentMode = mode;
            StartedAt = DateTime.Now;
        }

        public void Add(MeasurementSample sample)
        {
            Samples.Add(sample);
        }

        public void Reset()
        {
            Samples.Clear();
            CurrentMode = "Idle";
            StartedAt = null;
        }
    }
}
