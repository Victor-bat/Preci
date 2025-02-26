namespace MyWinForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel navBarPanel;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeToolStripMenuItem; // Dark mode toggle
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button uploadAsc;
        private System.Windows.Forms.Button uploadDbc;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button filterButton;
        private System.Windows.Forms.Button sortButton;

        private System.Windows.Forms.Panel contentPanel; // Panel to hold HomePage

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            homeToolStripMenuItem = new ToolStripMenuItem();
            analysisToolStripMenuItem = new ToolStripMenuItem();
            themeToolStripMenuItem = new ToolStripMenuItem(); // Theme toggle button
            navBarPanel = new Panel();
            openFileDialog = new OpenFileDialog();
            uploadAsc = new Button();
            uploadDbc = new Button();
            startButton = new Button();
            stopButton = new Button();
            filterButton = new Button();
            sortButton = new Button();
            menuStrip1.SuspendLayout();
            SuspendLayout();

            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill; // Make it fill Form1
            contentPanel.Location = new System.Drawing.Point(0, 88);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new System.Drawing.Size(500, 312);
            contentPanel.TabIndex = 2;

            // Add contentPanel to the Form
            Controls.Add(contentPanel);

            // MenuStrip
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, homeToolStripMenuItem, analysisToolStripMenuItem, themeToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(500, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.BackColor = Color.WhiteSmoke;

            // File Tab
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;

            // Home Tab
            homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            homeToolStripMenuItem.Size = new Size(64, 24);
            homeToolStripMenuItem.Text = "Home";
            homeToolStripMenuItem.Click += homeToolStripMenuItem_Click;

            // Analysis Tab
            analysisToolStripMenuItem.Name = "analysisToolStripMenuItem";
            analysisToolStripMenuItem.Size = new Size(76, 24);
            analysisToolStripMenuItem.Text = "Analysis";
            analysisToolStripMenuItem.Click += analysisToolStripMenuItem_Click;

            // Theme Toggle Button (Dark Mode)
            themeToolStripMenuItem.Name = "themeToolStripMenuItem";
            themeToolStripMenuItem.Size = new Size(100, 24);
            themeToolStripMenuItem.Text = "Dark Mode 🌙";
            themeToolStripMenuItem.Click += themeToolStripMenuItem_Click;

            // Navigation Bar Panel
            navBarPanel.BackColor = Color.LightGray;
            navBarPanel.Dock = DockStyle.Top;
            navBarPanel.Location = new Point(0, 28);
            navBarPanel.Name = "navBarPanel";
            navBarPanel.Size = new Size(500, 60);
            navBarPanel.TabIndex = 0;

            // Upload ASC Button
            uploadAsc.Location = new Point(10, 15);
            uploadAsc.Name = "uploadAsc";
            uploadAsc.Size = new Size(120, 35);
            uploadAsc.TabIndex = 0;
            uploadAsc.Text = "Upload ASC";
            uploadAsc.BackColor = Color.LightBlue;
            uploadAsc.FlatStyle = FlatStyle.Flat;
            uploadAsc.Click += uploadAsc_Click;

            // Upload DBC Button
            uploadDbc.Location = new Point(140, 15);
            uploadDbc.Name = "uploadDbc";
            uploadDbc.Size = new Size(120, 35);
            uploadDbc.TabIndex = 0;
            uploadDbc.Text = "Upload DBC";
            uploadDbc.BackColor = Color.LightBlue;
            uploadDbc.FlatStyle = FlatStyle.Flat;
            uploadDbc.Click += uploadDbc_Click;

            // Start Button
            startButton.Location = new Point(10, 15);
            startButton.Name = "startButton";
            startButton.Size = new Size(100, 35);
            startButton.TabIndex = 0;
            startButton.Text = "Start";
            startButton.BackColor = Color.LightGreen;
            startButton.FlatStyle = FlatStyle.Flat;
            startButton.Click += startButton_Click;

            // Stop Button
            stopButton.Location = new Point(130, 15);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(100, 35);
            stopButton.TabIndex = 0;
            stopButton.Text = "Stop";
            stopButton.BackColor = Color.IndianRed;
            stopButton.FlatStyle = FlatStyle.Flat;
            stopButton.Click += stopButton_Click;

            // Filter Button
            filterButton.Location = new Point(10, 15);
            filterButton.Name = "filterButton";
            filterButton.Size = new Size(100, 35);
            filterButton.TabIndex = 0;
            filterButton.Text = "Filter";
            filterButton.BackColor = Color.Khaki;
            filterButton.FlatStyle = FlatStyle.Flat;
            filterButton.Click += filterButton_Click;

            // Sort Button
            sortButton.Location = new Point(130, 15);
            sortButton.Name = "sortButton";
            sortButton.Size = new Size(100, 35);
            sortButton.TabIndex = 0;
            sortButton.Text = "Sort";
            sortButton.BackColor = Color.Khaki;
            sortButton.FlatStyle = FlatStyle.Flat;
            sortButton.Click += sortButton_Click;

            // Form1
            ClientSize = new Size(500, 400);
            Controls.Add(navBarPanel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            BackColor = Color.White;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();


        }
    }
}
