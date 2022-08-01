using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;

namespace DeckForge.GameElements
{
    /// <summary>
    /// Responsible for tracking the state of the game that takes place on the <see cref="Table"/> and where
    /// various elements are on it.
    /// </summary>
    public class Table : ITable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="mediator"><see cref="IGameMediator"/> that is used by the <see cref="Table"/> to
        /// communicate to other game elements.</param>
        /// <param name="playerCount">Number of <see cref="PlayerConstruction.IPlayer"/>s in the game.</param>
        public Table(IGameMediator mediator, int playerCount)
        {
            GM = mediator;
            GM.RegisterTable(this);

            PlayedCards = new ();
            for (var i = 0; i < playerCount; i++)
            {
                List<Card> cards = new ();
                PlayedCards.Add(cards);
            }

            TableDecks = new ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="mediator"><see cref="IGameMediator"/> that is used by the <see cref="Table"/> to
        /// communicate to other game elements.</param>
        /// <param name="playerCount">Number of <see cref="PlayerConstruction.IPlayer"/>s in the game.</param>
        /// <param name="initDeck">Initial <see cref="Deck"/> that is on the <see cref="Table"/>.</param>
        public Table(IGameMediator mediator, int playerCount, Deck initDeck)
        {
            GM = mediator;
            GM.RegisterTable(this);

            PlayedCards = new ();
            for (var i = 0; i < playerCount; i++)
            {
                List<Card> cards = new ();
                PlayedCards.Add(cards);
            }

            TableDecks = new ()
            {
                initDeck
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="mediator"><see cref="IGameMediator"/> that is used by the <see cref="Table"/> to
        /// communicate to other game elements.</param>
        /// <param name="playerCount">Number of <see cref="PlayerConstruction.IPlayer"/>s in the game.</param>
        /// <param name="initDecks">List of initial <see cref="Deck"/>s on the <see cref="Table"/>.</param>
        public Table(IGameMediator mediator, int playerCount, List<Deck> initDecks)
        {
            GM = mediator;
            GM.RegisterTable(this);

            PlayedCards = new ();
            for (var i = 0; i < playerCount; i++)
            {
                List<Card> cards = new ();
                PlayedCards.Add(cards);
            }

            TableDecks = initDecks;
        }

        /// <inheritdoc/>
        public List<Deck> TableDecks
        {
            get;
        }

        /// <summary>
        /// Gets the state of the <see cref="Table"/>.
        /// </summary>
        public List<List<Card>> TableState
        {
            get { return PlayedCards; }
        }

        /// <inheritdoc/>
        public List<List<Card>> PlayedCards
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IGameMediator"/> that the <see cref="Table"/> uses to communicate
        /// with other game elements.
        /// </summary>
        protected IGameMediator GM { get; }

        /// <inheritdoc/>
        public void PrintTableState()
        {
            foreach (List<Card> player in PlayedCards)
            {
                foreach (Card c in player)
                {
                    Console.WriteLine(c.PrintCard());
                }
            }
        }

        /// <inheritdoc/>
        public List<Card> GetCardsForSpecificPlayer(int playerID)
        {
            return PlayedCards[playerID];
        }

        /// <inheritdoc/>
        public void PlaceCardOnTable(int playerID, Card c)
        {
            try
            {
                PlayedCards[playerID].Add(c);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void Flip_AllCardsOneWay_SpecificPlayer(int playerID, bool facedown = false)
        {
            try
            {
                foreach (Card c in PlayedCards[playerID])
                {
                    if (c.Facedown != facedown)
                    {
                        c.Flip();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void Flip_AllCardsOneWay_AllPLayers(bool facedown = false)
        {
            for (var i = 0; i < PlayedCards.Count; i++)
            {
                Flip_AllCardsOneWay_SpecificPlayer(i, facedown);
            }
        }

        /// <inheritdoc/>
        public void Flip_AllCardsEitherWay_SpecificPlayer(int playerID)
        {
            try
            {
                foreach (Card c in PlayedCards[playerID])
                {
                    c.Flip();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void Flip_AllCardsEitherWay_AllPlayers()
        {
            for (var i = 0; i < PlayedCards.Count; i++)
            {
                Flip_AllCardsEitherWay_SpecificPlayer(i);
            }
        }

        /// <inheritdoc/>
        public Card Flip_SpecificCard_SpecificPlayer(int playerID, int cardPos)
        {
            try
            {
                PlayedCards[playerID][cardPos].Flip();
                return PlayedCards[playerID][cardPos];
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public Card Flip_SpecificCard_SpecificPlayer_SpecificWay(int playerID, int cardPos, bool facedown = false)
        {
            if (PlayedCards[playerID][cardPos].Facedown != facedown)
            {
                PlayedCards[playerID][cardPos].Flip();
                return PlayedCards[playerID][cardPos];
            }
            else
            {
                return PlayedCards[playerID][cardPos];
            }
        }

        /// <inheritdoc/>
        public Card RemoveSpecificCard_FromPlayer(int playerID, int cardPos)
        {
            try
            {
                Card c = PlayedCards[playerID][cardPos];
                PlayedCards[playerID].RemoveAt(cardPos);
                return c;
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public List<Card> PickUpAllCards_FromPlayer(int playerID)
        {
            try
            {
                List<Card> cards = new ();
                var numCardsToGrab = PlayedCards[playerID].Count;
                for (var i = 0; i < numCardsToGrab; i++)
                {
                    cards.Add(RemoveSpecificCard_FromPlayer(playerID: playerID, cardPos: 0));
                }

                return cards;
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public Card? DrawCardFromDeck(int deckNum = 0)
        {
            try
            {
                return TableDecks[deckNum].DrawCard();
            }
            catch
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(deckNum),
                    message: "Index was out of range. Did you give a deck to the table?");
            }
        }

        /// <inheritdoc/>
        public List<Card?> DrawMultipleCardsFromDeck(int cardCount, int deckNum = 0)
        {
            List<Card?> cards = new ();
            try
            {
                for (var i = 0; i < cardCount; i++)
                {
                    cards.Add(DrawCardFromDeck(deckNum));
                }

                return cards;
            }
            catch
            {
                throw;
            }
        }
    }
}
