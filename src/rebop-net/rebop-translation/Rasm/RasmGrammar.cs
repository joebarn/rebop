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

            //labels    
            RegexBasedTerminal label_terminal = new RegexBasedTerminal("label", "(_|[A-Z]|[a-z])([A-Z]|[a-z]|[0-9])*");
            NonTerminal label = new NonTerminal("label", typeof(LabelNode));
            label.Rule = label_terminal;

            //comments
            CommentTerminal eolComment = new CommentTerminal("eolComment", "//", "\n", "\r");
            NonGrammarTerminals.Add(eolComment);
            CommentTerminal multilineComment = new CommentTerminal("multilineComment", "/*", "*/");
            NonGrammarTerminals.Add(multilineComment);

            //literals
            RegexBasedTerminal decimal_terminal = new RegexBasedTerminal("decimal_terminal", "[0-9]*");
            NonTerminal decimal_literal = new NonTerminal("decimal_literal", typeof(LiteralNode));
            decimal_literal.Rule = decimal_terminal;

            RegexBasedTerminal hex_terminal = new RegexBasedTerminal("hex_terminal", "\\$(([0-9]|[a-f]|[A-F]){4}|([0-9]|[a-f]|[A-F]){2})");
            NonTerminal hex_literal = new NonTerminal("hex_literal", typeof(LiteralNode));
            hex_literal.Rule = hex_terminal;

            RegexBasedTerminal binary_terminal = new RegexBasedTerminal("binary_terminal", "%((0|1){16}|(0|1){8})");
            NonTerminal binary_literal = new NonTerminal("binary_literal", typeof(LiteralNode));
            binary_literal.Rule = binary_terminal;

            NonTerminal integer_ref = new NonTerminal("integer_ref", typeof(IntegerRefNode));
            integer_ref.Rule = decimal_literal | hex_literal | binary_literal | label;

            //expressions (todo)

            //mnemonics
            NonTerminal mnemonic = new NonTerminal("mnemonic", typeof(MnemonicNode));
            mnemonic.Rule = ToTerm("ADD") | "BLDX" | "HALT" | "INCA" | "LDA" | "NOP" | "OR" | "STA" | "SUB";

            NonTerminal abs_operand = new NonTerminal("abs_operand");
            abs_operand.Rule = "[" + integer_ref + "]";

            NonTerminal operand = new NonTerminal("operand", typeof(MnemonicNode));
            operand.Rule = abs_operand;

            NonTerminal instruction = new NonTerminal("instruction", typeof(InstructionNode));
            instruction.Rule = (mnemonic + operand | mnemonic + integer_ref | mnemonic) + ";";


            //file structure
            NonTerminal assembly_source_file = new NonTerminal("assembly_source_file", typeof(RootNode));
            assembly_source_file.Rule = MakeStarRule(assembly_source_file, instruction);

            Root = assembly_source_file;

            //LanguageFlags |= LanguageFlags.CreateAst;
        }

    }
}
