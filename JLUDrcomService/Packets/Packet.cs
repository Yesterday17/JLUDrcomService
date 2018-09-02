using System;

namespace JLUDrcomService.Packets
{
    class Packet
    {
        protected byte[] packet;
        public int Length
        {
            get
            {
                return this.packet.Length;
            }
        }

        public Packet()
        {
            packet = new byte[] { };
        }

        public Packet(byte[] data)
        {
            packet = data;
        }

        public static Packet operator +(Packet c1, Packet c2)
        {
            return new Packet(concat(c1.packet, c2.packet));
        }

        public static Packet operator +(Packet c1, byte[] c2)
        {
            return new Packet(concat(c1.packet, c2));
        }

        public static Packet operator +(byte[] c1, Packet c2)
        {
            return new Packet(concat(c1, c2.packet));
        }

        public static Packet operator +(Packet c1, byte c2)
        {
            return new Packet(concat(c1.packet, new byte[] { c2 }));
        }

        public static Packet operator +(byte c1, Packet c2)
        {
            return new Packet(concat(new byte[] { c1 }, c2.packet));
        }

        public static implicit operator Packet(byte[] data)
        {
            return new Packet(data);
        }

        public static implicit operator byte[] (Packet data)
        {
            return data.packet;
        }

        public static byte[] concat(byte[] a, byte[] b)
        {
            byte[] result = new byte[a.Length + b.Length];
            Array.Copy(a, 0, result, 0, a.Length);
            Array.Copy(b, 0, result, a.Length, b.Length);
            return result;
        }
    }
}
