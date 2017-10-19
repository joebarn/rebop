using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rebop.Vm;

namespace Rebop.Tests
{
    [SetUpFixture]
    public class Rebop
    {
        public static Driver Driver { get; } = new Driver();

        [OneTimeSetUp]  
        public static void SetUp()
        {
            Driver.Start();
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            Driver.Shutdown();
        }
    }
}
