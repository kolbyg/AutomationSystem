using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Devices
{
    class Relay
    {
        public int ID;
        public int ParentNodeID;
        public int ParentNodePort;
        public string Name;
        public string Desc;
        public bool Active;
        public bool State;
    }
}
