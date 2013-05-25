using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ilf.pgn.PgnParsers;

namespace ilf.pgn.Test
{
    [TestClass]
    [Ignore] //comment out this line to run the long-running tests
    public class LargeFileTests
    {
        private const string TestSet = "Large Files\\";

        [TestMethod]
        public void Twic944()
        {
            Basic.Debug.Default.DebugMode = Basic.DebugMode.File;
            Basic.Debug.Default.ParserLvl = 0;

            var startTime = DateTime.Now;
            var db = TestUtils.TestFile(TestSet + "twic944.pgn");

            Assert.AreEqual(2953, db.Games.Count);
            var duration = startTime - DateTime.Now;
            Console.WriteLine("twic944.pgn Took: "+duration.TotalMilliseconds+ " sec.");
        }

        [TestMethod]
        public void IB1313()
        {
            Basic.Debug.Default.DebugMode = Basic.DebugMode.File;
            Basic.Debug.Default.ParserLvl = 0;

            var startTime = DateTime.Now;
            var db = TestUtils.TestFile(TestSet + "IB1313.pgn");

            Assert.AreEqual(48984, db.Games.Count);
            var duration = startTime - DateTime.Now;
            Console.WriteLine("IB1313.pgn Took: " + duration.TotalMilliseconds + " sec.");
        }

        [TestMethod]
        public void Ficsgamesdb_2012_titled_movetimes_772441()
        {
            Basic.Debug.Default.DebugMode = Basic.DebugMode.File;
            Basic.Debug.Default.ParserLvl = 0;

            var startTime = DateTime.Now;
            var db = TestUtils.TestFile(TestSet + "ficsgamesdb_2012_titled_movetimes_772441.pgn");

            Assert.AreEqual(40218, db.Games.Count);
            var duration = startTime - DateTime.Now;
            Console.WriteLine("ficsgamesdb_2012_titled_movetimes_772441.pgn Took: " + duration.TotalMilliseconds + " sec.");
        }
    }
}
