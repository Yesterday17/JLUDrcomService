using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using JLUDrcomService.Utils;
using JLUDrcomService.Packets;

namespace JLUDrcomService
{
    class JLUDrcomClient
    {
        private String username;
        private String password;

        private byte[] challenge = new byte[4];
        private byte[] MD5A;
        private byte[] MD5B;
        private byte[] AuthInformation = new byte[16];

        private int heartbeatCount = 0;
        private byte[] tail = new byte[4];

        public JLUDrcomClient(String username, String password)
        {
            Constants.logger.log("Creating new JLUDrcomClient.");
            Constants.logger.log("\r\n MAC: " + BitConverter.ToString(Constants.MAC) + "\r\n IP: " + BitConverter.ToString(Constants.IP) + "\r\n LogPath: " + Constants.logPath);
            this.username = username;
            this.password = password;
        }

        public bool StartRequest(int retry)
        {
            byte[] packet = new byte[]
            {
                (byte)Code.StartRequest, (byte)(0x02 + retry),
                PacketUtils.RandomByte(), PacketUtils.RandomByte(),
                0x6a, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00,
                0x00, 0x00
            };
            Constants.logger.log("Preparing for StartRequest Packet:\n" + BitConverter.ToString(packet));
            byte[] receive = NetworkUtils.SendUDPDatagram(packet);

            if (receive[0] == (byte)Code.StartResponse)
            {
                Constants.logger.log("Received success StartRequest response:\n" + BitConverter.ToString(receive));
                Array.Copy(receive, 4, this.challenge, 0, 4);
                this.MD5A = PacketUtils.MD5A((byte)Code.LoginAuth, 0x01, challenge, password);
                this.MD5B = PacketUtils.MD5B(challenge, password);
                return true;
            }
            Constants.logger.error("Received failure StartRequest response:\n" + BitConverter.ToString(receive));
            return false;
        }

        public bool LoginAuth()
        {
            byte[] receive = LoginAuthPacket.SendPacket(username, password, MD5A, MD5B);

            if (receive[0] == (byte)Code.Success)
            {
                Constants.logger.log("Received success LoginAuth response:\n" + BitConverter.ToString(receive));
                Array.Copy(receive, 23, AuthInformation, 0, 16);
                return true;
            }

            Constants.logger.error("Received failure LoginAuth response:\n" + BitConverter.ToString(receive));
            return false;
        }

        public bool LogoutAuth()
        {
            return true;
        }

        public bool IsValid()
        {
            return username != "" && password != "";
        }

        public bool HeartBeat()
        {
            // TODO: 增加错误判定
            Packet aliveResponse = HeartBeatPacket.SendAlivePacket(MD5A, AuthInformation);

            if (heartbeatCount % 10 == 0)
            {
                HeartBeatPacket.SendKeepPacket(1, true, heartbeatCount, tail);
            }

            // 更新 tail
            Packet res1 = HeartBeatPacket.SendKeepPacket(1, false, heartbeatCount, tail);
            Array.Copy(res1, 16, tail, 0, 4);
            Packet res2 = HeartBeatPacket.SendKeepPacket(3, false, heartbeatCount, tail);

            // 计数菌 +1
            heartbeatCount++;
            return true;
        }
    }
}
