namespace MyWinForms
{
    partial class HomePage
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewAscii;
        private System.Windows.Forms.DataGridView dataGridViewDbc;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridViewAscii = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAscii)).BeginInit();
            this.SuspendLayout();

            // HomePage (UserControl)
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Padding = new System.Windows.Forms.Padding(20); // Padding for spacing

            // DataGridView for CAN Messages
            this.dataGridViewAscii.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAscii.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAscii.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAscii.AllowUserToAddRows = false;  // Disable adding rows manually
            this.dataGridViewAscii.AllowUserToDeleteRows = false; // Prevent deletion
            this.dataGridViewAscii.ReadOnly = true; // Read-only table

            // Define columns for CAN messages (exact match with AscParser)
            this.dataGridViewAscii.Columns.Add("Timestamp", "Timestamp");
            this.dataGridViewAscii.Columns.Add("CanId", "CAN ID");
            this.dataGridViewAscii.Columns.Add("IsExtended", "Extended Frame");
            this.dataGridViewAscii.Columns.Add("Direction", "Direction");
            this.dataGridViewAscii.Columns.Add("DataLength", "Data Length");
            this.dataGridViewAscii.Columns.Add("DataBytes", "Data Bytes");

            // Add Controls
            this.Controls.Add(this.dataGridViewAscii);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAscii)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
