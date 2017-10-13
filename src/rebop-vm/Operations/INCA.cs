using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [Opcode(0x80, AddressingModes.Implied)]
    class INCA : Operation
    {
        public INCA(AddressingModes addressingMode):base(addressingMode){ }

        protected override void OnExecute(Cpu cpu, AddressingModes addressingMode)
        {
            throw new NotImplementedException();
        }
    }
}
