using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Translation
{
    public class ROF
    {
        public ushort Start { internal set; get; }
        public ushort? Code { internal set; get; }
        public ushort End { internal set; get; }
        public byte[] Image { internal set; get; }
    }
}
