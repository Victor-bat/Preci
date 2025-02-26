namespace MyWinForms
{
    partial class HomePage
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewAscii;
        private System.Windows.Forms.DataGridView dataGridViewDbc;
        private System.Windows.Forms.Panel spacerPanel; // Spacer panel

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
            this.dataGridViewDbc = new System.Windows.Forms.DataGridView();
            this.spacerPanel = new System.Windows.Forms.Panel(); // Initialize spacer panel

            // HomePage (UserControl) - Set to Dock Fill
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Padding = new System.Windows.Forms.Padding(30); // 30px spacing on all sides

            // ASCII Table DataGridView
            this.dataGridViewAscii.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewAscii.Width = (this.Width - 90) / 2; // Split width between both tables
            this.dataGridViewAscii.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewAscii.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Spacer Panel (To add space between tables)
            this.spacerPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.spacerPanel.Width = 30; // 30px spacing between tables

            // DBC Table DataGridView
            this.dataGridViewDbc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDbc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Add Controls in order
            this.Controls.Add(this.dataGridViewDbc);
            this.Controls.Add(this.spacerPanel); // Spacer goes between tables
            this.Controls.Add(this.dataGridViewAscii);

            // Resize Event Handler for Responsive Layout
            this.Resize += new System.EventHandler(this.HomePage_Resize);
        }

        private void HomePage_Resize(object sender, System.EventArgs e)
        {
            int availableWidth = this.Width - 90; // Account for padding and spacing
            this.dataGridViewAscii.Width = availableWidth / 2;
        }
    }
}
