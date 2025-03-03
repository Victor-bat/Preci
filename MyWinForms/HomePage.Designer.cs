namespace MyWinForms
{
    partial class HomePage
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewAscii;
        private System.Windows.Forms.DataGridView dataGridViewDbc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;

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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAscii)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDbc)).BeginInit();
            this.SuspendLayout();

            // Table Layout (Splits ASC & DBC tables)
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));

            // DataGridView for ASC (Left Side)
            this.dataGridViewAscii.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAscii.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAscii.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAscii.AllowUserToAddRows = false;
            this.dataGridViewAscii.AllowUserToDeleteRows = false;
            this.dataGridViewAscii.ReadOnly = true;


            // DataGridView for DBC (Right Side)
            this.dataGridViewDbc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDbc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDbc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewDbc.AllowUserToAddRows = false;
            this.dataGridViewDbc.AllowUserToDeleteRows = false;
            this.dataGridViewDbc.ReadOnly = true;



            // Add Controls to Layout
            this.tableLayoutPanel.Controls.Add(this.dataGridViewAscii, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.dataGridViewDbc, 1, 0);
            this.Controls.Add(this.tableLayoutPanel);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAscii)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDbc)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
