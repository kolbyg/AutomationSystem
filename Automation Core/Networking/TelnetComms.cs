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
                client.TryLoginAsync("lutron", "integration", 60000).Wait();
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
        public static string sendTelnetCommand(string IP, string Port, string Command)
        {

            using (Client client = new Client("172.16.2.133", 23, new System.Threading.CancellationToken()))
            {
                if (client.IsConnected)
                {
                    client.TryLoginAsync("lutron", "integration", 100000).Wait();
                    client.WriteLine("lutron");
                    client.WriteLine("integration");
                    client.WriteLine(Command);
                    Task<string> t = client.ReadAsync();
                    t.Wait();
                    return t.Result;
                }
            }
            return "TASK FAILED";
        }
    }
}
