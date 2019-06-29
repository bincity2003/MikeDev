using MikeDev.DB;
using NUnit.Framework;

namespace MikeDev.Test
{
    [TestFixture]
    class DbTableTest
    {
        /// <summary>
        /// This test requires DbTable to initialize correctly.
        /// </summary>
        [Test]
        public void TestA()
        {
            DbTable obj = new DbTable(new string[]
            {
                "Name",
                "Age"
            });
            Assert.IsTrue(obj.FieldNames[0] == "Name");
            Assert.IsFalse(obj.FieldNames[1] != "Age");
            Assert.IsTrue(obj.Count == 2);

            obj.Dispose();
        }

        public void TestB()
        {
            DbTable obj = new DbTable(new string[]
            {
                "Name",
                "Age"
            });
        }
    }
}
