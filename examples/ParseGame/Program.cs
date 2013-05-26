using System;

namespace ParseGame
{
    class Program
    {
        static void Main()
        {
            var parser = new ilf.pgn.Parser();
            var gameDb = parser.ReadFromFile("Tarrasch.pgn");

            var game = gameDb.Games[0];

            Console.WriteLine("Game " + game.WhitePlayer + " vs. " + game.BlackPlayer);
            Console.WriteLine(game.Result);
        }
    }
}
