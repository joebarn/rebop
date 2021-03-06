﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Translation.Ast;
using Irony.Parsing;

namespace Rebop.Translation.Rasm.Ast
{
    public class LabelAstNode : AstNode, IIntegerRef
    {
        protected object _value;

        protected override void OnInit(ParseTreeNode parseTreeNode)
        {
            _value = parseTreeNode.ChildNodes[0].Token.Value;
        }

        public override string ToString()
        {
            return $"{base.ToString()} \"{_value}\"";
        }

        public string Value
        {
            get
            {
                return _value.ToString();
            }
        }

    }
}
