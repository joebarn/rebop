using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rebop.Tests.Operations
{
    [TestFixture]
    class HALT:Operation
    {

        [Test]
        public void NOP_Imp()
        {
            Ram[0x4000] = 0x00; //nop
            Ram[0x4001] = 0x00; //nop
            Ram[0x4002] = 0x01; //halt

            Driver.Clock();
            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Driver.IsHalted, true);


        }

    }
}
