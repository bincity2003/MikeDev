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

        /// <summary>
        /// This test requires CConfig to correctly add new attribute(s).
        /// </summary>
        [Test]
        public void TestB()
        {
            CConfig config = new CConfig();
            config.Add("name", "Mike");

            Assert.IsTrue(config.Count == 1);
            Assert.IsTrue(config.Attributes[0] == "name");
            Assert.IsTrue(config["name"] == "Mike");
        }
    }
}