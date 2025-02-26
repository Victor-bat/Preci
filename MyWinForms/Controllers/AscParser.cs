using PERICAN.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PERICAN.Controllers
{
    public class AscParser
    {
        public static async Task ParseAscFile(string filePath, Action<CanMessageModel> onMessageParsed)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    // Improved regex pattern to better match ASC format
                    var pattern = @"^(\d+\.?\d*)\s+\d+\s+([0-9A-Fa-f]+x?)\s+(Rx|Tx)\s+d\s+(\d+)\s+((?:[0-9A-Fa-f]{2}\s*)+)";

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var match = Regex.Match(line.Trim(), pattern);
                        if (match.Success)
                        {
                            try
                            {
                                string canIdStr = match.Groups[2].Value;
                                bool isExtended = canIdStr.EndsWith("x", StringComparison.OrdinalIgnoreCase);

                                // Remove the 'x' suffix before parsing
                                canIdStr = isExtended ? canIdStr.Substring(0, canIdStr.Length - 1) : canIdStr;

                                var message = new CanMessageModel
                                {
                                    Timestamp = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture),
                                    CanId = Convert.ToInt32(canIdStr, 16) & 0x1FFFFFFF,
                                    IsExtended = isExtended,
                                    Direction = match.Groups[3].Value,
                                    DataLength = int.Parse(match.Groups[4].Value),
                                    // Normalize the data bytes format (ensure consistent spacing)
                                    DataBytes = NormalizeDataBytes(match.Groups[5].Value)
                                };

                                // Validate the parsed message
                                if (ValidateMessage(message))
                                {
                                    onMessageParsed?.Invoke(message);
                                }
                                else
                                {
                                    Console.WriteLine($"Invalid message at timestamp {message.Timestamp}: {line}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error parsing line: {line}");
                                Console.WriteLine($"Error details: {ex.Message}");
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("date") && !line.StartsWith("base") && !line.StartsWith("//"))
                        {
                            // Log unmatched lines that aren't comments or metadata
                            Console.WriteLine($"Unmatched line: {line}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing ASC file: {ex.Message}");
                throw;
            }
        }

        private static string NormalizeDataBytes(string dataBytes)
        {
            // Remove extra spaces and ensure consistent formatting
            return string.Join(" ",
                Regex.Matches(dataBytes, @"[0-9A-Fa-f]{2}")
                    .Cast<Match>()
                    .Select(m => m.Value.ToUpper()));
        }

        private static bool ValidateMessage(CanMessageModel message)
        {
            // Count actual data bytes
            int actualBytes = message.DataBytes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;

            if (actualBytes != message.DataLength)
            {
                Console.WriteLine($"Data length mismatch: Expected {message.DataLength}, got {actualBytes} bytes");
                return false;
            }

            // Validate CAN ID range
            if (message.IsExtended)
            {
                if (message.CanId > 0x1FFFFFFF)
                {
                    Console.WriteLine($"Invalid extended CAN ID: 0x{message.CanId:X}");
                    return false;
                }
            }
            else
            {
                if (message.CanId > 0x7FF)
                {
                    Console.WriteLine($"Invalid standard CAN ID: 0x{message.CanId:X}");
                    return false;
                }
            }

            return true;
        }
    }
}
