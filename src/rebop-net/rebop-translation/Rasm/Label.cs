using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Translation.Rasm
{
    class Label
    {
        public LabelType LabelType { private set; get; }
        public object Integer { private set; get; }

        public Label(LabelType labelType, object integer)
        {
            LabelType = labelType;
            Integer = integer;
        }

    }
}
