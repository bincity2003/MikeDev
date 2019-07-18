using MikeDev.Config;
using NUnit.Framework;

namespace MikeDev.Test
{
    [TestFixture]
    internal class CConfigTest
    {
        /// <summary>
        /// This test requires CConfig to correctly create new empty config.
        /// </summary>
        [Test]
        public void TestA()
        {
            CConfig config = new CConfig();
            Assert.IsTrue(config.Count == 0);
            Assert.IsTrue(config.Attributes.Length == 0);
        }
    }
}