using Microsoft.Win32;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace JLUDrcomService.Utils
{
    static class NetworkUtils
    {
        public static byte[] SendUDPDatagram(byte[] packet)
        {
            IPEndPoint e = new IPEndPoint(IPAddress.Parse(Constants.authServer), Constants.authPort);
            UdpClient client = new UdpClient();

            try
            {
                client.Connect(e);
                client.Send(packet, packet.Length);
                byte[] receive = client.Receive(ref e);
                client.Close();
                return receive;
            }
            catch
            {
                return new byte[] { };
            }
        }

        public static byte[] GetStringBytify(String str, int len)
        {
            byte[] result = new byte[len], enc = Encoding.ASCII.GetBytes(str);
            Array.Copy(enc, result, enc.Length);
            return result;
        }

        public static byte[] GetUsernameB(String username)
        {
            return GetStringBytify(username, 36);
        }

        public static byte[] GetHostNameB()
        {
            return GetStringBytify(Dns.GetHostName(), 32);
        }

        public static byte[] GetIPB()
        {
            foreach (var t in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (t.AddressFamily == AddressFamily.InterNetwork) return t.GetAddressBytes();
            }
            return new byte[] { };
        }

        public static byte[] GetMacAddressB()
        {
            RegistryKey jlu = Registry.CurrentConfig.CreateSubKey(@"SOFTWARE\JLUDrcomService");
            var mac = jlu.GetValue("MAC");
            jlu.Close();

            if (mac != null)
            {
                return (byte[])mac;
            }

            foreach (var t in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (t.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    return t.GetPhysicalAddress().GetAddressBytes();
            }
            return new byte[6];
        }
    }
}
