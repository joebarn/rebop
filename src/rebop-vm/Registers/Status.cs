using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Registers
{
    //IONZC000

    class Status : Register<byte>, IStatus
    {
        public void Set(Flags flag)
        {
            _value = (byte)(_value | (byte)flag);
        }

        public void Clear(Flags flag)
        {
            _value = (byte)(_value | ~(byte)flag);
        }

        public bool IntMask
        {
            get
            {
                return ((_value & (byte)Flags.IntMask) != 0);
            }
        }

        public bool Overflow
        {
            get
            {
                return ((_value & (byte)Flags.Overflow) != 0);
            }
        }

        public bool Negative
        {
            get
            {
                return ((_value & (byte)Flags.Negative) != 0);
            }
        }

        public bool Zero
        {
            get
            {
                return ((_value & (byte)Flags.Zero) != 0);
            }
        }

        public bool Carry
        {
            get
            {
                return ((_value & (byte)Flags.Carry) != 0);
            }
        }

        public override string ToString()
        {
            return _value.ToString("X2");
        }

    }
}
