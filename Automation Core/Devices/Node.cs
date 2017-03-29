using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Devices
{
    class Node
    {
        public string IPAddress;
        public string Name;
        public string Type;
        public string Desc;
        public int ID;
        public bool Active;
        public bool TelnetConnected;
        public PrimS.Telnet.Client TelnetConnection;
        public bool MPDConnected;
        public Libmpc.Mpc MPDConnection;
    }
}
