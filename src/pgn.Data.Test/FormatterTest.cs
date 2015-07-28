using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ilf.pgn.Data.MoveText;

namespace ilf.pgn.Data.Format.Test
{
    [TestClass]
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
        [TestMethod]
        public void can_construct()
        {
            new Formatter();
        }

        [TestMethod]
        public void Format_should_accept_TextWriter()
        {
            var sut = new Formatter();
            var writer = new StringWriter();
            writer.NewLine = "\n";
            sut.Format(_testGame, writer);
            Assert.AreEqual(TestGameString, writer.ToString());
        }

        [TestMethod]
        public void Format_should_format_correctly()
        {
            var sut = new Formatter();
            Assert.AreEqual(TestGameString, sut.Format(_testGame));
        }
    }
}
