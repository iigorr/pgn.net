using System;
using ilf.pgn.Data;

namespace ParseGame
{
    class Program
    {
        static void Main()
        {
            var reader = new ilf.pgn.PgnReader();
            var gameDb = reader.ReadFromFile("Tarrasch.pgn");

            Game game = gameDb.Games[0];

            Console.WriteLine(game);

            Console.ReadKey();
        }
    }
}
