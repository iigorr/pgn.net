using Microsoft.VisualStudio.TestTools.UnitTesting;
using ilf.pgn.PgnParsers;

namespace ilf.pgn.Test
{
#if !PORTABLE
    [TestClass]
    public class RealFileTests
    {
        private const string TestSet = "Real Files\\";

        [TestMethod]
        public void ChessInformantSample()
        {
            var db = TestUtils.TestFile(TestSet+"chess-informant-sample.pgn");
            Assert.AreEqual(5, db.Games.Count);
        }

        [TestMethod]
        public void DemoGames()
        {
            var db = TestUtils.TestFile(TestSet + "demoGames.pgn");
            Assert.AreEqual(2, db.Games.Count);
        }

        [TestMethod]
        public void Lon09R5()
        {
            var db = TestUtils.TestFile(TestSet + "lon09r5.pgn");
            Assert.AreEqual(4, db.Games.Count);
        }

        [TestMethod]
        public void Tilb98R2()
        {
            var db = TestUtils.TestFile(TestSet + "tilb98r2.pgn");
            Assert.AreEqual(6, db.Games.Count);
        }
    }
#endif
}
