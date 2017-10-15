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
            int value=0;

            switch (_addressingMode)
            {

                case AddressingModes.Immediate:
                    value= _cpu.Acc.Value + Immediate8(); 
                    break;

                case AddressingModes.Absolute:
                case AddressingModes.AbsoluteIndexed:
                    value =_cpu.Acc.Value + Read8(Effective());
                    break;

                default:
                    NotImpl();
                    break;
            }

            //set
            _cpu._acc.Value = (byte)value;

            //flags
            Negative();
            Zero();
            Overflow(value);

            if (_cpu._status.Overflow)
            {
                _cpu._status.Set(Flags.Carry);
            }
            else
            {
                _cpu._status.Clear(Flags.Carry);
            }
        }
    }
}
