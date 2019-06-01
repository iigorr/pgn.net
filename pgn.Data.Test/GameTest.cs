using Xunit;
using ilf.pgn.Data.MoveText;

namespace ilf.pgn.Data.Test
{
    public class GameTest
    {
        [Fact]
        public void can_create()
        {
            new Game();
        }

        [Fact]
        public void MoveText_should_return_a_MoveTextEntryList()
        {
            Assert.IsType<MoveTextEntryList>(new Game().MoveText);
        }
    }
}
