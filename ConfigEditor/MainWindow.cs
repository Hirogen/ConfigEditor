using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NLog;

namespace ConfigEditor
{
    public partial class MainWindow : Form
    {
        List<KeyValuePair<Label, TextBox>> _attributesAndValues = new List<KeyValuePair<Label,TextBox>>();
        Logger log = LogManager.GetCurrentClassLogger();
        private XMLDocument _xmlD;
        public MainWindow()
        {
            log.Debug("Initailize MainWindow");
            InitializeComponent();
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

                if((stream = openFileDialog.OpenFile())!= null)
                {
                    log.Debug("<== Reading xml Dokument");
                    _xmlD = new XMLDocument(stream);
                    _attributesAndValues = _xmlD.AttributesAndValues;
                    log.Debug("<== Adding Labels and Textboxes:");
                    foreach (KeyValuePair <Label, TextBox> attributes in _attributesAndValues)
                    {
                        this.flowLayoutPanel.Controls.Add(attributes.Key);
                        log.Trace("<== adding Label: ", attributes.Key.Text);
                        this.flowLayoutPanel.Controls.Add(attributes.Value);
                        log.Trace("<== adding TextBox: ", attributes.Value.Text);
                    }
                    this.flowLayoutPanel.Update();
                }
            }
        }
    }
}
