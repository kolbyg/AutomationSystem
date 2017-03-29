using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Automation_Core.Devices;

namespace Automation_Core
{
    /// <summary>
    /// Automation core system namespace definitions:
    /// 
    /// 
    /// Automation - All learning and prediction based classes go here
    /// 
    /// Control - These classes are in between any commands being issued and the actual communication classes
    /// All commands must talk through the 'control' namespace no command may directly communicate with any other namespaces
    /// Control can execute hardware communicating methods based on the variables in the specific device
    /// If a class in control needs to issue a command to another device outside it's own class, this command MUST also pass through control.
    /// 
    /// Crypto - Anything cryptography related goes here
    /// 
    /// Devices - These are variable holding classes used to define physical or virtual items
    /// 
    /// IO - Any disk related methods go in here, usually only accessed through the control class, some exceptions, see classes inside IO for details.
    /// 
    /// Media - Media related activities go here
    /// 
    /// Networking - Any network communication methods go here
    /// 
    /// Resources - Any binary files needed by the application
    /// 
    /// </summary>
    class Program
    {
        static Timer mainRefresh = new Timer(mainRefresh_Tick);
		static Timer telnetRead = new Timer(telnetRead_Tick);

        private static void telnetRead_Tick(object state)
        {
            //Attempt to read the telnet output of each telnet connected node.
            for (int x = 0; x < Variables.nodes.Length; x++)
            {
                if (Variables.nodes[x] == null)
                    continue;
                if (Variables.nodes[x].TelnetConnected)
                {
                    string data = Networking.TelnetComms.ReadCommand(Variables.nodes[x].TelnetConnection);
                    if (data != null && data != "" && data != " ")
                        Variables.logger.LogLine(data);
                }
            }
        }

        private static void mainRefresh_Tick(object state)
        {
            //check if status are still correct
        }

        static void Main(string[] args)
        {
            //Load everything and startup
            Console.WriteLine("Automation System Core Loading...");
            Console.WriteLine("Data directory is " + Properties.Settings.Default.Path);
            Console.WriteLine("Log directory is " + Properties.Settings.Default.LogPath);
            //check if logging directory exists, if not, create it
            if (!Directory.Exists(Properties.Settings.Default.LogPath))
                Directory.CreateDirectory(Properties.Settings.Default.LogPath);
            //setup the logging system, likely to change to a premade solution, rather then this custom class
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
            //these timer intervals are subject to change after actual tests
            mainRefresh.Change(1000, 100);
            telnetRead.Change(2000, 5000);
            //setup the websockets panel server
            Networking.PanelComms.SetupPanelServer();
            if (Variables.InternalMediaPlayerEnabled)
            {
                Variables.logger.LogLine("Begin internal media player initialization");
                Control.Media.InitInternalPlayer();
            }
            Variables.logger.LogLine("Begin node connections and initializations (this may take a while)");
            InitNodes();
            Variables.logger.LogLine(1, "Automation system initialization complete");
            Variables.logger.LogLine("Setup complete, launch interactive prompt");
            Prompt();
        }
        static void InitNodes()
        {
            //loop to iterate through all the configured nodes and connect to them
            for (int x = 0; x < Variables.nodes.Length; x++)
            {
                if (Variables.nodes[x] == null)
                    continue;
                Control.Node.InitConnection(x);
            }
        }
        static void logger_LineLogged(object sender, LineLoggedEventArgs e)
        {
            //Logging event to write to console
            Console.WriteLine(e.Text);
        }
        static void Prompt()
        {
            //start a prompt, there's probably a better way to do this....
            Console.Write("ATM>");
            Variables.logger.LogLine(CommandParser.ParseConsoleCommand(Console.ReadLine()));
            Prompt();

        }
    }
}
