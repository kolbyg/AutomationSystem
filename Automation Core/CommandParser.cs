using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation_Core.Devices;

namespace Automation_Core
{
    class CommandParser
    {
        public static void SendResponse(string Response)
        {
            Variables.logger.LogLine(Response);
        }
        public static string ParsePanelCommand(string command)
        {
            Variables.logger.LogLine("Panel command: " + command);
            string parserResponse = ParseCommand(command);
            Variables.logger.LogLine("Parser response to panel command: " + parserResponse);
            return parserResponse;
        }
        public static string ParseAuthenticatedCommand(string command)
        {
            Variables.logger.LogLine("Panel command: " + command);
            string parserResponse = ParseCommand(command);
            Variables.logger.LogLine("Parser response to panel command: " + parserResponse);
            return parserResponse;
        }
        public static string ParseCommand(string command)
        {
            if (command == "" || command == null)
            {
                return "Blank or null command received, parse skipped.";
            }
            string[] splitCmd = command.ToLower().Split(' ');
            int cmdLength = splitCmd.Length;
            switch (splitCmd[0])
            {
                case "exit":
                    Environment.Exit(0);
                    break;
                case "keepalive":
                    return "Echo";
                case "alarm":
                    if (cmdLength <= 1)
                        return "alarm command requires more arguments";
                    switch (splitCmd[1])
                    {
                        case "panic":
                            return Control.Security.TriggerAlarm("PANIC");
                        case "alarmnow":
                            return Control.Security.TriggerAlarm(splitCmd[2]);
                        case "silencealarm":
                            return Control.Security.DisengageAlarm("CONSOLE");
                        case "disarm":
                            return Control.Security.DisengageAlarm(splitCmd[2]);
                    }
                    break;
                case "server":
                    if (cmdLength <= 1)
                        return "server command requires more arguments";
                    switch (splitCmd[1])
                    {
                        case "setlogverbosity":
                            if (cmdLength <= 2)
                                return "server setlogverbosity requires more arguments";
                            Properties.Settings.Default.LogVerbosity = Convert.ToInt32(splitCmd[2]);
                            return "Logging verbosity set to " + splitCmd[2];
                    }
                    break;
                case "music":
                    if (cmdLength <= 1)
                        return "music requires more arguments";
                    switch (splitCmd[1])
                    {
                        case "volume":
                            if (cmdLength <= 2)
                                return "Volume is " + Control.Media.GetVol();
                            return Control.Media.SetVol(Convert.ToInt32(splitCmd[2]));
                        case "volup":
                            return Control.Media.VolUp();
                        case "voldown":
                            return Control.Media.VolDown();
                        case "pause":
                            return Control.Media.Pause();
                        case "stop":
                            return Control.Media.Stop();
                        case "next":
                            return Control.Media.Next();
                        case "prev":
                            return Control.Media.Prev();
                        case "resetrand":
                            return Control.Media.ResetRandomization();
                        case "shuffle":
                            return Control.Media.Shuffle();
                        case "repeat":
                            return "Media Set to Repeat";
                        case "repeatoff":
                            return "Media Set not to Repeat";
                        case "resume":
                            return Control.Media.Resume();
                        case "play":
                            return Control.Media.Play();
                        case "rescan":
                            return Control.Media.Rescan();
                        case "buildpl":
                            return "Playlist Rebuilt";
                    }
                    break;
                case "lighting":
                    if (cmdLength <= 1)
                        return "Usage:\nlighting setlight <LightID> <Value> - Set light to specified value\nallon - All Lights On\nalloff - All Lights Off";
                    switch (splitCmd[1])
                    {
                        case "setlight":
                            if (cmdLength <= 2)
                                return "Usage:\nsetlight <lightid> <setvalue>";
                            return Control.Lighting.ControlLight(splitCmd[2], splitCmd[3]);
                        case "allon":
                            return Control.Lighting.AllOn();
                        case "alloff":
                            return Control.Lighting.AllOff();
                    }
                    break;
                case "reload":
                    IO.ConfigOps.LoadAllData();
                    break;
                case "io":
                    if (cmdLength <= 1)
                        return "derp";
                    switch (splitCmd[1])
                    {
                        case "lutroncontrol":
                            if (cmdLength <= 2)
                                return "Usage:\nlutroncontrol setlight <LightID> <Value> - Set light to specified value\nlutroncontrol triggerbutton <DeviceID> <ButtonID>";
                            switch (splitCmd[2])
                            {
                                case "setlight":
                                    if (cmdLength <= 4)
                                        return "Usage: setlight <lightID> <Value>";
                                    return Control.Lighting.ControlLutronDimmableLight(Convert.ToInt32(splitCmd[3]), splitCmd[4]);
                                case "triggerbutton":
                                    if (cmdLength <= 5)
                                        return "Usage: triggerbutton <NodeID> <DeviceID> <ButtonID>";
                                    return Control.Lighting.TriggerLutronDeviceButton(splitCmd[3], splitCmd[4], splitCmd[5]);
                            }
                            break;
                    }
                    break;
                case "var":
                    switch (splitCmd[1])
                    {
                        case "volume":
                            return "Volume:" + Control.Media.GetVol();
                        case "song":
                            return "Song:" + Control.Media.GetSong();
                        case "status":
                            return "Status:" + Control.System.GetStatus();
                        case "almstatus":
                            return "AlarmStatus:" + Control.Security.GetStatus();
                    }
                    break;
                case "auth":
                    return "Auth:" + Control.Security.CheckAuth(splitCmd[1]);
                case "irrigation":
                        switch (splitCmd[1])
                        {
                        case "set":
                            if (cmdLength <= 3)
                                return "sprinkler set requires more arguments";
                            Control.Sprinkler.SetState(Convert.ToInt32(splitCmd[2]), Convert.ToBoolean(splitCmd[3]));
                            break;
                        }
                    break;
                case "create":
                    switch (splitCmd[1])
                    {
                        case "sprinkler":
                            return IO.ConfigOps.CreateIrrigationConfig();
                        case "node":
                            return IO.ConfigOps.CreateNodeConfig();
                        case "relay":
                            return IO.ConfigOps.CreateRelayConfig();
                        case "light":
                            return IO.ConfigOps.CreateLightConfig();
                        case "user":
                            return IO.ConfigOps.CreateUserConfig();
                    }
                    break;

            }
            return "Invalid Command";
        }
    }
}
