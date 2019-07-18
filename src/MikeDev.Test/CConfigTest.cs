using System;
using MikeDev.Config;
using NUnit.Framework;

namespace MikeDev.Test
{
    [TestFixture]
    internal class CConfigTest
    {
        CConfig config;

        /// <summary>
        /// This test requires CConfig to correctly create new empty config.
        /// </summary>
        [Test]
        public void TestA()
        {
            config = new CConfig();
            Assert.IsTrue(config.Count == 0);
            Assert.IsTrue(config.Attributes.Length == 0);
        }

        /// <summary>
        /// This test requires CConfig to correctly add and read new attribute(s).
        /// </summary>
        [Test]
        public void TestB()
        {
            config = new CConfig();
            config.Add("name", "Mike");

            Assert.IsTrue(config.Count == 1);
            Assert.IsTrue(config.Attributes[0] == "name");
            Assert.IsTrue(config["name"] == "Mike");

            config.Add("age", "16");

            Assert.IsTrue(config.Count == 2);
            Assert.IsTrue(config.Attributes[1] == "age");
            Assert.IsTrue(config["age"] == "16");
        }

        /// <summary>
        /// This test requires CConfig to correctly remove attribute(s).
        /// </summary>
        [Test]
        public void TestC()
        {
            config = new CConfig();
            config.Add("name", "Mike");
            config.Add("age", "16");

            Assert.IsTrue(config.Count == 2);
            Assert.IsTrue(config.Attributes.Length == 2);

            config.Remove("name");
            Assert.IsTrue(config.Count == 1);
            Assert.Throws<ArgumentException>(_TestC_A);

            config.Remove("age");
            Assert.IsTrue(config.Count == 0);
            Assert.Throws<ArgumentException>(_TestC_B);
        }

        /// <summary>
        /// This test requires CConfig to correctly import/export data.
        /// </summary>
        [Test]
        public void TestD()
        {
            config = new CConfig();
            config.Add("name", "Mike");
            config.Add("age", "16");
            config.Add("occupation", "student");

            Assert.IsTrue(config.Count == 3);

            config.Export("config.cc");

            config = null;
            config = new CConfig("config.cc");

            Assert.IsTrue(config.Count == 3);
        }

        #region TestC supplement

        public void _TestC_A()
        {
            _ = config["name"];
        }
        public void _TestC_B()
        {
            _ = config["age"];
        }

        #endregion
    }
}