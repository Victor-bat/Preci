using MyWinForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWinForms.Controllers
{
    public static class MessageDecoder
    {
        public static string DecodeMessage(CanMessageModel message, List<DbcMessageModel> dbcMessages, List<DbcSignalModel> dbcSignals)
        {
            // Find matching DBC message using CAN ID
            var dbcMessage = dbcMessages.FirstOrDefault(m => m.CanId == message.CanId);
            if (dbcMessage == null)
            {
                return "No matching DBC message";
            }

            List<string> decodedValues = new List<string>();

            // Get all signals associated with this DBC message
            var signals = dbcSignals.Where(s => s.MessageId == dbcMessage.CanId).ToList();

            foreach (var signal in signals)
            {
                try
                {
                    double value = ExtractSignalValue(message.DataBytes, signal);
                    decodedValues.Add($"{signal.SignalName}: {value}{signal.Unit}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error decoding signal {signal.SignalName}: {ex.Message}");
                }
            }

            return decodedValues.Count > 0 ? string.Join(", ", decodedValues) : "No signals decoded";
        }

        private static double ExtractSignalValue(string dataBytes, DbcSignalModel signal)
        {
            string[] byteArray = dataBytes.Split(' ');
            int byteIndex = signal.StartBit / 8;
            if (byteIndex >= byteArray.Length)
            {
                throw new Exception("Start bit out of range");
            }

            int rawValue = Convert.ToInt32(byteArray[byteIndex], 16);
            double physicalValue = (rawValue * signal.Factor) + signal.Offset;

            return Math.Max(signal.Min, Math.Min(physicalValue, signal.Max));
        }
    }
}
