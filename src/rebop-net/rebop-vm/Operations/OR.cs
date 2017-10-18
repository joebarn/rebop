
namespace Rebop.Vm.Operations
{
    [Opcode(0x38, AddressingModes.Immediate)]
    [Opcode(0x39, AddressingModes.Absolute)]
    [Opcode(0x3A, AddressingModes.AbsoluteIndexed)]
    class OR : Operation
    {
        public OR(Cpu cpu, AddressingModes addressingMode) : base(cpu, addressingMode) { }

        protected override void OnExecute()
        {
            int value = 0;

            switch (_addressingMode)
            {

                case AddressingModes.Immediate:
                    value = _cpu.Acc.Value | Immediate8();
                    break;

                case AddressingModes.Absolute:
                case AddressingModes.AbsoluteIndexed:
                    value = _cpu.Acc.Value | Read8(Effective());
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
        }
    }
}
