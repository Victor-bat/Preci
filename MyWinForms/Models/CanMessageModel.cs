using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWinForms.Models
{
    public class CanMessageModel
    {
        public double Timestamp { get; set; }
        public int CanId { get; set; }
        public string Direction { get; set; } 
        public int DataLength { get; set; }
        public bool IsExtended { get; set; }
        public string DataBytes { get; set; }

        public Dictionary<string, double> DecodedSignals { get; set; } = new Dictionary<string, double>();

        public string DecodedSignalValues
        {
            get
            {
                return DecodedSignals != null && DecodedSignals.Count > 0
                    ? string.Join(", ", DecodedSignals.Select(kv => $"{kv.Key}: {kv.Value:F2}"))
                    : "No Signals";
            }
        }
        //public string DecodedSignalValues => string.Join(", ", DecodedSignals.Select(kv => $"{kv.Key}: {kv.Value}"));

    }
}
