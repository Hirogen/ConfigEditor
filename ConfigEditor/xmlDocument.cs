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
    public class XMLDocument
    {
        private Stream xmlPath;
        private TreeView xmlDocumentAsTreeView = null;
        private TreeNode xmlNodeAsTreeNode = null;
        static Logger log = LogManager.GetCurrentClassLogger();
        private List<KeyValuePair<Label, TextBox>> attributesAndValues;

        /// <summary>
        /// Entrypoint
        /// </summary>
        /// <param name="stream">XML Filename</param>
        public XMLDocument(Stream stream)
        {   
            //treeview and treenode test
            xmlDocumentAsTreeView = new TreeView();
            xmlPath = stream;
            attributesAndValues = new List<KeyValuePair<Label, TextBox>>();
            GetXmlDocucmentFromStream();

        }

        public List<KeyValuePair<Label, TextBox>> AttributesAndValues
        { get { return attributesAndValues; } }

        private Stream XMLPath
        {
             get { return xmlPath; }
             set { xmlPath = value; }
        }

        private TreeNode XMLNodeAsTreeNode 
        { 
            get { return xmlNodeAsTreeNode; }
            set { xmlNodeAsTreeNode = value; }
        }

        public TreeView XMLDocumentAsTreeView
        { get { return xmlDocumentAsTreeView; } }

        //TODO Should return xmlDocument for further editing
        //TODO For testing purpose added ref to textboxes and lables
        private void GetXmlDocucmentFromStream()
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(XMLPath);
                foreach (XmlNode xmlNodeItem in xmldoc)
                {
                    if (xmlNodeItem.HasChildNodes)
                    {
                        log.Debug("Anzahl: " + xmlNodeItem.ChildNodes.Count);
                        //BUG: First XML Node is being ignored
                        //initial 
                        ExtractAttributesAndValuesFromXMLNode(xmlNodeItem);
                        XMLNodes(xmlNodeItem);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("=== XML Document couldn't be read", e);
            }
        }

        //TODO Better workflow, for xmlNode Reading
        private void XMLNodes(XmlNode xmlNodeList)
        {
            foreach (XmlNode xmlNodeItem in xmlNodeList)
            {
                //TODO XML-comments are needed later on, need to read them when writing stuff back to the xml
                if (!(xmlNodeItem.NodeType == XmlNodeType.Comment))
                {
                    //log Node Name
                    //TODO Generieren des GruppenBereiches
                    log.Debug("Name:" + xmlNodeItem.Name);
                    ExtractAttributesAndValuesFromXMLNode(xmlNodeItem);
                }
                //Are there any other Childnodes, if yes, well just use the same method again, problematic for big xml files? memory leak?
                if (xmlNodeItem.HasChildNodes)
                {
                    XMLNodes(xmlNodeItem);
                }
            }
        }

        /// <summary>
        /// Extracts Attributes from a given XmlNode Object and adds it as KeyValuePair to the KeyValuePair-List
        /// </summary>
        /// <param name="xmlNodeItem"></param>
        private void ExtractAttributesAndValuesFromXMLNode(XmlNode xmlNodeItem)
        {
            XmlAttributeCollection xmlac;
            KeyValuePair<Label, TextBox> kvp;
            if (xmlNodeItem.Attributes != null && xmlNodeItem.Attributes.Count > 0)
            {
                xmlNodeAsTreeNode = new TreeNode(xmlNodeItem.Name);
                xmlac = xmlNodeItem.Attributes;
                log.Debug("\nAttribute Count: " + xmlNodeItem.Attributes.Count);
                foreach (XmlAttribute xmlAttributAlpha in xmlac)
                {
                    Label tempLabel = new Label();
                    log.Trace("Creating Label");
                    tempLabel.Text = xmlAttributAlpha.Name;
                    TextBox tempTextBox = new TextBox();
                    log.Trace("Creating Textbox");
                    tempTextBox.Text = xmlAttributAlpha.Value;
                    log.Trace("Adding Text Value to textbox");

                    kvp = new KeyValuePair<Label, TextBox>(tempLabel, tempTextBox);
                    //attribute loggin
                    //TODO Attributname => Label, Attribut Value => Textbox + Inhalt
                    log.Debug("\tAttributname: " + xmlAttributAlpha.Name + "\tValue:" + xmlAttributAlpha.Value);
                    attributesAndValues.Add(kvp);
                }
            }
        }
    }
}
