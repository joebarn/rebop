using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Translation.Ast;
using Irony.Parsing;

namespace Rebop.Translation.Rasm.Ast.Directives
{
    public abstract class ReservationAstNodeBase : AstNode
    {
        protected object _value;

        protected override void OnInit(ParseTreeNode parseTreeNode)
        {
            _value = parseTreeNode.ChildNodes[0].ChildNodes[0].Token.Value;
        }

        public override string ToString()
        {
            return $"{base.ToString()} \"{_value}\"";
        }

        public int Value
        {
            get
            {
                switch (_value.ToString())
                {
                    case ".byte":
                        return 1;
                    case ".2byte":
                        return 2;
                    case ".4byte":
                        return 4;
                    default:
                        throw new Exception("bad reservation");

                }

            }
        }
    }
}
