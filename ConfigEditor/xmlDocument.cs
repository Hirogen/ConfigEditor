using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using NLog;

namespace ConfigEditor
{
    [System.Runtime.InteropServices.Guid("7FAD38C1-D8A0-4EA8-A9A9-C77D44704230")]
    public class XMLDocument
    {
        private TreeNode _xmlNodeAsTreeNode = null;
        private TreeView _xmlDocumentAsTreeView = null;
        static Logger log = LogManager.GetCurrentClassLogger();
        private List<KeyValuePair<Label, TextBox>> _attributesAndValues;
        private Stream stream;

        /// <summary>
        /// Entrypoint
        /// </summary>
        /// <param name="stream">XML Filename</param>
        public XMLDocument(Stream stream)
        {
            //treeview and treenode test
            _xmlDocumentAsTreeView = new TreeView();
            XmlPath = stream;
            _attributesAndValues = new List<KeyValuePair<Label, TextBox>>();
            GetXmlDocucmentFromStream();

        }

        public List<KeyValuePair<Label, TextBox>> AttributesAndValues
        { get { return _attributesAndValues; } }

        private Stream XmlPath { get; set; }

        private TreeNode XMLNodeAsTreeNode 
        { 
            get { return _xmlNodeAsTreeNode; }
            set { _xmlNodeAsTreeNode = value; }
        }

        public TreeView XmlDocumentAsTreeView { get; } = null;

        //TODO Should return xmlDocument for further editing
        //TODO For testing purpose added ref to textboxes and lables
        private void GetXmlDocucmentFromStream()
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                //Loading XML Document
                xmldoc.Load(XmlPath);
                foreach (XmlNode xmlNodeItem in xmldoc)
                {
                    if (xmlNodeItem.HasChildNodes)
                    {
                        log.Debug("Anzahl: " + xmlNodeItem.ChildNodes.Count);
                        //BUG: First XML Node is being ignored
                        //initial 
                        ExtractAttributesAndValuesFromXmlNode(xmlNodeItem);
                        XmlNodes(xmlNodeItem);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("=== XML Document couldn't be read", e);
            }
        }

        //TODO Better workflow, for xmlNode Reading
        private void XmlNodes(XmlNode xmlNodeList)
        {
            foreach (XmlNode xmlNodeItem in xmlNodeList)
            {
                //TODO XML-comments are needed later on, need to read them when writing stuff back to the xml
                if (!(xmlNodeItem.NodeType == XmlNodeType.Comment))
                {
                    //log Node Name
                    //TODO Generieren des GruppenBereiches
                    log.Debug("Name:" + xmlNodeItem.Name);
                    ExtractAttributesAndValuesFromXmlNode(xmlNodeItem);
                }
                //Are there any other Childnodes, if yes, well just use the same method again, problematic for big xml files? memory leak?
                if (xmlNodeItem.HasChildNodes)
                {
                    XmlNodes(xmlNodeItem);
                }
            }
        }

        /// <summary>
        /// Extracts Attributes from a given XmlNode Object and adds it as KeyValuePair to the KeyValuePair-List
        /// </summary>
        /// <param name="xmlNodeItem"></param>
        private void ExtractAttributesAndValuesFromXmlNode(XmlNode xmlNodeItem)
        {
            
            
            if (xmlNodeItem.Attributes != null && xmlNodeItem.Attributes.Count > 0)
            {
                
                _xmlNodeAsTreeNode = new TreeNode(xmlNodeItem.Name);
                XmlAttributeCollection xmlac = xmlNodeItem.Attributes;
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

                    KeyValuePair<Label, TextBox> kvp = new KeyValuePair<Label, TextBox>(tempLabel, tempTextBox);
                    //attribute loggin
                    //TODO Attributname => Label, Attribut Value => Textbox + Inhalt
                    log.Debug("\tAttributname: " + xmlAttributAlpha.Name + "\tValue:" + xmlAttributAlpha.Value);
                    _attributesAndValues.Add(kvp);
                }
            }
        }
    }
}
