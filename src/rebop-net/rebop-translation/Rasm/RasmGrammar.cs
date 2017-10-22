using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using Rebop.Translation.Ast;
using Rebop.Translation.Rasm.Ast;
using Rebop.Translation.Rasm.Ast.Instructions;
using Rebop.Translation.Rasm.Ast.Directives;

namespace Rebop.Translation.Rasm
{

    [Language("Rasm", "1.0", "Wtf?")]
    public class RasmGrammar : Grammar
    {
        public override void BuildAst(LanguageData language, ParseTree parseTree)
        {
            if (LanguageFlags.IsSet(LanguageFlags.CreateAst))
            {
                var astContext = new AstContext(language);
                astContext.DefaultNodeType = typeof(DefaultAstNode);
                astContext.DefaultLiteralNodeType= typeof(DefaultAstNode);
                astContext.DefaultIdentifierNodeType = typeof(DefaultAstNode);


                var astBuilder = new AstBuilder(astContext);
                astBuilder.BuildAst(parseTree);
            }
        }

        public RasmGrammar() : base(false)
        {
            Rebop();
        }

        private void Rebop()
        {
            GrammarComments = @"This grammar is based on the BNF provided in Appendix E of 'The Official Beboputer Microprocessor Databook' Copyright © 1998, Maxfield & Montrose Interactive Inc.";

            //comments
            CommentTerminal eolComment = new CommentTerminal("eolComment", "//", "\n", "\r");
            NonGrammarTerminals.Add(eolComment);
            CommentTerminal multilineComment = new CommentTerminal("multilineComment", "/*", "*/");
            NonGrammarTerminals.Add(multilineComment);

            NumberLiteral integer = new NumberLiteral("integer", NumberOptions.IntOnly, typeof(IntegerAstNode));
            integer.DefaultIntTypes = new TypeCode[] { TypeCode.Byte, TypeCode.UInt16 };
            integer.AddPrefix("%", NumberOptions.Binary);
            integer.AddPrefix("$", NumberOptions.Hex);

            IdentifierTerminal label_terminal = new IdentifierTerminal("label_terminal");

            NonTerminal label = new NonTerminal("label", typeof(LabelAstNode));
            label.Rule = label_terminal;


            NonTerminal integer_ref = new NonTerminal("integer_ref");
            integer_ref.Rule = integer | label;

            //expressions (todo)


            //mnemonics
            NonTerminal mnemonic = new NonTerminal("mnemonic",typeof(MnemonicAstNode));
            mnemonic.Rule = ToTerm("ADD") | "BLDX" | "HALT" | "INCA" | "LDA" | "NOP" | "OR" | "STA" | "SUB";


            //operands
            NonTerminal imm_operand = new NonTerminal("imm_operand", typeof(ImmOperandAstNode));
            imm_operand.Rule = integer_ref;

            NonTerminal abs_operand = new NonTerminal("abs_operand", typeof(AbsOperandAstNode));
            abs_operand.Rule = "[" + integer_ref + "]";

            NonTerminal absx_operand = new NonTerminal("absx_operand", typeof(AbsXOperandAstNode));
            absx_operand.Rule = "[" + integer_ref + ","+"X"+"]";

            NonTerminal ind_operand = new NonTerminal("ind_operand", typeof(IndOperandAstNode));
            ind_operand.Rule = "["+"[" + integer_ref + "]" + "]";

            NonTerminal xind_operand = new NonTerminal("xind_operand", typeof(XIndOperandAstNode));
            xind_operand.Rule = "[" + "[" + integer_ref + "," + "X" + "]" + "]";

            NonTerminal indx_operand = new NonTerminal("indx_operand", typeof(IndXOperandAstNode));
            indx_operand.Rule = "[" + "[" + integer_ref + "]" + "," + "X" + "]";

            NonTerminal operand = new NonTerminal("operand");
            operand.Rule = imm_operand | abs_operand | absx_operand | ind_operand | xind_operand | indx_operand;

            NonTerminal instruction = new NonTerminal("instruction", typeof(InstructionAstNode));
            instruction.Rule = ((label + ":") | Empty)+ (mnemonic + (operand | Empty));

            //directives
            NonTerminal origin = new NonTerminal("origin", typeof(OriginAstNode));
            origin.Rule = ".org" + integer_ref;

            NonTerminal end = new NonTerminal("end", typeof(EndAstNode));
            end.Rule = ".end";

            NonTerminal declaration = new NonTerminal("declaration", typeof(DeclarationAstNode));
            declaration.Rule =".equ" + integer;

            NonTerminal reservation_star = new NonTerminal("reservation_star",typeof(ReservationStarAstNode));
            reservation_star.Rule = ((ToTerm(".byte") | ".2byte" | ".4byte")) + "*"+integer;

            NonTerminal byte_list_star = new NonTerminal("byte_list_star");
            MakeStarRule(byte_list_star, "," + integer);

            NonTerminal byte_list = new NonTerminal("byte_list");
            byte_list.Rule = integer + byte_list_star;

            NonTerminal reservation_init = new NonTerminal("reservation_init", typeof(ReservationInitAstNode));
            reservation_init.Rule = (ToTerm(".byte") | ".2byte" | ".4byte")+(byte_list | Empty);

            NonTerminal directive = new NonTerminal("directive", typeof(DirectiveAstNode));
            directive.Rule = ((label + ":") | Empty) + (origin | end | (reservation_star| reservation_init) |  declaration);

            //statements
            NonTerminal statement = new NonTerminal("statement");
            //statement.Rule = NewLineStar + ((label + ":") | Empty) + (instruction| directive)  + (NewLinePlus|Eof);
            statement.Rule = NewLineStar +  (instruction | directive) + (NewLinePlus | Eof);

            //file structure
            NonTerminal assembly_source_file = new NonTerminal("assembly_source_file",typeof(FileAstNode));
            assembly_source_file.Rule =  MakeStarRule(assembly_source_file, statement);

            Root = assembly_source_file;

            LanguageFlags |= LanguageFlags.CreateAst;
        }

    }
}
