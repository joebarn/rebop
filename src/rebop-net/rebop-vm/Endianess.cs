using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm
{
    public struct Endian16
    {
        public byte Msb { private set; get; }
        public byte Lsb { private set; get; }

        public static Endian16 FromNative(ushort value)
        {
            Endian16 endian16 = new Endian16();
            endian16.Msb = (byte)(value >> 8);
            endian16.Lsb = (byte)(0x00ff & value);
            return endian16;
        }

        public static ushort ToNative(byte msb, byte lsb)
        {
            return (ushort)((msb << 8) | lsb);
        }


    }
}
