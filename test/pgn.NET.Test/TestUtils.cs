using Microsoft.VisualStudio.TestTools.UnitTesting;
using ilf.pgn.Data;
using pgn.NET;

namespace ilf.pgn.Test
{
    public class TestUtils
    {
        public const string TestFolder = @"Test Files\";
        public static Database TestFile(string fileName)
        {
            if (!System.IO.File.Exists(TestFolder + fileName))
                Assert.Inconclusive("Test data not available (" + TestFolder + fileName + ")");

            var parser = new PgnReader();
            return parser.ReadFromFile(TestFolder + fileName);

        }
    }
}
