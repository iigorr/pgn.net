using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ilf.pgn.Test
{
    [TestClass]
    [DeploymentItem(TestFolder, TestFolder)]
    public class RealFileTests
    {
        private const string TestFolder = @"Test Files\Real Files\";

        private void TestFile(string fileName)
        {
            if (!System.IO.File.Exists(TestFolder + fileName))
                Assert.Inconclusive("Test data not available ");

            var parser = new Parser();
            parser.ReadFromFile(TestFolder + "chess-informant-sample.pgn");
        }
        [TestMethod]
        public void ChessInformantSample()
        {
            TestFile("chess-informant-sample.pgn");
        }
    }
}
