using System;
using Rebop.Vm.Registers;

namespace Rebop.Vm.Operations
{
    abstract class Operation
    {
        protected AddressingModes _addressingMode;
        protected Cpu _cpu;

        public Operation(Cpu cpu, AddressingModes addressingMode)
        {
            _cpu = cpu;
            _addressingMode = addressingMode;
        }

        public ushort Width
        {
            get
            {
                switch (_addressingMode)
                {
                    case AddressingModes.Implied:
                        return 1;

                    case AddressingModes.Immediate:
                        return 2;

                    default:
                        return 3;
                }

            }
        }

        public void Load()
        {
            switch (_addressingMode)
            {
                case AddressingModes.Implied:
                case AddressingModes.Immediate:
                    break;

                case AddressingModes.Absolute:
                    //includes big absolute
                    _cpu._tempA.Value = Immediate16();
                    break;

                case AddressingModes.AbsoluteIndexed:
                    _cpu._tempA.Value = Immediate16();
                    _cpu._mar.Value =(ushort)( _cpu._tempA.Value + _cpu._x.Value);
                    break;

                case AddressingModes.Indirect:
                    _cpu._tempA.Value = Immediate16();
                    _cpu._tempB.Value = Read16(_cpu._tempA.Value);
                    break;

                case AddressingModes.IndirectPreIndexed:
                    _cpu._tempA.Value = Immediate16();
                    _cpu._mar.Value = (ushort)(_cpu._tempA.Value + _cpu._x.Value);
                    _cpu._tempB.Value = Read16(_cpu._mar.Value);
                    break;

                case AddressingModes.IndirectPostIndexed:
                    _cpu._tempA.Value = Immediate16();
                    _cpu._tempB.Value = Read16(_cpu._tempA.Value);
                    _cpu._mar.Value = (ushort)(_cpu._tempB.Value + _cpu._x.Value);
                    break;


                default:
                    break;
            }
        }

        protected ushort Effective()
        {
            switch (_addressingMode)
            {
                //AddressingModes.Implied:
                //AddressingModes.Immediate:

                case AddressingModes.Absolute:
                    //includes big absolute
                    return _cpu._tempA.Value;

                case AddressingModes.AbsoluteIndexed:
                    return _cpu._mar.Value;

                case AddressingModes.Indirect:
                    return _cpu._tempB.Value;

                case AddressingModes.IndirectPreIndexed:
                    return _cpu._tempB.Value;

                case AddressingModes.IndirectPostIndexed:
                    return _cpu._mar.Value;

                default:
                    throw NotImpl();
            }

        }


        public void Execute()
        {
            OnExecute();
        }


        protected abstract void OnExecute();

        protected byte Immediate8()
        {
            return _cpu.Ram[(ushort)(_cpu.Pc.Value + 1)];
        }

        protected ushort Immediate16()
        {
            //bigendian
            byte msb= _cpu.Ram[(ushort)(_cpu.Pc.Value + 1)];
            byte lsb = _cpu.Ram[(ushort)(_cpu.Pc.Value + 2)];
            return (ushort)((msb << 8) | lsb);
        }

        protected byte Read8(ushort address)
        {
            return _cpu.Ram[address];
        }

        protected void Write8(ushort address, byte value)
        {
            _cpu.Ram[address] = value;
        }

        protected ushort Read16(ushort address)
        {
            //bigendian
            byte msb = _cpu.Ram[address];
            byte lsb = _cpu.Ram[(ushort)(address+1)];
            return (ushort)((msb << 8) | lsb);
        }

        protected void Write16(ushort address, ushort value)
        {
            //bigendian
            byte msb = (byte) (value>>8);
            byte lsb = (byte) (0x00ff & value);
            _cpu.Ram[address] = msb;
            _cpu.Ram[(ushort)(address+1)] = lsb;
        }


        protected Exception NotImpl()
        {
            return new NotImplementedException();
        }

        protected void Overflow(int value)
        {
            if (value > 255)
            {
                _cpu._status.Set(Flags.Overflow);
            }
            else
            {
                _cpu._status.Clear(Flags.Overflow);
            }
        }

        protected void Negative()
        {
            if ((_cpu._acc.Value & 0x80) == 0x80)
            {
                _cpu._status.Set(Flags.Negative);
            }
            else
            {
                _cpu._status.Clear(Flags.Negative);
            }
        }

        protected void Zero()
        {
            if (_cpu._acc.Value == 0)
            {
                _cpu._status.Set(Flags.Zero);
            }
            else
            {
                _cpu._status.Clear(Flags.Zero);
            }
        }

    }
}
