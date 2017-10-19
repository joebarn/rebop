using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rebop.Tests.Operations
{
    [TestFixture]
    class OR:Operation
    {

        [Test]
        public void OR_Imm()
        {
            Ram.Load(0x4000, new byte[] { 0x90, 0x0A, 0x38, 0x04 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x0E);
            Assert.AreEqual(Cpu.Status.Carry, false);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, false);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        [Test]
        public void OR_Abs()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x0A, 0x39, 0x50, 0x00 });
            Ram.Load(0x5000, new byte[] { 0x04 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x0E);
            Assert.AreEqual(Cpu.Status.Carry, false);
            Assert.AreEqual(Cpu.Status.Overflow, false);
            Assert.AreEqual(Cpu.Status.Zero, false);
            Assert.AreEqual(Cpu.Status.Negative, false);

        }

        //[Test]
        //public void OR_AbsX()
        //{
            
        //    Ram.Load(0x4000, new byte[] { 0x90, 0x0A, 0x3A, 0x00, 0x05, 0x12, 0x50, 0x00 });
        //    Ram.Load(0x5000, new byte[] { 0x04 });

        //    Driver.Clock();
        //    Driver.Clock();
        //    Driver.Clock();

        //    Assert.AreEqual(Cpu.Acc.Value, 0x0E);
        //    Assert.AreEqual(Cpu.Status.Carry, false);
        //    Assert.AreEqual(Cpu.Status.Overflow, false);
        //    Assert.AreEqual(Cpu.Status.Zero, false);
        //    Assert.AreEqual(Cpu.Status.Negative, false);

        //}

    }
}
