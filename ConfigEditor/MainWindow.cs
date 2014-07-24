using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

namespace ConfigEditor
{
    public partial class MainWindow : Form
    {
        Logger log = LogManager.GetCurrentClassLogger();
        xmlDocument xmlD;
        public MainWindow()
        {
            log.Debug("Initailize MainWindow");
            InitializeComponent();
            xmlD = new xmlDocument();
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.Stream stream = null;
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "config Files|*.config";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if((stream = openFileDialog.OpenFile()) != null)
                {
                    log.Debug("Reading xml Dokument");
                    richTextBoxLog.Clear();
                    xmlD.getXmlDocucmentFromStream(stream, ref richTextBoxLog);
                }
            }
        }
    }
}
