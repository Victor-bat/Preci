using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWinForms.Models
{
    public class DbcMessageModel
    {
        public uint CanId { get; set; }
        public bool IsExtended { get; set; }
        public string MessageName { get; set; }
        public int Dlc { get; set; }
    }
    public class DbcSignalModel
    {
        public uint MessageId { get; set; }
        public string SignalName { get; set; }
        public int StartBit { get; set; }
        public int Length { get; set; }
        public int ByteOrder { get; set; }
        public bool IsSigned { get; set; }
        public double Factor { get; set; }
        public double Offset { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public string Unit { get; set; }
        public string Receiver { get; set; }
    }
}
