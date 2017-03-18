using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Automation_Core.Control
{
    class Lighting
    {
        public static string AllOn()
        {
            string resp = "";
            foreach(Devices.Light light in Variables.lights)
            {
                if (light == null) continue;
                resp += ControlLight(light.ID.ToString(), "100");
            }
            resp += "All lights set to on\n";
            return resp;
        }
        public static string AllOff()
        {
            string resp = "";
            foreach (Devices.Light light in Variables.lights)
            {
                if (light == null) continue;
                resp += ControlLight(light.ID.ToString(), "0");
            }
            resp += "All lights set to off\n";
            return resp;
        }
        public static string ControlLight(string LightID, string Value)
        {
            string resp = "";
            bool errorstate = false;
            try
            {
                resp += "Attempting to control light " + LightID + " and set its value to " + Value + "\n";
                Devices.Light light = Variables.lights[Convert.ToInt32(LightID)];
                switch (light.Type)
                {
                    case "LUTRON":
                        resp += "Issuing command to lutron module\n";
                        resp += ControlLutronDimmableLight(light.ID, Value);
                        break;
                    case "RELAY":
                        resp += "Issuing command to relay module\n";
                        if (Convert.ToInt32(Value) > 1) Value = "1";
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
        public static string ControlLutronDimmableLight(int LightID, string Value)
        {
            string resp = "";
            bool errorstate = false;
            try
            {
                resp += Networking.TelnetComms.SendCommand(Variables.nodes[Variables.lights[LightID].ParentNodeID].TelnetConnection, "#OUTPUT," + Variables.lights[LightID].ParentNodeChannel + ",1," + Value);
            }
            catch (Exception ex)
            {
                Variables.logger.LogLine(ex.Message);
                errorstate = true;
            }
            if (errorstate)
                resp += "Error: ControlLutronDimmableLight encountered an error, please review logs.\n";
                return resp;
        }
        public static string TriggerLutronDeviceButton(string NodeID, string DeviceID, string ButtonID)
        {
            string resp = "";
            bool errorstate = false;
            try
            {
                resp += Networking.TelnetComms.SendCommand(Variables.nodes[Convert.ToInt32(NodeID)].TelnetConnection, "#DEVICE," + DeviceID + "," + ButtonID + ",3");
                resp += Networking.TelnetComms.SendCommand(Variables.nodes[Convert.ToInt32(NodeID)].TelnetConnection, "#DEVICE," + DeviceID + "," + ButtonID + ",4");
            }
            catch (Exception ex)
            {
                Variables.logger.LogLine(ex.Message);
                errorstate = true;
            }
            if (errorstate)
                resp += "Error: TriggerLutronDeviceButton encountered an error, please review logs.\n";
                return resp;
        }
    }
}
