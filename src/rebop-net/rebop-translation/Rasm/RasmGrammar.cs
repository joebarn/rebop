using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Rebop.Translation.Rasm
{
    public class DefaultNode { }

    public class Node : IBrowsableAstNode
    {
        protected string _parseTreeText;
        protected BnfTerm _bnfTerm;
        protected SourceSpan _sourceSpan;
        protected List<Node> _childNodes = new List<Node>();

        public override string ToString()
        {
            return $"{_bnfTerm.ToString()} [{GetType().Name}:{_bnfTerm.GetType().Name}] \"{_parseTreeText}\" {{{_bnfTerm.Flags.ToString()}}}";
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

    public class _NONAST_ : Node
    {
        public _NONAST_(string parseTreeText, BnfTerm bnfTerm, SourceSpan sourceSpan)
        {
            _parseTreeText = parseTreeText;
            _bnfTerm = bnfTerm;
            _sourceSpan = sourceSpan;
        }

        public override string ToString()
        {
            return $"<{_bnfTerm.Name}> {base.ToString()}";
        }

    }

    public class AstNodeInit : Node, IAstNodeInit
    {


        public virtual void Init(AstContext context, ParseTreeNode parseTreeNode)
        {
            _parseTreeText = parseTreeNode.ToString();
            _bnfTerm = parseTreeNode.Term;
            _sourceSpan = parseTreeNode.Span;

            AddImmediateAstNodes(_childNodes, parseTreeNode);
        }

        protected void AddImmediateAstNodes(List<Node> childNodes, ParseTreeNode parseTreeNode)
        {
            foreach (ParseTreeNode ptn in parseTreeNode.ChildNodes)
            {
                if (ptn.AstNode != null)
                {
                    if (ptn.AstNode is AstNodeInit)
                    {
                        childNodes.Add((AstNodeInit)ptn.AstNode);
                    }
                    else
                    {
                        AddImmediateAstNodes(childNodes, ptn);
                    }
                }
                else
                {
                    childNodes.Add(new _NONAST_(ptn.ToString(), ptn.Term, ptn.Span));
                }
            }

        }

    }

    public class RootNode : AstNodeInit
    {

    }

    public class TokenNode : AstNodeInit
    {
        protected string _value;

        public override string ToString()
        {
            return $"{base.ToString()} : {_value}";
        }

        public override void Init(AstContext context, ParseTreeNode parseTreeNode)
        {
            base.Init(context, parseTreeNode);

            //_value = parseTreeNode.ChildNodes[0].Token.Text;
        }

    }

    public class LabelNode : TokenNode { }
    public class LiteralNode : TokenNode { }
    public class IntegerRefNode : AstNodeInit
    {
        protected string _value;

        public override string ToString()
        {
            return $"{base.ToString()} : {_value}";
        }

        public override void Init(AstContext context, ParseTreeNode parseTreeNode)
        {
            base.Init(context, parseTreeNode);

            _value = parseTreeNode.ChildNodes[0].ChildNodes[0].Token.Text;
        }

    }
    public class MnemonicNode : TokenNode { }
    public class InstructionNode : AstNodeInit { }

    [Language("JoeLanguage", "1.0", "Wtf?")]
    public class RasmGrammar : Grammar
    {

        public override void BuildAst(LanguageData language, ParseTree parseTree)
        {
            if (LanguageFlags.IsSet(LanguageFlags.CreateAst))
            {
                var astContext = new AstContext(language);
                astContext.DefaultNodeType = typeof(DefaultNode);


                var astBuilder = new AstBuilder(astContext);
                astBuilder.BuildAst(parseTree);
            }
        }

        public RasmGrammar() : base(false)
        {
            Rebop();



        }

        private void Test()
        {
            //RegexBasedTerminal alpha_char = new RegexBasedTerminal("alpha_char", "[A-Z]|[a-z]");
            KeyTerm alpha_char = ToTerm("a");

            KeyTerm joe = ToTerm("joe");
            KeyTerm foo = ToTerm("foo");

            NonTerminal root = new NonTerminal("root", typeof(RootNode));

            //NonTerminal things1 = new NonTerminal("things1");
            //things1.Rule = (foo | joe | "bar" | alpha_char);
            //NonTerminal thingsstar = new NonTerminal("things*");
            //thingsstar.Rule = MakeStarRule(thingsstar, null, things1);

            NonTerminal thingsstar = new NonTerminal("things*");
            MakeStarRule(thingsstar, null, (foo | joe | "bar" | alpha_char));

            root.Rule = thingsstar;

            Root = root;

            LanguageFlags |= LanguageFlags.CreateAst;
        }


        private void Rebop()
        {
            GrammarComments = @"This grammar is based on the BNF provided in Appendix E of 'The Official Beboputer Microprocessor Databook' Copyright © 1998, Maxfield & Montrose Interactive Inc.";

            //comments
            CommentTerminal eolComment = new CommentTerminal("eolComment", "//", "\n", "\r");
            NonGrammarTerminals.Add(eolComment);
            CommentTerminal multilineComment = new CommentTerminal("multilineComment", "/*", "*/");
            NonGrammarTerminals.Add(multilineComment);

            NumberLiteral number = new NumberLiteral("number", NumberOptions.IntOnly);
            number.DefaultIntTypes = new TypeCode[] { TypeCode.Byte, TypeCode.UInt16 };
            number.AddPrefix("%", NumberOptions.Binary);
            number.AddPrefix("$", NumberOptions.Hex);

            IdentifierTerminal label = new IdentifierTerminal("label");

            NonTerminal integer_ref = new NonTerminal("integer_ref", typeof(IntegerRefNode));
            integer_ref.Rule = number | label;

            //expressions (todo)


            //mnemonics
            NonTerminal mnemonic = new NonTerminal("mnemonic", typeof(MnemonicNode));
            mnemonic.Rule = ToTerm("ADD") | "BLDX" | "HALT" | "INCA" | "LDA" | "NOP" | "OR" | "STA" | "SUB";


            //operands
            NonTerminal imm_operand = new NonTerminal("imm_operand");
            imm_operand.Rule = integer_ref;

            NonTerminal abs_operand = new NonTerminal("abs_operand");
            abs_operand.Rule = "[" + integer_ref + "]";

            NonTerminal absx_operand = new NonTerminal("absx_operand");
            absx_operand.Rule = "[" + integer_ref + "," + "X" + "]";

            NonTerminal ind_operand = new NonTerminal("ind_operand");
            ind_operand.Rule = "[" + "[" + integer_ref + "]" + "]";

            NonTerminal xind_operand = new NonTerminal("xind_operand");
            xind_operand.Rule = "[" + "[" + integer_ref + "," + "X" + "]" + "]";

            NonTerminal indx_operand = new NonTerminal("indx_operand");
            indx_operand.Rule = "[" + "[" + integer_ref + "]" + "," + "X" + "]";

            NonTerminal operand = new NonTerminal("operand", typeof(MnemonicNode));
            operand.Rule = imm_operand | abs_operand | absx_operand | ind_operand | xind_operand | indx_operand;

            NonTerminal instruction = new NonTerminal("instruction", typeof(InstructionNode));
            instruction.Rule = NewLineStar + ((mnemonic + operand) | mnemonic) + (NewLinePlus | Eof);


            //file structure
            NonTerminal assembly_source_file = new NonTerminal("assembly_source_file", typeof(RootNode));
            assembly_source_file.Rule = MakeStarRule(assembly_source_file, instruction);

            Root = assembly_source_file;

            //LanguageFlags |= LanguageFlags.CreateAst;
        }

    }
}
