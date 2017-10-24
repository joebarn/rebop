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
using Rebop.Vm;

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
                reserve statements must come between ORG and END (ORG doesn't mean the start of code, only the image)   

                address labels are ushort
                constant labels (EQU) may be bytes or ushorts


            */

            //cache instructions
            Dictionary<string, Instruction> instructions = new Dictionary<string, Instruction>();

            foreach (var instruction in Instruction.GetInstructions())
            {
                instructions.Add(instruction.Encode(), instruction);
            }

            byte[] image = new byte[0xFFFF + 1];

            Dictionary<string, Label> labels = new Dictionary<string, Label>();

            ushort? start = null;
            ushort? code = null;
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

                        labels.Add(label.Value, new Label(LabelType.Constant, integer.Value));
                    }
                    else
                    {

                        if (directive is OriginAstNode)
                        {
                            start = GetIntegerRef16(statement, (OriginAstNode)directive, labels);
                        }
                        else
                        {
                            if (start == null)
                            {
                                throw new AssembleException("ORG must be defined first", statement.SourceSpan);
                            }

                            if (directive is ReservationAstNodeBase)
                            {
                                if (label != null)
                                {
                                    labels.Add(label.Value, new Label(LabelType.Address, here + start.Value));
                                }

                                int byteSize = ((ReservationAstNodeBase)directive).Value;

                                if (directive is ReservationInitAstNode)
                                {
                                    var integers = ((AstNodeBase)directive).Find(typeof(IntegerAstNode));

                                    if (integers.Length == 0)
                                    {
                                        here += (ushort)byteSize;
                                    }
                                    else
                                    {
                                        foreach (var integer in integers)
                                        {
                                            var i = (IntegerAstNode)integer;

                                            if (byteSize == 1)
                                            {
                                                if (i.Value is ushort)
                                                {
                                                    throw new AssembleException("can't use 16 bit constants in .byte reservation", statement.SourceSpan);
                                                }

                                                image[here] = (byte)i.Value;
                                            }
                                            else if (byteSize == 2)
                                            {
                                                if (i.Value is uint)
                                                {
                                                    throw new AssembleException("can't use 32 bit constants in .2byte reservation", statement.SourceSpan);
                                                }

                                                var bytes = EndianUtils.FromNative(Convert.ToUInt16(i.Value));

                                                image[here] = bytes[0]; //msb;
                                                image[here + 1] = bytes[1]; //lsb;
                                            }
                                            else if (byteSize == 4)
                                            {
                                                var bytes = EndianUtils.FromNative(Convert.ToUInt32(i.Value));

                                                image[here] = bytes[0]; //msb;
                                                image[here + 1] = bytes[1]; 
                                                image[here + 2] = bytes[2]; 
                                                image[here + 3] = bytes[3]; //lsb;
                                            }
                                            else
                                            {
                                                throw new NotImplementedException("bad byte size");
                                            }

                                            here += (ushort)byteSize;

                                        }
                                    }


                                }
                                else if (directive is ReservationStarAstNode)
                                {
                                    uint bytes = GetIntegerRef32(statement, (AstNode)directive, labels);
                                    //TODO what if this overflows?
                                    here += (ushort)(bytes * byteSize);
                                }
                                else
                                {
                                    throw new InvalidOperationException("bad reservation directive");
                                }

                            }
                            else if (directive is EndAstNode)
                            {
                                end = (ushort?)(start + here - 1);
                            }

                        }


                    }

                }
                else if (statement is InstructionAstNode)
                {
                    if (label != null)
                    {
                        labels.Add(label.Value, new Label(LabelType.Address, here + start));
                    }

                    if (code==null)
                    {
                        code = (ushort)(here+start);
                    }

                    MnemonicAstNode mnemonic = statement[typeof(MnemonicAstNode)] as MnemonicAstNode;
                    IOperand operand= statement[typeof(IOperand)] as IOperand;
                    string encodedInstruction = EncodeInstruction(mnemonic.Value, operand);
                    Instruction instruction = instructions[encodedInstruction];

                    image[here] = instruction.OpCode;

                    if (instruction.Width==2)
                    {
                        image[here + 1] = GetIntegerRef8(statement, (AstNodeBase)operand, labels);
                    }
                    else if (instruction.Width==3)
                    {
                        ushort value = GetIntegerRef16(statement, (AstNodeBase)operand, labels);
                        var bytes = EndianUtils.FromNative(value);
                        image[here + 1] = bytes[0]; //msb;
                        image[here + 2] = bytes[1]; //lsb;
                    }

                    here += instruction.Width;

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

            return new ROF { Start = start.Value, Code=code, End = end.Value, Image = image.ToArray() };
        }

        private static uint GetIntegerRef32(AstNodeBase statement, AstNodeBase astNode, Dictionary<string, Label> labels)
        {
            object value = GetIntegerRef(statement, astNode, labels);
            return Convert.ToUInt32(value);
        }

        private static ushort GetIntegerRef16(AstNodeBase statement, AstNodeBase astNode, Dictionary<string, Label> labels)
        {
            object value = GetIntegerRef(statement, astNode, labels);

            if ((value is byte) || (value is ushort))
            {
                return Convert.ToUInt16(value);
            }
            else
            {
                throw new AssembleException("integer reference must be 16 bits", statement.SourceSpan);
            }


        }

        private static byte GetIntegerRef8(AstNodeBase statement, AstNodeBase astNode, Dictionary<string, Label> labels)
        {
            object value = GetIntegerRef(statement, astNode, labels);

            if (value is byte)
            {
                return (byte)value;
            }
            else
            {
                throw new AssembleException("integer reference must be 8 bits", statement.SourceSpan);
            }
        }

        private static object GetIntegerRef(AstNodeBase statement, AstNodeBase astNode, Dictionary<string, Label> labels)
        {

            IIntegerRef integerRef = astNode[typeof(IIntegerRef)] as IIntegerRef;

            if (integerRef is IntegerAstNode)
            {
                return ((IntegerAstNode)integerRef).Value;
            }
            else if (integerRef is LabelAstNode)
            {
                LabelAstNode label = integerRef as LabelAstNode;

                if (label != null && labels.ContainsKey(label.Value))
                {
                    return labels[label.Value].Integer;
                }
                else
                {
                    throw new AssembleException("label must be decared before use", statement.SourceSpan);
                }

            }
            else
            {
                throw new AssembleException("bad integer ref", statement.SourceSpan);
            }
        }

        private static string EncodeInstruction(string mnemonic, IOperand operand)
        {
            AddressingModes addressingMode;

            if (operand is ImmOperandAstNode)
            {
                if (mnemonic.ToUpper().StartsWith("B"))
                {
                    addressingMode = AddressingModes.BigImmediate;
                }
                else
                {
                    addressingMode = AddressingModes.Immediate;
                }

            }
            else if (operand is AbsOperandAstNode)
            {
                if (mnemonic.ToUpper().StartsWith("B"))
                {
                    addressingMode = AddressingModes.BigAbsolute;
                }
                else
                {
                    addressingMode = AddressingModes.Absolute;
                }
            }
            else if (operand is AbsXOperandAstNode)
            {
                addressingMode = AddressingModes.AbsoluteIndexed;
            }
            else if (operand is IndOperandAstNode)
            {
                addressingMode = AddressingModes.Indirect;
            }
            else if (operand is XIndOperandAstNode)
            {
                addressingMode = AddressingModes.IndirectPreIndexed;
            }
            else if (operand is IndXOperandAstNode)
            {
                addressingMode = AddressingModes.IndirectPostIndexed;
            }
            else
            {
                addressingMode = AddressingModes.Implied;
            }

            return Instruction.Encode(mnemonic, addressingMode);
        }

        

    }
}
