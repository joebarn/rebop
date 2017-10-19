using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rebop.Tests.Operations
{
    [TestFixture]
    class NOP:Operation
    {

        [Test]
        public void Imp()
        {
            Driver.Reset();
            
            Ram[0x4000] = 0x00; //nop

            Driver.Clock();

            Assert.AreEqual(Cpu.Pc.Value, 0x4001);
            Assert.AreEqual(Cpu.Ir.Value, 0x00);


        }

    }
}
