using DeckForge.GameElements.Resources;

namespace DeckForge.GameRules
{
    /// <summary>
    /// Base class for any set up rules that a game must concern itself before it begins playing.
    /// </summary>
    public class BaseSetUpRules
    {
        private int deckCount;
        private int initHandSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSetUpRules"/> class.
        /// </summary>
        /// <param name="deckCount">Number of decks to create on set up.</param>
        /// <param name="initHandSize">Initial hand size of <see cref="PlayerConstruction.IPlayer"/>s at the start
        /// of the game.</param>
        public BaseSetUpRules(int deckCount = 1, int initHandSize = 0)
        {
            Decks = new ();
            try
            {
                DeckCount = deckCount;
                InitHandSize = initHandSize;
            }
            catch
            {
                throw;
            }

            foreach (Deck d in Decks)
            {
                d.Shuffle();
            }
        }

        /// <summary>
        /// Gets the list of Decks that <see cref="BaseSetUpRules"/> has.
        /// </summary>
        public List<Deck> Decks { get; }

        /// <summary>
        /// Gets the number of decks that are managed by <see cref="BaseSetUpRules"/>.
        /// </summary>
        public int DeckCount
        {
            get
            {
                return deckCount;
            }

            private set
            {
                if (value > 0)
                {
                    deckCount = value;
                    for (var i = 0; i < value; i++)
                    {
                        Decks.Add(new Deck()); // TODO: use better logic to create the right decks and their types
                    }
                }
                else
                {
                    throw new ArgumentException("Cannot initialize a game and have a deck count that is 0 or less");
                }
            }
        }

        /// <summary>
        /// Gets the initial hand size of the <see cref="PlayerConstruction.IPlayer"/>s.
        /// </summary>
        public int InitHandSize
        {
            get
            {
                return initHandSize;
            }

            private set
            {
                if (value >= 0)
                {
                    initHandSize = value;
                }
                else
                {
                    throw new ArgumentException("Cannot have a negative hand size");
                }
            }
        }
    }
}
