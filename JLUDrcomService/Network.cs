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


        private readonly String username;
        private readonly String password;
        private byte[] MAC;
        private byte[] IP = new byte[4];

        private byte[] md5a = new byte[6];
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
            // 数据初始化
            MAC = NetworkUtils.GetMacAddressB();
            md5a = NetworkUtils.MD5A((byte)Code.LoginAuth, 0x01, salt, password);

            // 构建数据包
            byte[] packet = new byte[]
            {
                (byte)Code.LoginAuth, 0x01,
                0x00, (byte)(username.Length + 20)
            };
            // MD5A
            packet = NetworkUtils.concat(packet, md5a);
            // 用户名
            packet = NetworkUtils.concat(packet, NetworkUtils.GetUsernameB(username));
            // ControlCheckStatus, AdapterNum
            packet = NetworkUtils.concat(packet, new byte[] {
                0x20, 0x04
            });
            // Mac xor MD5A
            packet = NetworkUtils.concat(packet, NetworkUtils.MACXorMD5A(MAC, md5a));
            // MD5B
            packet = NetworkUtils.concat(packet, NetworkUtils.MD5B(salt, password));
            // NICCount
            packet = NetworkUtils.concat(packet, new byte[] {
                0x01
            });
            // 本机IP地址
            packet = NetworkUtils.concat(packet, NetworkUtils.GetIPB());
            // 其他填空IP地址
            packet = NetworkUtils.concat(packet, new byte[] {
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00
            });
            // MD5C
            packet = NetworkUtils.concat(packet, NetworkUtils.MD5C(packet));
            // IPDog
            packet = NetworkUtils.concat(packet, new byte[] {
                0x01,
                0x00, 0x00, 0x00, 0x00
            });
            packet = NetworkUtils.concat(packet, NetworkUtils.GetHostNameB());
            packet = NetworkUtils.concat(packet, new byte[]
            {
                // DNS
                0x0a, 0x0a, 0x0a, 0x0a,
                // DHCP
                0x00, 0x00, 0x00, 0x00,
                // 备用 DNS
                0x08, 0x08, 0x08, 0x08,
                // WINSIP1
                0x00, 0x00, 0x00, 0x00,
                // WINSIP2
                0x00, 0x00, 0x00, 0x00,
                // OSVersionInfoSize
                0x94, 0x00, 0x00, 0x00,
                // OS major
                0x06, 0x00, 0x00, 0x00,
                // OS minor
                0x02, 0x00, 0x00, 0x00,
                // OS build
                0xf0, 0x23, 0x00, 0x00,
                // PlatformID
                0x02, 0x00, 0x00, 0x00,
                // servicepack
                0x44, 0x72, 0x43, 0x4f,
                0x4d, 0x00, 0xcf, 0x07,
                0x6a, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x31, 0x63, 0x32, 0x31,
                0x30, 0x63, 0x39, 0x39,
                0x35, 0x38, 0x35, 0x66,
                0x64, 0x32, 0x32, 0x61,
                0x64, 0x30, 0x33, 0x64,
                0x33, 0x35, 0x63, 0x39,
                0x35, 0x36, 0x39, 0x31,
                0x31, 0x61, 0x65, 0x65,
                0x63, 0x31, 0x65, 0x62,
                0x34, 0x34, 0x39, 0x62,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                // ClientVerInfoAndInternetMode
                0x6a, 0x00, 0x00,
                // 密码长度
                (byte)(password.Length > 16 ? 16 : password.Length)
            });
            // ROR
            packet = NetworkUtils.concat(packet, NetworkUtils.ROR(md5a, password));
            // AuthExtDataOption
            packet = NetworkUtils.concat(packet, new byte[] {
                0x02, 0x0c
            });
            // Checksum
            packet = NetworkUtils.concat(packet, NetworkUtils.CheckSum(packet));
            // AutoLogout(?), Broadcast mode(?)
            packet = NetworkUtils.concat(packet, new byte[] {
                0x00, 0x00
            });
            // MAC
            packet = NetworkUtils.concat(packet, NetworkUtils.GetMacAddressB());
            packet = NetworkUtils.concat(packet, new byte[] {
                NetworkUtils.RandomByte(), NetworkUtils.RandomByte()
            });
            byte[] result = NetworkUtils.SendUDPDatagram(packet);
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
                return (byte)new Random((int)(DateTime.Now.ToBinary())).Next();
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

            public static byte[] MD5B(byte[] salt, String password)
            {
                MD5 md5 = MD5.Create();
                byte[] content = new byte[]{
                    0x01
                };
                content = concat(content, Encoding.ASCII.GetBytes(password));
                content = concat(content, salt);
                content = concat(content, new byte[] { 0x00, 0x00, 0x00, 0x00 });
                return md5.ComputeHash(content);
            }

            public static byte[] MD5C(byte[] packet)
            {
                MD5 md5 = MD5.Create();
                byte[] content = new byte[101];
                Array.Copy(concat(packet, new byte[] { 0x14, 0x00, 0x07, 0x0b }), content, packet.Length + 4);
                return md5.ComputeHash(content).Take(8).ToArray();
            }

            public static byte[] ROR(byte[] md5a, String password)
            {
                byte[] passwd = Encoding.ASCII.GetBytes(password);
                int len = passwd.Length;
                byte[] ret = new byte[len];
                int x;
                for (int i = 0; i < len; i++)
                {
                    x = md5a[i] ^ passwd[i];
                    ret[i] = (byte)(((x << 3) & 0xFF) + (x >> 5));
                }
                return ret;
            }

            public static byte[] MACXorMD5A(byte[] mac, byte[] md5a)
            {
                byte[] result = new byte[6];
                for (int i = 0; i <= 5; i++)
                {
                    result[i] = (byte)(mac[i] ^ md5a[i]);
                }
                return result;
            }

            /// <summary>
            /// https://github.com/drcoms/jlu-drcom-client/blob/master/C-version/drcom.c
            /// </summary>
            /// <param name="packet"></param>
            /// <returns></returns>
            public static byte[] CheckSum(byte[] packet)
            {
                UInt64 sum = 1234, check = 0;
                for (int i = 0; i < packet.Length; i += 4)
                {
                    check = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        check = (check * 256) + packet[i + j];
                    }
                    sum ^= check;
                }
                return BitConverter.GetBytes((1968 * sum) & 0xFFFFFFFF).Take(4).ToArray();
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
