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
