using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using NetworksApi.TCP.SERVER;
using System.Threading;
using Automation_Core.Devices;

namespace Automation_Core
{
    class Program
    {
        //fucking test
        static Timer mainRefresh = new Timer(mainRefresh_Tick);
		static Timer telnetRead = new Timer(telnetRead_Tick);

        private static void telnetRead_Tick(object state)
        {
            for(int x = 0; x < 64; x++){
                if (Variables.nodes[x] != null)
                {
                    if (Variables.nodes[x].TelnetConnected)
                    {
                        string data = Networking.TelnetComms.ReadCommand(Variables.nodes[x].TelnetConnection);
                        if (data != null && data != "" && data != " ")
                            Variables.logger.LogLine(data);
                    }
                }
            }
        }

        private static void mainRefresh_Tick(object state)
        {
            //Variables.sysstate = 0;
            //Variables.envstate = 0;
            //Variables.alrmstate = 0;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Automation System Core Loading...");
            Console.WriteLine("Data directory is " + Properties.Settings.Default.Path);
            Console.WriteLine("Log directory is " + Properties.Settings.Default.LogPath);
            if (!Directory.Exists(Properties.Settings.Default.LogPath))
                Directory.CreateDirectory(Properties.Settings.Default.LogPath);
            Variables.logger = new Logger(Properties.Settings.Default.LogVerbosity, Properties.Settings.Default.LogPath);
            Variables.logger.LineLogged += logger_LineLogged;
            Console.WriteLine("Logging initialized");
            Variables.logger.LogLine(1,"Begin initialization of automation system");
            Variables.logger.LogLine(1, "===========================================");
            Variables.logger.LogLine(1, "Automation system, Created by Kolby Graham");
            Variables.logger.LogLine(1, "Copyright 2016 Kolby's Computers, All Rights Reserved.");
            Variables.logger.LogLine(1, "Automation version 0.2");
            Variables.logger.LogLine(1, "===========================================");
            Variables.logger.LogLine("Active Features:\nMusic Player\nLighting\nIrrigation");
            Variables.logger.LogLine("Begin Loading Configuration");
            IO.ConfigOps.CheckCoreDirectoryExistance();
            IO.ConfigOps.CheckCoreConfigExistance();
            IO.ConfigOps.LoadAllData();
            Variables.logger.LogLine("Begin Timer initialization");
            mainRefresh.Change(1000, 100);
            telnetRead.Change(2000, 5000);
            Networking.PanelComms.SetupPanelServer();
            Variables.logger.LogLine("Begin media initialization");
            Control.Media.InitPlayer();
            Variables.logger.LogLine("Begin node connections (this may take a while)");
            InitNodes();
            Variables.logger.LogLine(1, "Automation system initialization complete");
            Variables.logger.LogLine("Setup complete, launch interactive prompt");
            Prompt();
        }
        static void InitNodes()
        {
            for(int x=0; x < 64; x++)
            {
                if(Variables.nodes[x] != null)
                {
                    Control.Node.InitConnection(x);
                }
            }
        }
        static void logger_LineLogged(object sender, LineLoggedEventArgs e)
        {
            Console.WriteLine(e.Text);
        }
        static void Prompt()
        {
            Console.Write("ATM>");
            string input = Console.ReadLine();
            Variables.logger.LogLine(CommandParser.ParseCommand(input));
            Prompt();

        }
    }
}
