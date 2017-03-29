using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using IniParser;
using IniParser.Model;

namespace Automation_Core.IO
{
    class ConfigOps
    {
        public static bool CreateDirectory(string Path)
        {
            bool directoryExists = true;
            string path = Properties.Settings.Default.Path;
            if (!Directory.Exists(Path))
            {
                directoryExists = false;
                Directory.CreateDirectory(Path);
            }
            return directoryExists;
        }
        public static bool CheckCoreDirectoryExistance()
        {
            CreateDirectory(Properties.Settings.Default.Path);
            CreateDirectory(Properties.Settings.Default.Path + "\\Config");
            CreateDirectory(Properties.Settings.Default.Path + "\\Logs");
            CreateDirectory(Properties.Settings.Default.Path + "\\Docs");
            CreateDirectory(Properties.Settings.Default.Path + "\\DB");
            CreateDirectory(Properties.Settings.Default.Path + "\\.atm");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\Panel");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\Server");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\Nodes");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\Irrigation");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\Lighting");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\HVAC");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\Learning");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\Relays");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\Nodes");
            CreateDirectory(Properties.Settings.Default.Path + "\\Config\\Users");
            return false;
        }
        public static void CheckCoreConfigExistance()
        {
            if (!File.Exists(Properties.Settings.Default.Path + "\\Config\\main.ini"))
                CreateMainConfig();
        }
        public static void LoadAllData()
        {
            Variables.logger.LogLine(1, LoadMainConfig());
            Variables.logger.LogLine(1, LoadUsers());
            Variables.logger.LogLine(1, LoadNodes());
            Variables.logger.LogLine(1, LoadRelay());
            Variables.logger.LogLine(1, LoadIrrigation());
            Variables.logger.LogLine(1, LoadLights());
        }
        static string LoadIrrigation()
        {
            Variables.logger.LogLine("Begin loading irrigation");
            int Count = 0;
            FileIniDataParser parser = new FileIniDataParser();
            IniData data;
            for (int x = 0; x < Variables.sprinklers.Length; x++)
            {
                if(File.Exists(Properties.Settings.Default.Path + "\\Config\\Irrigation\\" + x.ToString() + ".ini"))
                {
                    Count++;
                    data = parser.ReadFile(Properties.Settings.Default.Path + "\\Config\\Irrigation\\" + x.ToString() + ".ini");
                    Variables.sprinklers[x] = new Devices.Sprinkler();
                    Variables.sprinklers[x].ID = Convert.ToInt32(data["Irrigation"]["ID"]);
                    Variables.sprinklers[x].RelayID = Convert.ToInt32(data["Irrigation"]["RelayID"]);
                    Variables.sprinklers[x].CycleTime = Convert.ToInt32(data["Irrigation"]["CycleTime"]);
                    Variables.sprinklers[x].Desc = data["Irrigation"]["Description"];
                    Variables.sprinklers[x].Name = data["Irrigation"]["Name"];
                    Variables.sprinklers[x].Type = data["Irrigation"]["Type"];
                    Variables.sprinklers[x].TimeOn = data["Irrigation"]["TimeOn"];
                    Variables.sprinklers[x].Active = Convert.ToBoolean(data["Irrigation"]["Active"]);
                }
                else
                {
                    Variables.sprinklers[x] = null;
                }
            }
            return "Finished loading " + Count + " irrigation items.";
        }
        static string LoadNodes()
        {
            Variables.logger.LogLine("Begin loading nodes");
            int Count = 0;
            FileIniDataParser parser = new FileIniDataParser();
            IniData data;
            for (int x = 0; x < Variables.nodes.Length; x++)
            {
                if (File.Exists(Properties.Settings.Default.Path + "\\Config\\Nodes\\" + x.ToString() + ".ini"))
                {
                    Count++;
                    data = parser.ReadFile(Properties.Settings.Default.Path + "\\Config\\Nodes\\" + x.ToString() + ".ini");
                    Variables.nodes[x] = new Devices.Node();
                    Variables.nodes[x].ID = Convert.ToInt32(data["Node"]["ID"]);
                    Variables.nodes[x].IPAddress = data["Node"]["IPAddress"];
                    Variables.nodes[x].Type = data["Node"]["Type"];
                    Variables.nodes[x].Desc = data["Node"]["Description"];
                    Variables.nodes[x].Name = data["Node"]["Name"];
                    Variables.nodes[x].Active = Convert.ToBoolean(data["Node"]["Active"]);
                } else
                {
                    Variables.nodes[x] = null;
                }
            }
            return "Finished loading " + Count + " nodes.";
        }
        static string LoadRelay()
        {
            Variables.logger.LogLine("Begin loading relays");
            int Count = 0;
            FileIniDataParser parser = new FileIniDataParser();
            IniData data;
            for (int x = 0; x < Variables.nodes.Length; x++)
            {
                if (File.Exists(Properties.Settings.Default.Path + "\\Config\\Relays\\" + x.ToString() + ".ini"))
                {
                    Count++;
                    data = parser.ReadFile(Properties.Settings.Default.Path + "\\Config\\Relays\\" + x.ToString() + ".ini");
                    Variables.relays[x] = new Devices.Relay();
                    Variables.relays[x].ID = Convert.ToInt32(data["Relay"]["ID"]);
                    Variables.relays[x].ParentNodeID = Convert.ToInt32(data["Relay"]["ParentNodeID"]);
                    Variables.relays[x].ParentNodePort = Convert.ToInt32(data["Relay"]["ParentNodePort"]);
                    Variables.relays[x].Desc = data["Relay"]["Description"];
                    Variables.relays[x].Name = data["Relay"]["Name"];
                    Variables.relays[x].Active = Convert.ToBoolean(data["Relay"]["Active"]);
                } else
                {
                    Variables.relays[x] = null;
                }
            }
            return "Finished loading " + Count + " relays.";
        }
        static string LoadUsers()
        {
            Variables.logger.LogLine("Begin loading users");
            int Count = 0;
            FileIniDataParser parser = new FileIniDataParser();
            IniData data;
            for (int x = 0; x < Variables.users.Length; x++)
            {
                if (File.Exists(Properties.Settings.Default.Path + "\\Config\\Users\\" + x.ToString() + ".ini"))
                {
                    Count++;
                    data = parser.ReadFile(Properties.Settings.Default.Path + "\\Config\\Users\\" + x.ToString() + ".ini");
                    Variables.users[x] = new Devices.User();
                    Variables.users[x].ID = Convert.ToInt32(data["User"]["ID"]);
                    Variables.users[x].Desc = data["User"]["Description"];
                    Variables.users[x].Name = data["User"]["Name"];
                    Variables.users[x].Active = Convert.ToBoolean(data["User"]["Active"]);
                    Variables.users[x].PIN = data["User"]["PIN"];
                    Variables.users[x].PermLevel = Convert.ToInt32(data["User"]["PermLevel"]);
                }
                else
                {
                    Variables.users[x] = null;
                }
            }
            return "Finished loading " + Count + " users.";
        }
        static string LoadLights()
        {
            Variables.logger.LogLine("Begin loading lights");
            int Count = 0;
            FileIniDataParser parser = new FileIniDataParser();
            IniData data;
            for (int x = 0; x < Variables.lights.Length; x++)
            {
                if (File.Exists(Properties.Settings.Default.Path + "\\Config\\Lighting\\" + x.ToString() + ".ini"))
                {
                    Count++;
                    data = parser.ReadFile(Properties.Settings.Default.Path + "\\Config\\Lighting\\" + x.ToString() + ".ini");
                    Variables.lights[x] = new Devices.Light();
                    Variables.lights[x].ID = Convert.ToInt32(data["Light"]["ID"]);
                    Variables.lights[x].Room = data["Light"]["Room"];
                    Variables.lights[x].Type = data["Light"]["Type"];
                    Variables.lights[x].Desc = data["Light"]["Description"];
                    Variables.lights[x].Name = data["Light"]["Name"];
                    Variables.lights[x].ParentNodeID = Convert.ToInt32(data["Light"]["ParentNodeID"]);
                    Variables.lights[x].RelayID = Convert.ToInt32(data["Light"]["RelayID"]);
                    Variables.lights[x].ParentNodeChannel = data["Light"]["ParentNodeChannel"];
                    Variables.lights[x].Active = Convert.ToBoolean(data["Light"]["Active"]);
                } else
                {
                    Variables.lights[x] = null;
                }
            }
            return "Finished loading " + Count + " lights.";
        }
        static string LoadMainConfig()
        {
            Variables.logger.LogLine("Begin loading main config");
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(Properties.Settings.Default.Path + "\\Config\\main.ini");
            Variables.MediaPlayerEnabled = Convert.ToBoolean(data["Music"]["Enabled"]);
            Variables.LightingEnabled = Convert.ToBoolean(data["Lighting"]["Enabled"]);
            Variables.HVACEnabled = Convert.ToBoolean(data["HVAC"]["Enabled"]);
            Variables.LearningEnabled = Convert.ToBoolean(data["Learning"]["Enabled"]);
            Variables.IrrigationEnabled = Convert.ToBoolean(data["Irrigation"]["Enabled"]);
            Variables.MaxVolume = Convert.ToInt32(data["Music"]["MaxVolume"]);
            Variables.InternalMediaPlayerEnabled = Convert.ToBoolean(data["Music"]["InternalPlayerEnabled"]);
            Variables.InternalMediaPath = data["Music"]["InternalPlayerPath"];
            return "Finished loading main config";
        }
        public static void CreateMainConfig()
        {
            Variables.logger.LogLine("Begin creating main config");
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = new IniData();
            data["Music"]["Enabled"] = "true";
            data["Music"]["InternalPlayerEnabled"] = "true";
            data["Music"]["InternalPlayerPath"] = @"I:\Users\Kolby\OneDrive\PL Music\PLAYLIST";
            data["Music"]["MaxVolume"] = "75";
            data["Learning"]["Enbaled"] = "true";
            data["Irrigation"]["Enbaled"] = "true";
            data["Lighting"]["Enbaled"] = "true";
            data["HVAC"]["Enbaled"] = "true";
            parser.WriteFile(Properties.Settings.Default.Path + "\\Config\\main.ini", data);
            Variables.logger.LogLine("Writing config to file, main.ini");
        }
        public static string CreateIrrigationConfig()
        {
            //Find next available ID
            int AvailableID = 0;
            Variables.logger.LogLine("Looking for an available ID to create an irrigation item.");
            for (int x=0; x < Variables.sprinklers.Length; x++)
            {
                if (Variables.sprinklers[x] == null)
                {
                    Variables.logger.LogLine("Found ID " + x + " available for use");
                    AvailableID = x;
                    break;
                }
                if (x == Variables.sprinklers.Length - 1)
                {
                    return "Unable to create a new irrigation item, no IDs available.";
                }
            }
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = new IniData();
            data["Irrigation"]["ID"] = AvailableID.ToString();
            data["Irrigation"]["RelayID"] = "0";
            data["Irrigation"]["CycleTime"] = "30";
            data["Irrigation"]["Description"] = "A sprinkler";
            data["Irrigation"]["Name"] = "A Sprinkler";
            data["Irrigation"]["Type"] = "RELAY";
            data["Irrigation"]["TimeOn"] = "12:00";
            data["Irrigation"]["Active"] = "false";
            Variables.logger.LogLine("Writing data to file");
            parser.WriteFile(Properties.Settings.Default.Path + "\\Config\\Irrigation\\"+AvailableID.ToString()+".ini", data);
            Variables.logger.LogLine("Reloading all configs");
            LoadAllData();
            return "Sucessfully created the config file, please configure it, ID: " + AvailableID;
        }
        public static string CreateRelayConfig()
        {
            //Find next available ID
            int AvailableID = 0;
            Variables.logger.LogLine("Looking for an available ID to create a relay.");
            for (int x = 0; x < Variables.relays.Length; x++)
            {
                if (Variables.relays[x] == null)
                {
                    Variables.logger.LogLine("Found ID " + x + " available for use");
                    AvailableID = x;
                    break;
                }
                if (x == Variables.relays.Length - 1)
                {
                    return "Unable to create a new relay, no IDs available.";
                }
            }
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = new IniData();
            data["Relay"]["ID"] = AvailableID.ToString();
            data["Relay"]["ParentNodeID"] = "0";
            data["Relay"]["ParentNodePort"] = "0";
            data["Relay"]["Description"] = "A relay";
            data["Relay"]["Name"] = "A Relay";
            data["Relay"]["Active"] = "false";
            Variables.logger.LogLine("Writing data to file");
            parser.WriteFile(Properties.Settings.Default.Path + "\\Config\\Relays\\" + AvailableID.ToString() + ".ini", data);
            Variables.logger.LogLine("Reloading all configs");
            LoadAllData();
            return "Sucessfully created the config file, please configure it, ID: " + AvailableID;
        }
        public static string CreateNodeConfig()
        {
            //Find next available ID
            int AvailableID = 0;
            Variables.logger.LogLine("Looking for an available ID to create a node.");
            for (int x = 0; x < Variables.nodes.Length; x++)
            {
                if (Variables.nodes[x] == null)
                {
                    Variables.logger.LogLine("Found ID " + x + " available for use");
                    AvailableID = x;
                    break;
                }
                if (x == Variables.nodes.Length - 1)
                {
                    return "Unable to create a new node, no IDs available.";
                }
            }
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = new IniData();
            data["Node"]["ID"] = AvailableID.ToString();
            data["Node"]["IPAddress"] = "0.0.0.0";
            data["Node"]["Type"] = "ARDUINO";
            data["Node"]["Description"] = "A node";
            data["Node"]["Name"] = "A Node";
            data["Node"]["Active"] = "false";
            Variables.logger.LogLine("Writing data to file");
            parser.WriteFile(Properties.Settings.Default.Path + "\\Config\\Nodes\\" + AvailableID.ToString() + ".ini", data);
            Variables.logger.LogLine("Reloading all configs");
            LoadAllData();
            return "Sucessfully created the config file, please configure it, ID: " + AvailableID;
        }
        public static string CreateLightConfig()
        {
            //Find next available ID
            int AvailableID = 0;
            Variables.logger.LogLine("Looking for an available ID to create a light.");
            for (int x = 0; x < Variables.lights.Length; x++)
            {
                if (Variables.lights[x] == null)
                {
                    Variables.logger.LogLine("Found ID " + x + " available for use");
                    AvailableID = x;
                    break;
                }
                if (x == Variables.lights.Length - 1)
                {
                    return "Unable to create a new light, no IDs available.";
                }
            }
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = new IniData();
            data["Light"]["ID"] = AvailableID.ToString();
            data["Light"]["Room"] = "A room";
            data["Light"]["Type"] = "LUTRON";
            data["Light"]["Description"] = "A Light";
            data["Light"]["Name"] = "A Light";
            data["Light"]["ParentNodeID"] = "0";
            data["Light"]["RelayID"] = "0";
            data["Light"]["ParentNodeChannel"] = "0";
            data["Light"]["Active"] = "false";
            Variables.logger.LogLine("Writing data to file");
            parser.WriteFile(Properties.Settings.Default.Path + "\\Config\\Lighting\\" + AvailableID.ToString() + ".ini", data);
            Variables.logger.LogLine("Reloading all configs");
            LoadAllData();
            return "Sucessfully created the config file, please configure it, ID: " + AvailableID;
        }
        public static string CreateUserConfig()
        {
            //Find next available ID
            int AvailableID = 0;
            Variables.logger.LogLine("Looking for an available ID to create a user.");
            for (int x = 0; x < Variables.users.Length; x++)
            {
                if (Variables.users[x] == null)
                {
                    Variables.logger.LogLine("Found ID " + x + " available for use");
                    AvailableID = x;
                    break;
                }
                if (x == Variables.users.Length - 1)
                {
                    return "Unable to create a new user, no IDs available.";
                }
            }
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = new IniData();
            data["User"]["ID"] = AvailableID.ToString();
            data["User"]["Description"] = "A User";
            data["User"]["Name"] = "A User";
            data["User"]["Active"] = "false";
            data["User"]["PIN"] = "0000";
            data["User"]["PermLevel"] = "0";
            Variables.logger.LogLine("Writing data to file");
            parser.WriteFile(Properties.Settings.Default.Path + "\\Config\\Users\\" + AvailableID.ToString() + ".ini", data);
            Variables.logger.LogLine("Reloading all configs");
            LoadAllData();
            return "Sucessfully created the config file, please configure it, ID: " + AvailableID;
        }
    }
}
