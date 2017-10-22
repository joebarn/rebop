using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Rebop.Translation.Ast
{
    public class KeyTermAstNode : AstNodeBase
    {
        public KeyTermAstNode(string parseTreeText, BnfTerm bnfTerm, SourceSpan sourceSpan)
        {
            _parseTreeText = parseTreeText;
            _bnfTerm = bnfTerm;
            _sourceSpan = sourceSpan;
        }

        public override string ToString()
        {
            return $"\"{_bnfTerm.Name}\"";
        }

    }
}
