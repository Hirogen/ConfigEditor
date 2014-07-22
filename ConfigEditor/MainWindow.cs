using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfigEditor
{
    public partial class MainWindow : Form
    {
        xmlDocument xmlD;
        public MainWindow()
        {
            InitializeComponent();
            xmlD = new xmlDocument();
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.Stream stream = null;
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Dispatcher config Files|*.config";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if((stream = openFileDialog.OpenFile()) != null)
                {
                    xmlD.getXmlDocucmentFromStream(stream, ref richTextBoxLog);
                }
            }
        }
    }
}
