using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Control
{
    class Sprinkler
    {
        public static bool GetCurrentState(int SprinklerID)
        {
            return Variables.sprinklers[SprinklerID].State;
        }
        public static int SetState(int SprinklerID, bool State)
        {
            Variables.logger.LogLine(0, "Received command to set sprinkler ID " + SprinklerID + " to state " + State.ToString());
            int RelayID = Variables.sprinklers[SprinklerID].RelayID;
            Variables.logger.LogLine(0, "Sprinkler id " + SprinklerID + " is relay ID " + RelayID.ToString());
            bool CurrentState = GetCurrentState(SprinklerID);
            Variables.logger.LogLine(0, "Sprinkler ID " + SprinklerID + " current state is " + CurrentState.ToString());
            if (State == CurrentState)
            {

                Variables.logger.LogLine(0, "Command to set Sprinkler ID " + SprinklerID + " to state " + State.ToString() + " failed. The sprinkler is already in that state.");
                return 2;
            }
            Variables.logger.LogLine(0, "Dispatching command to relay ID" + RelayID + " to set its state to " + State.ToString());
            Relay.SetState(RelayID, State);
            return 0;
        }
    }
}
