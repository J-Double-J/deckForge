using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards.CardEvents;
using DeckForge.HelperObjects;
using DeckForge.PlayerConstruction;
using System.Xml.Linq;

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
        }

        public Table(IGameMediator gm, List<TableZone> zones)
        {
            GM = gm;
            GM.RegisterTable(this);

            this.zones = zones;

            // TODO: Temp patch. TODO: REMOVE REFACTOR COMMENT
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
        public void RemoveCardFromTable(ICard card, TablePlacementZoneType zoneType, int area)
        {
            try
            {
                FindZoneBasedOnType(zoneType)!.RemoveCard(card, area);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void RemoveCardFromTable(ICard card, TablePlacementZoneType zoneType, int area, int placementInArea)
        {
            try
            {
                FindZoneBasedOnType(zoneType)!.RemoveCard(card, area, placementInArea);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void RemoveCardFromTable(TablePlacementZoneType zoneType, int area, int placementInArea)
        {
            try
            {
                FindZoneBasedOnType(zoneType)!.RemoveCard(area, placementInArea);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public List<ICard> PickUpAllCards_FromArea(TablePlacementZoneType zoneType, int area)
        {
            try
            {
                TableZone zone = FindZoneBasedOnType(zoneType)!;
                List<ICard> pickedUpCards = new();

                while (zone.GetCardsInArea(area).Count > 0)
                {
                    pickedUpCards.Add(zone.GetCardsInArea(area)[0]);
                    zone.RemoveCard(area, 0);
                }

                return pickedUpCards;
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public List<ICard> DrawMultipleCardsFromDeck(int cardCount, TablePlacementZoneType zoneType, int area = 0)
        {
            try
            {
                TableZone zone = FindZoneBasedOnType(zoneType)!;
                List<ICard> cards = new();

                for (int i = 0; i < cardCount; i++)
                {
                    ICard? card = zone.DrawCardFromZone(area);
                    if (card is not null)
                    {
                        cards.Add(card);
                    }
                    else
                    {
                        break;
                    }
                }

                return cards;
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void ShuffleDeck(TablePlacementZoneType tablePlacementZoneType, int area = 0)
        {
            try
            {
                FindZoneBasedOnType(tablePlacementZoneType)?.ShuffleDeck(area);
            }
            catch
            {
                throw;
            }
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
                    RemoveCardFromTable((ICard)sender!, e.PlacementDetails.TablePlacementZone, e.PlacementDetails.Area);
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
                    RemoveCardFromTable((ICard)sender!, e.PlacementDetails.TablePlacementZone, e.PlacementDetails.Area);
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
                        RemoveCardFromTable((ICard)sender!, e.PlacementDetails.TablePlacementZone, e.PlacementDetails.Area);
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
        public List<ICard> Remove_AllCardsFromTable()
        {
            List<ICard> cards = new();

            foreach (var zone in zones)
            {
                for (int i = 0; i < zone.AreaCount; i++)
                {
                    while (zone.GetCardsInArea(i).Count > 0)
                    {
                        cards.Add(zone.GetCardsInArea(i)[0]);
                        zone.RemoveCard(i, 0);
                    }
                }
            }

            // TODO: REMOVE REFACTOR COMMENT
            playerZones.Clear();
            neutralZones.Clear();

            return cards;
        }

        /// <inheritdoc/>
        public IReadOnlyList<IDeck> GetDecksFromZone(TablePlacementZoneType zoneType)
        {
            return FindZoneBasedOnType(zoneType, false)?.Decks ?? new List<IDeck>();
        }

        /// <inheritdoc/>
        public IDeck? GetDeckFromAreaInZone(TablePlacementZoneType zoneType, int area)
        {
            try
            {
                return FindZoneBasedOnType(zoneType)?.Decks[area];
            }
            catch
            {
                return null;
            }
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
