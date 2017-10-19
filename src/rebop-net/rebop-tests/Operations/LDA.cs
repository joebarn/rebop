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
        public void LDA_Imm()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x88 });

            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x88);

        }

        [Test]
        public void LDA_Abs()
        {

            Ram.Load(0x4000, new byte[] { 0x91, 0x50, 0x00 });
            Ram.Load(0x5000, new byte[] { 0x88 });

            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x88);

        }

        [Test]
        public void LDA_AbsX()
        {

            Ram.Load(0x4000, new byte[] { 0xa0, 0x00, 0x05, 0x92, 0x50, 0x00 });
            Ram.Load(0x5005, new byte[] { 0x88 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x88);

        }

        [Test]
        public void LDA_Ind()
        {

            Ram.Load(0x4000, new byte[] { 0x93, 0x50, 0x00 });
            Ram.Load(0x5000, new byte[] { 0x60, 0x00 });
            Ram.Load(0x6000, new byte[] { 0x88 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x88);

        }

        [Test]
        public void LDA_XInd()
        {

            Ram.Load(0x4000, new byte[] { 0xa0, 0x00, 0x05, 0x94, 0x50, 0x00 });
            Ram.Load(0x5005, new byte[] { 0x60, 0x00 });
            Ram.Load(0x6000, new byte[] { 0x88 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x88);

        }

        [Test]
        public void LDA_IndX()
        {

            Ram.Load(0x4000, new byte[] { 0xa0, 0x00, 0x05, 0x95, 0x50, 0x00 });
            Ram.Load(0x5000, new byte[] { 0x60, 0x00 });
            Ram.Load(0x6005, new byte[] { 0x88 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Cpu.Acc.Value, 0x88);

        }

    }
}
