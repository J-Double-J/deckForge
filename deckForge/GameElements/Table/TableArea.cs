using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;

namespace DeckForge.GameElements.Table
{
    /// <summary>
    /// An area in a <see cref="TableZone"/>.
    /// </summary>
    public class TableArea
    {
        private IDeck? deck = null;
        private List<IDeck> decks = new();
        private List<ICard> playArea = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TableArea"/> class.
        /// </summary>
        /// <param name="id">ID of the <see cref="TableArea"/></param>
        /// <param name="owningZoneType">Type of <see cref="TableZone"/> that manages this <see cref="TableArea"/>.</param>
        /// <param name="areaCardLimit">Limit on the number of <see cref="ICard"/>s that can be played in an area. If -1
        /// there is no limit.</param>
        /// <param name="discardPile">Discard pile for the <see cref="TableArea"/>.</param>
        /// <param name="trashPile">Trash pile for the <see cref="TableArea"/>.</param>
        public TableArea(int id, TablePlacementZoneType owningZoneType, int areaCardLimit = -1, bool discardPile = false, bool trashPile = false)
        {
            ID = id;
            OwningZoneType = owningZoneType;
            AreaCardLimit = areaCardLimit;

            if (AreaCardLimit > 0)
            {
                for (int i = 0; i < AreaCardLimit; i++)
                {
                    playArea.Add(new NullCard());
                }
            }

            if (discardPile)
            {
                DiscardPile = new EmptyDeck();
            }

            if (trashPile)
            {
                TrashPile = new EmptyDeck();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableArea"/> class. Has a <see cref="IDeck"/> managed in this <see cref="TableArea"/>.
        /// </summary>
        /// <param name="id">ID of the <see cref="TableArea"/></param>
        /// <param name="owningZoneType">Type of <see cref="TableZone"/> that manages this <see cref="TableArea"/>.</param>
        /// <param name="deck"><see cref="IDeck"/> managed by this <see cref="TableArea"/>.</param>
        /// <param name="areaCardLimit">Limit on the number of <see cref="ICard"/>s that can be played in an area. If -1
        /// there is no limit.</param>
        /// <param name="discardPile">Discard pile for the <see cref="TableArea"/>.</param>
        /// <param name="trashPile">Trash pile for the <see cref="TableArea"/>.</param>
        public TableArea(int id, TablePlacementZoneType owningZoneType, IDeck deck, int areaCardLimit = -1, bool discardPile = false, bool trashPile = false)
            : this(id, owningZoneType, areaCardLimit, discardPile, trashPile)
        {
            Deck = deck;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableArea"/> class. Has a list of <see cref="IDeck"/>s managed in this
        /// <see cref="TableArea"/>.
        /// </summary>
        /// <param name="id">ID of the <see cref="TableArea"/></param>
        /// <param name="owningZoneType">Type of <see cref="TableZone"/> that manages this <see cref="TableArea"/>.</param>
        /// <param name="decks"><see cref="IDeck"/>s managed by this <see cref="TableArea"/>.</param>
        /// <param name="areaCardLimit">Limit on the number of <see cref="ICard"/>s that can be played in an area. If -1
        /// there is no limit.</param>
        /// <param name="discardPile">Discard pile for the <see cref="TableArea"/>.</param>
        /// <param name="trashPile">Trash pile for the <see cref="TableArea"/>.</param>
        public TableArea(int id, TablePlacementZoneType owningZoneType,  List<IDeck> decks, int areaCardLimit = -1, bool discardPile = false, bool trashPile = false)
            : this(id, owningZoneType, areaCardLimit, discardPile, trashPile)
        {
            Decks = decks;
        }

        /// <summary>
        /// Gets the <see cref="TableArea"/> ID.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Gets the type of <see cref="TableZone"/> that owns this <see cref="TableArea"/>.
        /// </summary>
        public TablePlacementZoneType OwningZoneType { get; }

        /// <summary>
        /// Gets the <see cref="ICard"/>s played to this <see cref="TableArea"/>.
        /// </summary>
        public IReadOnlyList<ICard> PlayArea
        {
            get { return playArea; }
        }

        /// <summary>
        /// Gets or sets the maximum number of cards that can be played to this area.
        /// </summary>
        public int AreaCardLimit { get; protected set; }

        /// <summary>
        /// Gets or sets the discard pile of <see cref="ICard"/>s. If no discard pile is managed by this area,
        /// returns <c>null</c>.
        /// </summary>
        public IDeck? DiscardPile { get; protected set; } = null;

        /// <summary>
        /// Gets or sets <see cref="TableArea"/>-specific trash pile for when cards are intended to never be accessed again, or rarely. 
        /// If no trash pile is managed by this <see cref="TableArea"/> returns <c>null</c>.
        /// </summary>
        public IDeck? TrashPile { get; protected set; } = null;

        /// <summary>
        /// Gets or sets the <see cref="IDeck"/> of the area. If there is more than one <see cref="IDeck"/>, then it gets the first <see cref="IDeck"/>.
        /// If no <see cref="IDeck"/> is managed by this <see cref="TableArea"/> returns <c>null</c>.
        /// </summary>
        public IDeck? Deck
        {
            get
            {
                if (decks.Count > 0)
                {
                    return decks[0];
                }

                return deck;
            }

            protected set
            {
                deck = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of <see cref="IDeck"/>s in the area. If there is only one deck, then it gets that <see cref="IDeck"/>.
        /// If there are no <see cref="IDeck"/>s managed by this area, returns an empty list.
        /// </summary>
        public List<IDeck> Decks
        {
            get
            {
                if (decks.Count == 0 && deck is not null)
                {
                    return new() { deck };
                }

                return decks;
            }

            protected set
            {
                decks = value;
            }
        }

        // TODO: Start extracting methods from Table Zone and putting them here. Then once you get this working Josh, you can go ahead with
        // dominion since the areas are primed.

        /// <summary>
        /// Plays a card to a specific place in an area in the zone. Cannot replace a card if spot is filled.
        /// </summary>
        /// <param name="card">Card to play.</param>
        /// <param name="placementInArea">Where to play in the area. Places the card in the first free space.</param>
        /// <exception cref="InvalidOperationException">Throws if trying to specify where to place a card in an area
        /// without a defined limit, or placing a card on another card.</exception>
        public void PlayCard(ICard card, int placementInArea)
        {
            try
            {
                CardPlacementRulesExecutioner(card, true, placementInArea);
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
        /// <exception cref="InvalidOperationException">Throws if all spots in area are filled with non-<see cref="NullCard"/>s.</exception>
        public void PlayCard(ICard card)
        {
            try
            {
                CardPlacementRulesExecutioner(card, true);
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
        public void PlayCards(List<ICard> cards)
        {
            try
            {
                foreach (ICard card in cards)
                {
                    PlayCard(card);
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
        /// <param name="placementInArea">Where to place in the area. Places the card in the first free space.</param>
        /// <exception cref="InvalidOperationException">Throws if trying to specify where to place a card in an area
        /// without a defined limit, or placing a card on another card.</exception>
        public void PlaceCard(ICard card, int placementInArea)
        {
            try
            {
                CardPlacementRulesExecutioner(card, false, placementInArea);
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
        /// <exception cref="InvalidOperationException">Throws if all spots in area are filled with non-<see cref="NullCard"/>s.</exception>
        public void PlaceCard(ICard card)
        {
            try
            {
                CardPlacementRulesExecutioner(card, false);
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
        public void PlaceMultipleCardsToArea(List<ICard> cards)
        {
            try
            {
                foreach (ICard card in cards)
                {
                    PlaceCard(card);
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
        /// <param name="placementInArea">Place in area to attempt to remove a <see cref="ICard"/> from.</param>
        /// <returns><c>true</c> if a <see cref="ICard"/> was succesffuly removed; otherwise, <c>false</c>. This method
        /// also returns <c>false</c> if no non-<see cref="NullCard"/> <see cref="ICard"/> was found in specified location.</returns>
        /// <exception cref="ArgumentException">Throws if invalid location is given for <paramref name="area"/>
        /// or <paramref name="placementInArea"/>.</exception>
        public bool RemoveCard(int placementInArea)
        {
            try
            {
                ValidatePlaceInAreaArgument(placementInArea);
                if (AreaCardLimit == -1)
                {
                    ICard card = playArea[placementInArea];
                    playArea.RemoveAt(placementInArea);
                    card.OnRemoval();
                    return true;
                }
                else
                {
                    ICard card = playArea[placementInArea];
                    if (card is not NullCard)
                    {
                        playArea[placementInArea] = new NullCard();
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
        /// <returns><c>true</c> if a <see cref="ICard"/> was succesffuly removed; otherwise, <c>false</c>. This method
        /// also returns <c>false</c> if no non-<see cref="NullCard"/> <see cref="ICard"/> was found in the area.</returns>
        public bool RemoveCard(ICard card)
        {
            try
            {
                if (AreaCardLimit == -1)
                {
                    if (playArea.Remove(card))
                    {
                        card.OnRemoval();
                        return true;
                    }
                }
                else
                {
                    for (int i = 0; i < playArea.Count; i++)
                    {
                        if (playArea[i] == card)
                        {
                            playArea[i] = new NullCard();
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
        /// <param name="placementInArea">Place in <paramref name="area"/> to search for <paramref name="card"/>.</param>
        /// <returns><c>true</c> if a <see cref="ICard"/> was succesffuly removed; otherwise, <c>false</c>. This method
        /// also returns <c>false</c> if no non-<see cref="NullCard"/> <see cref="ICard"/> was found in the area.</returns>
        public bool RemoveCard(ICard card, int placementInArea)
        {
            try
            {
                ValidatePlaceInAreaArgument(placementInArea);

                if (playArea[placementInArea] == card)
                {
                    if (AreaCardLimit == -1)
                    {
                        playArea.RemoveAt(placementInArea);
                    }
                    else
                    {
                        playArea[placementInArea] = new NullCard();
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
        /// Flips a <see cref="ICard"/>.
        /// </summary>
        /// <param name="placementInArea">Specific place in area the <see cref="ICard"/> is.</param>
        public virtual void FlipCard(int placementInArea)
        {
            try
            {
                ValidatePlaceInAreaArgument(placementInArea);
                playArea[placementInArea].Flip();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flips a <see cref="ICard"/> to a certain direction if it's not already that orientation.
        /// </summary>
        /// <param name="placementInArea">Specific place in area the <see cref="ICard"/> is.</param>
        /// <param name="facedown">If <c>true</c> flips the <see cref="ICard"/> facedown, otherwise faceup.</param>
        public virtual void FlipCardCertainWay(int placementInArea, bool facedown)
        {
            try
            {
                ValidatePlaceInAreaArgument(placementInArea);
                playArea[placementInArea].Flip(facedown);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Flips all <see cref="ICard"/>s in an area in the <see cref="TableZone"/>.
        /// </summary>
        public virtual void FlipAllCards()
        {
            try
            {
                foreach (ICard card in playArea)
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
        /// <param name="facedown">If <c>true</c> flips the <see cref="ICard"/>s facedown, otherwise faceup.</param>
        public virtual void FlipAllCardsCertainWay(bool facedown)
        {
            try
            {
                foreach (ICard card in playArea)
                {
                    card.Flip(facedown);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Draws <see cref="ICard"/>s from the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="cardCount">Number of <see cref="ICard"/>s to draw.</param>
        /// <returns>A list of <see cref="ICard"/>s drawn from the <see cref="IDeck"/>. If the <see cref="IDeck"/> is empty
        /// <see cref="NullCard"/>s are replaced for any missing draw.</returns>
        public List<ICard?> DrawCardsFromDeck(int cardCount)
        {
            return DrawCardsFromDeck(0, cardCount);
        }

        /// <summary>
        /// Draws <see cref="ICard"/>s from specified <see cref="IDeck"/> managed by this <see cref="TableArea"/>.
        /// </summary>
        /// <param name="deckNum">Which <see cref="IDeck"/> to draw a card from.</param>
        /// <param name="cardCount">Number of <see cref="ICard"/>s to draw.</param>
        /// <returns>A list of <see cref="ICard"/>s drawn from the <see cref="IDeck"/>. If the <see cref="IDeck"/> is empty
        /// <see cref="NullCard"/>s are replaced for any missing draw.</returns>
        public virtual List<ICard?> DrawCardsFromDeck(int deckNum, int cardCount)
        {
            try
            {
                if (!(deckNum < Decks.Count && deckNum >= 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(deckNum), "Argument out of range, no such deck exists");
                }

                return Decks[deckNum].DrawMultipleCards(cardCount);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Shuffles <see cref="IDeck"/> in area.
        /// </summary>
        public void ShuffleDeck()
        {
            try
            {
                ShuffleDeck(0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Shuffles <see cref="IDeck"/> in area.
        /// </summary>
        /// <param name="deckNum">Which <see cref="IDeck"/> to target in <see cref="TableArea"/>.</param>
        public virtual void ShuffleDeck(int deckNum)
        {
            try
            {
                if (deckNum > 0 && deckNum < Decks.Count)
                {
                    Decks[deckNum].Shuffle();
                }

                throw new ArgumentOutOfRangeException(nameof(deckNum), "Argument out of range, no such deck exists");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Helper function that follows rules for placing card to a spot in the zone.
        /// </summary>
        /// <param name="played">If <c>true</c>, triggers <see cref="ICard.OnPlay(CardPlacedOnTableDetails)"/> if placed, else triggers
        /// <see cref="ICard.OnPlace(CardPlacedOnTableDetails)"/> if placed.</param>
        private void CardPlacementRulesExecutioner(ICard card, bool played)
        {
            try
            {
                if (AreaCardLimit == -1)
                {
                    playArea.Add(card);
                    ExecuteCardOnPlayOrOnPlace(card, played, playArea.Count - 1);
                }
                else
                {
                    bool placementFound = false;

                    for (int i = 0; i < playArea.Count; i++)
                    {
                        if (playArea[i] is NullCard)
                        {
                            playArea[i] = card;
                            ExecuteCardOnPlayOrOnPlace(card, played, i);
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

        private void CardPlacementRulesExecutioner(ICard card, bool played, int placementInArea)
        {
            try
            {
                ValidatePlaceInAreaArgument(placementInArea);
                if (playArea[placementInArea] is NullCard)
                {
                    playArea[placementInArea] = card;
                    ExecuteCardOnPlayOrOnPlace(card, played, placementInArea);
                }
                else
                {
                    throw new InvalidOperationException($"A card already exists in Area {ID} at Placement {placementInArea}");
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
        private void ExecuteCardOnPlayOrOnPlace(ICard card, bool played, int placementInArea)
        {
            if (played)
            {
                card.OnPlay(new CardPlacedOnTableDetails(OwningZoneType, ID, placementInArea));
            }
            else
            {
                card.OnPlace(new CardPlacedOnTableDetails(OwningZoneType, ID, placementInArea));
            }
        }

        /// <summary>
        /// Validates <paramref name="placementInArea"/> argument.
        /// </summary>
        /// <param name="placementInArea">Placement in an <see cref="TableArea"/> in the zone.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if any argument is out range
        /// of bounds of the zone or area.</exception>
        private void ValidatePlaceInAreaArgument(int placementInArea)
        {
            try
            {
                if (!(placementInArea < playArea.Count && placementInArea >= 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(placementInArea));
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
