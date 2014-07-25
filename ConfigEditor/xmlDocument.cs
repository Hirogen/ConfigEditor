using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using NLog;

namespace ConfigEditor
{
    public class xmlDocument
    {
        private Stream stream = null;
        static Logger log = LogManager.GetCurrentClassLogger();

        public xmlDocument()
        {

        }

        public Stream xmlPfad
        {
             get { return stream; }
             set { stream = value; }
        }

        //TODO Should return xmlDocument for further editing
        //TODO For testing purpose added ref to textboxes and lables
        public void getXmlDocucmentFromStream(Stream stream, ref List<KeyValuePair<Label, TextBox>> attributesAndValues)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(stream);
                foreach (XmlNode xmlNodeItemAlpha in xmldoc)
                {
                    if (xmlNodeItemAlpha.HasChildNodes)
                    {
                        //Console.WriteLine("Anzahl: " + xmlNodeItemAlpha.ChildNodes.Count);
                        log.Debug("Anzahl: " + xmlNodeItemAlpha.ChildNodes.Count);
                        //richTextBoxLog.AppendText("Anzahl: " + xmlNodeItemAlpha.ChildNodes.Count + "\n");
                        //richTextBoxLog.AppendText("---------------------------------------------------\n");
                        //initial 
                        xmlnodes(xmlNodeItemAlpha, ref attributesAndValues);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("=== XML Document could'nt be read", e);
            }
        }

        //TODO Better workflow, for xmlNode Reading
        private static void xmlnodes(XmlNode xmlNodeItemAlpha, ref List<KeyValuePair<Label, TextBox>> attributesAndValues)
        {
            XmlAttributeCollection xmlac;
            KeyValuePair<Label, TextBox> kvp;
            //XmlNode xmln;
            foreach (XmlNode xmlNodeItemBeta in xmlNodeItemAlpha)
            {
                //TODO XML-comments are needed later on, need to read them when writing stuff back to the xml
                if (!(xmlNodeItemBeta.NodeType == XmlNodeType.Comment))
                {
                    //Node Name ausgeben
                    //TODO Generieren des GruppenBereiches
                    //rtx.AppendText("---------------------------------------------------\n");
                    log.Debug("Name:" + xmlNodeItemBeta.Name);
                    //rtx.AppendText("Name:" + xmlNodeItemBeta.Name + "\n");
                    if (xmlNodeItemBeta.Attributes != null && xmlNodeItemBeta.Attributes.Count > 0)
                    {
                        xmlac = xmlNodeItemBeta.Attributes;
                        log.Debug("\nAnzahl Attribute: " + xmlNodeItemBeta.Attributes.Count);
                        //rtx.AppendText("\nAnzahl Attribute: " + xmlNodeItemBeta.Attributes.Count + "\n");
                        foreach (XmlAttribute xmlAttributAlpha in xmlac)
                        {
                            Label tempLabel = new Label();
                            tempLabel.Text = xmlAttributAlpha.Name;
                            TextBox tempTextBox = new TextBox();
                            tempTextBox.Text = xmlAttributAlpha.Value;

                            kvp = new KeyValuePair<Label, TextBox>(tempLabel, tempTextBox);
                            //Attribut zeugs ausgeben
                            //TODO Attributname => Label, Attribut Value => Textbox + Inhalt
                            log.Debug("\tAttributname: " + xmlAttributAlpha.Name + "\tValue:" + xmlAttributAlpha.Value);
                            attributesAndValues.Add(kvp);
                            //rtx.AppendText("\tAttributname: " + xmlAttributAlpha.Name + "\tValue:" + xmlAttributAlpha.Value + "\n");

                        }
                    }
                }
                //Sollte es weitere Childnodes geben, wird die statische Methode nochmal aufgerufen, damit auch diese ChildNodes verarbeitet werden können!
                if (xmlNodeItemBeta.HasChildNodes)
                {
                    //Add /T for first Lvl and 2nd lvl append just a new one /T ?? only for testing, should not be added or done, string + string performance to slow
                    xmlnodes(xmlNodeItemBeta, ref attributesAndValues);
                }
            }
        }
    }
}
