using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm
{
    interface ICycle
    {
        void Fetch();
        void Decode();
        void Load();
        void Execute();
    }
}
