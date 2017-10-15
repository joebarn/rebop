using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Vm.Registers;

namespace Rebop.Vm.Operations
{
    [Opcode(0x80, AddressingModes.Implied)]
    class INCA : Operation
    {
        public INCA(Cpu cpu, AddressingModes addressingMode):base(cpu, addressingMode){ }

        protected override void OnExecute()
        {
            _cpu._acc.Value++;

            Negative();
            Zero();
        }
    }
}
