using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ilf.pgn.Test
{
    [TestClass]
    public class ParserTest
    {
        private const string NormalGame = @"
[Event ""Breslau""]
[Site ""Breslau""]
[Date ""1879.??.??""]
[Round ""?""]
[White ""Tarrasch, Siegbert""]
[Black ""Mendelsohn, J.""]
[Result ""1-0""]
[WhiteElo """"]
[BlackElo """"]
[ECO ""C49""]

1.e4 e5 2.Nf3 Nc6 3.Nc3 Nf6 4.Bb5 Bb4 5.Nd5 Nxd5 6.exd5 Nd4 7.Ba4 b5 8.Nxd4 bxa4
9.Nf3 O-O 10.O-O d6 11.c3 Bc5 12.d4 exd4 13.Nxd4 Ba6 14.Re1 Bc4 15.Nc6 Qf6
16.Be3 Rfe8 17.Bxc5 Rxe1+ 18.Qxe1 dxc5 19.Qe4 Bb5 20.d6 Kf8 21.Ne7 Re8 22.Qxh7 Qxd6
23.Re1 Be2 24.Nf5  1-0
";
        private void PrepareFile(string fileName, string content)
        {
            var file = new StreamWriter(fileName);
            file.WriteLine(content);
            file.Close();
        }

        private Stream PrepareStream(string content)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        [TestMethod]
        public void Parser_constructor_should_work()
        {
            new Parser();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ReadFromFile_should_throw_Exception_if_file_is_not_found()
        {
            var parser = new Parser();
            Assert.IsInstanceOfType(parser.ReadFromFile("blub_non-existent.pgn"), typeof(Database));
        }

        [TestMethod]
        public void ReadFromFile_should_return_a_Database()
        {
            var parser = new Parser();
            PrepareFile("test.pgn", NormalGame);
            Assert.IsInstanceOfType(parser.ReadFromFile("test.pgn"), typeof(Database));
        }

        [TestMethod]
        public void ReadFromStream_should_return_a_Database()
        {
            var parser = new Parser();
            var stream = PrepareStream(NormalGame);
            Assert.IsInstanceOfType(parser.ReadFromStream(stream), typeof(Database));
        }

        [TestMethod]
        public void ReadFromStream_should_return_empty_Database_if_data_is_empty()
        {
            var parser = new Parser();
            var stream = PrepareStream(NormalGame);
            var db = parser.ReadFromStream(stream);

            Assert.AreEqual(0, db.Games.Count);
        }
    }
}
