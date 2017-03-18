using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Control
{
    class Climate
    {//hvac system types, 1=heat pump, direct control, 2=heat pump, thermosdat control
        public static string TriggerHeat()
        {
            return "Invalid HVAC System";
        }
        public static string TriggerCool()
        {
            return "Invalid HVAC System";
        }
    }
}
