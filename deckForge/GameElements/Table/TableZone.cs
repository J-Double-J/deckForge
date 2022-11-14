using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;
using System.Formats.Asn1;

namespace DeckForge.GameElements.Table
{
    /// <summary>
    /// Creates rules for zones that can be on a <see cref="ITable"/>.
    /// </summary>
    public class TableZone
    {
        protected List<List<ICard>> zone = new();
        protected List<IDeck> decks = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TableZone"/> class.
        /// </summary>
        /// <param name="placementZoneType">Type of zone this is.</param>
        /// <param name="areaCount">Number of areas in this zone.</param>
        /// <param name="areaCardLimit">Optional parameter that specifies how large each area can be. If set
        /// it fills all positions in area with <see cref="NullCard"/>s.</param>
        public TableZone(TablePlacementZoneType placementZoneType, int areaCount, int areaCardLimit = -1)
        {
            PlacementZoneType = placementZoneType;
            AreaCount = areaCount;
            AreaCardLimit = areaCardLimit;

            StandardConstruction(placementZoneType, areaCount, areaCardLimit);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableZone"/> class. There is a <see cref="IDeck"/> that is managed
        /// by this zone.
        /// </summary>
        /// <param name="placementZoneType">Type of zone this is.</param>
        /// <param name="areaCount">Number of areas in this zone.</param>
        /// <param name="deck"><see cref="IDeck"/> that is managed by this zone.</param>
        /// <param name="areaCardLimit">Optional parameter that specifies how large each area can be. If set
        /// it fills all positions in area with <see cref="NullCard"/>s.</param>
        public TableZone(TablePlacementZoneType placementZoneType, int areaCount, IDeck deck, int areaCardLimit = -1)
        {
            PlacementZoneType = placementZoneType;
            AreaCount = areaCount;
            AreaCardLimit = areaCardLimit;

            StandardConstruction(placementZoneType, areaCount, areaCardLimit);

            decks = new() { deck };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableZone"/> class. There is a <see cref="IDeck"/> that is managed
        /// by each area in this zone.
        /// </summary>
        /// <param name="placementZoneType">Type of zone this is.</param>
        /// <param name="areaCount">Number of areas in this zone.</param>
        /// <param name="decks"><see cref="List{IDeck}"/> that contains <see cref="IDeck"/>s that are assigned to each area.
        /// Number of <see cref="IDeck"/>s must be equal to the number of areas in zone.</param>
        /// <param name="areaCardLimit">Optional parameter that specifies how large each area can be. If set
        /// it fills all positions in area with <see cref="NullCard"/>s.</param>
        /// <exception cref="ArgumentException">Throws if the number of <paramref name="decks"/> is not equal
        /// to the number of areas in this zone.</exception>
        public TableZone(TablePlacementZoneType placementZoneType, int areaCount, List<IDeck> decks, int areaCardLimit = -1)
        {
            PlacementZoneType = placementZoneType;
            AreaCount = areaCount;
            AreaCardLimit = areaCardLimit;

            StandardConstruction(placementZoneType, areaCount, areaCardLimit);

            if (decks.Count == areaCount)
            {
                this.decks = decks;
            }
            else
            {
                throw new ArgumentException("Argument Exception: decks must have the same number of decks as areas", nameof(decks));
            }
        }

        /// <summary>
        /// Gets the type of zone.
        /// </summary>
        public TablePlacementZoneType PlacementZoneType { get; }

        /// <summary>
        /// Gets how many different card areas are managed by this zone.
        /// </summary>
        public int AreaCount { get; }

        /// <summary>
        /// Gets the limit of cards that can be in each area.
        /// -1 means there is no limit to cards in an area.
        /// </summary>
        public int AreaCardLimit { get; }

        /// <summary>
        /// Gets the list of <see cref="IDeck"/>s in the <see cref="TableZone"/>.
        /// </summary>
        public IReadOnlyList<IDeck> Decks
        {
            get { return decks; }
        }

        /// <summary>
        /// Gets a readonly list of all the <see cref="ICard"/>s managed by this zone. If there is an area limit
        /// some cards returned may be <see cref="NullCard"/>s.
        /// </summary>
        public IReadOnlyList<IReadOnlyList<ICard>> CardsInTableZone
        {
            get { return zone; }
        }

        /// <summary>
        /// Gets a readonly list of all the <see cref="ICard"/>s managed by the area in the zone.
        /// If there is an area limit some cards returned may be <see cref="NullCard"/>s.
        /// </summary>
        /// <param name="area">Area identifier for area in zone.</param>
        /// <returns>A readonly list of <see cref="ICard"/>s in area.</returns>
        public IReadOnlyList<ICard> GetCardsInArea(int area)
        {
            try
            {
                ValidateAreaArgument(area);
                return zone[area];
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Plays a card to a specific place in an area in the zone. Cannot replace a card if spot is filled.
        /// </summary>
        /// <param name="card">Card to play.</param>
        /// <param name="area">Area in the zone to play to.</param>
        /// <param name="placementInArea">Where to play in the area. Places the card in the first free space.</param>
        /// <exception cref="InvalidOperationException">Throws if trying to specify where to place a card in an area
        /// without a defined limit, or placing a card on another card.</exception>
        public void PlayCardToArea(ICard card, int area, int placementInArea)
        {
            try
            {
                CardPlacementRulesExecutioner(card, true, area, placementInArea);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Plays a card to an area in the zone, finding the first empty spot. Cannot place a card if area is filled.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to play.</param>
        /// <param name="area">Area Identifier for area to play to.</param>
        /// <exception cref="InvalidOperationException">Throws if all spots in area are filled with non-<see cref="NullCard"/>s.</exception>
        public void PlayCardToArea(ICard card, int area)
        {
            try
            {
                CardPlacementRulesExecutioner(card, true, area);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Plays a list of <see cref="ICard"/>s to an area.
        /// </summary>
        /// <param name="cards"><see cref="ICard"/>s to play.</param>
        /// <param name="area">Area Identifier for area to play to.</param>
        public void PlayCardsToArea(List<ICard> cards, int area)
        {
            try
            {
                foreach (ICard card in cards)
                {
                    PlayCardToArea(card, area);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Plays a card to a specific place in an area in the zone. Cannot replace a card if spot is filled.
        /// </summary>
        /// <param name="card">Card to place.</param>
        /// <param name="area">Area in the zone to place to.</param>
        /// <param name="placementInArea">Where to place in the area. Places the card in the first free space.</param>
        /// <exception cref="InvalidOperationException">Throws if trying to specify where to place a card in an area
        /// without a defined limit, or placing a card on another card.</exception>
        public void PlaceCardToArea(ICard card, int area, int placementInArea)
        {
            try
            {
                CardPlacementRulesExecutioner(card, false, area, placementInArea);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Plays a card to an area in the zone, finding the first empty spot. Cannot place a card if area is filled.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to place.</param>
        /// <param name="area">Area Identifier for area to place to.</param>
        /// <exception cref="InvalidOperationException">Throws if all spots in area are filled with non-<see cref="NullCard"/>s.</exception>
        public void PlaceCardToArea(ICard card, int area)
        {
            try
            {
                CardPlacementRulesExecutioner(card, false, area);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Places a list of <see cref="ICard"/>s to an area.
        /// </summary>
        /// <param name="cards"><see cref="ICard"/>s to place.</param>
        /// <param name="area">Area Identifier for area to place to.</param>
        public void PlaceMultipleCardsToArea(List<ICard> cards, int area)
        {
            try
            {
                foreach (ICard card in cards)
                {
                    PlaceCardToArea(card, area);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Removes a <see cref="ICard"/> in a specific spot in an area in the zone.
        /// </summary>
        /// <param name="area">Area identifier for an area in the zone.</param>
        /// <param name="placementInArea">Place in area to attempt to remove a <see cref="ICard"/> from.</param>
        /// <returns><c>true</c> if a <see cref="ICard"/> was succesffuly removed; otherwise, <c>false</c>. This method
        /// also returns <c>false</c> if no non-<see cref="NullCard"/> <see cref="ICard"/> was found in specified location.</returns>
        /// <exception cref="ArgumentException">Throws if invalid location is given for <paramref name="area"/>
        /// or <paramref name="placementInArea"/>.</exception>
        public bool RemoveCard(int area, int placementInArea)
        {
            try
            {
                ValidatePlaceInAreaArgument(area, placementInArea);
                if (AreaCardLimit == -1)
                {
                    ICard card = zone[area][placementInArea];
                    zone[area].RemoveAt(placementInArea);
                    card.OnRemoval();
                    return true;
                }
                else
                {
                    ICard card = zone[area][placementInArea];
                    if (card is not NullCard)
                    {
                        zone[area][placementInArea] = new NullCard();
                        card.OnRemoval();
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Removes the first instance of <see cref="ICard"/> in an area in zone.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to search for in area.</param>
        /// <param name="area">Area identifier for an area in a zone.</param>
        /// <returns><c>true</c> if a <see cref="ICard"/> was succesffuly removed; otherwise, <c>false</c>. This method
        /// also returns <c>false</c> if no non-<see cref="NullCard"/> <see cref="ICard"/> was found in the area.</returns>
        public bool RemoveCard(ICard card, int area)
        {
            try
            {
                ValidateAreaArgument(area);

                if (AreaCardLimit == -1)
                {
                    if (zone[area].Remove(card))
                    {
                        card.OnRemoval();
                        return true;
                    }
                }
                else
                {
                    for (int i = 0; i < zone[area].Count; i++)
                    {
                        if (zone[area][i] == card)
                        {
                            zone[area][i] = new NullCard();
                            card.OnRemoval();
                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Removes a <see cref="ICard"/> in a specific location in an area.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to search for.</param>
        /// <param name="area">Area identifier for area in zone.</param>
        /// <param name="placementInArea">Place in <paramref name="area"/> to search for <paramref name="card"/>.</param>
        /// <returns><c>true</c> if a <see cref="ICard"/> was succesffuly removed; otherwise, <c>false</c>. This method
        /// also returns <c>false</c> if no non-<see cref="NullCard"/> <see cref="ICard"/> was found in the area.</returns>
        public bool RemoveCard(ICard card, int area, int placementInArea)
        {
            try
            {
                ValidatePlaceInAreaArgument(area, placementInArea);

                if (zone[area][placementInArea] == card)
                {
                    if (AreaCardLimit == -1)
                    {
                        zone[area].RemoveAt(placementInArea);
                    }
                    else
                    {
                        zone[area][placementInArea] = new NullCard();
                    }

                    card.OnRemoval();
                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Draws a card from the <see cref="IDeck"/> in the zone.
        /// If there are multiple decks managed in the zone, draws from <see cref="IDeck"/> in <paramref name="area"/>.
        /// </summary>
        /// <param name="area">Area that owns the <see cref="IDeck"/> to draw from. If not specified and there are multiple
        /// <see cref="IDeck"/>s it draws from the first one.</param>
        /// <returns><see cref="ICard"/> drawn from the <see cref="IDeck"/>.</returns>
        /// <exception cref="ArgumentException">Throws if invalid <paramref name="area"/> passed.</exception>
        public ICard? DrawCardFromZone(int area = 0)
        {
            if (area > AreaCount - 1 || area < -1)
            {
                throw new ArgumentException(" area is not a valid area ID managed by this Zone.", nameof(area));
            }

            return Decks?[area].DrawCard();
        }

        /// <summary>
        /// Shuffles a <see cref="IDeck"/> in the area.
        /// </summary>
        /// <param name="area">Area that owns the <see cref="IDeck"/> to shuffle. If not specified and there are multiple
        /// <see cref="IDeck"/>s it shuffles the first one.</param>
        /// <exception cref="ArgumentException">Throws if invalid <paramref name="area"/> passed.</exception>
        public void ShuffleDeck(int area = 0)
        {
            try
            {
                ValidateAreaArgument(area);
                Decks?[area].Shuffle();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flips a <see cref="ICard"/>.
        /// </summary>
        /// <param name="area">Area in the <see cref="TableZone"/> the <see cref="ICard"/> resides in.</param>
        /// <param name="placementInArea">Specific place in area the <see cref="ICard"/> is.</param>
        public void FlipCard(int area, int placementInArea)
        {
            try
            {
                ValidatePlaceInAreaArgument(area, placementInArea);
                zone[area][placementInArea].Flip();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flips a <see cref="ICard"/> to a certain direction if it's not already that orientation.
        /// </summary>
        /// <param name="area">Area in the <see cref="TableZone"/> the <see cref="ICard"/> resides in.</param>
        /// <param name="placementInArea">Specific place in area the <see cref="ICard"/> is.</param>
        /// <param name="facedown">If <c>true</c> flips the <see cref="ICard"/> facedown, otherwise faceup.</param>
        public void FlipCardCertainWay(int area, int placementInArea, bool facedown)
        {
            try
            {
                ValidatePlaceInAreaArgument(area, placementInArea);
                zone[area][placementInArea].Facedown = facedown;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flips all <see cref="ICard"/>s in an area in the <see cref="TableZone"/>.
        /// </summary>
        /// <param name="area">Area in the <see cref="TableZone"/> holding the <see cref="ICard"/>s to flip.</param>
        public void FlipAllCardsInArea(int area)
        {
            try
            {
                ValidateAreaArgument(area);
                foreach (ICard card in zone[area])
                {
                    card.Flip();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flips all <see cref="ICard"/>s in an area a certain direction if it's not already that orientation.
        /// </summary>
        /// <param name="area">Area in the <see cref="TableZone"/> holding the <see cref="ICard"/>s to flip.</param>
        /// <param name="facedown">If <c>true</c> flips the <see cref="ICard"/>s facedown, otherwise faceup.</param>
        public void FlipAllCardsInAreaCertainWay(int area, bool facedown)
        {
            try
            {
                ValidateAreaArgument(area);
                foreach (ICard card in zone[area])
                {
                    card.Facedown = facedown;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flips all <see cref="ICard"/>s in the <see cref="TableZone"/>.
        /// </summary>
        public void FlipAllCardsInZone()
        {
            foreach (var area in zone)
            {
                foreach (ICard card in area)
                {
                    card.Flip();
                }
            }
        }

        /// <summary>
        /// Flips all <see cref="ICard"/>s in the <see cref="TableZone"/> a certain way.
        /// </summary>
        /// <param name="facedown">If <c>true</c> flips the <see cref="ICard"/>s facedown, otherwise faceup.</param>
        public void FlipAllCardsInZoneCertainWay(bool facedown)
        {
            foreach (var area in zone)
            {
                foreach (ICard card in area)
                {
                    card.Facedown = facedown;
                }
            }
        }

        // Shared among all creation of TableZones
        private void StandardConstruction(TablePlacementZoneType placementZoneType, int areaCount, int areaCardLimit = -1)
        {
            if (areaCount < 1)
            {
                throw new ArgumentException("Invalid Argument: areaCount must be greater than 1");
            }

            for (int i = 0; i < areaCount; i++)
            {
                if (areaCardLimit <= -1)
                {
                    zone.Add(new List<ICard>());
                }
                else
                {
                    List<ICard> nullCards = Enumerable.Repeat(new NullCard(), areaCardLimit).Cast<ICard>().ToList();
                    zone.Add(nullCards);
                }
            }
        }

        /// <summary>
        /// Helper function that follows rules for placing card to a spot in the zone.
        /// </summary>
        /// <param name="played">If <c>true</c>, triggers <see cref="ICard.OnPlay(CardPlacedOnTableDetails)"/> if placed, else triggers
        /// <see cref="ICard.OnPlace(CardPlacedOnTableDetails)"/> if placed.</param>
        private void CardPlacementRulesExecutioner(ICard card, bool played, int area)
        {
            try
            {
                ValidateAreaArgument(area);

                if (AreaCardLimit == -1)
                {
                    zone[area].Add(card);
                    ExecuteCardOnPlayOrOnPlace(card, played, area, zone[area].Count - 1);
                }
                else
                {
                    bool placementFound = false;

                    for (int i = 0; i < zone[area].Count; i++)
                    {
                        if (zone[area][i] is NullCard)
                        {
                            zone[area][i] = card;
                            ExecuteCardOnPlayOrOnPlace(card, played, area, i);
                            placementFound = true;
                            break;
                        }
                    }

                    if (!placementFound)
                    {
                        throw new InvalidOperationException("No open space for card to be played.");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void CardPlacementRulesExecutioner(ICard card, bool played, int area, int placementInArea)
        {
            try
            {
                ValidatePlaceInAreaArgument(area, placementInArea);
                if (zone[area][placementInArea] is NullCard)
                {
                    zone[area][placementInArea] = card;
                    ExecuteCardOnPlayOrOnPlace(card, played, area, placementInArea);
                }
                else
                {
                    throw new InvalidOperationException($"A card already exists in Area {area} at Placement {placementInArea}");
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// If <paramref name="played"/> is <c>true</c>, executes <see cref="ICard.OnPlay(CardPlacedOnTableDetails)"/> else executes <see cref="ICard.OnPlace(CardPlacedOnTableDetails)"/>.
        /// </summary>
        /// <param name="played">Determines which function to call.</param>
        private void ExecuteCardOnPlayOrOnPlace(ICard card, bool played, int area, int placementInArea)
        {
            if (played)
            {
                card.OnPlay(new CardPlacedOnTableDetails(PlacementZoneType, area, placementInArea));
            }
            else
            {
                card.OnPlace(new CardPlacedOnTableDetails(PlacementZoneType, area, placementInArea));
            }
        }

        private void ValidateAreaArgument(int area)
        {
            if (!(area < zone.Count && area >= 0))
            {
                throw new ArgumentOutOfRangeException(nameof(area));
            }
        }

        /// <summary>
        /// Validates <paramref name="placementInArea"/> argument. This also means
        /// that <paramref name="area"/> must also be validated.
        /// </summary>
        /// <param name="area">Area in the zone.</param>
        /// <param name="placementInArea">Placement in an area in the zone.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if any argument is out range
        /// of bounds of the zone or area.</exception>
        private void ValidatePlaceInAreaArgument(int area, int placementInArea)
        {
            try
            {
                ValidateAreaArgument(area);
                if (!(placementInArea < zone[area].Count && placementInArea >= 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(placementInArea));
                }
            }
            catch
            {
                throw;
            }
        }

        private void CardPlacementCallCorrectTrigger(ICard card, int area, int placementInArea, bool played)
        {
            if (played)
            {
                card.OnPlay(new CardPlacedOnTableDetails(PlacementZoneType, area, placementInArea));
            }
            else
            {
            }
        }
    }
}
