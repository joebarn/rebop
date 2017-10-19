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
        public void STA_Imm()
        {

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void STA_Abs()
        {

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void STA_AbsX()
        {

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void STA_Ind()
        {

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void STA_XInd()
        {

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void STA_IndX()
        {

            Assert.AreEqual(1, 0x88);

        }

    }
}
