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
    class Network
    {
        private String username;
        private String password;

        private byte[] challenge = new byte[4];

        private int heartbeatCount = 0;
        private byte[] tail = new byte[2];
        
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
            byte[] receive = NetworkUtils.SendUDPDatagram(packet);

            if (receive[0] == (byte)Code.StartResponse)
            {
                Array.Copy(receive, 4, this.challenge, 0, 4);
                return true;
            }
            return false;
        }

        public bool LoginAuth()
        {
            return new LoginAuthPacket(username, password, challenge).SendPacket()[0] == (byte)Code.Success;
        }

        public bool LogoutAuth()
        {
            return true;
        }

        public bool HeartBeat()
        {
            bool fakePacket = heartbeatCount % 10 == 0;
            return true;
        }
    }
}
