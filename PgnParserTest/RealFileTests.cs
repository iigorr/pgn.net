using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ilf.pgn.Test
{
    [TestClass]
    [DeploymentItem(TestFolder, TestFolder)]
    public class RealFileTests
    {
        private const string TestFolder = @"Test Files\Real Files\";

        private Database TestFile(string fileName)
        {
            if (!System.IO.File.Exists(TestFolder + fileName))
                Assert.Inconclusive("Test data not available ");

            var parser = new Parser();
            return parser.ReadFromFile(TestFolder + fileName);

        }

        [TestMethod]
        public void ChessInformantSample()
        {
            var db = TestFile("chess-informant-sample.pgn");
            Assert.AreEqual(5, db.Games.Count);
        }

        [TestMethod]
        public void DemoGames()
        {
            var db = TestFile("demoGames.pgn");
            Assert.AreEqual(2, db.Games.Count);
        }
    }
}
