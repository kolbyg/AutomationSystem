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
        public static void SendNodeCommand(string DataToSend, int NodeID)
        {
            Variables.logger.LogLine(0, "Received command to send \"" + DataToSend + "\" to node ID " + NodeID.ToString());
            string NodeIP = Variables.nodes[NodeID].IPAddress;
            string NodeType = Variables.nodes[NodeID].Type;
            Variables.logger.LogLine(0, "Node ID " + NodeID.ToString() + "is at the address " + NodeIP + " and is type " + NodeType);
            switch (NodeType)
            {
                case "ARDUINOETH":
                    //send command over tcpip
                    break;
                case "ARDUINOTTY":
                    //send command over serial
                    break;
            }
        }
    }
}
