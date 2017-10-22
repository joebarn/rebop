using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Rebop.Translation.Rasm.Ast;
using Rebop.Translation.Rasm.Ast.Directives;
using Rebop.Translation.Rasm.Ast.Instructions;
using Rebop.Translation.Ast;

namespace Rebop.Translation.Rasm
{
    public class Assembler
    {
        public static ParseTreeNode Parse(string rasm)
        {
            Grammar grammar = new Rebop.Translation.Rasm.RasmGrammar();

            LanguageData language = new LanguageData(grammar);

            Parser parser = new Parser(language);

            ParseTree parseTree = parser.Parse(rasm);

            ParseTreeNode root = parseTree.Root;

            return root;
        }


        public static ROF Assemble(FileAstNode file)
        {
            /*
                Rules:

                .ORG and .END required
                file must start with EQUs or ORG
                EQUs must be before ORG
                EQUs must have labels
                file must end with END
                
                address labels are ushort
                constant labels (EQU) may be bytes or ushorts


            */
            List<byte> image = new List<byte>();
            Dictionary<string, object> labels = new Dictionary<string, object>();
            ushort? start = null;
            ushort? end = null;
            ushort here = 0;


            foreach (var statement in file.ChildNodes)
            {
                if (end != null)
                {
                    throw new AssembleException("END must be last statement in file", statement.SourceSpan);
                }

                LabelAstNode label = statement[typeof(LabelAstNode)] as LabelAstNode;

                if (statement is DirectiveAstNode)
                {
                    IDirective directive = statement[typeof(IDirective)] as IDirective;

                    if (directive is DeclarationAstNode)
                    {
                        if (start != null)
                        {
                            throw new AssembleException("EQU must be before ORG", statement.SourceSpan);
                        }

                        if (label == null)
                        {
                            throw new AssembleException("EQU must have a label", statement.SourceSpan);
                        }

                        IntegerAstNode integer = directive[typeof(IntegerAstNode)] as IntegerAstNode;

                        labels.Add(label.Value, integer.Value);
                    }
                    else
                    {

                        if (directive is OriginAstNode)
                        {
                            start = GetIntegerRef16(statement, (OriginAstNode) directive, labels);
                            here = start.Value;
                        }
                        else
                        {
                            if (start == null)
                            {
                                throw new AssembleException("ORG must be defined first", statement.SourceSpan);
                            }

                            if (directive is ReservationAstNodeBase)
                            {
                                //TODO
                            }
                            else if (directive is EndAstNode)
                            {
                                end = here;
                            }

                        }


                    }

                }
                else if (statement is InstructionAstNode)
                {

                }
                else
                {
                    throw new AssembleException("Line must be a statement.", statement.SourceSpan);
                }

            }

            if (end==null)
            {
                throw new InvalidOperationException("file must have END");
            }

            return new ROF { Start = start.Value, End = end.Value, Image = image.ToArray() };
        }

        private static ushort GetIntegerRef16(AstNodeBase statement, AstNodeBase integerRefNode, Dictionary<string, object> labels)
        {
            object value = GetIntegerRef(statement, integerRefNode, labels);

            return Convert.ToUInt16(value);
        }

        private static byte GetIntegerRef8(AstNodeBase statement, AstNodeBase integerRefNode, Dictionary<string, object> labels)
        {
            object value = GetIntegerRef(statement, integerRefNode, labels);

            if (value is byte)
            {
                return (byte)value;
            }
            else
            {
                throw new AssembleException("integer reference must be 8 bits", statement.SourceSpan);
            }
        }

        private static object GetIntegerRef(AstNodeBase statement, AstNodeBase integerRefNode, Dictionary<string, object> labels)
        {

            IntegerAstNode integer = integerRefNode[typeof(IntegerAstNode)] as IntegerAstNode;

            if (integer != null)
            {
                return integer.Value;
            }
            else
            {
                LabelAstNode l = integerRefNode[typeof(LabelAstNode)] as LabelAstNode;


                if (l!=null && labels.ContainsKey(l.Value))
                {
                    return labels[l.Value];
                }
                else
                {
                    throw new AssembleException("label must be decared before use", statement.SourceSpan);
                }

            }
        }

    }
}
