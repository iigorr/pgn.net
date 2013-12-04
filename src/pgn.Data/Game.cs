using System.Collections.Generic;
using System.ComponentModel;
using ilf.pgn.Data.Format;
using ilf.pgn.Data.MoveText;

namespace ilf.pgn.Data
{
    /// <summary>
    /// A PGN Game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            AdditionalInfo = new List<GameInfo>();
            MoveText = new MoveTextEntryList();
        }
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>
        /// The event.
        /// </value>
        public string Event { get; set; }
        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        /// <value>
        /// The site.
        /// </value>
        public string Site { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year or <c>null</c> if set to ????.
        /// </value>
        public int? Year { get; set; }
        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month or <c>null</c> if set to ??.
        /// </value>
        public int? Month { get; set; }
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>
        /// The day or <c>null</c> if set to ??.
        /// </value>
        public int? Day { get; set; }

        /// <summary>
        /// Gets or sets the round.
        /// </summary>
        /// <value>
        /// The round.
        /// </value>
        public string Round { get; set; }

        /// <summary>
        /// Gets or sets the white player's name.
        /// </summary>
        /// <value>
        /// The white player's name.
        /// </value>
        public string WhitePlayer { get; set; }
        /// <summary>
        /// Gets or sets the black player's name.
        /// </summary>
        /// <value>
        /// The black player's name.
        /// </value>
        public string BlackPlayer { get; set; }

        /// <summary>
        /// Gets or sets the result (default is <see cref="GameResult.Open"/>).
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [DefaultValue(GameResult.Open)]
        public GameResult Result { get; set; }

        /// <summary>
        /// Gets or sets the additional info.
        /// </summary>
        /// <value>
        /// The additional info.
        /// </value>
        public List<GameInfo> AdditionalInfo { get; set; }

        /// <summary>
        /// Gets or sets the full move text including moves, comments, annotations, etc.
        /// </summary>
        /// <value>
        /// The move text.
        /// </value>
        public MoveTextEntryList MoveText { get; set; }

        /// <summary>
        /// Gets or sets the board setup.
        /// </summary>
        public BoardSetup BoardSetup { get; set; }


        /// <summary>
        /// Returns the PGN representation of the game as <see cref="System.String" />.
        /// </summary>
        /// <returns>
        /// PGN representation of the game as <see cref="System.String" />
        /// </returns>
        public override string ToString()
        {
            return new Formatter().Format(this);
        }
    }
}
