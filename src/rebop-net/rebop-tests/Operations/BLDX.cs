using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rebop.Tests.Operations
{
    [TestFixture]
    class BLDX:Operation
    {

        [Test]
        public void BLDX_Imm()
        {

            Ram.Load(0x4000, new byte[] { 0xA0, 0x77, 0x88 });

            Driver.Clock();

            Assert.AreEqual(Cpu.X.Value, 0x7788);

        }

        [Test]
        public void BLDX_Abs()
        {

            Ram.Load(0x4000, new byte[] { 0xA1, 0x50, 0x00 });
            Ram.Load(0x5000, new byte[] { 0x77, 0x88 });

            Driver.Clock();

            Assert.AreEqual(Cpu.X.Value, 0x7788);

        }


    }
}
