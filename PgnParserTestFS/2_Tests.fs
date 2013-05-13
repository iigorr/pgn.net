namespace ilf.pgn.Test

open ilf.pgn
open ilf.pgn.PgnParsers
open ilf.pgn.Test.TestBase

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type ParserTests() = 
    [<TestMethod; ExpectedException(typeof<ParseException>)>]
    member this.pTag_should_fail_if_expressions_starts_with_non_bracket() =
        tryParse pTag "test"

    [<TestMethod>]
    member this.pTag_should_accept_random_spaces_before_and_after_brackets() =
        tryParse pTag "          [Date \"Foo\"]"
        tryParse pTag "  \t \n  [Date \"Foo\"]"
        tryParse pTag "  \t \n  [Date \"Foo\"]"
        tryParse pTag "[  \t \nDate \"Foo\"\t \n ]  \t \n   \t \n"
        tryParse pTag "  \t \n  [  \t \n   \t \n Date \"Foo\"  \t \n   \t \n ]  \t\n\t \n   \t\t\n\n   \t \n\n "

    [<TestMethod>]
    member this.pTag_should_accept_random_spaces_between_tag_name_and_value() =
        tryParse pTag "[Date\t \n   \t \"Foo\"]"
        tryParse pTag "[Date \"Foo\"]"
        tryParse pTag "[Date \t \n   \t\"Foo\"]"
        tryParse pTag "[  \t \nDate         \"Foo\"\t \n ]  \t \n   \t \n"
        tryParse pTag "  \t \n  [  \t \n   \t \n Date \t \n   \t \n  \"Foo\"  \t \n   \t \n ]  \t\n\t \n   \t\t\n\n   \t \n\n "
