using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rebop.Tests.Operations
{
    [TestFixture]
    class ADD:Operation
    {

        [Test]
        public void ADD_Imm()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x02, 0x10, 0x02 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x04);
            Assert.AreEqual(Cpu.Status.Carry, false);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, false);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        [Test]
        public void ADD_Abs()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x02, 0x11, 0x50, 0x00 });
            Ram.Load(0x5000, new byte[] { 0x02 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x04);
            Assert.AreEqual(Cpu.Status.Carry, false);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, false);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        [Test]
        public void ADD_AbsX()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x02, 0xa0, 0x00, 0x05, 0x12, 0x50, 0x00 });
            Ram.Load(0x5005, new byte[] { 0x02 });

            Driver.Clock();
            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x04);
            Assert.AreEqual(Cpu.Status.Carry, false);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, false);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        [Test]
        public void ADD_Carry1()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x02, 0x10, 0x02 });
            Driver.Clock();
            Driver.Clock();
            Assert.AreEqual(Cpu.Acc.Value, 0x04);
            Assert.AreEqual(Cpu.Status.Carry, false);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, false);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        [Test]
        public void ADD_Carry2()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0xFF, 0x10, 0x01 });
            Driver.Clock();
            Driver.Clock();
            Assert.AreEqual(Cpu.Acc.Value, 0x00);
            Assert.AreEqual(Cpu.Status.Carry, true);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, true);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        [Test]
        public void ADD_Overflow1()
        {
            //120 + 10 = 130 = (2)
            //x78 + x0A = x82

            Ram.Load(0x4000, new byte[] { 0x90, 0x78, 0x10, 0x0A });
            Driver.Clock();
            Driver.Clock();
            Assert.AreEqual(Cpu.Acc.Value, 0x82);
            Assert.AreEqual(Cpu.Status.Carry, false);
            Assert.AreEqual(Cpu.Status.Overflow, true);
            Assert.AreEqual(Cpu.Status.Zero, false);
            Assert.AreEqual(Cpu.Status.Negative, true);

        }


    }
}
