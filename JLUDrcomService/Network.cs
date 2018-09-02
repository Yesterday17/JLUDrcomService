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


        private String username;
        private String password;
        private byte[] tail = new byte[2];

        enum Code
        {
            StartRequest = 0x01,
            StartResponse = 0x02,
            LoginAuth = 0x03,
            Success = 0x04,
            Failure = 0x05,
            LogoutAuth = 0x06,
            Misc = 0x07,
            Unknown = 0x08,
            NewPassword = 0x09,
            Message = 0x4d,
            Alive4Client = 0xfe,
            Alive = 0xff
        }

        public Network(String username, String password)
        {
            this.username = username;
            this.password = password;
        }

        public bool StartRequest(int retry)
        {
            byte[] packet = new byte[]
            {
                (byte)Code.StartRequest, (byte)(0x02 + retry),
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
            return receive[0] == 0x02;
        }

        public bool LoginAuth()
        {
            return true;
        }

        public bool LogoutAuth()
        {
            return true;
        }

        public bool HeartBeat()
        {
            return true;
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
