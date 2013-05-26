using System;
using ilf.pgn.Data;

namespace ParseGame
{
    class Program
    {
        static void Main()
        {
            var parser = new ilf.pgn.Parser();
            var gameDb = parser.ReadFromFile("Tarrasch.pgn");

            Game game = gameDb.Games[0];

            Console.WriteLine("Game " + game.WhitePlayer + " vs. " + game.BlackPlayer);
            Console.WriteLine("Game result: " + game.Result + " in Round " + (game.Round??"?"));

            Console.ReadKey();
        }
    }
}
