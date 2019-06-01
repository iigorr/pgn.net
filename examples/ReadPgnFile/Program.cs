using System;
using ilf.pgn;
using ilf.pgn.Data;

namespace ReadPgnFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new PgnReader();
            var db = reader.ReadFromFile("Tarrasch.pgn");
            var game = db.Games[0];

            Console.WriteLine("{0} vs {1} in {2} ({3})", game.WhitePlayer, game.BlackPlayer, game.Site, game.Year);

            if (game.Result == GameResult.White)
            {
                Console.WriteLine("\nWhite wins in {0} moves.", game.MoveText.FullMoveCount);
            }
            else if (game.Result == GameResult.Black)
            {
                Console.WriteLine("\nBlack wins in {0} moves.", game.MoveText.FullMoveCount);
            }
            else if (game.Result == GameResult.Draw)
            {
                Console.WriteLine("\nGame ends in a draw.");
            }
            else
            {
                Console.WriteLine("\nGame not unfinshed.");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();
        }
    }
}
