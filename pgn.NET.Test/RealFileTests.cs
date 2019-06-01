using Xunit;

namespace ilf.pgn.Test
{
    public class RealFileTests
    {
        private const string TestSet = "RealGames/";

        [Fact]
        public void ChessInformantSample()
        {
            var db = TestUtils.TestFile(TestSet+"chess-informant-sample.pgn");
            Assert.Equal(5, db.Games.Count);
        }

        [Fact]
        public void DemoGames()
        {
            var db = TestUtils.TestFile(TestSet + "demoGames.pgn");
            Assert.Equal(2, db.Games.Count);
        }

        [Fact]
        public void Lon09R5()
        {
            var db = TestUtils.TestFile(TestSet + "lon09r5.pgn");
            Assert.Equal(4, db.Games.Count);
        }

        [Fact]
        public void Tilb98R2()
        {
            var db = TestUtils.TestFile(TestSet + "tilb98r2.pgn");
            Assert.Equal(6, db.Games.Count);
        }
    }
}
