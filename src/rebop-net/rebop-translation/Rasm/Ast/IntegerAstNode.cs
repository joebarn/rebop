﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Translation.Ast;
using Irony.Parsing;

namespace Rebop.Translation.Rasm.Ast
{
    public class IntegerAstNode : AstNode, IIntegerRef
    {
        protected object _value;

        protected override void OnInit(ParseTreeNode parseTreeNode)
        {
            _value = parseTreeNode.Token.Value;
        }

        public override string ToString()
        {
            return $"{base.ToString()} {_value} [{_value.GetType().Name}]";
        }

        public object Value
        {
            get
            {
                return _value;
            }
        }

    }


}
