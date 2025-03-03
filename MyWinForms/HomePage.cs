using System;
using System.Data;
using System.Windows.Forms;
using MyWinForms.Models;
using System.Collections.Generic;

namespace MyWinForms
{
    public partial class HomePage : UserControl
    {
        private DataTable ascTable;
        private DataTable dbcTable;

        public HomePage()
        {
            InitializeComponent();
            InitializeTables();
        }

        // Initialize Tables
        private void InitializeTables()
        {
            // Initialize ASC Table (Left Side)
            ascTable = new DataTable();
            ascTable.Columns.Add("Timestamp");
            ascTable.Columns.Add("CAN ID");
            ascTable.Columns.Add("Direction");
            ascTable.Columns.Add("DLC");
            ascTable.Columns.Add("Extended");
            ascTable.Columns.Add("Data Bytes");

            dataGridViewAscii.DataSource = ascTable;

            // Initialize DBC Table (Right Side)
            dbcTable = new DataTable();
            dbcTable.Columns.Add("Message Name");
            dbcTable.Columns.Add("CAN ID");
            dbcTable.Columns.Add("DLC");
            dbcTable.Columns.Add("Signal Name");
            dbcTable.Columns.Add("Start Bit");
            dbcTable.Columns.Add("Length");
            dbcTable.Columns.Add("Factor");
            dbcTable.Columns.Add("Offset");
            dbcTable.Columns.Add("Unit");

            dataGridViewDbc.DataSource = dbcTable;
        }

        // Clears both tables
        public void ClearMessages()
        {
            ascTable.Rows.Clear();
            dbcTable.Rows.Clear();
        }

        // Add ASC message to left table
        public void AddExactCanMessage(CanMessageModel message)
        {
            ascTable.Rows.Add(
                message.Timestamp,
                message.CanId,
                message.Direction,
                message.DataLength,
                message.IsExtended ? "Yes" : "No",
                message.DataBytes
            );
        }

        // Display parsed DBC messages & signals in right table
        public void DisplayDbcMessages(List<DbcMessageModel> messages, List<DbcSignalModel> signals)
        {
            dbcTable.Rows.Clear(); // Clear previous DBC data

            foreach (var message in messages)
            {
                // Get signals for the current message
                var messageSignals = signals.FindAll(s => s.MessageId == message.CanId);

                foreach (var signal in messageSignals)
                {
                    dbcTable.Rows.Add(
                        message.MessageName,
                        message.CanId,
                        message.Dlc,
                        signal.SignalName,
                        signal.StartBit,
                        signal.Length,
                        signal.Factor,
                        signal.Offset,
                        signal.Unit
                    );
                }

                // If no signals, still add the message row
                if (messageSignals.Count == 0)
                {
                    dbcTable.Rows.Add(
                        message.MessageName,
                        message.CanId,
                        message.Dlc,
                        "No Signals",
                        "-", "-", "-", "-", "-"
                    );
                }
            }
        }
    }
}
