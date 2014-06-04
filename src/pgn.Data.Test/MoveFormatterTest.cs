using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ilf.pgn.Data.Format.Test
{
    [TestClass]
    public class MoveFormatterTest
    {
        [TestMethod]
        public void default_constructor_should_exist()
        {
            new MoveFormatter();
        }

        [TestMethod]
        public void Format_should_accept_TextWriter()
        {
            var sut = new MoveFormatter();
            var writer = new StringWriter();
            writer.Write("Foo ");
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = PieceType.Rook
            };
            sut.Format(move, writer);

            Assert.AreEqual("Foo Rc5", writer.ToString());
        }
        [TestMethod]
        public void Format_should_format_castling_moves()
        {
            var sut = new MoveFormatter();
            Assert.AreEqual("O-O", sut.Format(new Move { Type = MoveType.CastleKingSide }));
            Assert.AreEqual("O-O-O", sut.Format(new Move { Type = MoveType.CastleQueenSide }));
        }

        [TestMethod]
        public void Format_should_format_simple_target_only_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = PieceType.Rook
            };

            Assert.AreEqual("Rc5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_simple_pawn_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5)
            };

            Assert.AreEqual("c5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_simple_pawn_move_with_explict_pawn()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = PieceType.Pawn
            };

            Assert.AreEqual("c5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_origin_to_target_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = PieceType.Knight,
                OriginSquare = new Square(File.B, 7)
            };

            Assert.AreEqual("Nb7c5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_origin_file_to_target_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = PieceType.Knight,
                OriginFile = File.B
            };

            Assert.AreEqual("Nbc5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_origin_rank_to_target_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = PieceType.Knight,
                OriginRank = 7
            };

            Assert.AreEqual("N7c5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_a_capturing_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                TargetSquare = new Square(File.C, 5),
                Piece = PieceType.Knight,
                TargetPiece = PieceType.Bishop
            };

            Assert.AreEqual("NxBc5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_a_pawn_capturing_move_with_origin_file_info()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                TargetSquare = new Square(File.C, 6),
                OriginFile = File.B,
                Piece = PieceType.Pawn
            };

            Assert.AreEqual("bxc6", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_a_capturing_move_with_origin_square()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = PieceType.Knight,
                OriginSquare = new Square(File.B, 7),
                TargetPiece = PieceType.Bishop,
                TargetSquare = new Square(File.C, 5)
            };

            Assert.AreEqual("Nb7xBc5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_Nb7xc5()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = PieceType.Knight,
                OriginSquare = new Square(File.B, 7),
                TargetPiece = PieceType.Pawn,
                TargetSquare = new Square(File.C, 5)
            };

            Assert.AreEqual("Nb7xc5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_exd5()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = PieceType.Pawn,
                OriginFile = File.E,
                TargetPiece = PieceType.Pawn,
                TargetSquare = new Square(File.D, 5)
            };

            Assert.AreEqual("exd5", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_e4xd5ep()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.CaptureEnPassant,
                Piece = PieceType.Pawn,
                OriginSquare = new Square(File.E, 4),
                TargetPiece = PieceType.Pawn,
                TargetSquare = new Square(File.D, 5)
            };

            Assert.AreEqual("e4xd5e.p.", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_piece_promotion()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                Piece = PieceType.Pawn,
                TargetSquare = new Square(File.E, 8),
                PromotedPiece = PieceType.Queen
            };

            Assert.AreEqual("e8=Q", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_piece_promotion_after_capture()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = PieceType.Pawn,
                OriginSquare = new Square(File.D, 7),
                TargetPiece = PieceType.Rook,
                TargetSquare = new Square(File.E, 8),
                PromotedPiece = PieceType.Queen
            };

            Assert.AreEqual("d7xRe8=Q", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_check_annotation()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = PieceType.Knight,
                OriginSquare = new Square(File.B, 7),
                TargetPiece = PieceType.Pawn,
                TargetSquare = new Square(File.C, 5),
                IsCheck = true
            };

            Assert.AreEqual("Nb7xc5+", sut.Format(move));
        }

        [TestMethod]
        public void Format_should_format_any_annotation()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = PieceType.Rook,
                OriginSquare = new Square(File.B, 1),
                TargetSquare = new Square(File.B, 8),
                IsCheckMate = true,
                Annotation = MoveAnnotation.Brilliant
            };

            Assert.AreEqual("Rb1xb8#!!", sut.Format(move));
        }


        [TestMethod]
        public void Format_should_ommit_redudant_piece_definition___N7c5_and_not_N7Nc5()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = PieceType.Knight,
                TargetPiece = PieceType.Knight,
                OriginRank = 7
            };

            Assert.AreEqual("N7c5", sut.Format(move));

        }

        [TestMethod]
        public void Format_should_include_captured_piece_even_if_its_the_same()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = PieceType.Knight,
                OriginSquare = new Square(File.B, 7),
                TargetPiece = PieceType.Knight,
                TargetSquare = new Square(File.C, 5)
            };

            Assert.AreEqual("Nb7xNc5", sut.Format(move));

        }
    }
}
