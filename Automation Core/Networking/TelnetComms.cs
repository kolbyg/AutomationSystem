using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimS.Telnet;
using PrimS;
using System.Net.Sockets;

namespace Automation_Core.Networking
{
    class TelnetComms
    {
        public static Client setupClient(string IP, int Port, string Username, string Password)
        {
            Client client = new Client(IP, Port, new System.Threading.CancellationToken());
            if (client.IsConnected)
            {
                client.TryLoginAsync(Username, Password, 60000).Wait();
            }
            return client;
        }
        public static string SendCommand(Client TelnetClient, string Command)
        {
            try
            {
                TelnetClient.WriteLine(Command);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Telnet message sent successfully\n";
        }
        public static string ReadCommand(Client TelnetClient)
        {
            Task<string> t = TelnetClient.ReadAsync();
            t.Wait();
            return t.Result;
        }
    }
}
