using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Translation.Ast;
using Irony.Parsing;

namespace Rebop.Translation.Rasm.Ast.Instructions
{
    public class MnemonicAstNode : AstNode
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
    }
}
