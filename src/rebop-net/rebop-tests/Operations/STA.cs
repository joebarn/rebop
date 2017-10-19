using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Rebop.Tests.Operations
{
    [TestFixture]
    class STA:Operation
    {

        [Test]
        public void STA_Abs()
        {
            Ram.Load(0x4000, new byte[] { 0x90, 0x88, 0x99, 0x50, 0x00 });

            Driver.Clock();
            Driver.Clock();
            Assert.AreEqual(Ram[0x5000], 0x88);
            

        }

        [Test]
        public void STA_AbsX()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x88, 0xa0, 0x00, 0x05, 0x9A, 0x50, 0x00 });

            Driver.Clock();
            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Ram[0x5005], 0x88);

        }

        [Test]
        public void STA_Ind()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x88, 0x9B, 0x50, 0x00 });
            Ram.Load(0x5000, new byte[] { 0x60, 0x00 });

            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Ram[0x6000], 0x88);


        }

        [Test]
        public void STA_XInd()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x88, 0xa0, 0x00, 0x05, 0x9C, 0x50, 0x00 });
            Ram.Load(0x5005, new byte[] { 0x60, 0x00 });

            Driver.Clock();
            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Ram[0x6000], 0x88);


        }

        [Test]
        public void STA_IndX()
        {

            Ram.Load(0x4000, new byte[] { 0x90, 0x88, 0xa0, 0x00, 0x05, 0x9D, 0x50, 0x00 });
            Ram.Load(0x5000, new byte[] { 0x60, 0x00 });

            Driver.Clock();
            Driver.Clock();
            Driver.Clock();

            Assert.AreEqual(Ram[0x6005], 0x88);


        }

    }
}
