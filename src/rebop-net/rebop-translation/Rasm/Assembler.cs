using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Rebop.Translation.Rasm
{
    public class Assembler
    {
        public static ParseTreeNode Parse(string rasm)
        {
            Grammar grammar = new Rebop.Translation.QueryGrammar();

            LanguageData language = new LanguageData(grammar);

            Parser parser = new Parser(language);

            ParseTree parseTree = parser.Parse(rasm);

            ParseTreeNode root = parseTree.Root;

            return root;
        }


        public static void Walk(ParseTreeNode node, int level=0)
        {

            for (int i = 0; i < level; i++)
            {
                Console.Write("  ");
            }

            Console.WriteLine(node);

            foreach (ParseTreeNode child in node.ChildNodes)
            {
                Walk(child, level + 1);
            }


        }
    }
}
