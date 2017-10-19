using Rebop.Vm.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [Opcode(0x10, AddressingModes.Immediate)]
    [Opcode(0x11, AddressingModes.Absolute)]
    [Opcode(0x12, AddressingModes.AbsoluteIndexed)]
    class ADD : Operation
    {
        public ADD(Cpu cpu, AddressingModes addressingMode) : base(cpu, addressingMode) { }

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

            //calculate
            value = a + b;

            //set
            _cpu._acc.Value = (byte)value;

            //flags
            Carry(value, false);
            Overflow(a, b, (byte)value, false);
            Negative();
            Zero();


        }
    }
}
