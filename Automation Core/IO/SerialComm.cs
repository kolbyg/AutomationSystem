using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Automation_Core.IO
{
    class SerialComm
    {
        public static string SendSerialCommand(string serialData, string ComPort)
        {
            SerialPort port = new SerialPort(ComPort, 115200);
            try
            {
                port.Open();
                port.WriteLine(serialData);
            }
            catch(Exception ex)
            {
                Variables.logger.LogLine(ex.Message);
            }
            finally
            {
                if(port.IsOpen)
                port.Close();
            }
            return "";
        }
    }
}
