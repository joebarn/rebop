using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Registers
{
    public interface IStatus
    {
        bool IntMask { get; }
        bool Overflow { get; }
        bool Negative { get; }
        bool Zero { get; }
        bool Carry { get; }
    }
}
