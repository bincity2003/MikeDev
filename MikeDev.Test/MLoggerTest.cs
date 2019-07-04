using MikeDev.Debug;
using NUnit.Framework;

namespace MikeDev.Test
{
    [TestFixture]
    internal class MLoggerTest
    {
        /// <summary>
        /// This test requires MLogger to correctly process message.
        /// </summary>
        [Test]
        public void TestA()
        {
            System.IO.File.Create("log.txt").Close();
            MLogger obj = new MLogger("log.txt");
            string Message = "This is a test message!";

            string Result = obj.ProcessMessage(Message, false);
            Assert.AreEqual(Result, "Info : This is a test message!\n");

            Result = obj.ProcessMessage(Message, false, MLogger.LogLevel.Warning);
            Assert.AreEqual(Result, "Warning : This is a test message!\n");
        }

        /// <summary>
        /// This test requires MLogger to correctly write logs.
        /// </summary>
        [Test]
        public void TestB()
        {
            System.IO.File.Create("log.txt").Close();
            MLogger obj = new MLogger("log.txt");
            string Message = "This is a test message!";

            bool Flag = obj.Log(Message);
            Assert.IsTrue(Flag);
        }
    }
}
