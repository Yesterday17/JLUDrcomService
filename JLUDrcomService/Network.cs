using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace JLUDrcomService
{
    class Network
    {
        private const String authServer = "10.100.61.3";
        private const int authPort = 61440;

        public static bool Challenge(int retry)
        {
            byte[] packet = new byte[]
            {
                0x01, (byte)(0x02 + retry),
                NetworkUtils.RandomByte(), NetworkUtils.RandomByte(),
                0x6a, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00
            };
            byte[] receive = NetworkUtils.SendUDPDatagram(packet);
            return true;
        }

        public static bool Login(String username, String password)
        {
            return true;
        }

        public static String HeartBeat(byte[] tail)
        {
            return "";
        }

        static class NetworkUtils
        {
            public static byte RandomByte()
            {
                return (byte)new Random().Next();
            }

            public static byte[] SendUDPDatagram(byte[] packet)
            {
                IPEndPoint e = new IPEndPoint(IPAddress.Parse(authServer), authPort);
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

            public static String GetHostName()
            {
                return Dns.GetHostName();
            }
        }

    }
}
