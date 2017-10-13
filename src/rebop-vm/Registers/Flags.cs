using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Registers
{
    [Flags]
    enum Flags
    {
        IntMask = 0x80,   //1000 0000
        Overflow = 0x40, //0100 0000
        Negative = 0x20,  //0010 0000
        Zero = 0x10,     //0001 0000
        Carry = 0x08      //0000 1000
    }
}
