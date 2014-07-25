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
        List<KeyValuePair<Label, TextBox>> attributesAndValues = new List<KeyValuePair<Label,TextBox>>();
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
                    log.Debug("<== Reading xml Dokument");
                    xmlD.getXmlDocucmentFromStream(stream, ref attributesAndValues);
                    log.Debug("<== Adding Labels and Textboxes:");
                    foreach (KeyValuePair <Label, TextBox> attributes in attributesAndValues)
                    {
                        this.flowLayoutPanel.Controls.Add(attributes.Key);
                        log.Trace("<== adding Label: ", attributes.Key);
                        this.flowLayoutPanel.Controls.Add(attributes.Value);
                        log.Trace("<== adding TextBox: ", attributes.Value);
                    }
                    this.flowLayoutPanel.Update();
                }
            }
        }
    }
}
