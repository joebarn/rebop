using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm
{
    public class Instruction
    {
        public byte OpCode { internal set; get; }
        public string Mnemonic { internal set; get; }
        public AddressingModes AddressingMode { internal set; get; }
        public int Width { internal set; get; }
    }
}
