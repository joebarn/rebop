using System;
using Rebop.Vm.Registers;

namespace Rebop.Vm.Operations
{
    internal abstract class Operation
    {
        protected AddressingModes _addressingMode;
        protected Cpu _cpu;

        public Operation(Cpu cpu, AddressingModes addressingMode)
        {
            _cpu = cpu;
            _addressingMode = addressingMode;
        }

        public static ushort GetWidth(AddressingModes addressingMode)
        {
            switch (addressingMode)
            {
                case AddressingModes.Implied:
                    return 1;

                case AddressingModes.Immediate:
                    return 2;

                default:
                    return 3;
            }
        }

        public ushort Width
        {
            get
            {
                return GetWidth(_addressingMode);
            }
        }

        internal void Load()
        {
            switch (_addressingMode)
            {
                case AddressingModes.Implied:
                case AddressingModes.Immediate:
                case AddressingModes.BigImmediate:
                    break;

                case AddressingModes.Absolute:
                case AddressingModes.BigAbsolute:
                    _cpu._tempA.Value = Immediate16();
                    break;

                case AddressingModes.AbsoluteIndexed:
                    _cpu._tempA.Value = Immediate16();
                    _cpu._mar.Value = (ushort)(_cpu._tempA.Value + _cpu._x.Value);
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
                //AddressingModes.BigImmediate:

                case AddressingModes.Absolute:
                case AddressingModes.BigAbsolute:
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
                    throw new NotImplementedException();
            }

        }


        internal void Execute()
        {
            OnExecute();
        }


        protected abstract void OnExecute();

        protected byte Immediate8()
        {
            return Read8((ushort)(_cpu.Pc.Value + 1));
        }

        protected ushort Immediate16()
        {
            return Read16((ushort)(_cpu.Pc.Value + 1));
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
            //convert big endian ushort in ram to native ushort
            byte msb = _cpu.Ram[address];
            byte lsb = _cpu.Ram[(ushort)(address + 1)];
            return (ushort)((msb << 8) | lsb);
        }

        protected void Write16(ushort address, ushort value)
        {
            //convert natice ushort to big endian format in ram
            byte msb = (byte)(value >> 8);
            byte lsb = (byte)(0x00ff & value);
            _cpu.Ram[address] = msb;
            _cpu.Ram[(ushort)(address + 1)] = lsb;
        }

        protected bool Signed8(byte value)
        {
            return (value & 0x80) == 0x80;
        }

        protected bool Signed16(ushort value)
        {
            return (value & 0x8000) == 0x8000;
        }

        protected void Negative()
        {
            if (Signed8(_cpu._acc.Value))
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

        protected void Carry(int result, bool subtracting)
        {
            //we treat addition/subtraction as unsigned when setting the carry flag
            //we ignore the "overflow flag" for unsigned operations

            //"overflow" means will not fit into the unsigned 8 bit register
            // result > 255
            // result < 0

            //does the result "overflow" or "underflow" the register?
            //note: when adding, value can't be less than zero,
            //      and when subtracting, value can't be greater than 255

            bool carry = false;

            if ((result > 255) || (result < 0))
            {
                carry = true;
            }

            //wen subtracting (in bebop) the carry flag is treated as a "borrow-not" flag (inverted)
            if (subtracting)
            {
                carry = !carry;
            }

            if (carry)
            {
                _cpu._status.Set(Flags.Carry);
            }
            else
            {
                _cpu._status.Clear(Flags.Carry);
            }
        }

        protected void Overflow(byte a, byte b, byte r, bool subtracting)
        {
            /*

            we treat addition / subtraction as signed when setting the "overflow flag"
            we ignore carry for unsigned operations

            "overflow" means the result won't fit into a signed 2s complement representation
             result > 127
             result < -128

            overflow happens when the sign bits don't match the operation

            notation
              b = unsigned number(no sign bit)
             (b) = 2s complement negative signed number(sign bit set)

            example

                120  = 0111 1000
                10   = 0000 1010
                (10) = 1111 0110
                2    = 1111 0010
                (2)  = 1111 1110

                Adding two positives should not yield a negative (this is 001 below):

                    120 + 10 = 130 = (2)

                The result overflows the [-128 > r < 127] limits.

                Subtracting a negative from a positive should not yield a negative (011 below):

                    120 - (10) = 130 = (2) 

            */

            bool overflow = false;

            if (!subtracting)
            {
                /*

                ADDITION (aSign + bSign = rSign)

                    000  a + b = r
                    001  a + b = (r)        -->OVERFLOW
                    010  a + (b) = r
                    011  a + (b) = (r)
                    100 (a) + b = r
                    101 (a) + b = (r)
                    110 (a) + (b) = r       -->OVERFLOW
                    111 (a) + (b) = (r)

                */
                if (!Signed8(a) && !Signed8(b) && Signed8(r))
                {
                    overflow = true;
                }
                else if (Signed8(a) && Signed8(b) && !Signed8(r))
                {
                    overflow = true;
                }
            }
            else
            {
                /*
                
                SUBTRACTION

                    000  a - b = r
                    001  a - b = (r) 
                    010  a - (b) = r
                    011  a - (b) = (r)      -->OVERFLOW
                    100 (a) - b = r         -->OVERFLOW
                    101 (a) - b = (r)
                    110 (a) - (b) = r
                    111 (a) - (b) = (r)

                */
                if (!Signed8(a) && Signed8(b) && Signed8(r))
                {
                    overflow = true;
                }
                else if (Signed8(a) && !Signed8(b) && !Signed8(r))
                {
                    overflow = true;
                }

            }


            if (overflow)
            {
                _cpu._status.Set(Flags.Overflow);
            }
            else
            {
                _cpu._status.Clear(Flags.Overflow);
            }

        }

    }
}
