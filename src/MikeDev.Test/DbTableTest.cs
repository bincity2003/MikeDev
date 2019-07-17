using MikeDev.Database;
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

            obj.Dispose();
        }

        /// <summary>
        /// This test requires DbTable to correctly remove multiple entries.
        /// </summary>
        [Test]
        public void TestE()
        {
            var obj = new DbTable(new string[]
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

            // Remove multiple entries
            obj.RemoveEntry(Names);
            Assert.IsTrue(obj.Count == 0);
            Assert.IsTrue(obj.GetFieldLength == 3);

            obj.Dispose();
        }

        /// <summary>
        /// This test requires DbTable to correctly remove an entry/ multiple entries.
        /// </summary>
        [Test]
        public void TestF()
        {
            var obj = new DbTable(new string[]
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

            Assert.IsTrue(obj["Mike"][1] == "15");
            Assert.IsTrue(obj["Nike"][1] == "12");
            Assert.IsTrue(obj["Nike"][2] == "Student");

            // Replace single entry
            string[] Single = { "Mike", "30", "Lawyers" };
            obj.ReplaceEntry("Mike", Single);

            Assert.IsTrue(obj["Mike"][1] == "30");
            Assert.IsTrue(obj["Mike"][2] == "Lawyers");

            // Replace multiple entries
            string[][] NewValues = { new string[] { "Mike", "10", "Student" },
                                     new string[] { "Nike", "25", "Hackers" },
                                     new string[] { "Jess", "11", "Student" },
                                     new string[] { "Eric", "09", "Student" },
                                     new string[] { "Tram", "10", "Teacher" }};
            obj.ReplaceEntry(Names, NewValues);

            Assert.IsTrue(obj["Mike"][1] == "10");
            Assert.IsTrue(obj["Nike"][1] == "25");
            Assert.IsTrue(obj["Nike"][2] == "Hackers");

            obj.Dispose();
        }

        /// <summary>
        /// This test requires DbTable to correctly generate index with standard.
        /// </summary>
        [Test]
        public void TestG()
        {
            string n = "test";

            Assert.IsTrue(DbTable.GetIndex(n) == "098f6bcd4621d373cade4e832627b4f6");
        }

        /// <summary>
        /// This test requires DbTable to correctly export/import data from string.
        /// </summary>
        [Test]
        public void TestH()
        {
            var obj = new DbTable(new string[]
            {
                "Name",
                "Age",
                "Job"
            });

            string[] Names = { "Mike", "Nike", "Jess", "Eric", "Tram" };
            string[][] Values = { new string[] { "Mike", "15", "Student" },
                                  new string[] { "Nike", "12", "Student" },
                                  new string[] { "Jess", "12", "Student" },
                                  new string[] { "Eric", "11", "Student" },
                                  new string[] { "Tram", "12", "Teacher" }};
            obj.AddEntry(Names, Values);
            Assert.IsTrue(obj.Count == 5);

            string Data = obj.Export();
            obj.Dispose();

            obj = new DbTable(Data, true);

            Assert.IsTrue(obj.Count == 5);
            Assert.IsTrue(obj.GetFieldLength == 3);
            Assert.IsTrue(obj.GetEntriesNames.Length == 5);
            Assert.IsTrue(obj.GetEntriesNames[0] == "Mike");
            Assert.IsTrue(obj.GetEntriesNames[1] == "Nike");
            Assert.IsTrue(obj.GetEntriesNames[4] == "Tram");

            obj.Dispose();
        }

        /// <summary>
        /// This test requires DbTable to correctly export/import data from file.
        /// </summary>
        [Test]
        public void TestI()
        {
            var obj = new DbTable(new string[]
            {
                "Name",
                "Age",
                "Job"
            });

            string[] Names = { "Mike", "Nike", "Jess", "Eric", "Tram" };
            string[][] Values = { new string[] { "Mike", "15", "Student" },
                                  new string[] { "Nike", "12", "Student" },
                                  new string[] { "Jess", "12", "Student" },
                                  new string[] { "Eric", "11", "Student" },
                                  new string[] { "Tram", "12", "Teacher" }};
            obj.AddEntry(Names, Values);
            Assert.IsTrue(obj.Count == 5);

            obj.Export("test.dbtable");
            obj.Dispose();

            obj = new DbTable("test.dbtable");

            Assert.IsTrue(obj.Count == 5);
            Assert.IsTrue(obj.GetFieldLength == 3);
            Assert.IsTrue(obj.GetEntriesNames.Length == 5);
            Assert.IsTrue(obj.GetEntriesNames[0] == "Mike");
            Assert.IsTrue(obj.GetEntriesNames[1] == "Nike");
            Assert.IsTrue(obj.GetEntriesNames[4] == "Tram");

            obj.Dispose();
        }
    }
}
