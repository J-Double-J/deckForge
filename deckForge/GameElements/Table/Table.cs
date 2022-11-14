using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards.CardEvents;
using DeckForge.HelperObjects;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameElements.Table
{
    /// <summary>
    /// Responsible for tracking the state of the game that takes place on the <see cref="Table"/> and where
    /// various elements are on it.
    /// </summary>
    public class Table : ITable
    {
        private List<List<ICard>> playerZones;
        private List<List<ICard>> neutralZones;
        private List<TableZone> zones = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="mediator"><see cref="IGameMediator"/> that is used by the <see cref="Table"/> to
        /// communicate to other game elements.</param>
        /// <param name="playerCount">Number of <see cref="IPlayer"/>s in the game.</param>
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
        /// <param name="playerCount">Number of <see cref="IPlayer"/>s in the game.</param>
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
        /// <param name="playerCount">Number of <see cref="IPlayer"/>s in the game.</param>
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

        public Table(IGameMediator gm, List<TableZone> zones)
        {
            GM = gm;
            GM.RegisterTable(this);

            this.zones = zones;

            // TODO: Temp patch.
            playerZones = new();
            neutralZones = new();

            for (var i = 0; i < FindZoneBasedOnType(TablePlacementZoneType.PlayerZone, false)?.AreaCount; i++)
            {
                List<ICard> cards = new();
                playerZones.Add(cards);
            }

            for (var i = 0; i < FindZoneBasedOnType(TablePlacementZoneType.NeutralZone, false)?.AreaCount; i++)
            {
                List<ICard> cards = new();
                neutralZones.Add(cards);
            }

            TableDecks = new();
            foreach (TableZone zone in zones)
            {
                foreach (var deck in zone.Decks)
                {
                    TableDecks.Add(deck);
                }
            }
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
            get { return FindZoneBasedOnType(TablePlacementZoneType.PlayerZone)?.CardsInTableZone ?? new List<List<ICard>>(); }
        }

        /// <inheritdoc/>
        public IReadOnlyList<IReadOnlyList<ICard>> TableNeutralZones
        {
            get { return FindZoneBasedOnType(TablePlacementZoneType.NeutralZone)?.CardsInTableZone ?? new List<List<ICard>>(); }
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
                FindZoneBasedOnType(TablePlacementZoneType.PlayerZone)!.RemoveCard(playerID, cardPos); // TODO: REMOVE REFACTOR COMMENT (This is temp.)
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

        public ICard? DrawCardFromDeck(TablePlacementZoneType zoneType, int area = 0)
        {
            try
            {
                return FindZoneBasedOnType(zoneType)!.DrawCardFromZone(area);
            }
            catch
            {
                throw;
            }
        }

        public List<ICard?> DrawMultipleCardsFromDeck(int cardCount, TablePlacementZoneType zoneType, int area = 0)
        {
            try
            {
                TableZone zone = FindZoneBasedOnType(zoneType)!;
                List<ICard?> cards = new();

                for (int i = 0; i < cardCount; i++)
                {
                    cards.Add(zone.DrawCardFromZone(area));
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
                    FindZoneBasedOnType(TablePlacementZoneType.NeutralZone)!.PlayCardToArea(drawnCard, neutralZone); // TODO: REMOVE REFACTOR COMMENT
                    neutralZones[neutralZone].Add(drawnCard);
                    retVal.Add(drawnCard);
                }
            }

            return retVal;
        }

        /// <inheritdoc/>
        public void PlayCardToZone(ICard card, TablePlacementZoneType placementZone, int area)
        {
            try
            {
                FindZoneBasedOnType(placementZone)!.PlayCardToArea(card, area);

                // TODO: REMOVE REFACTOR COMMENT
                if (placementZone == TablePlacementZoneType.PlayerZone)
                {
                    playerZones[area].Add(card);
                }
                else
                {
                    neutralZones[area].Add(card);
                }
                card.CardIsRemovedFromTable += (sender, e) =>
                {
                    RemoveCard_FromPlayerZone((ICard)sender!, area);
                };
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void PlayCardToZone(ICard card, TablePlacementZoneType placementZone, int area, int placementInArea)
        {
            try
            {
                FindZoneBasedOnType(placementZone)!.PlayCardToArea(card, area, placementInArea);

                // TODO: REMOVE REFACTOR COMMENT
                if (placementZone == TablePlacementZoneType.PlayerZone)
                {
                    playerZones[area].Add(card);
                }
                else
                {
                    neutralZones[area].Add(card);
                }
                card.CardIsRemovedFromTable += (sender, e) =>
                {
                    RemoveCard_FromPlayerZone((ICard)sender!, area);
                };
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void PlayCardsToZone(List<ICard> cards, TablePlacementZoneType placementZone, int area)
        {
            try
            {
                foreach (ICard card in cards)
                {
                    card.CardIsRemovedFromTable += (sender, e) =>
                    {
                        RemoveCard_FromPlayerZone((ICard)sender!, area);
                    };
                }

                FindZoneBasedOnType(placementZone)!.PlayCardsToArea(cards, area);

                // TODO: REMOVE REFACTOR COMMENT
                if (placementZone == TablePlacementZoneType.PlayerZone)
                {
                    playerZones[area].AddRange(cards);
                }
                else
                {
                    neutralZones[area].AddRange(cards);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void RemoveCard_FromPlayerZone(ICard card, int playerZone)
        {
            playerZones[playerZone].Remove(card);
            FindZoneBasedOnType(TablePlacementZoneType.PlayerZone)!.RemoveCard(card, playerZone); // TODO: REMOVE REFACTOR COMMENT
        }

        /// <inheritdoc/>
        public List<ICard> Remove_AllCardsFromTable() // TODO: REMOVE REFACTOR COMMENT (Honestly you need to just redo logic.)
        {
            List<ICard> cards = new();

            for (int i = 0; i < neutralZones.Count; i++)
            {
                var neutralCards = neutralZones[i];
                cards.AddRange(neutralCards);

                foreach (ICard card in neutralCards)
                {
                    FindZoneBasedOnType(TablePlacementZoneType.NeutralZone)!.RemoveCard(card, i);
                }

                neutralCards.Clear();
            }

            for (int i = 0; i < playerZones.Count; i++)
            {
                var playerCards = playerZones[i];
                cards.AddRange(playerCards);

                foreach (ICard card in playerCards)
                {
                    FindZoneBasedOnType(TablePlacementZoneType.PlayerZone)!.RemoveCard(card, i);
                }

                playerCards.Clear();
            }

            return cards;
        }

        /// <summary>
        /// Finds a zone on the <see cref="Table"/> based on the <see cref="TablePlacementZoneType"/>.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> to search for.</param>
        /// <param name="throwIfNotFound">If <c>true</c>, throws an <see cref="ArgumentException"/> if the
        /// <see cref="TablePlacementZoneType"/> is not found. Default <c>true</c>.</param>
        /// <returns><see cref="TableZone"/> on <see cref="Table"/> that is the correct type.</returns>
        /// <exception cref="ArgumentException">Throws if <see cref="TablePlacementZoneType"/> is not found
        /// and <paramref name="throwIfNotFound"/> is true.</exception>
        protected TableZone? FindZoneBasedOnType(TablePlacementZoneType zoneType, bool throwIfNotFound = true)
        {
            var result = zones.Find(zone => zone.PlacementZoneType == zoneType);
            if (result is not null || !throwIfNotFound)
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"There is no zone of type {zoneType} on this table.");
            }
        }
    }
}
