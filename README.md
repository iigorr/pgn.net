pgn.net [![Build status](https://ci.appveyor.com/api/projects/status?id=mmyw0sm68bh4w9ac)](https://ci.appveyor.com/project/pgn-net)
=======

Portable Game Notation (PGN) implementation in .NET

## About PGN

Read more about [PGN on Wikipedia](http://en.wikipedia.org/wiki/Portable_Game_Notation).

Here is an excellent [spec document](http://www.saremba.de/chessgml/standards/pgn/pgn-complete.htm).


## About pgn.NET

pgn.NET is a library which can be used to handle chess games and read/write them in the PGN format. It is implemented in F# and C# and uses [FParsec](http://www.quanttec.com/fparsec/).

To support as many .pgn file formats as possible the parsers try to be as tolerant as possible. It mostly conforms to [this specs](http://www.saremba.de/chessgml/standards/pgn/pgn-complete.htm).
However, other than the specification says we use UTF-8 as encoding, cause it's the 21st century.

## How To Use

``` csharp
//READ FILE
var reader = new PgnReader();
var gameDb = reader.ReadFromFile("Tarrasch.pgn");

Game game = gameDb.Games[0];

Console.WriteLine(game);
```


## How To Contribute

1. Fork
1. Clone
1. Code
1. Create Pull Request

