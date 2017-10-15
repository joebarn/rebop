using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [Opcode(0, AddressingModes.Implied)]
    class NOP : Operation
    {
        public NOP(Cpu cpu, AddressingModes addressingMode):base(cpu, addressingMode){ }

        protected override void OnExecute()
        {
            //do nothing
        }
    }
}
