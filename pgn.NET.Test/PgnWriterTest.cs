using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using ilf.pgn.Data;
using ilf.pgn.Data.MoveText;

namespace ilf.pgn.Test
{
    public class PgnWriterTest
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

        public PgnWriterTest()
        {
            _testGame = new Game
                {
                    Event = "Breslau",
                    Site = "Breslau",
                    Year = 1879,
                    WhitePlayer = "Tarrasch, Siegbert",
                    BlackPlayer = "Mendelsohn, J.",
                    Result = GameResult.White,
                    MoveText =
                        new MoveTextEntryList {new CommentEntry("some moves"), new GameEndEntry(GameResult.White)},
                };
        }

        [Fact]
        public void can_construct_with_stream()
        {
            new PgnWriter(new MemoryStream(0));
        }

        [Fact]
        public void can_construct_with_file_name()
        {
            new PgnWriter("test.txt");
        }

        [Fact]
        public void Write_should_write_game_correctly()
        {
            var stream = new MemoryStream();
            var sut = new PgnWriter(stream);

            var db = new Database();
            db.Games.Add(_testGame);

            sut.Write(db);

            var actual = Encoding.UTF8.GetString(stream.ToArray());
            Assert.Equal(TestGameString, actual);
        }

        [Fact]
        public void parser_should_read_written_game_correctly()
        {
            var stream = new MemoryStream();
            var sut = new PgnWriter(stream);

            var db = new Database();
            db.Games.Add(_testGame);

            sut.Write(db);
            var writtenResult = Encoding.UTF8.GetString(stream.ToArray());

            var reader = new PgnReader();
            var actualDb = reader.ReadFromString(writtenResult);

            Assert.Equal(db.Games[0].ToString(), actualDb.Games[0].ToString());
        }
    }
}
