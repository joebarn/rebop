using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [Opcode(0x10, AddressingModes.Immediate)]
    [Opcode(0x11, AddressingModes.Absolute)]
    [Opcode(0x12, AddressingModes.Indexed)]
    class ADD : Operation
    {
        public ADD(AddressingModes addressingMode) : base(addressingMode) { }

        protected override void OnExecute(Cpu cpu, AddressingModes addressingMode)
        {
            switch (addressingMode)
            {

                case AddressingModes.Immediate:
                    cpu.Acc.Value = (byte) (cpu.Acc.Value+cpu.Ram[(ushort)(cpu.Pc.Value + 1)]);
                    //TODO set flags
                    break;

                case AddressingModes.Absolute:
                    break;

                default:
                    break;
            }

        }
    }
}
