using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Memory
{
    public interface IRegion
    {
        ushort Start { get; }
        ushort End { get; }
        DateTime Last { get; }
    }
}
