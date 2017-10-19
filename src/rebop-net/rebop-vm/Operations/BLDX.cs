using Rebop.Vm.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [Opcode(0xA0, AddressingModes.BigImmediate)]
    [Opcode(0xA1, AddressingModes.BigAbsolute)]
    class BLDX : Operation
    {
        public BLDX(Cpu cpu, AddressingModes addressingMode) : base(cpu, addressingMode) { }

        protected override void OnExecute()
        {
            ushort value;

            switch (_addressingMode)
            {

                case AddressingModes.BigImmediate:
                    value = Immediate16();
                    break;

                case AddressingModes.BigAbsolute:
                    value = Read16(Effective());
                    break;

                default:
                    throw new NotImplementedException();
            }

            //set
            _cpu._x.Value = value;

        }
    }
}
