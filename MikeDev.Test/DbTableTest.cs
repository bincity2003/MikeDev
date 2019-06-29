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
            Assert.IsTrue(obj.FieldNames == new string[] { "Name", "Age" });
            Assert.IsFalse(obj.FieldNames == new string[] { "Name", "age" });
            Assert.IsTrue(obj.Count == 2);
        }
    }
}
