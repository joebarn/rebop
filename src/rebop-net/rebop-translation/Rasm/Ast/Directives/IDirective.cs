﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Translation.Ast;

namespace Rebop.Translation.Rasm.Ast.Directives
{
    public interface IDirective
    {
        AstNodeBase this[Type type] { get; }
    }
}
