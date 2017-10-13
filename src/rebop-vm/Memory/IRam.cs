using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Memory
{
    public interface IRam
    {
        void Reset();
        byte this[ushort address] { get; set; }
        ushort? LastRead { get; }
        ushort? LastWrite { get; }
        IList<IRegion> Regions { get; }
    }
}
