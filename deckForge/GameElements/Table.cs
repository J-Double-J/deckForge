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
        /// <param name="tableNeutralZonesCount">Number of nonplayer controlled areas where <see cref="ICard"/>s
        /// can be played.</param>
        public Table(IGameMediator mediator, int playerCount, int tableNeutralZonesCount = 0)
        {
            GM = mediator;
            GM.RegisterTable(this);

            PlayerPlayedCards = new();
            TableNeutralZones = new();

            for (var i = 0; i < playerCount; i++)
            {
                List<ICard> cards = new();
                PlayerPlayedCards.Add(cards);
            }

            for (var i = 0; i < tableNeutralZonesCount; i++)
            {
                List<ICard> cards = new();
                TableNeutralZones.Add(cards);
            }

            TableDecks = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="mediator"><see cref="IGameMediator"/> that is used by the <see cref="Table"/> to
        /// communicate to other game elements.</param>
        /// <param name="playerCount">Number of <see cref="PlayerConstruction.IPlayer"/>s in the game.</param>
        /// <param name="initDeck">Initial <see cref="DeckOfPlayingCards"/> that is on the <see cref="Table"/>.</param>
        /// <param name="tableNeutralZonesCount">Number of nonplayer controlled areas where <see cref="ICard"/>s
        /// can be played.</param>
        public Table(IGameMediator mediator, int playerCount, DeckOfPlayingCards initDeck, int tableNeutralZonesCount = 0)
        {
            GM = mediator;
            GM.RegisterTable(this);

            PlayerPlayedCards = new();
            TableNeutralZones = new();

            for (var i = 0; i < playerCount; i++)
            {
                List<ICard> cards = new();
                PlayerPlayedCards.Add(cards);
            }

            for (var i = 0; i < tableNeutralZonesCount; i++)
            {
                List<ICard> cards = new();
                TableNeutralZones.Add(cards);
            }

            TableDecks = new()
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
        /// <param name="initDecks">List of initial <see cref="DeckOfPlayingCards"/>s on the <see cref="Table"/>.</param>
        /// <param name="tableNeutralZonesCount">Number of nonplayer controlled areas where <see cref="ICard"/>s
        /// can be played.</param>
        public Table(
            IGameMediator mediator,
            int playerCount,
            List<DeckOfPlayingCards> initDecks,
            int tableNeutralZonesCount = 0)
        {
            GM = mediator;
            GM.RegisterTable(this);

            PlayerPlayedCards = new();
            TableNeutralZones = new();

            for (var i = 0; i < playerCount; i++)
            {
                List<ICard> cards = new();
                PlayerPlayedCards.Add(cards);
            }

            for (var i = 0; i < tableNeutralZonesCount; i++)
            {
                List<ICard> cards = new();
                TableNeutralZones.Add(cards);
            }

            TableDecks = initDecks;
        }

        /// <inheritdoc/>
        public List<DeckOfPlayingCards> TableDecks
        {
            get;
        }

        /// <summary>
        /// Gets the state of the <see cref="Table"/>.
        /// </summary>
        public List<List<ICard>> TableState
        {
            get { return PlayerPlayedCards; }
        }

        /// <inheritdoc/>
        public List<List<ICard>> PlayerPlayedCards
        {
            get;
        }

        /// <inheritdoc/>
        public List<List<ICard>> TableNeutralZones
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
            foreach (List<ICard> player in PlayerPlayedCards)
            {
                foreach (ICard c in player)
                {
                    Console.WriteLine(c.PrintCard());
                }
            }
        }

        /// <inheritdoc/>
        public List<ICard> GetCardsForSpecificPlayer(int playerID)
        {
            return PlayerPlayedCards[playerID];
        }

        /// <inheritdoc/>
        public List<ICard> GetCardsForSpecificNeutralZone(int neutralZone)
        {
            return TableNeutralZones[neutralZone];
        }

        /// <inheritdoc/>
        public void PlaceCardOnTable(int playerID, ICard c)
        {
            try
            {
                PlayerPlayedCards[playerID].Add(c);
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
                foreach (ICard c in PlayerPlayedCards[playerID])
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
            for (var i = 0; i < PlayerPlayedCards.Count; i++)
            {
                Flip_AllCardsOneWay_SpecificPlayer(i, facedown);
            }
        }

        /// <inheritdoc/>
        public void Flip_AllCardsEitherWay_SpecificPlayer(int playerID)
        {
            try
            {
                foreach (ICard c in PlayerPlayedCards[playerID])
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
            for (var i = 0; i < PlayerPlayedCards.Count; i++)
            {
                Flip_AllCardsEitherWay_SpecificPlayer(i);
            }
        }

        /// <inheritdoc/>
        public ICard Flip_SpecificCard_SpecificPlayer(int playerID, int cardPos)
        {
            try
            {
                PlayerPlayedCards[playerID][cardPos].Flip();
                return PlayerPlayedCards[playerID][cardPos];
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public ICard Flip_SpecificCard_SpecificPlayer_SpecificWay(int playerID, int cardPos, bool facedown = false)
        {
            if (PlayerPlayedCards[playerID][cardPos].Facedown != facedown)
            {
                PlayerPlayedCards[playerID][cardPos].Flip();
                return PlayerPlayedCards[playerID][cardPos];
            }
            else
            {
                return PlayerPlayedCards[playerID][cardPos];
            }
        }

        /// <inheritdoc/>
        public ICard RemoveSpecificCard_FromPlayer(int playerID, int cardPos)
        {
            try
            {
                ICard c = PlayerPlayedCards[playerID][cardPos];
                PlayerPlayedCards[playerID].RemoveAt(cardPos);
                return c;
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public List<ICard> PickUpAllCards_FromPlayer(int playerID)
        {
            try
            {
                List<ICard> cards = new();
                var numCardsToGrab = PlayerPlayedCards[playerID].Count;
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
        public ICard? DrawCardFromDeck(int deckNum = 0)
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
        public List<ICard?> DrawMultipleCardsFromDeck(int cardCount, int deckNum = 0)
        {
            List<ICard?> cards = new();
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

        /// <inheritdoc/>
        public void ShuffleDeck(int deckPosition)
        {
            try
            {
                TableDecks[deckPosition].Shuffle();
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public List<ICard> PlayCards_FromTableDeck_ToNeutralZone(
            int numCards,
            int deckPos,
            int neutralZone,
            bool isFaceup = true)
        {
            List<ICard> retVal = new();
            for (int i = 0; i < numCards; i++)
            {
                ICard? drawnCard = TableDecks[deckPos].DrawCard();
                if (drawnCard is not null)
                {
                    drawnCard.Facedown = isFaceup;
                    TableNeutralZones[neutralZone].Add(drawnCard);
                    retVal.Add(drawnCard);
                }
            }

            return retVal;
        }

        /// <inheritdoc/>
        public void AddCardTo_NeutralZone(ICard card, int neutralZone)
        {
            TableNeutralZones[neutralZone].Add(card);
        }

        /// <inheritdoc/>
        public void AddCardsTo_NeutralZone(List<ICard> cards, int neutralZone)
        {
            var mergedLists = TableNeutralZones[neutralZone].Concat(cards).ToList();
            TableNeutralZones[neutralZone] = mergedLists;
        }

        /// <inheritdoc/>
        public List<ICard> PickUp_AllCardsFromTable()
        {
            List<ICard> cards = new();

            foreach (var neutralCards in TableNeutralZones)
            {
                cards.AddRange(neutralCards);
                neutralCards.Clear();
            }

            foreach (var playerCards in PlayerPlayedCards)
            {
                cards.AddRange(playerCards);
                playerCards.Clear();
            }

            return cards;
        }
    }
}
