using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rebop.Tests.Operations
{
    [TestFixture]
    class SUB:Operation
    {

        [Test]
        public void SUB_Imm()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x02, 0x20, 0x02 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x00);
            Assert.AreEqual(Cpu.Status.Carry, true);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, true);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        [Test]
        public void SUB_Abs()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x02, 0x21, 0x50, 0x00 });
            Ram.Load(0x5000, new byte[] { 0x02 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x00);
            Assert.AreEqual(Cpu.Status.Carry, true);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, true);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        [Test]
        public void SUB_AbsX()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x02, 0xa0, 0x00, 0x05, 0x22, 0x50, 0x00 });
            Ram.Load(0x5005, new byte[] { 0x02 });

            Driver.Clock();
            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x00);
            Assert.AreEqual(Cpu.Status.Carry, true);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, true);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        [Test]
        public void SUB_Carry1()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x04, 0x20, 0x02 });
            Driver.Clock();
            Driver.Clock();
            Assert.AreEqual(Cpu.Acc.Value, 0x02);
            Assert.AreEqual(Cpu.Status.Carry, true);
            Assert.AreEqual(Cpu.Status.Overflow, false);

        }

        [Test]
        public void SUB_Carry2()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x04, 0x20, 0x05 });
            Driver.Clock();
            Driver.Clock();
            Assert.AreEqual(Cpu.Acc.Value, 0xFF);
            Assert.AreEqual(Cpu.Status.Carry, false);
            Assert.AreEqual(Cpu.Status.Overflow, false);

        }

        [Test]
        public void SUB_Overflow1()
        {
            //120 - (10) = 130 = (2) 
            //x78 - xF6 = x82

            Ram.Load(0x4000, new byte[] { 0x90, 0x78, 0x20, 0xF6 });
            Driver.Clock();
            Driver.Clock();
            Assert.AreEqual(Cpu.Status.Carry, false);
            Assert.AreEqual(Cpu.Status.Overflow, true);

        }


    }
}
