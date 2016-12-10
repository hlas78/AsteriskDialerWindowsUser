using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AsteriskDialerWindowsUser
{
    class Logger
    {
        StreamWriter sw;

        public Logger()
        {
            try
            {
                string sFileName = System.AppDomain.CurrentDomain.FriendlyName;
                sw = new StreamWriter(System.AppDomain.CurrentDomain.FriendlyName + ".log", true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error opening log file" + e.ToString());
            }
        }

        public void log(string s)
        {
            try
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-dd-MM hh:mm:ss ") + s);
                sw.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing log" + e.ToString());
            }

        }

        ~Logger()
        {
            try
            {
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
