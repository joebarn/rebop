using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Ast;

namespace Rebop.Translation.Ast
{
    public abstract class AstNodeBase : IBrowsableAstNode
    {
        protected string _parseTreeText;
        protected BnfTerm _bnfTerm;
        protected SourceSpan _sourceSpan;
        protected AstNodeBase _parent;
        protected List<AstNodeBase> _childNodes = new List<AstNodeBase>();

        public override string ToString()
        {
            return $"{_bnfTerm.ToString()}";
        }

        public int Position
        {
            get
            {
                return _sourceSpan.Location.Position;
            }
        }

        public IEnumerable GetChildNodes()
        {
            return _childNodes;
        }

    }
}
