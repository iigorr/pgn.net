using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ilf.pgn.Test
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void Database_constructor_should_work()
        {
            new Database();
        }

        [TestMethod]
        public void Database_should_contain_a_list_of_games()
        {
            var sut = new Database();
            Assert.IsInstanceOfType(sut.Games, typeof(List<Game>));
        }
    }
}
