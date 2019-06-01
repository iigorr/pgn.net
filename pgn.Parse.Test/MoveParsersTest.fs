namespace ilf.pgn.Test

open ilf.pgn.Data
open ilf.pgn.PgnParsers.Move
open ilf.pgn.Test.TestBase

open Xunit

type MoveParserTest() =
    [<Fact>]
    member this.pTarget_should_accept_a_normal_move() =
        tryParse pTarget "Rc5"

    [<Fact>]
    member this.pTarget_should_accept_a_normal_move_with_lower_case_piece_letter() =
        tryParse pTarget "qc5"

    [<Fact>]
    member this.pTarget_should_accept_a_normal_move_with_upper_case_file_letter() =
        tryParse pTarget "QF5"

    [<Fact>]
    member this.pTarget_should_accept_a_pawn_move_with_omitted_FigureSymbol() =
        tryParse pTarget "c5"
        tryParse pTarget "b5 "

    [<Fact>]
    member this.pTarget_should_fail_on_incorrect_format() =
        shouldFail pTarget "z7"
        shouldFail pTarget "a0"
        shouldFail pTarget "a9"
        shouldFail pTarget "Ka0"
        shouldFail pTarget "Fa1"

    [<Fact>]
    member this.pTarget_should_yield_a_MoveInfo_object() =
        let moveInfo = parse pTarget "Qf5"
        Assert.Equal(PieceType.Queen, moveInfo.Piece.Value)
        Assert.Equal(File.F, moveInfo.File.Value)
        Assert.Equal(5, moveInfo.Rank.Value)

    [<Fact>]
    member this.pOrigin_should_yield_a_MoveInfo_object() =
        let moveInfo = parse pOrigin "Nd"
        Assert.Equal(PieceType.Knight, moveInfo.Piece.Value)
        Assert.Equal(File.D, moveInfo.File.Value)
        Assert.False(moveInfo.Rank.HasValue)

    [<Fact>]
    member this.pOrigin_should_yield_a_MoveInfo_object_by_rank() =
        let moveInfo = parse pOrigin "N5"
        Assert.Equal(PieceType.Knight, moveInfo.Piece.Value)
        Assert.False(moveInfo.File.HasValue)
        Assert.Equal(5, moveInfo.Rank.Value)

    [<Fact>]
    member this.pOrigin_should_yield_a_MoveInfo_object_on_file_rank() =
        let moveInfo = parse pOrigin "d5"
        Assert.Equal(PieceType.Pawn, moveInfo.Piece.Value)
        Assert.Equal(File.D, moveInfo.File.Value)
        Assert.Equal(5, moveInfo.Rank.Value)

    [<Fact>]
    member this.pOrigin_should_yield_a_MoveInfo_object_on_file_only() =
        let moveInfo = parse pOrigin "d"
        Assert.Equal(PieceType.Pawn, moveInfo.Piece.Value)
        Assert.Equal(File.D, moveInfo.File.Value)
        Assert.False(moveInfo.Rank.HasValue)


    [<Fact>]
    member this.pCapturingMove_should_accept_a_standard_capturing_move() =
        tryParse pCapturingMove "c5xQD5"
        tryParse pCapturingMove "c5:QD5"

    [<Fact>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capture_info_at_back() =
        tryParse pCapturingMove "Qd5x"
        tryParse pCapturingMove "Qd5:"

    [<Fact>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_piece_file_rank() =
        tryParse pCapturingMove "Qc5xBd5"//queen on c5 captures bishop on d5
        tryParse pCapturingMove "Qc5:Bd5"//queen on c5 captures bishop on d5

    [<Fact>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_piece_rank() =
        tryParse pCapturingMove "Q5xBd7"//queen on d5 captures bishop on d7
        tryParse pCapturingMove "Q5:Bd7"//queen on d5 captures bishop on d7

    [<Fact>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_piece_and_file() =
        tryParse pCapturingMove "QcxBd5"//queen on C file captures bishop on D5
        tryParse pCapturingMove "Qc:Bd5"//queen on C file captures bishop on D5

    [<Fact>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_piece() =
        tryParse pCapturingMove "QxBd5"//queen captures bishop on D5
        tryParse pCapturingMove "Q:Bd5"//queen captures bishop on D5

    [<Fact>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_file() =
        tryParse pCapturingMove "CxQD5" //piece on C captures queen on D5
        tryParse pCapturingMove "c:QD5"

    [<Fact>]
    member this.pCapturingMove_should_accept_a_pawn_capturing_move_with_capturing_file() =
        tryParse pCapturingMove "cxd5" //pawn on C captures D5
        tryParse pCapturingMove "c:D5"


    [<Fact>]
    member this.pCapturingMove_should_accept_enpassent_suffix() =
        tryParse pCapturingMove "cxd5e.p." //pawn on c captures pawn on d5 en passant


    [<Fact>]
    member this.pCapturingMove_should_accept_simplified_pawn_capturing_moves() =
        tryParse pCapturingMove "cxd" //pawn on c captures pawn on d5 en passant
        tryParse pCapturingMove "cxde.p" //pawn on c captures pawn on d5 en passant

    [<Fact>]
    member this.pCapturingMove_should_accept_simplified_pawn_capturing_on_same_lane_should_fail() =
        shouldFail pCapturingMove "axa" //illegal move

    [<Fact>]
    member this.pCapturing_move_should_parse_normal_move_correctly() =
        let move = parse pCapturingMove "Qd5xBh5"
        Assert.Equal(PieceType.Queen, move.Piece.Value)
        Assert.Equal(MoveType.Capture, move.Type)
        Assert.Equal(PieceType.Bishop, move.TargetPiece.Value)
        Assert.Equal(Square(File.D, 5), move.OriginSquare)
        Assert.Equal(Square(File.H, 5), move.TargetSquare)

    [<Fact>]
    member this.pCapturing_move_should_parse_move_correctly_with_origin_piece() =
        let move = parse pCapturingMove "QxBh5"
        Assert.Equal(PieceType.Queen, move.Piece.Value)
        Assert.Equal(MoveType.Capture, move.Type)
        Assert.Equal(PieceType.Bishop, move.TargetPiece.Value)
        Assert.Equal(null, move.OriginSquare)
        Assert.Equal(Square(File.H, 5), move.TargetSquare)

    [<Fact>]
    member this.pCapturing_move_should_parse_move_correctly_with_originSquare() =
        let move = parse pCapturingMove "a1xBh8"
        Assert.Equal(PieceType.Pawn, move.Piece.Value)
        Assert.Equal(MoveType.Capture, move.Type)
        Assert.Equal(PieceType.Bishop, move.TargetPiece.Value)
        Assert.Equal(Square(File.A, 1), move.OriginSquare)
        Assert.Equal(Square(File.H, 8), move.TargetSquare)

    [<Fact>]
    member this.pBasicMove_should_accept_pawn_move() =
        tryParse pBasicMove "e2e4"


    [<Fact>]
    member this.pBasicMove_should_accept_simple_target_move() =
        tryParse pBasicMove "Rc5"
        tryParse pBasicMove "d4d5"
        tryParse pBasicMove "d5"

    [<Fact>]
    member this.pBasicMove_should_correctly_parse_simple_target_move() =
        let move = parse pBasicMove "Rc5"
        Assert.Equal(PieceType.Rook, move.Piece.Value)
        Assert.Equal(MoveType.Simple, move.Type)
        Assert.Equal(Square(File.C, 5), move.TargetSquare)

    [<Fact>]
    member this.pBasicMove_should_correctly_parse_pawnMove() =
        let move = parse pBasicMove "d4d5"
        Assert.Equal(MoveType.Simple, move.Type)
        Assert.Equal(Square(File.D, 4), move.OriginSquare)
        Assert.Equal(Square(File.D, 5), move.TargetSquare)

    [<Fact>]
    member this.pBasicMove_should_accept_disambiguated_moves() =
        tryParse pBasicMove "Ngf3"
        tryParse pBasicMove "N5f3"
        tryParse pBasicMove "Rdd5"
        tryParse pBasicMove "R3d5"

    [<Fact>]
    member this.pBasicMove_should_correctly_parse_disambiguated_moves() =
        let move = parse pBasicMove "Ngf3"
        Assert.Equal(PieceType.Knight, move.Piece.Value)
        Assert.Equal(Square(File.F, 3), move.TargetSquare)
        Assert.Equal(File.G, move.OriginFile.Value)

    [<Fact>]
    member this.pPawnPromotion_should_accept_pawn_promotion_moves() =
        tryParse pPawnPromotion "d8=Q"
        tryParse pPawnPromotion "d8(Q)"
        tryParse pPawnPromotion "c8=R"
        tryParse pPawnPromotion "Pc8=R"

    [<Fact>]
    member this.pPawnPromotion_should_accept_capturing_pawn_promotion_moves() =
        tryParse pPawnPromotion "cxd8=Q"
        tryParse pPawnPromotion "dxe8(Q)"
        tryParse pPawnPromotion "b7c8=N"

    [<Fact>]
    member this.pPawnPromotion_should_correctly_parse_promoting_move() =
        let move = parse pPawnPromotion "d8=Q"
        Assert.Equal(Square(File.D, 8), move.TargetSquare)
        Assert.Equal(PieceType.Queen, move.PromotedPiece.Value)


    [<Fact>]
    member this.pCastle_should_accept_catling() =
        tryParse pCastle "O-O"
        tryParse pCastle "O-O-O"
        tryParse pCastle "0-0"
        tryParse pCastle "0-0-0"
        tryParse pCastle "O - O"
        tryParse pCastle "O - O - O"
        tryParse pCastle "0 - 0"
        tryParse pCastle "0 - 0 - 0"

    [<Fact>]
    member this.pCastle_should_correctly_parse_castling_move() =
        let move = parse pCastle "0-0-0"
        Assert.Equal(MoveType.CastleQueenSide, move.Type)

    [<Fact>]
    member this.pMove_should_accept_all_kinds_of_moves() =
        tryParse pMove "Rc5"
        tryParse pMove "e2e4"
        tryParse pMove "QF1"
        tryParse pMove "c5xQD5"
        tryParse pMove "Qc5:Bd5"
        tryParse pMove "cxd5e.p."
        tryParse pMove "d5"
        tryParse pMove "N5f3"
        tryParse pMove "Pc8=Q"
        tryParse pMove "O-O"
        tryParse pMove "0-0-0"


    [<Fact>]
    member this.pMove_should_accept_moves_with_check_annotation() =
        tryParse pMove "c5xQD5+"
        tryParse pMove "Qc5:Bd5†"
        tryParse pMove "Qc5:Bd5ch"

    [<Fact>]
    member this.pMove_should_accept_moves_with_doublecheck_annotation() =
        tryParse pMove "c5xQD5++"
        tryParse pMove "Qc5:Bd5††"
        tryParse pMove "Qc5:Bd5dbl ch"

    [<Fact>]
    member this.pMove_should_accept_moves_with_check_mate_annotation() =
        tryParse pMove "Qd2#"
        tryParse pMove "Qxd2‡"

    [<Fact>]
    member this.pMove_should_correctly_parse_simple_move() =
        let move = parse pMove "Rc5"
        Assert.Equal(MoveType.Simple, move.Type);
        Assert.Equal(PieceType.Rook, move.Piece.Value);
        Assert.Equal(Square(File.C, 5), move.TargetSquare);

    [<Fact>]
    member this.pMove_should_correctly_parse_pawn_move() =
        let move = parse pMove "e2e4"
        Assert.Equal(MoveType.Simple, move.Type);
        Assert.Equal(Square(File.E, 2), move.OriginSquare);
        Assert.Equal(Square(File.E, 4), move.TargetSquare);

    [<Fact>]
    member this.pMove_should_correctly_parse_simple_move_with_different_case() =
        let move = parse pMove "QF1"
        Assert.Equal(MoveType.Simple, move.Type);
        Assert.Equal(PieceType.Queen, move.Piece.Value);
        Assert.Equal(Square(File.F, 1), move.TargetSquare);

    [<Fact>]
    member this.pMove_should_correctly_parse_capturing_move() =
        let move = parse pMove "c5xQD5"
        Assert.Equal(MoveType.Capture, move.Type);
        Assert.Equal(PieceType.Queen, move.TargetPiece.Value);
        Assert.Equal(Square(File.D, 5), move.TargetSquare);
        Assert.Equal(Square(File.C, 5), move.OriginSquare);

    [<Fact>]
    member this.pMove_should_correctly_parse_capturing_move_with_colon_sign() =
        let move = parse pMove "Qc5:Bd5"
        Assert.Equal(MoveType.Capture, move.Type);
        Assert.Equal(PieceType.Queen, move.Piece.Value);
        Assert.Equal(PieceType.Bishop, move.TargetPiece.Value);
        Assert.Equal(Square(File.D, 5), move.TargetSquare);
        Assert.Equal(Square(File.C, 5), move.OriginSquare);

    [<Fact>]
    member this.pMove_should_correctly_parse_capturing_move_with_origin_file_and_no_target_piece() =
        let move = parse pMove "bxc6"
        Assert.Equal(MoveType.Capture, move.Type);
        Assert.Equal(PieceType.Pawn, move.Piece.Value);
        Assert.Equal(Square(File.C, 6), move.TargetSquare);
        Assert.Equal(File.B, move.OriginFile.Value);


    [<Fact>]
    member this.pMove_should_correctly_parse_en_passant() =
        let move = parse pMove "cxd5e.p."
        Assert.Equal(MoveType.CaptureEnPassant, move.Type);
        Assert.Equal(Square(File.D, 5), move.TargetSquare);
        Assert.Equal(File.C, move.OriginFile.Value);

    [<Fact>]
    member this.pMove_should_correctly_parse_simple_pawn_move() =
        let move = parse pMove "d5"
        Assert.Equal(MoveType.Simple, move.Type);
        Assert.Equal(Square(File.D, 5), move.TargetSquare);

    [<Fact>]
    member this.pMove_should_correctly_parse_disambigued_move() =
        let move = parse pMove "N5f3"
        Assert.Equal(MoveType.Simple, move.Type);
        Assert.Equal(PieceType.Knight, move.Piece.Value);
        Assert.Equal(Square(File.F, 3), move.TargetSquare);
        Assert.Equal(5, move.OriginRank.Value);

    [<Fact>]
    member this.pMove_should_correctly_parse_promotion_move() =
        let move = parse pMove "c8(Q)"
        Assert.Equal(MoveType.Simple, move.Type);
        Assert.Equal(Square(File.C, 8), move.TargetSquare);
        Assert.Equal(PieceType.Queen, move.PromotedPiece.Value);

    [<Fact>]
    member this.pMove_should_correctly_parse_capturing_promotion_move() =
        let move = parse pMove "d7xc8(Q)"
        Assert.Equal(MoveType.Capture, move.Type);
        Assert.Equal(Square(File.C, 8), move.TargetSquare);
        Assert.Equal(Square(File.D, 7), move.OriginSquare);
        Assert.Equal(PieceType.Queen, move.PromotedPiece.Value);

    [<Fact>]
    member this.pMove_should_correctly_parse_promotion_move_wiht_check() =
        let move = parse pMove "c8=Q+"
        Assert.Equal(MoveType.Simple, move.Type);
        Assert.Equal(Square(File.C, 8), move.TargetSquare);
        Assert.Equal(PieceType.Queen, move.PromotedPiece.Value);
        Assert.True(move.IsCheck.Value);

    [<Fact>]
    member this.pMove_should_correctly_parse_castle_king_side() =
        let move = parse pMove "O-O"
        Assert.Equal(MoveType.CastleKingSide, move.Type);

    [<Fact>]
    member this.pMove_should_correctly_parse_castle_queen_side() =
        let move = parse pMove "0-0-0"
        Assert.Equal(MoveType.CastleQueenSide, move.Type);

    [<Fact>]
    member this.pMove_should_correctly_parse_check_indicator() =
        let move = parse pMove "Bb5+"

        Assert.Equal(MoveType.Simple, move.Type)
        Assert.True(move.IsCheck.Value)

    [<Fact>]
    member this.pMove_should_correctly_parse_dbl_check_indicator() =
        let move = parse pMove "Bb5++"

        Assert.Equal(MoveType.Simple, move.Type)
        Assert.True(move.IsCheck.Value)
        Assert.True(move.IsDoubleCheck.Value)

    [<Fact>]
    member this.pMove_should_correctly_parse_checkmate_indicator() =
        let move = parse pMove "Bb5#"

        Assert.Equal(MoveType.Simple, move.Type)
        Assert.True(move.IsCheckMate.Value)

    [<Fact>]
    member this.pMove_should_correctly_parse_annotation() =
        let move = parse pMove "Bb5+!"

        Assert.Equal(MoveType.Simple, move.Type)
        Assert.True(move.IsCheck.Value)
        Assert.Equal(MoveAnnotation.Good, move.Annotation.Value)

    [<Fact>]
    member this.pMove_should_correctly_parse_differentAnnotations() =
        Assert.Equal(MoveAnnotation.MindBlowing, (parse pMove "Bb5!!!").Annotation.Value)
        Assert.Equal(MoveAnnotation.Brilliant, (parse pMove "Bb5!!").Annotation.Value)
        Assert.Equal(MoveAnnotation.Good, (parse pMove "Bb5!").Annotation.Value)
        Assert.Equal(MoveAnnotation.Interesting, (parse pMove "Bb5!?").Annotation.Value)
        Assert.Equal(MoveAnnotation.Dubious, (parse pMove "Bb5?!").Annotation.Value)
        Assert.Equal(MoveAnnotation.Mistake, (parse pMove "Bb5?").Annotation.Value)
        Assert.Equal(MoveAnnotation.Blunder, (parse pMove "Bb5??").Annotation.Value)
        Assert.Equal(MoveAnnotation.Abysmal, (parse pMove "Bb5???").Annotation.Value)
        Assert.Equal(MoveAnnotation.FascinatingButUnsound, (parse pMove "Bb5!?!").Annotation.Value)
        Assert.Equal(MoveAnnotation.Unclear, (parse pMove "Bb5∞").Annotation.Value)
        Assert.Equal(MoveAnnotation.WithCompensation, (parse pMove "Bb5=/∞").Annotation.Value)
        Assert.Equal(MoveAnnotation.EvenPosition, (parse pMove "Bb5=").Annotation.Value)
        Assert.Equal(MoveAnnotation.SlightAdvantageWhite, (parse pMove "Bb5+/=").Annotation.Value)
        Assert.Equal(MoveAnnotation.SlightAdvantageBlack, (parse pMove "Bb5=/+").Annotation.Value)
        Assert.Equal(MoveAnnotation.AdvantageWhite, (parse pMove "Bb5+/-").Annotation.Value)
        Assert.Equal(MoveAnnotation.AdvantageBlack, (parse pMove "Bb5-/+").Annotation.Value)
        Assert.Equal(MoveAnnotation.DecisiveAdvantageWhite, (parse pMove "Bb5+-").Annotation.Value)
        Assert.Equal(MoveAnnotation.Space, (parse pMove "Bb5○").Annotation.Value)
        Assert.Equal(MoveAnnotation.Initiative, (parse pMove "Bb5↑").Annotation.Value)
        Assert.Equal(MoveAnnotation.Development, (parse pMove "Bb5↑↑").Annotation.Value)
        Assert.Equal(MoveAnnotation.Counterplay, (parse pMove "Bb5⇄").Annotation.Value)
        Assert.Equal(MoveAnnotation.Countering, (parse pMove "Bb5∇").Annotation.Value)
        Assert.Equal(MoveAnnotation.Idea, (parse pMove "Bb5Δ").Annotation.Value)
        Assert.Equal(MoveAnnotation.TheoreticalNovelty, (parse pMove "Bb5N").Annotation.Value)
