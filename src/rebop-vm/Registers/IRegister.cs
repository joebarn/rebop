using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Registers
{
    public interface IRegister<T>
    {
        T Value { get; }
        bool Dirty { get; }
    }
}
