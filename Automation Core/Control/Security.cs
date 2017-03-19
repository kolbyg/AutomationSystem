using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Core.Control
{
    class Security
    {//alarm types 0=speakers only, 1=relay only, 2=speakers + relay
        public static string TriggerAlarm(string Reason)
        {
            Variables.logger.LogLine(1, "ALARM TRIGGERED, Reason: " + Reason);
            Variables.logger.LogLine("Alarm response sequence starting");
            //FIX THIS, some way of tracking WHY the state is at that level
            if(Variables.sysstate <=4) Variables.sysstate = 4;
            if (Variables.alrmstate <= 4) Variables.alrmstate = 4;
            Variables.logger.LogLine("Sending command to lighting control to turn all lights on");
            Control.Lighting.AllOn();
            Variables.logger.LogLine("Stopping all audio");
            Media.Pause();
            Variables.logger.LogLine("Sending response to email list");
            //send email
            Variables.logger.LogLine("Engaging alarm");
            string alarmMessage = "Invalid Alarm Type";
            /**if (Properties.Settings.Default.AlarmType ==0)
            {
                //Automation_Core.Security.Alarm.TriggerRelayAlarm();
                alarmMessage = "Alarm Relay Triggered";
            }
            else if(Properties.Settings.Default.AlarmType == 1)
            {
                //Automation_Core.Security.Alarm.TriggerSpeakerAlarm();
                //Automation_Core.Security.Alarm.TriggerRelayAlarm();
                alarmMessage = "Alarm Relay and Sound Triggered";
            }
            else if(Properties.Settings.Default.AlarmType == 2)
            {
                //Automation_Core.Security.Alarm.TriggerSpeakerAlarm();
                alarmMessage = "Alarm Sound Triggered";
            }*/
            return alarmMessage;
        }
        public static string GetStatus()
        {
            return Variables.almStates[Variables.alrmstate];
        }
        public static bool CheckAuth(string PIN, int PermLevel)
        {
            bool isAuth = false;
            foreach (Devices.User user in Variables.users)
            {
                if (user == null) continue;
                if (user.PIN == PIN && user.PermLevel >= PermLevel) isAuth = true;
            }
            return isAuth;
        }
        public static bool CheckAuth(string PIN)
        {
            bool isAuth = false;
            foreach (Devices.User user in Variables.users)
            {
                if (user == null) continue;
                if (user.PIN == PIN) isAuth = true;
            }
            return isAuth;
        }
        public static string DisengageAlarm(string Reason)
        {
            Variables.logger.LogLine(1, "ALARM DISENGAGED, Reason: " + Reason);
            Variables.logger.LogLine("Alarm disengage sequence starting");
            //FIX THIS, some way of tracking WHY the state is at that level
            Variables.sysstate = 0;
            Variables.alrmstate = 0;
            Variables.logger.LogLine("Sending command to lighting control to turn all lights off");
            Control.Lighting.AllOff();
            Variables.logger.LogLine("Sending response to email list");
            //send email
            Variables.logger.LogLine("Disengaging alarm");
            string alarmMessage = "Invalid Alarm Type";
            /**if (Properties.Settings.Default.AlarmType == 0)
            {
                //Automation_Core.Security.Alarm.StopRelayAlarm();
                alarmMessage = "Alarm Relay Disengaged";
            }
            else if (Properties.Settings.Default.AlarmType == 1)
            {
                //Automation_Core.Security.Alarm.StopSpeakerAlarm();
                //Automation_Core.Security.Alarm.StopRelayAlarm();
                alarmMessage = "Alarm Relay and Sound Disengaged";
            }
            else if (Properties.Settings.Default.AlarmType == 2)
            {
                //Automation_Core.Security.Alarm.StopSpeakerAlarm();
                alarmMessage =  "Alarm Sound Disengaged";
            }*/
            Variables.logger.LogLine("resuming audio");
            Media.Resume();
            return alarmMessage;
        }
    }
}
