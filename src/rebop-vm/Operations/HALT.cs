using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [Opcode(0x01, AddressingModes.Implied)]
    class HALT : Operation
    {
        public HALT(AddressingModes addressingMode):base(addressingMode){ }

        protected override void OnExecute(Cpu cpu, AddressingModes addressingMode)
        {
            cpu.Halt();
        }
    }
}
