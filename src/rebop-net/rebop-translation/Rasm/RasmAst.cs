using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Translation.Ast;
using Irony.Parsing;

namespace Rebop.Translation.Rasm
{
    public class FileAstNode : AstNode
    {

    }

    public class DirectiveAstNode : AstNode
    {

    }

    public abstract class ReservationAstNode : AstNode
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
    }

    public class ReservationStarAstNode : ReservationAstNode
    {

    }

    public class ReservationInitAstNode : ReservationAstNode
    {


    }
    public class DeclarationAstNode : AstNode
    {

    }
    public class OriginAstNode : AstNode
    {

    }
    public class EndAstNode : AstNode
    {

    }
    public class InstructionAstNode : AstNode
    {
    }

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

    public class ImmOperandAstNode : AstNode
    {

    }

    public class AbsOperandAstNode : AstNode
    {

    }

    public class AbsXOperandAstNode : AstNode
    {

    }

    public class IndOperandAstNode : AstNode
    {

    }

    public class XIndOperandAstNode : AstNode
    {

    }

    public class IndXOperandAstNode : AstNode
    {

    }

    public class IntegerAstNode : AstNode
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
    }

    public class LabelAstNode : AstNode
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
