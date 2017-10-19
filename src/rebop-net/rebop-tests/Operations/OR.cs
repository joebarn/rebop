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

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void OR_Abs()
        {

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void OR_AbsX()
        {

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void OR_Ind()
        {

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void OR_XInd()
        {

            Assert.AreEqual(1, 0x88);

        }

        [Test]
        public void OR_IndX()
        {

            Assert.AreEqual(1, 0x88);

        }

    }
}
