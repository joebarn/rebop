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
        public HALT(Cpu cpu, AddressingModes addressingMode):base(cpu, addressingMode){ }

        protected override void OnExecute()
        {
            _cpu.Halt();
        }
    }
}
