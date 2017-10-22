using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Rebop.Translation.Rasm
{
    public class AssembleException:Exception
    {
        public int Line { internal set; get; }
        public int Column { internal set; get; }
        public int Position { internal set; get; }

        public AssembleException(string message, SourceSpan sourceSpan) : base(message)
        {
            Line = sourceSpan.Location.Line+1;
            Column = sourceSpan.Location.Column+1;
            Position = sourceSpan.Location.Position;
        }

        public override string ToString()
        {
            return $"{Message} {Line}";
        }
    }
}
