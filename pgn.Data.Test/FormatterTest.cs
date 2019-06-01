using System.Collections.Generic;
using System.IO;
using Xunit;
using ilf.pgn.Data.MoveText;

namespace ilf.pgn.Data.Format.Test
{
    public class FormatterTest
    {
        private readonly Game _testGame;
        private const string TestGameString =
@"[Event ""Breslau""]
[Site ""Breslau""]
[Date ""1879.??.??""]
[Round ""?""]
[White ""Tarrasch, Siegbert""]
[Black ""Mendelsohn, J.""]
[Result ""1-0""]

{some moves} 1-0";

        public FormatterTest()
        {
            _testGame = new Game()
                {
                    Event = "Breslau",
                    Site = "Breslau",
                    Year = 1879,
                    WhitePlayer = "Tarrasch, Siegbert",
                    BlackPlayer = "Mendelsohn, J.",
                    Result = GameResult.White,
                };
            _testGame.MoveText = new MoveTextEntryList { new CommentEntry("some moves"), new GameEndEntry(GameResult.White) };
        }
        [Fact]
        public void can_construct()
        {
            new Formatter();
        }

        [Fact]
        public void Format_should_accept_TextWriter()
        {
            var sut = new Formatter();
            var writer = new StringWriter();
            writer.NewLine = "\n";
            sut.Format(_testGame, writer);
            Assert.Equal(TestGameString, writer.ToString());
        }

        [Fact]
        public void Format_should_format_correctly()
        {
            var sut = new Formatter();
            Assert.Equal(TestGameString, sut.Format(_testGame));
        }
    }
}
