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
        public static string ParseCommand(string Command)
        {
            Variables.logger.LogLine("Sending command to parser: " + Command);
            string[] splitCmd = Command.ToLower().Split(' ');
            string ParserResponse;
            if (splitCmd[0].StartsWith("$$"))
            {
                string UserPIN = splitCmd[0].Substring(2);
                Devices.User AuthUser = null; ;
                foreach (Devices.User user in Variables.users)
                {
                    if (user == null) continue;
                    if (UserPIN == user.PIN)
                    {
                        AuthUser = user;
                    }
                }
                if (AuthUser == null)
                {
                    Variables.logger.LogLine("Authentication check failed on user: " + UserPIN);
                    return "Authentication failed, command skipped";
                }
                Variables.logger.LogLine(AuthUser.Name + " successfully authenticated, attempting command execution");
                ParserResponse = DoParse(Command, AuthUser.PermLevel);
            }
            else
                ParserResponse = DoParse(Command, 7);
            Variables.logger.LogLine("Parser response: " + ParserResponse);
            return ParserResponse;
        }
        public static string ParseConsoleCommand(string Command)
        {
            Variables.logger.LogLine("Sending command to parser: " + Command);
            Variables.logger.LogLine("Command sent from console, authenticating with root level permissions");
            string ParserResponse = DoParse(Command, 0);
            Variables.logger.LogLine("Parser response: " + ParserResponse);
            return ParserResponse;
        }
        static string DoParse(string Command, int AuthLevel)
        {
            if (Command == null || Command == "")
                return "Blank or null command received, parse skipped.";
            //Split the command by spaces
            string[] cmd;
            if (Command.StartsWith("$"))
                cmd = Command.Substring(Command.IndexOf(" ") + 1).Split(' ');
            else
            cmd = Command.ToLower().Split(' ');
            //Dispatch auth level command list
            switch (AuthLevel)
            {
                case 0: //Console only
                    return CmdListLevel0(cmd);
                case 1: //low level commands, tests, debug commands
                    return CmdListLevel1(cmd);
                case 2: //full non debug controls, allowed to change most system settings
                    return CmdListLevel2(cmd);
                case 3: //full security control
                    return CmdListLevel3(cmd);
                case 4: //scheduling control, some security control
                    return CmdListLevel4(cmd);
                case 5: //full irrigation, lighting and music control
                    return CmdListLevel5(cmd);
                case 6: //basic user, music and lighting control allowed
                    return CmdListLevel6(cmd);
                case 7: //anonymous user, only auth checks and status reports
                    return CmdListLevel7(cmd);
            }
            return "invalid auth level, check logs";
        }
        static string CmdListLevel0(string[] cmd)
        {
            switch (cmd[0])
            {
                case "exit":
                    Environment.Exit(0);
                    break;
                case "create":
                    switch (cmd[1])
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
                case "reload":
                    IO.ConfigOps.LoadAllData();
                    break;
            }
            //Command not hit, try next list
            return CmdListLevel1(cmd);
        }
        static string CmdListLevel1(string[] cmd)
        {
            switch (cmd[0])
            {
                case "io":
                    if (cmd.Length <= 1)
                        return "derp";
                    switch (cmd[1])
                    {
                        case "lutroncontrol":
                            if (cmd.Length <= 2)
                                return "Usage:\nlutroncontrol setlight <LightID> <Value> - Set light to specified value\nlutroncontrol triggerbutton <DeviceID> <ButtonID>";
                            switch (cmd[2])
                            {
                                case "setlight":
                                    if (cmd.Length <= 4)
                                        return "Usage: setlight <lightID> <Value>";
                                    return Control.Lighting.ControlLutronDimmableLight(Convert.ToInt32(cmd[3]), cmd[4]);
                                case "triggerbutton":
                                    if (cmd.Length <= 5)
                                        return "Usage: triggerbutton <NodeID> <DeviceID> <ButtonID>";
                                    return Control.Lighting.TriggerLutronDeviceButton(cmd[3], cmd[4], cmd[5]);
                            }
                            break;
                    }
                    break;
            }
            //Command not hit, try next list
            return CmdListLevel2(cmd);
        }
        static string CmdListLevel2(string[] cmd)
        {
            switch (cmd[0])
            {
            }
            //Command not hit, try next list
            return CmdListLevel3(cmd);
        }
        static string CmdListLevel3(string[] cmd)
        {
            switch (cmd[0])
            {
                case "alarm":
                    if (cmd.Length <= 1)
                        return "alarm command requires more arguments";
                    switch (cmd[1])
                    {
                        case "silencealarm":
                            return Control.Security.DisengageAlarm("CONSOLE");
                    }
                    break;
            }
            //Command not hit, try next list
            return CmdListLevel4(cmd);
        }
        static string CmdListLevel4(string[] cmd)
        {
            switch (cmd[0])
            {
                case "alarm":
                    if (cmd.Length <= 1)
                        return "alarm command requires more arguments";
                    switch (cmd[1])
                    {
                        case "alarmnow":
                            return Control.Security.TriggerAlarm(cmd[2]);
                        case "disarm":
                            return Control.Security.DisengageAlarm(cmd[2]);
                    }
                    break;
            }
            //scheduling commands, to be made
            //Command not hit, try next list
            return CmdListLevel5(cmd);
        }
        static string CmdListLevel5(string[] cmd)
        {
            switch (cmd[0])
            {
                case "irrigation":
                    switch (cmd[1])
                    {
                        case "set":
                            if (cmd.Length <= 3)
                                return "sprinkler set requires more arguments";
                            Control.Sprinkler.SetState(Convert.ToInt32(cmd[2]), Convert.ToBoolean(cmd[3]));
                            break;
                    }
                    break;
                case "music":
                    if (cmd.Length <= 1)
                        return "music requires more arguments";
                    switch (cmd[1])
                    {
                        case "stop":
                            return Control.Media.Stop();
                        case "resetrand":
                            return Control.Media.ResetRandomization();
                        case "rescan":
                            return Control.Media.Rescan();
                        case "buildpl":
                            return "Playlist Rebuilt";
                    }
                    break;
                case "lighting":
                    if (cmd.Length <= 1)
                        return "Usage:\nlighting setlight <LightID> <Value> - Set light to specified value\nallon - All Lights On\nalloff - All Lights Off";
                    switch (cmd[1])
                    {
                        case "allon":
                            return Control.Lighting.AllOn();
                        case "alloff":
                            return Control.Lighting.AllOff();
                    }
                    break;
            }
            //Command not hit, try next list
            return CmdListLevel6(cmd);
        }
        static string CmdListLevel6(string[] cmd)
        {
            switch (cmd[0])
            {
                case "music":
                    if (cmd.Length <= 1)
                        return "music requires more arguments";
                    switch (cmd[1])
                    {
                        case "volume":
                            if (cmd.Length <= 2)
                                return "Volume is " + Control.Media.GetVol();
                            return Control.Media.SetVol(Convert.ToInt32(cmd[2]));
                        case "playlist":
                            if (cmd.Length <= 2)
                                return "playlist name required";
                            return Control.Media.LoadPL(cmd[2]);
                        case "volup":
                            return Control.Media.VolUp();
                        case "voldown":
                            return Control.Media.VolDown();
                        case "pause":
                            return Control.Media.Pause();
                        case "next":
                            return Control.Media.Next();
                        case "prev":
                            return Control.Media.Prev();
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
                    }
                    break;
                case "lighting":
                    if (cmd.Length <= 1)
                        return "Usage:\nlighting setlight <LightID> <Value> - Set light to specified value\nallon - All Lights On\nalloff - All Lights Off";
                    switch (cmd[1])
                    {
                        case "setlight":
                            if (cmd.Length <= 2)
                                return "Usage:\nsetlight <lightid> <setvalue>";
                            return Control.Lighting.ControlLight(cmd[2], cmd[3]);
                    }
                    break;
            }
            //Command not hit, try next list
            return CmdListLevel7(cmd);
        }
        static string CmdListLevel7(string[] cmd)
        {
            switch (cmd[0])
            {
                case "var":
                    switch (cmd[1])
                    {
                        case "volume":
                            return "Volume:" + Control.Media.GetVol();
                        case "song":
                            if(cmd.Length > 2)
                                return "Song:" + Control.Media.GetSong(Convert.ToInt32(cmd[2]));
                            else
                                return "Song:" + Control.Media.GetSong();
                        case "status":
                            return "Status:" + Control.System.GetStatus();
                        case "almstatus":
                            return "AlarmStatus:" + Control.Security.GetStatus();
                    }
                    break;
                case "auth":
                    return "Auth:" + Control.Security.CheckAuth(cmd[1]);
                case "keepalive":
                    return "Hello";
                case "alarm":
                    if (cmd.Length <= 1)
                        return "alarm command requires more arguments";
                    switch (cmd[1])
                    {
                        case "panic":
                            return Control.Security.TriggerAlarm("PANIC");
                    }
                    break;
            }
            //Command not hit after trying all auth levels
            return "Invalid command or permission denied";
        }
    }
}
