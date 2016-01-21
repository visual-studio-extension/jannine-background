using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoBackground
{
    public partial class SettingControl : UserControl
    {
        public SettingControl()
        {
            InitializeComponent();
        }

        internal void Initialize()
        {
            if (string.IsNullOrEmpty(OptionPageGrid.ImagePath))
            {
                PathTextBox.Text = OptionPageGrid.ImagePath;
            }
        }

        internal OptionPageGrid OptionPageGrid { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Multiselect = false;
            OpenFileDialog.RestoreDirectory = true;
            OpenFileDialog.Title = "Select Background Image";
            OpenFileDialog.InitialDirectory = @"D:\";
            OpenFileDialog.Filter = "Image Files(*.png;*.jpg)|*.png;*.jpg";
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                OptionPageGrid.ImagePath = OpenFileDialog.FileName;
                PathTextBox.Text = OpenFileDialog.FileName;
            }
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            OptionPageGrid.ImagePath = "";
            PathTextBox.Text = "";
        }
    }
}
