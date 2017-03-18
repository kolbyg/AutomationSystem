using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Control
{
    class System
    {
        public static string GetStatus()
        {
            return Variables.sysStates[Variables.sysstate];
        }
    }
}
