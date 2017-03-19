using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Control
{
    class Relay
    {
        public static void SetState(int RelayID, bool State)
        {
            Variables.logger.LogLine(0, "Received command to set relay ID " + RelayID.ToString() + " to state " + State.ToString());
            //get the node info based on the relay id
            int NodeID = Variables.relays[RelayID].ParentNodeID;
            int NodePort = Variables.relays[RelayID].ParentNodePort;
            Variables.logger.LogLine(0, "Relay ID " + RelayID.ToString() + " parent node ID is " + NodeID.ToString() + " and node port is " + NodePort.ToString());
            Variables.logger.LogLine(0, "Command to set relay ID " + RelayID.ToString() + " to state " + State.ToString() + " has been sent to the node command dispatcher.");
            //send the command to the node controller R for relay, port #, and state example R21:1 would turn relay 21 on
            Control.Node.SendNodeCommand("R" + NodePort + ":" + State, NodeID);
        }
    }
}
