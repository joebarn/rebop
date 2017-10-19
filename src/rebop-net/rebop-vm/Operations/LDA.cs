using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Vm.Registers;

namespace Rebop.Vm.Operations
{
    [Opcode(0x90, AddressingModes.Immediate)]
    [Opcode(0x91, AddressingModes.Absolute)]
    [Opcode(0x92, AddressingModes.AbsoluteIndexed)]
    [Opcode(0x93, AddressingModes.Indirect)]
    [Opcode(0x94, AddressingModes.IndirectPreIndexed)]
    [Opcode(0x95, AddressingModes.IndirectPostIndexed)]
    class LDA : Operation
    {
        public LDA(Cpu cpu, AddressingModes addressingMode) : base(cpu, addressingMode) { }

        protected override void OnExecute()
        {
            switch (_addressingMode)
            {

                case AddressingModes.Immediate:
                    _cpu._acc.Value = Immediate8();
                    break;

                case AddressingModes.Absolute:
                case AddressingModes.AbsoluteIndexed:
                case AddressingModes.Indirect:
                case AddressingModes.IndirectPreIndexed:
                case AddressingModes.IndirectPostIndexed:
                    _cpu._acc.Value = Read8(Effective());
                    break;

                default:
                    throw new NotImplementedException();
            }

            //flags
            Negative();
            Zero();


        }
    }
}
