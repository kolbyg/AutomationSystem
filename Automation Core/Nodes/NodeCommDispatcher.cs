using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Nodes
{
    class NodeCommDispatcher
    {
        public static void SendNodeCommand(string DataToSend, int NodeID)
        {
            Variables.logger.LogLine(0, "Received command to send \"" + DataToSend + "\" to node ID " + NodeID.ToString());
            string NodeIP = Variables.nodes.First(item => item.ID == NodeID).IPAddress;
            string NodeType = Variables.nodes.First(item => item.ID == NodeID).Type;
            Variables.logger.LogLine(0, "Node ID " + NodeID.ToString() + "is at the address " + NodeIP + " and is type " + NodeType);
            switch (NodeType)
            {
                case "ARDUINOETH":
                    //send command over tcpip
                    break;
                case "ARDUINOTTY":

                    break;
            }
        }
    }
}
