using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyWinForms
{
    public partial class Form1 : Form
    {
        private bool isDarkMode = false;

        public Form1()
        {
            InitializeComponent();
            ApplyTheme(); // Apply initial theme
            LoadFileNavigation(); // Default view
        }

        // Clears Navigation Bar and Adds New Controls
        private void LoadNavigationControls(Control[] controls)
        {
            navBarPanel.Controls.Clear();
            navBarPanel.Controls.AddRange(controls);
        }

        // Load File Tab Navigation Buttons
        private void LoadFileNavigation()
        {
            LoadNavigationControls(new Control[] { uploadAsc, uploadDbc });
        }

        // Load Home Tab Navigation Buttons
        private void LoadHomeNavigation()
        {
            LoadNavigationControls(new Control[] { startButton, stopButton });
        }

        // Load Analysis Tab Navigation Buttons
        private void LoadAnalysisNavigation()
        {
            LoadNavigationControls(new Control[] { filterButton, sortButton });
        }

        // Apply Dark Mode or Light Mode
        private void ApplyTheme()
        {
            Color bgColor = isDarkMode ? Color.FromArgb(30, 30, 30) : Color.White;
            Color menuColor = isDarkMode ? Color.FromArgb(50, 50, 50) : Color.WhiteSmoke;
            Color textColor = isDarkMode ? Color.White : Color.Black;
            Color navColor = isDarkMode ? Color.FromArgb(60, 60, 60) : Color.LightGray;

            this.BackColor = bgColor;
            menuStrip1.BackColor = menuColor;
            menuStrip1.ForeColor = textColor;
            navBarPanel.BackColor = navColor;

            // Update Button Styles
            foreach (Control control in navBarPanel.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = isDarkMode ? Color.DimGray : Color.LightBlue;
                    button.ForeColor = textColor;
                    button.FlatStyle = FlatStyle.Flat;
                }
            }

            themeToolStripMenuItem.Text = isDarkMode ? "Light Mode ☀️" : "Dark Mode 🌙";
        }

        // Toggle Dark Mode
        private void themeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;
            ApplyTheme();
        }

        // Event Handlers for Menu Clicks
        private void fileToolStripMenuItem_Click(object sender, EventArgs e) => LoadFileNavigation();
        private void homeToolStripMenuItem_Click(object sender, EventArgs e) => LoadHomeNavigation();
        private void analysisToolStripMenuItem_Click(object sender, EventArgs e) => LoadAnalysisNavigation();

        // File Upload Handlers
        private void uploadAsc_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "ASC Files (*.asc)|*.asc";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("ASC File Uploaded: " + openFileDialog.FileName);
            }
        }

        private void uploadDbc_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "DBC Files (*.dbc)|*.dbc";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("DBC File Uploaded: " + openFileDialog.FileName);
            }
        }

        // Start & Stop Handlers
        private void startButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CAN Analysis Started!");
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CAN Analysis Stopped!");
        }

        // Analysis Handlers
        private void filterButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Filter Applied!");
        }

        private void sortButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorting Applied!");
        }
    }
}
