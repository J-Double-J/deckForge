using DeckNameSpace;

namespace deckForge.GameRules
{
    public class BaseSetUpRules
    {
        private int _deckCount;
        private int _initHandSize;
        public readonly List<Deck> Decks = new List<Deck>();
        

        public BaseSetUpRules(int deckCount = 1, int initHandSize = 0)
        {
            try
            {
                DeckCount = deckCount;
                InitHandSize = initHandSize;
            }
            catch
            {
                throw;
            }

            foreach (Deck d in Decks) {
                d.Shuffle();
            }
        }

        public int DeckCount
        {
            get { return _deckCount; }
            private set
            {
                if (value > 0)
                {
                    _deckCount = value;
                    for (var i = 0; i < value; i++)
                    {
                        Decks.Add(new Deck()); //TODO use better logic to create the right decks and their types
                    }
                }
                else
                {
                    throw new ArgumentException("Cannot initialize a game and have a deck count that is 0 or less");
                }
            }
        }

        public int InitHandSize
        {
            get { return _initHandSize; }
            private set
            {
                if (value >= 0)
                    _initHandSize = value;
                else
                    throw new ArgumentException("Cannot have a negative hand size");
            }
        }
    }
}
