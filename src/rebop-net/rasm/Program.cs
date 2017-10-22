using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebop.Translation.Rasm;

namespace rasm
{
    class Program
    {
        static void Main(string[] args)
        {
            var root=Assembler.Parse("[foo].[ha] = 1 AND [foo].[ha] = 1");
            Console.WriteLine($"valid = {root!=null}");

            //Assembler.Assemble(root);
        }


       

    }
}
