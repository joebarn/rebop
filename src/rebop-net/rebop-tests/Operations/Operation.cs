using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Vm;
using Rebop.Vm.Memory;

namespace Rebop.Tests.Operations
{
    abstract class Operation
    {
        protected Driver Driver => Rebop.Driver;
        protected ICpu Cpu => Rebop.Driver.Cpu;
        protected IRam Ram => Rebop.Driver.Cpu.Ram;

    }
}
