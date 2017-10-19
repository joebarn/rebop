using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [Opcode(0x99, AddressingModes.Absolute)]
    [Opcode(0x9A, AddressingModes.AbsoluteIndexed)]
    [Opcode(0x9B, AddressingModes.Indirect)]
    [Opcode(0x9C, AddressingModes.IndirectPreIndexed)]
    [Opcode(0x9D, AddressingModes.IndirectPostIndexed)]
    class STA : Operation
    {
        public STA(Cpu cpu, AddressingModes addressingMode):base(cpu, addressingMode){ }

        protected override void OnExecute()
        {
            switch (_addressingMode)
            {

                case AddressingModes.Absolute:
                case AddressingModes.AbsoluteIndexed:
                case AddressingModes.Indirect:
                case AddressingModes.IndirectPreIndexed:
                case AddressingModes.IndirectPostIndexed:
                    Write8(Effective(), _cpu._acc.Value);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
