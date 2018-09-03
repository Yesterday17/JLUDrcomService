using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLUDrcomService.Utils
{
    class Logger
    {
        private StreamWriter w = File.AppendText(Constants.logPath);

        public void log(string logMessage)
        {
            write(logMessage, "Log");
        }

        public void error(string logMessage)
        {
            write(logMessage, "Error");
        }

        public void info(string logMessage)
        {
            write(logMessage, "Info");
        }

        public void warn(string logMessage)
        {
            write(logMessage, "Warning");
        }

        public void write(string logMessage, string level)
        {
            w.WriteLine("\r\nLog Level: {0}", level);
            w.WriteLine("Time: {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            w.WriteLine("Content: {0}", logMessage);
            w.WriteLine("-------------------------------");
            w.Flush();
        }
    }
}
