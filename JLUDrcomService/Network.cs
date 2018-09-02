using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Net.NetworkInformation;

namespace JLUDrcomService
{
    class Network
    {
        private const String authServer = "10.100.61.3";
        private const int authPort = 61440;


        private String username;
        private String password;
        private byte[] salt = new byte[4];
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

            if (receive[0] == 0x02)
            {
                Array.Copy(receive, 4, this.salt, 0, 4);
                return true;
            }
            return false;
        }

        public bool LoginAuth()
        {
            byte[] packet = new byte[]
            {
                (byte)Code.LoginAuth, 0x01,
                0x00, (byte)(username.Length + 20)
            };
            packet = NetworkUtils.concat(packet, NetworkUtils.MD5A((byte)Code.LoginAuth, 0x01, salt, password));
            packet = NetworkUtils.concat(packet, NetworkUtils.GetUsernameB(username));
            packet = NetworkUtils.concat(packet, new byte[] {
                0x20, 0x04
            });
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

            public static byte[] concat(byte[] a, byte[] b)
            {
                byte[] result = new byte[a.Length + b.Length];
                Array.Copy(a, 0, result, 0, a.Length);
                Array.Copy(b, 0, result, a.Length, b.Length);
                return result;
            }

            public static byte[] MD5A(byte code, byte type, byte[] salt, String password)
            {
                MD5 md5 = MD5.Create();
                byte[] content = new byte[]{
                    code, type
                };
                content = concat(content, salt);
                content = concat(content, Encoding.ASCII.GetBytes(password));
                return md5.ComputeHash(content);
            }

            public static byte[] GetUsernameB(String username)
            {
                byte[] result = new byte[36], un = Encoding.ASCII.GetBytes(username);
                Array.Copy(un, result, un.Length);
                return result;
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

            public static byte[] GetMacAddress()
            {
                foreach (var t in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (t.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        return t.GetPhysicalAddress().GetAddressBytes();
                }
                return new byte[6];
            }
        }

    }
}
