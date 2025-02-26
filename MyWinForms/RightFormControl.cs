using System;
using System.Windows.Forms;

namespace MyWinForms
{
    public partial class RightFormControl : UserControl
    {
        public RightFormControl()
        {
            InitializeComponent();
            Label label = new Label();
            label.Text = "This is the Right Side Form";
            label.Dock = DockStyle.Top;
            this.Controls.Add(label);
        }
    }
}
