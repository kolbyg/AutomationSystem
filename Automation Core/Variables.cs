using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation_Core.Devices;

namespace Automation_Core
{
    class Variables
    {
        public static Logger logger;

        public static Media.MusicPlayer musicPlayer;
        public static string[] musicFiles;
        public static int curMusicFile;

        //public static List<IOSensor> ioSensors;
        //public static List<DSensor> dSensors;

        //GENERAL CONFIG
        public static bool MediaPlayerEnabled;
        public static bool LightingEnabled;
        public static bool IrrigationEnabled;
        public static bool LearningEnabled;
        public static bool HVACEnabled;

        public static string MediaPlayerType;
        public static int MaxVolume;
        public static string MediaPath;

        public static string PanelServerIP = "0.0.0.0";
        public static string PanelServerPort = "5400";

        public static Sprinkler[] sprinklers = new Sprinkler[256];
        public static Relay[] relays = new Relay[1024];
        public static Node[] nodes = new Node[64];
        public static Light[] lights = new Light[256];
        public static User[] users = new User[64];

        //State lists
        public static string[] sysStates = { "Ready", "Warning", "Error", "Power Fail", "ALARM" };
        public static string[] almStates = { "Ready", "Motion", "Arming", "Error", "ALARM" };
        //States of systems
        public static int mpdstate = 0;
        public static int envstate = 0;
        public static int sysstate = 0;
        public static int alrmstate = 0;

        //Power Draw
        public static int lightingdraw = 0;
        
    }
}
