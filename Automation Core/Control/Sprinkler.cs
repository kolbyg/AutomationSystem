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
        public static string SetState(int SprinklerID, bool State)
        {
            string resp = "";
            bool errorstate = false;
            try
            {
                Variables.logger.LogLine(0, "Received command to set sprinkler ID " + SprinklerID + " to state " + State.ToString());
                Devices.Sprinkler sprinkler = Variables.sprinklers[Convert.ToInt32(SprinklerID)];
                if (State == sprinkler.State)
                {
                    return "Command to set Sprinkler ID " + SprinklerID + " to state " + State.ToString() + " failed. The sprinkler is already in that state.";
                }
                Variables.logger.LogLine(0, "Sprinkler ID " + SprinklerID + " current state is " + sprinkler.State.ToString());
                switch (sprinkler.Type)
                {
                    case "RELAY":
                        Variables.logger.LogLine(0, "Sprinkler id " + SprinklerID + " is relay ID " + sprinkler.RelayID.ToString());
                        resp += "Issuing command to relay module\n";
                        Variables.logger.LogLine(0, "Dispatching command to relay ID" + sprinkler.RelayID + " to set its state to " + State.ToString());
                        Control.Relay.SetState(sprinkler.RelayID, State);
                        break;
                }
            }
            catch (Exception ex)
            {
                Variables.logger.LogLine(ex.Message);
                errorstate = true;
            }
            if (errorstate)
                return "Error: ControlLight encountered an error, please review logs.\n";
            else
                return resp;
        }
    }
}
