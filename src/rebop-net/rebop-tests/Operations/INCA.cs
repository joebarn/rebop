using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rebop.Tests.Operations
{
    [TestFixture]
    class INCA:Operation
    {

        [Test]
        public void INCA_Imp()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x05, 0x80, 0x80 });

            Driver.Clock();
            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x07);

        }



    }
}
