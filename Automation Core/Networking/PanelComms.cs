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
            //Setup the websockets server for the HTML/JS interface to connect to
            WebSocketServer wss = new WebSocketServer("ws://" + Variables.PanelServerIP + ":" + Variables.PanelServerPort);
            wss.Start(socket =>
            {
                //Log open and closeing of connections, send all messages through the panel parser
                socket.OnOpen = () => Variables.logger.LogLine("A client has connected to the WebSockets Server");
                socket.OnClose = () => Variables.logger.LogLine("A client has disconnected from the WebSockets Server");
                socket.OnMessage = message => socket.Send(CommandParser.ParseCommand(message));
            });
        }
        }
    }
