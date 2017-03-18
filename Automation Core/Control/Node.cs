using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Control
{
    class Node
    {
       public static string InitConnection(int NodeID)
        {
            if(Variables.nodes[NodeID].Type == "LUTRON")
            {
                Variables.nodes[NodeID].TelnetConnection = Networking.TelnetComms.setupClient(Variables.nodes[NodeID].IPAddress, 23, "lutron", "integration");
                Variables.nodes[NodeID].TelnetConnected = true;
                return "Successfully setup lutron node with telnet.";
            }
            else
            {
                return "Invalid Node.";
            }
        }
    }
}
