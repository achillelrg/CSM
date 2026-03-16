using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using RS232_PC.Models;

namespace RS232_PC.Services
{
    public sealed class CsvExportService
    {
        public void Export(string filePath, IEnumerable<MeasurementSample> samples)
        {
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("Timestamp,Mode,Force,Unit,FilteredForce,Setpoint,Error,DeltaZ,PositionZ,RawLine");
                foreach (var sample in samples)
                {
                    writer.WriteLine(string.Join(",",
                        Escape(sample.Timestamp.ToString("O", CultureInfo.InvariantCulture)),
                        Escape(sample.Mode),
                        FormatNumber(sample.Force),
                        Escape(sample.Unit),
                        FormatNullable(sample.FilteredForce),
                        FormatNullable(sample.Setpoint),
                        FormatNullable(sample.Error),
                        FormatNullable(sample.DeltaZ),
                        FormatNullable(sample.PositionZ),
                        Escape(sample.RawLine)));
                }
            }
        }

        private static string FormatNumber(double value)
        {
            return value.ToString("G17", CultureInfo.InvariantCulture);
        }

        private static string FormatNullable(double? value)
        {
            return value.HasValue ? FormatNumber(value.Value) : string.Empty;
        }

        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var needsQuotes = value.Contains(",") || value.Contains("\"") || value.Contains("\r") || value.Contains("\n");
            if (!needsQuotes)
            {
                return value;
            }

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
