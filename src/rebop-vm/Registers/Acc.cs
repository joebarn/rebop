﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebop.Vm.Registers
{
    class Acc:Register<byte>
    {

        public override string ToString()
        {
            return _value.ToString("X2");
        }
    }
}