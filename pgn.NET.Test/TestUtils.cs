using Xunit.Sdk;
using ilf.pgn.Data;

namespace ilf.pgn.Test
{
    public class TestUtils
    {
        public const string TestFolder = @"TestExamples/";
        public static Database TestFile(string fileName)
        {
            if (!System.IO.File.Exists(TestFolder + fileName))
                throw new XunitException("Test data not available (" + TestFolder + fileName + ")");

            var parser = new PgnReader();
            return parser.ReadFromFile(TestFolder + fileName);

        }
    }
}
