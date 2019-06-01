using Xunit;
using ilf.pgn;
using ilf.pgn.PgnParsers;

namespace ilf.pgn.Test
{
    public class FileTestCases
    {
        private const string TestFolder = @"TestExamples/";

        [Fact]
        public void EmptyFile()
        {
            var parser = new PgnReader();
            var db = parser.ReadFromFile(TestFolder + "empty-file.pgn");
            Assert.Empty(db.Games);
        }

        [Fact]
        public void SimpleGame()
        {
            var parser = new Parser();
            var db = parser.ReadFromFile(TestFolder + "simple-game.pgn");
            Assert.Single(db.Games);
        }

        [Fact]
        public void TimeAnnotatedGames()
        {
            var parser = new Parser();
            var db = parser.ReadFromFile(TestFolder + "time-annotated-games.pgn");
            var noOfGames = db.Games.Count;
            Assert.Equal(4, noOfGames);
        }
    }
}
