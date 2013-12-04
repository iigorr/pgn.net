using Microsoft.VisualStudio.TestTools.UnitTesting;
using ilf.pgn.Data.MoveText;

namespace ilf.pgn.Data.Test
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void can_create()
        {
            new Game();
        }

        [TestMethod]
        public void MoveText_should_return_a_MoveTextEntryList()
        {
            Assert.IsInstanceOfType(new Game().MoveText, typeof(MoveTextEntryList));
        }


    }
}
