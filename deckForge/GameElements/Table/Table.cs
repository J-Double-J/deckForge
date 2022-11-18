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
        private List<TableZone> zones = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other elements in the game.</param>
        /// <param name="zones">List of <see cref="TableZone"/>s that are on this <see cref="Table"/> and be interacted
        /// with.</param>
        public Table(IGameMediator gm, List<TableZone> zones)
        {
            GM = gm;
            GM.RegisterTable(this);

            this.zones = zones;
        }

        /// <summary>
        /// Gets the state of the <see cref="Table"/>.
        /// </summary>
        public IReadOnlyList<IReadOnlyList<ICard>> TableState
        {
            // TODO: REMOVE REFACTOR COMMENT Why the hell is this returning only players???
            get { return FindZoneBasedOnType(TablePlacementZoneType.PlayerZone)?.CardsInTableZone ?? new List<List<ICard>>(); }
        }

        /// <inheritdoc/>
        public IReadOnlyList<TableZone> TableZones
        {
            get { return zones; }
        }

        /// <summary>
        /// Gets the <see cref="IGameMediator"/> that the <see cref="Table"/> uses to communicate
        /// with other game elements.
        /// </summary>
        protected IGameMediator GM { get; }

        /// <inheritdoc/>
        public void PrintTableState()
        {
            // TODO: REMOVE REFACTOR COMMENT This too? Why only print the player zones??
            foreach (IReadOnlyList<ICard> player in FindZoneBasedOnType(TablePlacementZoneType.PlayerZone)!.CardsInTableZone)
            {
                foreach (ICard c in player)
                {
                    Console.WriteLine(c.PrintCard());
                }
            }
        }

        /// <inheritdoc/>
        public IReadOnlyList<IReadOnlyList<ICard>> GetCardsInZone(TablePlacementZoneType zoneType)
        {
            return FindZoneBasedOnType(zoneType)!.CardsInTableZone;
        }

        /// <inheritdoc/>
        public void FlipCardInZone(TablePlacementZoneType zoneType, int area, int placementInArea)
        {
            FindZoneBasedOnType(zoneType)!.FlipCard(area, placementInArea);
        }

        /// <inheritdoc/>
        public void FlipCardInZoneCertainWay(TablePlacementZoneType zoneType, int area, int placementInArea, bool facedown)
        {
            FindZoneBasedOnType(zoneType)!.FlipCardCertainWay(area, placementInArea, facedown);
        }

        /// <inheritdoc/>
        public void FlipAllCardsInAreaInZone(TablePlacementZoneType zoneType, int area)
        {
            FindZoneBasedOnType(zoneType)!.FlipAllCardsInArea(area);
        }

        /// <inheritdoc/>
        public void FlipAllCardsInAreaInZoneCertainWay(TablePlacementZoneType zoneType, int area, bool facedown)
        {
            FindZoneBasedOnType(zoneType)!.FlipAllCardsInAreaCertainWay(area, facedown);
        }

        /// <inheritdoc/>
        public void FlipAllCardsInZone(TablePlacementZoneType zoneType)
        {
            FindZoneBasedOnType(zoneType)!.FlipAllCardsInZone();
        }

        /// <inheritdoc/>
        public void FlipAllCardsInZoneCertainWay(TablePlacementZoneType zoneType, bool facedown)
        {
            FindZoneBasedOnType(zoneType)!.FlipAllCardsInZoneCertainWay(facedown);
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
