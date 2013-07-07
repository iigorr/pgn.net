pgn.net
=======

Portable Game Notation (PGN) implementation in .NET

Read more about [PGN on Wikipedia](http://en.wikipedia.org/wiki/Portable_Game_Notation).


The following document was used as spec basis:
http://www.saremba.de/chessgml/standards/pgn/pgn-complete.htm

However, other than the specification says we use UTF-8 as encoding, cause it's the 21st century.

Example usage:

``` csharp
//READ FILE
var reader = new PgnReader();
var gameDb = reader.ReadFromFile("Tarrasch.pgn");

Game game = gameDb.Games[0];

Console.WriteLine(game);
```
