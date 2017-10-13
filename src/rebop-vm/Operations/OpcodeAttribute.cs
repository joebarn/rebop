using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Operations
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = true)]
    public class OpcodeAttribute : Attribute
    {
        public byte Opcode { get; private set; }
        public AddressingModes AddressingMode { get; private set; }

        public OpcodeAttribute(byte opcode, AddressingModes addresssingMode)
        {
            Opcode = opcode;
            AddressingMode = addresssingMode;
        }

    }
}
