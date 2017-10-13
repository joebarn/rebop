using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [Opcode(0x90, AddressingModes.Immediate)]
    [Opcode(0x91, AddressingModes.Absolute)]
    [Opcode(0x92, AddressingModes.Indexed)]
    [Opcode(0x93, AddressingModes.Indirect)]
    [Opcode(0x94, AddressingModes.IndirectPreIndexed)]
    [Opcode(0x95, AddressingModes.IndirectPostIndexed)]
    class LDA : Operation
    {
        public LDA(AddressingModes addressingMode) : base(addressingMode) { }

        protected override void OnExecute(Cpu cpu, AddressingModes addressingMode)
        {
            switch (addressingMode)
            {

                case AddressingModes.Immediate:
                    cpu.Acc.Value = cpu.Ram[(ushort)(cpu.Pc.Value + 1)];
                    break;

                case AddressingModes.Absolute:
                    break;

                default:
                    break;
            }

        }
    }
}
