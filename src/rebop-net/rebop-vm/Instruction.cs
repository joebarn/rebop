using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rebop.Vm.Operations;

namespace Rebop.Vm
{
    public class Instruction
    {
        public byte OpCode { internal set; get; }
        public string Mnemonic { internal set; get; }
        public AddressingModes AddressingMode { internal set; get; }
        public ushort Width { internal set; get; }

        public static Instruction[] GetInstructions()
        {
            List<Instruction> instructions = new List<Instruction>();

            List<Type> operationTypes = Assembly.GetAssembly(typeof(Operation)).GetTypes().Where(type => type.IsSubclassOf(typeof(Operation))).ToList<Type>();

            foreach (Type operationType in operationTypes)
            {
                OpcodeAttribute[] oas = (OpcodeAttribute[])Attribute.GetCustomAttributes(operationType, typeof(OpcodeAttribute));

                foreach (OpcodeAttribute oa in oas)
                {
                    instructions.Add(new Instruction { OpCode = oa.Opcode, Mnemonic = operationType.Name, AddressingMode = oa.AddressingMode, Width = Operation.GetWidth(oa.AddressingMode) });
                }

            }

            return instructions.ToArray();
        }

        public static string Encode(string mnemonic, AddressingModes addressingMode)
        {
            mnemonic = mnemonic.ToUpper();
            string addressingPart;

            if (addressingMode == AddressingModes.Immediate)
            {
                addressingPart = "imm";
            }
            else if (addressingMode == AddressingModes.BigImmediate)
            {
                addressingPart = "imm-b";
            }
            else if (addressingMode == AddressingModes.Absolute)
            {
                addressingPart = "abs";
            }
            else if (addressingMode == AddressingModes.BigAbsolute)
            {
                addressingPart = "abs-b";
            }
            else if (addressingMode == AddressingModes.AbsoluteIndexed)
            {
                addressingPart = "abs-x";
            }
            else if (addressingMode == AddressingModes.Indirect)
            {
                addressingPart = "ind";
            }
            else if (addressingMode == AddressingModes.IndirectPreIndexed)
            {
                addressingPart = "x-ind";
            }
            else if (addressingMode == AddressingModes.IndirectPostIndexed)
            {
                addressingPart = "ind-x";
            }
            else
            {
                addressingPart = "imp";
            }

            return $"{mnemonic}-{addressingPart}";
        }

        public string Encode()
        {
            return Encode(Mnemonic, AddressingMode);
        }

        public override string ToString()
        {
            return Encode();
        }

    }
}
