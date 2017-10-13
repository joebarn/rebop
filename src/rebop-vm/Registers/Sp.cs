using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Registers
{
    class Sp:Register<ushort>
    {
        public override string ToString()
        {
            return _value.ToString("X2");
        }

    }
}
