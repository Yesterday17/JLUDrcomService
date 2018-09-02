using JLUDrcomService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLUDrcomService.Packets
{
    class HeartBeatPacket
    {
        public static byte[] SendAlivePacket(byte[] md5a, byte[] authInformation)
        {
            Packet packet = new Packet();

            packet += 0xff;
            packet += md5a;
            packet += new byte[] {
                0x00, 0x00, 0x00
            };
            packet += authInformation;
            packet += PacketUtils.RandomByte();
            packet += PacketUtils.RandomByte();

            return NetworkUtils.SendUDPDatagram(packet);
        }

        public static byte[] SendKeepPacket(byte type, bool fake, int count, byte[] tail)
        {
            Packet packet = new Packet(new byte[] {
                0x07, (byte)count,
                0x28, 0x00,
                0x0b, type,
                (byte)(fake ? 0xdc : 0x0f), (byte)(fake ? 0x02 : 0x27),
                PacketUtils.RandomByte(), PacketUtils.RandomByte(),
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            });
            packet += tail;
            packet += new byte[] {
                0x00, 0x00, 0x00, 0x00
            };
            if(type == 0x03)
            {
                packet += PacketUtils.CRC(packet + Constants.IP);
                packet += Constants.IP;
                packet += new byte[] {
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00
                };
            }
            else
            {
                packet += new byte[]
                {
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00
                };
            }
            return NetworkUtils.SendUDPDatagram(packet);
        }
    }
}
