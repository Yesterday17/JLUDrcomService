using JLUDrcomService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLUDrcomService.Packets
{
    class LoginAuthPacket
    {
        public static byte[] SendPacket(String username, String password, byte[] MD5A, byte[] MD5B, byte[] IP)
        {
            Packet packet = new Packet();
            try
            {
                // 构建数据包
                packet += new byte[]
                {
                    (byte)Code.LoginAuth, 0x01,
                    0x00, (byte)(username.Length + 20)
                };
                // MD5A
                packet += MD5A;
                // 用户名
                packet += NetworkUtils.GetUsernameB(username);
                // ControlCheckStatus, AdapterNum
                packet += new byte[] {
                    0x20, 0x04
                };
                // Mac xor MD5A
                packet += PacketUtils.MACXorMD5A(Constants.MAC, MD5A);
                // MD5B
                packet += MD5B;
                // NICCount
                packet += (byte)0x01;
                // 本机IP地址
                packet += IP;
                // 其他填空IP地址
                packet += new byte[] {
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00
                };
                // MD5C
                packet += PacketUtils.MD5C(packet);
                // IPDog
                packet += new byte[] {
                    0x01,
                    0x00, 0x00, 0x00, 0x00
                };
                packet += NetworkUtils.GetHostNameB();
                packet += new byte[]
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
                    (byte) (password.Length > 16 ? 16 : password.Length)
                };
                // ROR
                packet += PacketUtils.ROR(MD5A, password);
                // AuthExtDataOption
                packet += new byte[] {
                    0x02, 0x0c
                };
                // Checksum
                packet += PacketUtils.CheckSum(packet);
                // AutoLogout(?), Broadcast mode(?)
                packet += new byte[] {
                    0x00, 0x00
                };
                // MAC
                packet += Constants.MAC;
                packet += new byte[] {
                    PacketUtils.RandomByte(), PacketUtils.RandomByte()
                };
            }
            catch (Exception e)
            {
                Constants.logger.log(e.Message);
            }
            // Log
            Constants.logger.log("Preparing for LoginAuth Packet:\n" + BitConverter.ToString(packet));
            return NetworkUtils.SendUDPDatagram(packet);
        }
    }
}
