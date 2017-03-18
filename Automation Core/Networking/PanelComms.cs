using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Fleck;

namespace Automation_Core.Networking
{
    public static class PanelComms
    {
        public static void SetupPanelServer()
        {
            WebSocketServer wss = new WebSocketServer("ws://" + Properties.Settings.Default.PanelServerIP + ":" + Properties.Settings.Default.PanelServerPort);
            wss.Start(socket =>
            {
                socket.OnOpen = () => Variables.logger.LogLine("A client has connected to the WebSockets Server");
                socket.OnClose = () => Variables.logger.LogLine("A client has disconnected from the WebSockets Server");
                socket.OnMessage = message => socket.Send(CommandParser.ParsePanelCommand(message));
            });
        }
        //i love you
        }
    }
