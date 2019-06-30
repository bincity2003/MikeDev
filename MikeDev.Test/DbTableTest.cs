using MikeDev.Db;
using NUnit.Framework;

namespace MikeDev.Test
{
    [TestFixture]
    internal class DbTableTest
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
            Assert.IsTrue(obj.GetFieldLength == 2);
            Assert.IsTrue(obj.Count == 0);

            obj.Dispose();
        }

        /// <summary>
        /// This test requires DbTable to correctly manage a single entry.
        /// </summary>
        [Test]
        public void TestB()
        {
            DbTable obj = new DbTable(new string[]
            {
                "Name",
                "Age"
            });
            obj.AddEntry("Mike", new string[] { "Mike", "15" });
            Assert.IsTrue(obj.Count == 1);
            Assert.IsTrue(obj.GetFieldLength == 2);
            Assert.IsTrue(obj["Mike"][0] == "Mike");
            Assert.IsTrue(obj["Mike"][1] == "15");

            obj.Dispose();
        }

        /// <summary>
        /// This test requires DbTable to correctly manage multiple entries.
        /// </summary>
        [Test]
        public void TestC()
        {
            DbTable obj = new DbTable(new string[]
            {
                "Name",
                "Age",
                "Job"
            });

            // Add entries one by one
            obj.AddEntry("Mike", new string[] { "Mike", "15", "Student" });
            obj.AddEntry("Nike", new string[] { "Nike", "12", "Student" });
            obj.AddEntry("Jess", new string[] { "Jess", "12", "Student" });
            obj.AddEntry("Eric", new string[] { "Eric", "11", "Student" });
            obj.AddEntry("Tram", new string[] { "Tram", "12", "Teacher" });

            Assert.IsTrue(obj.Count == 5);
            Assert.IsTrue(obj.GetFieldLength == 3);
            Assert.IsTrue(obj.GetEntriesNames.Length == 5);
            Assert.IsTrue(obj.GetEntriesNames[0] == "Mike");
            Assert.IsTrue(obj.GetEntriesNames[1] == "Nike");
            Assert.IsTrue(obj.GetEntriesNames[4] == "Tram");

            obj.Dispose();

            obj = new DbTable(new string[]
            {
                "Name",
                "Age",
                "Job"
            });

            // Add multiple entries at the same time
            string[] Names = { "Mike", "Nike", "Jess", "Eric", "Tram" };
            string[][] Values = { new string[] { "Mike", "15", "Student" },
                                  new string[] { "Nike", "12", "Student" },
                                  new string[] { "Jess", "12", "Student" },
                                  new string[] { "Eric", "11", "Student" },
                                  new string[] { "Tram", "12", "Teacher" }};
            obj.AddEntry(Names, Values);

            Assert.IsTrue(obj.Count == 5);
            Assert.IsTrue(obj.GetFieldLength == 3);
            Assert.IsTrue(obj.GetEntriesNames.Length == 5);
            Assert.IsTrue(obj.GetEntriesNames[0] == "Mike");
            Assert.IsTrue(obj.GetEntriesNames[1] == "Nike");
            Assert.IsTrue(obj.GetEntriesNames[4] == "Tram");

            obj.Dispose();
        }

        /// <summary>
        /// This test requires DbTable to correcly remove a single entry.
        /// </summary>
        [Test]
        public void TestD()
        {
            DbTable obj = new DbTable(new string[]
            {
                "Name",
                "Age"
            });

            // Add a single entry
            obj.AddEntry("Mike", new string[] { "Mike", "15" });
            Assert.IsTrue(obj.Count == 1);
            Assert.IsTrue(obj.GetFieldLength == 2);
            Assert.IsTrue(obj["Mike"][0] == "Mike");
            Assert.IsTrue(obj["Mike"][1] == "15");

            // Remove that entry
            obj.RemoveEntry("Mike");
            Assert.IsTrue(obj.Count == 0);
            Assert.IsTrue(obj.GetFieldLength == 2);
        }
    }
}
