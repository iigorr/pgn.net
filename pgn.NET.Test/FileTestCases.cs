using Xunit;
using ilf.pgn.PgnParsers;

namespace ilf.pgn.Test
{
    [TestClass]
    [DeploymentItem(TestFolder, TestFolder)]
    public class FileTestCases
    {
        private const string TestFolder = @"Test Files\";

        [Fact]
        public void EmptyFile()
        {
            var parser = new PgnReader();
            var db = parser.ReadFromFile(TestFolder + "empty-file.pgn");
            Assert.Equal(0, db.Games.Count);
        }

        [Fact]
        public void SimpleGame()
        {
            var parser = new Parser();
            var db = parser.ReadFromFile(TestFolder + "simple-game.pgn");
            Assert.Equal(1, db.Games.Count);
        }

        [Fact]
        public void TimeAnnotatedGames()
        {
            var parser = new Parser();
            var db = parser.ReadFromFile(TestFolder + "time-annotated-games.pgn");
            Assert.Equal(4, db.Games.Count);
        }
    }
}
