using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using MyWinForms.Models;

namespace MyWinForms
{
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }

        // Load ASCII File Data into the Table
        public void LoadAsciiFile(string filePath)
        {
            try
            {
                DataTable asciiTable = new DataTable();
                asciiTable.Columns.Add("Character");
                asciiTable.Columns.Add("ASCII Value");

                string content = File.ReadAllText(filePath);
                foreach (char c in content)
                {
                    asciiTable.Rows.Add(c, (int)c);
                }

                dataGridViewAscii.DataSource = asciiTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading ASCII file: " + ex.Message);
            }
        }

        // Load DBC File Data into the Table
        public void LoadDbcFile(string filePath)
        {
            try
            {
                DataTable dbcTable = new DataTable();
                dbcTable.Columns.Add("Line Number");
                dbcTable.Columns.Add("Content");

                string[] lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    dbcTable.Rows.Add(i + 1, lines[i]);
                }

                dataGridViewDbc.DataSource = dbcTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading DBC file: " + ex.Message);
            }
        }
        public void ClearMessages()
        {
            if (dataGridViewAscii.InvokeRequired)
            {
                dataGridViewAscii.Invoke(new Action(ClearMessages));
                return;
            }

            dataGridViewAscii.Rows.Clear();
        }

        public void AddExactCanMessage(CanMessageModel message)
        {
            if (dataGridViewAscii.InvokeRequired)
            {
                dataGridViewAscii.Invoke(new Action(() => AddExactCanMessage(message)));
                return;
            }

            dataGridViewAscii.Rows.Add(
                message.Timestamp,      // Exact timestamp from ASC file
                message.CanId,          // Exact CAN ID from ASC file
                message.IsExtended,     // Whether it's an extended frame (true/false)
                message.Direction,      // "Rx" or "Tx" (received/transmitted)
                message.DataLength,     // Length of data bytes
                message.DataBytes       // Exact byte data from ASC file
            );
        }

    }
}
