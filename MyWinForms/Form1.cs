using System;
using System.Drawing;
using System.Windows.Forms;
using MyWinForms.Controllers;
using MyWinForms.Models;

namespace MyWinForms
{
    public partial class Form1 : Form
    {
        private bool isDarkMode = false;
        private bool isParsing = false; // Controls whether ASC parsing should continue
        private HomePage homePage; // Store HomePage instance globally

        private string uploadedAscFilePath = "";
        private string uploadedDbcFilePath = "";
        private CancellationTokenSource cancellationTokenSource;




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
            contentPanel.Controls.Clear(); // Clear content when switching tabs
        }

        // Load Home Tab Navigation Buttons
        private void LoadHomeNavigation()
        {
            LoadNavigationControls(new Control[] { startButton, stopButton });
            DisplayHomePage();
        }

        // Load Analysis Tab Navigation Buttons
        private void LoadAnalysisNavigation()
        {
            LoadNavigationControls(new Control[] { filterButton, sortButton });
            contentPanel.Controls.Clear(); // Clear content when switching tabs
        }

        // Display HomePage inside Form1 (Instead of a separate window)

        private void DisplayHomePage()
        {
            if (homePage == null) // Only create a new HomePage if it doesn’t exist
            {
                homePage = new HomePage();
                homePage.Dock = DockStyle.Fill;
            }

            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(homePage);
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

        // Menu Click Handlers
        private void fileToolStripMenuItem_Click(object sender, EventArgs e) => LoadFileNavigation();
        private void homeToolStripMenuItem_Click(object sender, EventArgs e) => LoadHomeNavigation();
        private void analysisToolStripMenuItem_Click(object sender, EventArgs e) => LoadAnalysisNavigation();
        // File Upload Handlers
        private void uploadAsc_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "ASC Files (*.asc)|*.asc";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                uploadedAscFilePath = openFileDialog.FileName;
                MessageBox.Show("ASC File Uploaded: " + uploadedAscFilePath);
            }
        }

        private void uploadDbc_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "DBC Files (*.dbc)|*.dbc";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                uploadedDbcFilePath = openFileDialog.FileName;
                MessageBox.Show("DBC File Uploaded: " + uploadedDbcFilePath);
            }
        }

        // Start & Stop Handlers
        private async void startButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(uploadedAscFilePath))
            {
                MessageBox.Show("Please upload an ASC file first!");
                return;
            }
            if (string.IsNullOrEmpty(uploadedDbcFilePath))
            {
                MessageBox.Show("Please upload a DBC file first!");
                return;
            }

            isParsing = true; // Allow parsing when Start is clicked

            if (homePage == null)
            {
                homePage = new HomePage();
                homePage.Dock = DockStyle.Fill;
            }

            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(homePage);

            homePage.ClearMessages(); // Clear previous messages

            // ✅ Start parsing ASC file in a separate thread for immediate updates
            _ = Task.Run(async () =>
            {
                await AscParser.ParseAscFile(uploadedAscFilePath, message =>
                {
                    if (isParsing)
                    {
                        homePage.Invoke(new Action(() => homePage.AddExactCanMessage(message)));
                    }
                });
            });

            // ✅ Start parsing DBC file in a separate thread for real-time display
            _ = Task.Run(() =>
            {
                try
                {
                    DbcParser.ParseDbcFile(uploadedDbcFilePath);
                    homePage.Invoke(new Action(() => homePage.DisplayDbcMessages(DbcParser.Messages, DbcParser.Signals)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error parsing DBC file: " + ex.Message);
                }
            });

            MessageBox.Show("ASC & DBC File Parsing Started!");
        }




        private void stopButton_Click(object sender, EventArgs e)
        {
            isParsing = false; // Stop data updates
            MessageBox.Show("Stopped Data Display!");
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
