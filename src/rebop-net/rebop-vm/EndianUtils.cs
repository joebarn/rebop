using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm
{
    public struct EndianUtils
    {

        public static byte[] FromNative(ushort value)
        {
            //most significant bytes first
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(value >> 8);
            bytes[1] = (byte)(0x00ff & value);
            return bytes;
        }

        public static byte[] FromNative(uint value)
        {
            //most significant bytes first
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(value >> 24);
            bytes[1] = (byte)((value >> 16) & 0x000000ff);
            bytes[2] = (byte)((value >> 8) &  0x000000ff);
            bytes[3] = (byte)(value & 0x000000ff);
            return bytes;
        }

        public static ushort ToNative16(byte msb, byte lsb)
        {
            return (ushort)((msb << 8) | lsb);
        }


    }
}
