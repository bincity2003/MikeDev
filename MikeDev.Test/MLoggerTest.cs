﻿using MikeDev.Debug;
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
            MLogger obj = new MLogger("C:\\Users\\Thanh Cong\\Desktop\\log.txt");
            string Message = "This is a test message!";
            string Result = obj.ProcessMessage(Message, false);
            Assert.AreEqual(Result, "Info : This is a test message!\n");
        }
    }
}