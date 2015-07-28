using System;
using ilf.pgn;

namespace ReadPgnFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new PgnReader();

            var gamesCollection = reader.ReadGamesFromFile(@"c:\tmp\millionbase-2.22.pgn"); 

            int count = 0;
            var start = DateTime.Now;
            foreach (var item in gamesCollection)
            {
                if (++count % 100 == 0)
                {
                    Console.WriteLine();
                    var now = DateTime.Now;
                    var timespan = now - start;
                    var gamesPerSecond = ((double)count) / timespan.TotalSeconds;
                    Console.SetCursorPosition(1, 0);
                    Console.WriteLine("Time passed: {0}. Games: {1}, Games per Second: {2}", timespan, count, gamesPerSecond);
                    if (count > 10000)
                        break;
                }
            }

            Console.ReadKey();
        }
    }
}
