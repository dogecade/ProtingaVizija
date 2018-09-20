using System;
using System.Windows.Forms;

namespace WindowsForms
{
    public partial class FormPopup : Form
    {
        public FormPopup()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
