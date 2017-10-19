using Rebop.Vm.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [Opcode(0x20, AddressingModes.Immediate)]
    [Opcode(0x21, AddressingModes.Absolute)]
    [Opcode(0x22, AddressingModes.AbsoluteIndexed)]
    class SUB : Operation
    {
        public SUB(Cpu cpu, AddressingModes addressingMode) : base(cpu, addressingMode) { }

        protected override void OnExecute()
        {
            byte a = _cpu.Acc.Value;
            byte b;
            int value;

            switch (_addressingMode)
            {

                case AddressingModes.Immediate:
                    b = Immediate8();
                    break;

                case AddressingModes.Absolute:
                case AddressingModes.AbsoluteIndexed:
                    b = Read8(Effective());
                    break;

                default:
                    throw new NotImplementedException();
            }

            value = a - b;

            //set
            _cpu._acc.Value = (byte)value;

            //flags
            Carry(value, true);
            Overflow(a, b, (byte)value, true);
            Negative();
            Zero();

        }
    }
}
