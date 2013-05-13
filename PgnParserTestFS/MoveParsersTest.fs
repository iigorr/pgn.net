namespace ilf.pgn.Test

open ilf.pgn
open ilf.pgn.PgnParsers.Move
open ilf.pgn.Test.TestBase

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type MoveParserTest() = 
    [<TestMethod>]
    member this.pTarget_should_accept_a_normal_move() =
        tryParse pTarget "Rc5"

    [<TestMethod>]
    member this.pTarget_should_accept_a_normal_move_with_lower_case_piece_letter() =
        tryParse pTarget "qc5"

    [<TestMethod>]
    member this.pTarget_should_accept_a_normal_move_with_upper_case_file_letter() =
        tryParse pTarget "QF5"

    [<TestMethod>]
    member this.pTarget_should_accept_a_pawn_move_with_omitted_FigureSymbol() =
        tryParse pTarget "c5"

    [<TestMethod>]
    member this.pTarget_should_fail_on_incorrect_format() =
        shouldFail pTarget "z7"
        shouldFail pTarget "a0"
        shouldFail pTarget "a9"
        shouldFail pTarget "Ka0"
        shouldFail pTarget "Fa1"

    [<TestMethod>]
    member this.pCapturingMove_should_accept_a_standard_capturing_move() =
        tryParse pCapturingMove "c5xQD5"
        tryParse pCapturingMove "c5:QD5"
        
    [<TestMethod>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capture_info_at_back() =
        tryParse pCapturingMove "Qd5x"
        tryParse pCapturingMove "Qd5:"

    [<TestMethod>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_piece_file_rank() =
        tryParse pCapturingMove "Qc5xBd5"//queen on c5 captures bishop on d5
        tryParse pCapturingMove "Qc5:Bd5"//queen on c5 captures bishop on d5

    [<TestMethod>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_piece_rank() =
        tryParse pCapturingMove "Q5xBd7"//queen on d5 captures bishop on d7
        tryParse pCapturingMove "Q5:Bd7"//queen on d5 captures bishop on d7

    [<TestMethod>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_piece_and_file() =
        tryParse pCapturingMove "QcxBd5"//queen on C file captures bishop on D5
        tryParse pCapturingMove "Qc:Bd5"//queen on C file captures bishop on D5

    [<TestMethod>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_piece() =
        tryParse pCapturingMove "QxBd5"//queen captures bishop on D5
        tryParse pCapturingMove "Q:Bd5"//queen captures bishop on D5

    [<TestMethod>]
    member this.pCapturingMove_should_accept_a_capturing_move_with_capturing_file() =
        tryParse pCapturingMove "CxQD5" //piece on C captures queen on D5
        tryParse pCapturingMove "c:QD5"

    [<TestMethod>]
    member this.pCapturingMove_should_accept_a_pawn_capturing_move_with_capturing_file() =
        tryParse pCapturingMove "cxd5" //pawn on C captures pawn on D5
        tryParse pCapturingMove "c:D5"

    [<TestMethod>]
    member this.pCapturingMove_should_accept_enpassent_suffix() =
        tryParse pCapturingMove "cxd5e.p." //pawn on c captures pawn on d5 en passant

    [<TestMethod>]
    member this.pCapturingMove_should_accept_simplified_pawn_capturing_moves() =
        tryParse pCapturingMove "cxd" //pawn on c captures pawn on d5 en passant
        tryParse pCapturingMove "cd" //pawn on c captures pawn on d5 en passant
        tryParse pCapturingMove "cxde.p" //pawn on c captures pawn on d5 en passant


    [<TestMethod>]
    member this.pBasicMove_should_accept_simple_target_move() =
        tryParse pBasicMove "Rc5"
        tryParse pBasicMove "d4d5"
        tryParse pBasicMove "d5"

    [<TestMethod>]
    member this.pBasicMove_should_accept_disambiguated_moves() =
        tryParse pBasicMove "Ngf3"
        tryParse pBasicMove "N5f3"
        tryParse pBasicMove "Rdd5"
        tryParse pBasicMove "R3d5"

    [<TestMethod>]
    member this.pPawnPromotion_should_accept_pawn_promotion_moves() =
        tryParse pPawnPromotion "d8Q"
        tryParse pPawnPromotion "c8R"
        tryParse pPawnPromotion "Pc8R"

    [<TestMethod>]
    member this.pCastle_should_accept_catling() =
        tryParse pCastle "O-O"
        tryParse pCastle "O-O-O"
        tryParse pCastle "0-0"
        tryParse pCastle "0-0-0"

    [<TestMethod>]
    member this.pMove_should_accept_all_kinds_of_moves() =
        tryParse pMove "Rc5"
        tryParse pMove "QF1"
        tryParse pMove "c5xQD5"
        tryParse pMove "Qc5:Bd5"
        tryParse pMove "cxd5e.p."
        tryParse pMove "d5"
        tryParse pMove "N5f3"
        tryParse pMove "Pc8R"
        tryParse pMove "O-O"
        tryParse pMove "0-0-0"

        
    [<TestMethod>]
    member this.pMove_should_accept_moves_with_check_annotation() =
        tryParse pMove "c5xQD5+"
        tryParse pMove "Qc5:Bd5†"
        tryParse pMove "Qc5:Bd5ch"

    [<TestMethod>]
    member this.pMove_should_accept_moves_with_doublecheck_annotation() =
        tryParse pMove "c5xQD5++"
        tryParse pMove "Qc5:Bd5††"
        tryParse pMove "Qc5:Bd5dbl ch"

    [<TestMethod>]
    member this.pMove_should_accept_moves_with_check_mate_annotation() =
        tryParse pMove "Qd2#"
        tryParse pMove "Qxd2‡"