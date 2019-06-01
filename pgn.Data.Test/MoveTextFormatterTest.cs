using System.Collections.Generic;
using System.IO;
using Xunit;
using ilf.pgn.Data.MoveText;

namespace ilf.pgn.Data.Format.Test
{
    public class MoveTextFormatterTest
    {
        private readonly Move _move1;
        private readonly Move _move2;

        public MoveTextFormatterTest()
        {
            _move1 = new Move
            {
                Type = MoveType.Capture,
                TargetSquare = new Square(File.D, 5),
                OriginFile = File.E
            };

            _move2 = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.D, 4),
                Piece = PieceType.Knight
            };
        }

        [Fact]
        public void can_create()
        {
            new MoveTextFormatter();
        }

        [Fact]
        public void Format_should_accept_TextWriter()
        {
            var sut = new MoveTextFormatter();
            var writer = new StringWriter();
            writer.Write("Foo ");

            var movePair = new MovePairEntry(_move1, _move2);

            sut.Format(movePair, writer);

            Assert.Equal("Foo exd5 Nd4", writer.ToString());
        }



        [Fact]
        public void Format_should_format_move_pair()
        {
            var sut = new MoveTextFormatter();
            var movePair = new MovePairEntry(_move1, _move2);

            Assert.Equal("exd5 Nd4", sut.Format(movePair));
        }

        [Fact]
        public void Format_should_format_move_pair_with_number()
        {
            var sut = new MoveTextFormatter();
            var movePair = new MovePairEntry(_move1, _move2) { MoveNumber = 6 };

            Assert.Equal("6. exd5 Nd4", sut.Format(movePair));
        }

        [Fact]
        public void Format_should_format_starting_single_move()
        {
            var sut = new MoveTextFormatter();
            var entry = new HalfMoveEntry(_move1) { MoveNumber = 6 };

            Assert.Equal("6. exd5", sut.Format(entry));
        }

        [Fact]
        public void Format_should_format_continued_single_move()
        {
            var sut = new MoveTextFormatter();
            var entry = new HalfMoveEntry(_move2) { MoveNumber = 6, IsContinued = true };

            Assert.Equal("6... Nd4", sut.Format(entry));
        }

        [Fact]
        public void Format_should_format_a_GameEndEntry()
        {
            var sut = new MoveTextFormatter();
            Assert.Equal("1-0", sut.Format(new GameEndEntry(GameResult.White)));
            Assert.Equal("0-1", sut.Format(new GameEndEntry(GameResult.Black)));
            Assert.Equal("*", sut.Format(new GameEndEntry(GameResult.Open)));
            // TODO: Case doesn't work due to weird encoding issues.
            // Assert.Equal("½-½", sut.Format(new GameEndEntry(GameResult.Draw)));
        }

        [Fact]
        public void Format_should_format_a_CommentEntry()
        {
            var sut = new MoveTextFormatter();
            Assert.Equal("{This is a test comment}", sut.Format(new CommentEntry("This is a test comment")));
        }

        [Fact]
        public void Format_should_format_a_NAGEntry()
        {
            var sut = new MoveTextFormatter();
            Assert.Equal("$5", sut.Format(new NAGEntry(5)));
        }

        [Fact]
        public void Format_should_format_a_RAVEntry()
        {
            var sut = new MoveTextFormatter();
            var halfMoveEntry = new HalfMoveEntry(_move2) { MoveNumber = 6, IsContinued = true };
            var ravEntry = new RAVEntry(new MoveTextEntryList { halfMoveEntry });
            Assert.Equal("(6... Nd4)", sut.Format(ravEntry));
        }

        [Fact]
        public void Format_should_format_move_text()
        {
            var sut = new MoveTextFormatter();
            var entry1 =
                new HalfMoveEntry(new Move
                    {
                        Type = MoveType.Capture,
                        Piece = PieceType.Knight,
                        TargetSquare = new Square(File.E, 5),
                        Annotation = MoveAnnotation.Good
                    }) { MoveNumber = 37 };

            var entry2 = new NAGEntry(13);

            var rav1 = new CommentEntry("comment");
            var rav2 =
                new HalfMoveEntry(new Move
                    {
                        Type = MoveType.Simple,
                        Piece = PieceType.Knight,
                        TargetSquare = new Square(File.E, 3),
                        Annotation = MoveAnnotation.Blunder
                    }) { MoveNumber = 37 };

            var entry3 = new RAVEntry(new MoveTextEntryList { rav1, rav2 });

            var entry4 =
                new HalfMoveEntry(new Move
                    {
                        Type = MoveType.Simple,
                        Piece = PieceType.Rook,
                        TargetSquare = new Square(File.D, 8)
                    }) { MoveNumber = 37, IsContinued = true };

            var entry5 = new MovePairEntry(
                new Move { Type = MoveType.Simple, TargetSquare = new Square(File.H, 4) },
                new Move { Type = MoveType.Simple, Piece = PieceType.Rook, TargetSquare = new Square(File.D, 5) }) { MoveNumber = 38 };

            var entry6 = new GameEndEntry(GameResult.Draw);
            var entry7 = new CommentEntry("game ends in draw, whooot");

            var moveText = new List<MoveTextEntry> { entry1, entry2, entry3, entry4, entry5, entry6, entry7 };
            // TODO: Case doesn't work due to weird encoding issues.
            // Assert.Equal("37. Nxe5! $13 ({comment} 37. Ne3??) 37... Rd8 38. h4 Rd5 ½-½ {game ends in draw, whooot}", sut.Format(moveText));
        }

        [Fact]
        public void Format_should_deal_with_empty_move_text()
        {
            var sut = new MoveTextFormatter();
            var moveText = new List<MoveTextEntry>();
            Assert.Equal("", sut.Format(moveText));
        }

    }
}
