using System;
using System.Windows.Forms;

namespace MyWinForms
{
    public partial class LeftFormControl : UserControl
    {
        public LeftFormControl()
        {
            InitializeComponent();
            Label label = new Label();
            label.Text = "This is the Left Side Form";
            label.Dock = DockStyle.Top;
            this.Controls.Add(label);
        }
    }
}
