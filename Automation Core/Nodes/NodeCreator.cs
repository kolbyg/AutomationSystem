using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xml.Net;

namespace Automation_Core.Nodes
{
    class NodeCreator
    {
        public static string AddNode(string NodeType, string NodeID, string NodeName, string IP, string NodeDesc)
        {
            Variables.logger.LogLine("Begin Writing Node XML");
            using (XmlWriter writer = XmlWriter.Create(Properties.Settings.Default.Path + "\\Config\\Nodes\\" + NodeID + ".xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Node");
                    writer.WriteElementString("ID", NodeID);
                    writer.WriteElementString("Type", NodeType);
                    writer.WriteElementString("Name", NodeName);
                    writer.WriteElementString("IP", IP);
                writer.WriteElementString("Desc", NodeDesc);

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            return "Node Added, reload the config for changes to take effect";
        }
    }
}
