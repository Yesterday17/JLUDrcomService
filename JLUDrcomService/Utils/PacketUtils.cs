using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JLUDrcomService.Packets;

namespace JLUDrcomService.Utils
{
    static class PacketUtils
    {
        public static byte RandomByte()
        {
            return (byte)new Random((int)(DateTime.Now.ToBinary())).Next();
        }

        public static Packet MD5A(byte code, byte type, Packet salt, String password)
        {
            MD5 md5 = MD5.Create();
            Packet content = new Packet();
            content += code;
            content += type;
            content += salt;
            content += Encoding.ASCII.GetBytes(password);
            return md5.ComputeHash(content);
        }

        public static Packet MD5B(byte[] salt, String password)
        {
            MD5 md5 = MD5.Create();
            Packet content = new Packet();
            content += (byte)0x01;
            content += Encoding.ASCII.GetBytes(password);
            content += salt;
            content += new byte[] { 0x00, 0x00, 0x00, 0x00 };
            return md5.ComputeHash(content);
        }

        public static Packet MD5C(Packet packet)
        {
            MD5 md5 = MD5.Create();
            byte[] content = new byte[101];
            Array.Copy(packet + new byte[] { 0x14, 0x00, 0x07, 0x0b }, content, packet.Length + 4);
            return md5.ComputeHash(content).Take(8).ToArray();
        }

        public static Packet ROR(byte[] md5a, String password)
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

        public static Packet MACXorMD5A(byte[] mac, byte[] md5a)
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
        public static Packet CheckSum(byte[] packet)
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
    }
}
