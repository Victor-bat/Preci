using System;
using System.Collections.Generic;
using System.Linq;
using PERICAN.Models;

namespace PERICAN.Controllers
{
    public class MessageDecoder
    {
        public static void DecodeMessages(List<CanMessageModel> ascMessages, List<DbcMessageModel> dbcMessages, List<DbcSignalModel> dbcSignals)
        {
            foreach (var ascMessage in ascMessages)
            {
                uint maskedAscCanId;
                if (ascMessage.IsExtended)
                {
                    // Masking the extended CAN ID for 29-bit CAN frames
                    maskedAscCanId = (uint)(ascMessage.CanId & 0x1FFFFFFF);
                }
                else
                {
                    // Masking the standard CAN ID for 11-bit CAN frames
                    maskedAscCanId = (uint)(ascMessage.CanId & 0x7FF);
                }

                // Find the DBC message corresponding to the ASC CAN ID
                var dbcMessage = dbcMessages.FirstOrDefault(m =>
                    m.CanId == maskedAscCanId &&
                    m.IsExtended == ascMessage.IsExtended);

                if (dbcMessage == null)
                {
                    // Log if no matching DBC message
                    LogMismatch(ascMessage, maskedAscCanId);
                    continue;
                }
                else
                {
                    // Log when matching DBC message is found
                    Console.WriteLine($"Matched DBC message for ASC ID: 0x{maskedAscCanId:X}");
                }

                // Retrieve associated signals
                var signals = dbcSignals.Where(s => s.MessageId == dbcMessage.CanId).ToList();

                // Log the number of signals and the signals found
                Console.WriteLine($"DBC Message {dbcMessage.MessageName} has {signals.Count} signals.");
                if (signals.Any())
                {
                    Console.WriteLine($"Signals for {dbcMessage.MessageName}: {string.Join(", ", signals.Select(s => s.SignalName))}");
                }

                if (!signals.Any())
                {
                    // Log if no signals are found for the message
                    Console.WriteLine($"No signals found for DBC message: {dbcMessage.MessageName} (ID: 0x{maskedAscCanId:X})");
                    continue;
                }

                byte[] dataBytes;
                try
                {
                    // Convert hex string to byte array
                    dataBytes = ConvertHexToByteArray(ascMessage.DataBytes);
                    Console.WriteLine($"Expected Data Length: {ascMessage.DataLength}, Actual Data Length: {dataBytes.Length}");
                    if (dataBytes.Length != ascMessage.DataLength)
                    {
                        Console.WriteLine($"Data length mismatch for message {dbcMessage.MessageName}: Expected {ascMessage.DataLength}, got {dataBytes.Length}");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting hex data for message {dbcMessage.MessageName}: {ex.Message}");
                    continue;
                }

                // Decode each signal in the DBC message
                foreach (var signal in signals)
                {
                    try
                    {
                        // Extract raw value from signal
                        double rawValue = ExtractRawSignalValue(dataBytes, signal.StartBit, signal.Length, signal.ByteOrder, signal.IsSigned);
                        Console.WriteLine($"Raw value extracted for signal {signal.SignalName}: {rawValue}");

                        // Scale the value using factor and offset
                        double scaledValue = (rawValue * signal.Factor) + signal.Offset;

                        // Log scaling results
                        if (scaledValue < signal.Min || scaledValue > signal.Max)
                        {
                            Console.WriteLine($"Warning: Signal {signal.SignalName} value {scaledValue} outside valid range [{signal.Min},{signal.Max}]");
                        }
                        else
                        {
                            Console.WriteLine($"Decoded signal {signal.SignalName}: {scaledValue}");
                        }

                        ascMessage.DecodedSignals[signal.SignalName] = scaledValue;
                    }
                    catch (Exception ex)
                    {
                        // Log error during signal decoding
                        Console.WriteLine($"Error decoding signal {signal.SignalName}: {ex.Message}");
                    }
                }
            }
        }

        private static void LogMismatch(CanMessageModel ascMessage, uint maskedAscCanId)
        {
            string idFormat = ascMessage.IsExtended ? "0x{0:X8}" : "0x{0:X3}";
            Console.WriteLine($"No matching DBC message for ASC ID: {string.Format(idFormat, maskedAscCanId)}");
            Console.WriteLine($"Original ASC ID: {string.Format(idFormat, ascMessage.CanId)}");
        }

        private static byte[] ConvertHexToByteArray(string hex)
        {
            hex = hex.Replace(" ", "").Replace("-", "").ToUpper();
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string must have an even number of characters");
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        public static double ExtractRawSignalValue(byte[] data, int startBit, int length, int byteOrder, bool isSigned)
        {
            if (startBit + length > data.Length * 8)
            {
                throw new ArgumentException("Signal extends beyond message data length");
            }

            ulong rawValue = 0;
            int remainingBits = length;
            int currentBit = startBit;

            if (byteOrder == 1) // Little-endian
            {
                while (remainingBits > 0)
                {
                    int byteIndex = currentBit / 8;
                    int bitIndex = currentBit % 8;
                    int bitsToRead = Math.Min(remainingBits, 8 - bitIndex);

                    byte mask = (byte)((1 << bitsToRead) - 1);
                    byte extractedBits = (byte)((data[byteIndex] >> bitIndex) & mask);

                    rawValue |= ((ulong)extractedBits << (length - remainingBits));

                    remainingBits -= bitsToRead;
                    currentBit += bitsToRead;
                }
            }
            else // Big-endian
            {
                while (remainingBits > 0)
                {
                    int byteIndex = (currentBit / 8);
                    int bitIndex = 7 - (currentBit % 8);
                    int bitsToRead = Math.Min(remainingBits, bitIndex + 1);

                    byte mask = (byte)((1 << bitsToRead) - 1);
                    byte extractedBits = (byte)((data[byteIndex] >> (bitIndex - bitsToRead + 1)) & mask);

                    rawValue = (rawValue << bitsToRead) | extractedBits;

                    remainingBits -= bitsToRead;
                    currentBit += bitsToRead;
                }
            }

            // Handle signed values
            if (isSigned && length < 64)
            {
                ulong signBit = 1UL << (length - 1);
                if ((rawValue & signBit) != 0)
                {
                    rawValue |= ~((1UL << length) - 1); // Sign extend
                }
            }

            return isSigned ? (double)(long)rawValue : (double)rawValue;
        }
    }
}
