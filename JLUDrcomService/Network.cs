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

        private const int initPacketSize = 20;

        public static String Login(String username, String password)
        {
            UdpClient client = new UdpClient(authServer, authPort);
            IPEndPoint endPoint = new IPEndPoint(NetworkUtils.IP2Int(authServer), authPort);
            client.Send(NetworkUtils.GenerateInitPacket(1), initPacketSize);

            byte[] packet1 = client.Receive(ref endPoint);
            return "";
        }

        static class NetworkUtils
        {
            public static long IP2Int(string ip)
            {
                char[] separator = new char[] { '.' };
                string[] items = ip.Split(separator);
                return long.Parse(items[0]) << 24
                        | long.Parse(items[1]) << 16
                        | long.Parse(items[2]) << 8
                        | long.Parse(items[3]);
            }

            public static byte[] RandomBytes(int size)
            {
                byte[] b = new byte[size];
                new Random().NextBytes(b);
                return b;
            }

            public static byte RandomByte()
            {
                return RandomBytes(1)[0];
            }

            public static byte[] GenerateInitPacket(int trytime)
            {
                byte[] packet = new byte[]
                {
                    0x01, (byte)(0x02 + trytime), RandomByte(), RandomByte(), 0x6a,
                    0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00
                };
                return packet;
            }
        }

    }
}
