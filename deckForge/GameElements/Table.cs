using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards.CardEvents;
using DeckForge.HelperObjects;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameElements
{
    /// <summary>
    /// Responsible for tracking the state of the game that takes place on the <see cref="Table"/> and where
    /// various elements are on it.
    /// </summary>
    public class Table : ITable
    {
        private List<List<ICard>> playerZones;
        private List<List<ICard>> neutralZones;

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

            playerZones = new();
            neutralZones = new();

            for (var i = 0; i < playerCount; i++)
            {
                List<ICard> cards = new();
                playerZones.Add(cards);
            }

            for (var i = 0; i < tableNeutralZonesCount; i++)
            {
                List<ICard> cards = new();
                neutralZones.Add(cards);
            }

            TableDecks = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="mediator"><see cref="IGameMediator"/> that is used by the <see cref="Table"/> to
        /// communicate to other game elements.</param>
        /// <param name="playerCount">Number of <see cref="PlayerConstruction.IPlayer"/>s in the game.</param>
        /// <param name="initDeck">Initial <see cref="IDeck"/> that is on the <see cref="Table"/>.</param>
        /// <param name="tableNeutralZonesCount">Number of nonplayer controlled areas where <see cref="ICard"/>s
        /// can be played.</param>
        public Table(IGameMediator mediator, int playerCount, IDeck initDeck, int tableNeutralZonesCount = 0)
        {
            GM = mediator;
            GM.RegisterTable(this);

            playerZones = new();
            neutralZones = new();

            for (var i = 0; i < playerCount; i++)
            {
                List<ICard> cards = new();
                playerZones.Add(cards);
            }

            for (var i = 0; i < tableNeutralZonesCount; i++)
            {
                List<ICard> cards = new();
                neutralZones.Add(cards);
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
        /// <param name="initDecks">List of initial <see cref="IDeck"/>s on the <see cref="Table"/>.</param>
        /// <param name="tableNeutralZonesCount">Number of nonplayer controlled areas where <see cref="ICard"/>s
        /// can be played.</param>
        public Table(
            IGameMediator mediator,
            int playerCount,
            List<IDeck> initDecks,
            int tableNeutralZonesCount = 0)
        {
            GM = mediator;
            GM.RegisterTable(this);

            playerZones = new();
            neutralZones = new();

            for (var i = 0; i < playerCount; i++)
            {
                List<ICard> cards = new();
                playerZones.Add(cards);
            }

            for (var i = 0; i < tableNeutralZonesCount; i++)
            {
                List<ICard> cards = new();
                neutralZones.Add(cards);
            }

            TableDecks = initDecks;
        }

        /// <inheritdoc/>
        public List<IDeck> TableDecks
        {
            get;
        }

        /// <summary>
        /// Gets the state of the <see cref="Table"/>.
        /// </summary>
        public IReadOnlyList<IReadOnlyList<ICard>> TableState
        {
            get { return PlayerZones; }
        }

        /// <inheritdoc/>
        public IReadOnlyList<IReadOnlyList<ICard>> PlayerZones
        {
            get { return playerZones; }
        }

        /// <inheritdoc/>
        public IReadOnlyList<IReadOnlyList<ICard>> TableNeutralZones
        {
            get { return neutralZones; }
        }

        /// <summary>
        /// Gets the <see cref="IGameMediator"/> that the <see cref="Table"/> uses to communicate
        /// with other game elements.
        /// </summary>
        protected IGameMediator GM { get; }

        /// <inheritdoc/>
        public void PrintTableState()
        {
            foreach (IReadOnlyList<ICard> player in PlayerZones)
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
            return playerZones[playerID];
        }

        /// <inheritdoc/>
        public List<ICard> GetCardsForSpecificNeutralZone(int neutralZone)
        {
            return neutralZones[neutralZone];
        }

        // TODO: Deprecated?

        /// <inheritdoc/>
        /*
        public void PlaceCardOnTable(int playerID, ICard card)
        {
            try
            {
                playerZones[playerID].Add(card);
                card.OnPlay(
                    new CardPlacedOnTableDetails(
                        TablePlacementZones.PlayerZone,
                        playerZones.Count - 1));
            }
            catch
            {
                throw;
            }
        }*/

        /// <inheritdoc/>
        public void Flip_AllCardsOneWay_SpecificPlayer(int playerID, bool facedown = false)
        {
            try
            {
                foreach (ICard c in PlayerZones[playerID])
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
            for (var i = 0; i < PlayerZones.Count; i++)
            {
                Flip_AllCardsOneWay_SpecificPlayer(i, facedown);
            }
        }

        /// <inheritdoc/>
        public void Flip_AllCardsEitherWay_SpecificPlayer(int playerID)
        {
            try
            {
                foreach (ICard c in PlayerZones[playerID])
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
            for (var i = 0; i < PlayerZones.Count; i++)
            {
                Flip_AllCardsEitherWay_SpecificPlayer(i);
            }
        }

        /// <inheritdoc/>
        public ICard Flip_SpecificCard_SpecificPlayer(int playerID, int cardPos)
        {
            try
            {
                PlayerZones[playerID][cardPos].Flip();
                return PlayerZones[playerID][cardPos];
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public ICard Flip_SpecificCard_SpecificPlayer_SpecificWay(int playerID, int cardPos, bool facedown = false)
        {
            if (PlayerZones[playerID][cardPos].Facedown != facedown)
            {
                PlayerZones[playerID][cardPos].Flip();
                return PlayerZones[playerID][cardPos];
            }
            else
            {
                return PlayerZones[playerID][cardPos];
            }
        }

        /// <inheritdoc/>
        public ICard RemoveSpecificCard_FromPlayer(int playerID, int cardPos)
        {
            try
            {
                ICard c = PlayerZones[playerID][cardPos];
                playerZones[playerID].RemoveAt(cardPos);
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
                var numCardsToGrab = PlayerZones[playerID].Count;
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
                    neutralZones[neutralZone].Add(drawnCard);
                    retVal.Add(drawnCard);
                }
            }

            return retVal;
        }

        public void AddCardTo_PlayerZone(int playerZone, ICard card)
        {
            card.CardIsRemovedFromTable += (sender, e) =>
            {
                RemoveCardFromTable_FromPlayerZone((ICard)sender!, playerZone);
            };
            playerZones[playerZone].Add(card);
            card.OnPlay(
                    new CardPlacedOnTableDetails(
                        TablePlacementZones.PlayerZone,
                        playerZones.Count - 1));
        }

        /// <inheritdoc/>
        public void AddCardsTo_PlayerZone(int playerZone, List<ICard> cards)
        {
            foreach (ICard card in cards)
            {
                AddCardTo_PlayerZone(playerZone, card);
            }
        }

        /// <inheritdoc/>
        public void AddCardTo_NeutralZone(ICard card, int neutralZone)
        {
            neutralZones[neutralZone].Add(card);
        }

        /// <inheritdoc/>
        public void AddCardsTo_NeutralZone(List<ICard> cards, int neutralZone)
        {
            var mergedLists = TableNeutralZones[neutralZone].Concat(cards).ToList();
            neutralZones[neutralZone] = mergedLists;
        }

        /// <inheritdoc/>
        public void RemoveCardFromTable_FromPlayerZone(ICard card, int playerZone)
        {
            playerZones[playerZone].Remove(card);
        }

        /// <inheritdoc/>
        public List<ICard> PickUp_AllCardsFromTable()
        {
            List<ICard> cards = new();

            foreach (var neutralCards in neutralZones)
            {
                cards.AddRange(neutralCards);
                neutralCards.Clear();
            }

            foreach (var playerCards in playerZones)
            {
                cards.AddRange(playerCards);
                playerCards.Clear();
            }

            return cards;
        }
    }
}
