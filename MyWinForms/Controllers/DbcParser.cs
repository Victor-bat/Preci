using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using MyWinForms.Models;

namespace MyWinForms.Controllers
{
    public class DbcParser
    {
        public static List<DbcMessageModel> Messages = new List<DbcMessageModel>();
        public static List<DbcSignalModel> Signals = new List<DbcSignalModel>();

        private static uint ParseCanId(string idStr)
        {
            uint canId;

            // First, try parsing as decimal (standard DBC format)
            if (uint.TryParse(idStr, out canId))
            {
                uint maskedId = canId & 0xFFFFFFFE; // Mask the last bit
                Console.WriteLine($"🔹 Parsed CAN ID (Decimal): {canId} | Masked: {maskedId}");
                return maskedId;
            }

            // If that fails, try parsing as hex (if it's in hex format)
            if (idStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                idStr = idStr.Substring(2);
            }

            if (uint.TryParse(idStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out canId))
            {
                uint maskedId = canId & 0xFFFFFFFE; // Mask the last bit
                Console.WriteLine($"🔹 Parsed CAN ID (Hex): 0x{idStr.ToUpper()} | Decimal: {canId} | Masked: {maskedId}");
                return maskedId;
            }

            throw new ArgumentException($"❌ Invalid CAN ID format: {idStr}");
        }

        public static void ParseDbcFile(string filePath)
        {
            Messages.Clear();
            Signals.Clear();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    DbcMessageModel currentMessage = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            if (line.StartsWith("BO_"))
                            {
                                currentMessage = ParseMessageLine(line);
                                if (currentMessage != null)
                                {
                                    Messages.Add(currentMessage);
                                    LogMessageInfo(currentMessage);
                                }
                            }
                            else if (line.StartsWith(" SG_") && currentMessage != null)
                            {
                                var signal = ParseSignalLine(line, currentMessage);
                                if (signal != null)
                                {
                                    Signals.Add(signal);
                                    LogSignalInfo(signal);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"❌ Error parsing line: {line}");
                            Console.WriteLine($"   Error details: {ex.Message}");
                        }
                    }
                }

                ValidateParsingResults();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error parsing DBC file: {ex.Message}");
                throw;
            }
        }

        private static DbcMessageModel ParseMessageLine(string line)
        {
            var matches = Regex.Match(line, @"BO_\s+(\d+)\s+(\w+)\s*:\s*(\d+)\s+(\w+)");

            if (!matches.Success)
            {
                Console.WriteLine($"❌ Invalid message format: {line}");
                return null;
            }

            uint canId = ParseCanId(matches.Groups[1].Value);
            bool isExtended = canId > 0x7FF;

            return new DbcMessageModel
            {
                CanId = canId,
                IsExtended = isExtended,
                MessageName = matches.Groups[2].Value,
                Dlc = int.Parse(matches.Groups[3].Value)
            };
        }

        private static DbcSignalModel ParseSignalLine(string line, DbcMessageModel currentMessage)
        {
            // Updated regex pattern to handle more signal format variations
            var matches = Regex.Match(line, @"SG_\s+(\w+)\s*:\s*(\d+)\|(\d+)@([01])([+-])\s*\(([-+]?[\d.]+),([-+]?[\d.]+)\)\s*\[([-+]?[\d.]+)\|([-+]?[\d.]+)\]\s*""([^""]*)""");

            if (!matches.Success)
            {
                Console.WriteLine($"❌ Invalid signal format: {line}");
                return null;
            }

            try
            {
                return new DbcSignalModel
                {
                    SignalName = matches.Groups[1].Value,
                    MessageId = currentMessage.CanId,
                    StartBit = int.Parse(matches.Groups[2].Value),
                    Length = int.Parse(matches.Groups[3].Value),
                    ByteOrder = matches.Groups[4].Value == "1" ? 1 : 0,
                    IsSigned = matches.Groups[5].Value == "-",
                    Factor = double.Parse(matches.Groups[6].Value, CultureInfo.InvariantCulture),
                    Offset = double.Parse(matches.Groups[7].Value, CultureInfo.InvariantCulture),
                    Min = double.Parse(matches.Groups[8].Value, CultureInfo.InvariantCulture),
                    Max = double.Parse(matches.Groups[9].Value, CultureInfo.InvariantCulture),
                    Unit = matches.Groups[10].Value,
                    Receiver = currentMessage.MessageName
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error parsing signal values: {ex.Message}");
                return null;
            }
        }

        private static void LogMessageInfo(DbcMessageModel message)
        {
            Console.WriteLine($"📌 Loaded DBC Message: {message.MessageName}");
            Console.WriteLine($"   ID: 0x{message.CanId:X} (Decimal: {message.CanId})");
            Console.WriteLine($"   Extended: {message.IsExtended}");
            Console.WriteLine($"   DLC: {message.Dlc}");
        }

        private static void LogSignalInfo(DbcSignalModel signal)
        {
            Console.WriteLine($"✅ Loaded Signal: {signal.SignalName}");
            Console.WriteLine($"   Message: {signal.Receiver}");
            Console.WriteLine($"   Start Bit: {signal.StartBit}, Length: {signal.Length}");
            Console.WriteLine($"   Byte Order: {(signal.ByteOrder == 1 ? "Intel" : "Motorola")}");
            Console.WriteLine($"   Scaling: Factor={signal.Factor}, Offset={signal.Offset}");
        }

        private static void ValidateParsingResults()
        {
            if (Messages.Count == 0)
            {
                Console.WriteLine("❌ No messages found in DBC file.");
            }
            else
            {
                Console.WriteLine($"✅ Successfully parsed {Messages.Count} messages.");
            }

            if (Signals.Count == 0)
            {
                Console.WriteLine("❌ No signals found in DBC file.");
            }
            else
            {
                Console.WriteLine($"✅ Successfully parsed {Signals.Count} signals.");
            }
        }
    }
}
