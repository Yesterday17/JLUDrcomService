using JLUDrcomService.Utils;
using System;

namespace JLUDrcomService
{
    static class Constants
    {
        public static String authServer = "10.100.61.3";
        public static int authPort = 61440;

        public static byte[] MAC = NetworkUtils.GetMacAddressB();
        public static byte[] IP = NetworkUtils.GetIPB();

        public static string logPath = System.AppDomain.CurrentDomain.BaseDirectory + "Log.log";
        public static Logger logger = new Logger();
    }
}
