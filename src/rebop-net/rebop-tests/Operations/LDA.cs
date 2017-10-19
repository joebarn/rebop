using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rebop.Tests.Operations
{
    [TestFixture]
    class LDA:Operation
    {

        [Test]
        public void Imm()
        {
            Driver.Reset();

            Ram.Load(0x4000, new byte[] { 0x90, 0x88 });

            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x88);


        }

    }
}
