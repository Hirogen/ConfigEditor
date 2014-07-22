using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace ConfigEditor
{
    class xmlDocument
    {

        private Stream stream = null;
        public xmlDocument()
        {

        }

        public Stream xmlPfad
        {
             get { return stream; }
             set { stream = value; }
        }

        public void getXmlDocucmentFromStream(Stream stream, ref RichTextBox richTextBoxLog)
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
                        richTextBoxLog.AppendText("Anzahl: " + xmlNodeItemAlpha.ChildNodes.Count + "\n");
                        richTextBoxLog.AppendText("---------------------------------------------------\n");
                        xmlnodes(xmlNodeItemAlpha, ref richTextBoxLog);
                    }
                }
                //Console.ReadKey();
            }
            catch (Exception e)
            {

                richTextBoxLog.AppendText(e.ToString());
                //Console.ReadKey();
            }
        }
         
         //
         //   

        private static void xmlnodes(XmlNode xmlNodeItemAlpha, ref RichTextBox rtx)
        {
            XmlAttributeCollection xmlac;
            //XmlNode xmln;
            foreach (XmlNode xmlNodeItemBeta in xmlNodeItemAlpha)
            {
                //TODO comments are needed later on, need to read them when writing stuff back to the xml
                if (!(xmlNodeItemBeta.NodeType == XmlNodeType.Comment))
                {
                    //Node Name ausgeben
                    //Console.WriteLine("Name:" + xmlNodeItemBeta.Name); // Generieren des GruppenBereiches
                    rtx.AppendText("---------------------------------------------------\n");
                    rtx.AppendText("Name:" + xmlNodeItemBeta.Name + "\n");
                    if (xmlNodeItemBeta.Attributes != null && xmlNodeItemBeta.Attributes.Count > 0)
                    {
                        xmlac = xmlNodeItemBeta.Attributes;
                        foreach (XmlAttribute xmlAttributAlpha in xmlac)
                        {
                            //Attribut zeugs ausgeben
                            //Attributname => Label, Attribut Value => Textbox + Inhalt
                            //Console.WriteLine("\tAttributname: " + xmlAttributAlpha.Name + "\tValue:" + xmlAttributAlpha.Value);
                            rtx.AppendText("---------------------------------------------------\n");
                            rtx.AppendText("\tAttributname: " + xmlAttributAlpha.Name + "\tValue:" + xmlAttributAlpha.Value + "\n");

                        }
                    }
                }
                //Sollte es weitere Childnodes geben, wird die statische Methode nochmal aufgerufen, damit auch diese ChildNodes verarbeitet werden können!
                if (xmlNodeItemBeta.HasChildNodes)
                {
                    xmlnodes(xmlNodeItemBeta, ref rtx);
                }
            }
        }
    }
}
