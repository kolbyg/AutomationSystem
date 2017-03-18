using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Devices
{
    class Sprinkler
    {
        public int ID;
        public int RelayID;
        public string Name;
        public string Desc;
        public bool Active;
        public bool State;
        public int CycleTime;
        public string TimeOn;
    }
}
