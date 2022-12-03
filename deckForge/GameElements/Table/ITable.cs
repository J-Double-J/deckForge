using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;

// TODO: Do these functions need to return void or cards?
namespace DeckForge.GameElements.Table
{
    /// <summary>
    /// Responsible for tracking played <see cref="ICard"/>s and where they are played.
    /// <see cref="ITable"/> is also in charge of managing <see cref="ITable"/> resources.
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Gets a readonly list of <see cref="TableZone"/>s in on the <see cref="Table"/>.
        /// </summary>
        public IReadOnlyList<TableZone> TableZones { get; }

        /// <summary>
        /// Gets the <see cref="ICard"/>s in a <see cref="TableZone"/> with the matching <see cref="TablePlacementZoneType"/>.
        /// </summary>
        /// <param name="zoneType">Type of <see cref="TableZone"/> to get the <see cref="ICard"/>s from.</param>
        /// <returns>The list of list of <see cref="ICard"/>s from the matching <see cref="TableZone"/> type.
        /// Indexes of first list are for each area in zone.</returns>
        /// <exception cref="ArgumentException">If <paramref name="zoneType"/>
        /// does not match a <see cref="TableZone"/> on the <see cref="ITable"/>.</exception>
        public IReadOnlyList<IReadOnlyList<ICard>> GetCardsInZone(TablePlacementZoneType zoneType);

        /// <summary>
        /// Prints the current <see cref="ITable"/> state.
        /// </summary>
        public void PrintTableState();

        /// <summary>
        /// Flips a <see cref="ICard"/> in the selected <see cref="TableZone"/>.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> of the <see cref="TableZone"/> managing the <see cref="ICard"/>.</param>
        /// <param name="area">Area in the <see cref="TableZone"/> the <see cref="ICard"/> resides in.</param>
        /// <param name="placementInArea">Specific place in area the <see cref="ICard"/> is.</param>
        public void FlipCardInZone(TablePlacementZoneType zoneType, int area, int placementInArea);

        /// <summary>
        /// Flips a <see cref="ICard"/> to a certain direction if it's not already that orientation.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> of the <see cref="TableZone"/> managing the <see cref="ICard"/>.</param>
        /// <param name="area">Area in the <see cref="TableZone"/> the <see cref="ICard"/> resides in.</param>
        /// <param name="placementInArea">Specific place in area the <see cref="ICard"/> is.</param>
        /// <param name="facedown">If <c>true</c> flips the <see cref="ICard"/> facedown, otherwise faceup.</param>
        public void FlipCardInZoneCertainWay(TablePlacementZoneType zoneType, int area, int placementInArea, bool facedown);

        /// <summary>
        /// Flips all <see cref="ICard"/>s in an area in the <see cref="TableZone"/>.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> of the <see cref="TableZone"/> managing the <see cref="ICard"/>s.</param>
        /// <param name="area">Area in the <see cref="TableZone"/> holding the <see cref="ICard"/>s to flip.</param>
        public void FlipAllCardsInAreaInZone(TablePlacementZoneType zoneType, int area);

        /// <summary>
        /// Flips all <see cref="ICard"/>s in an area a certain direction if it's not already that orientation.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> of the <see cref="TableZone"/> managing the <see cref="ICard"/>s.</param>
        /// <param name="area">Area in the <see cref="TableZone"/> holding the <see cref="ICard"/>s to flip.</param>
        /// <param name="facedown">If <c>true</c> flips the <see cref="ICard"/>s facedown, otherwise faceup.</param>
        public void FlipAllCardsInAreaInZoneCertainWay(TablePlacementZoneType zoneType, int area, bool facedown);

        /// <summary>
        /// Flips all <see cref="ICard"/>s in the <see cref="TableZone"/>.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> of the <see cref="TableZone"/> managing the <see cref="ICard"/>s.</param>
        public void FlipAllCardsInZone(TablePlacementZoneType zoneType);

        /// <summary>
        /// Flips all <see cref="ICard"/>s in the <see cref="TableZone"/> a certain way.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> of the <see cref="TableZone"/> managing the <see cref="ICard"/>s.</param>

        /// <param name="facedown">If <c>true</c> flips the <see cref="ICard"/>s facedown, otherwise faceup.</param>
        public void FlipAllCardsInZoneCertainWay(TablePlacementZoneType zoneType, bool facedown);

        /// <summary>
        /// Removes a <see cref="ICard"/> from the specified area in a <see cref="TableZone"/>.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to remove.</param>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> that the <see cref="ICard"/> resides in.</param>
        /// <param name="area">Area identifier in the <see cref="TableZone"/>.</param>
        public void RemoveCardFromTable(ICard card, TablePlacementZoneType zoneType, int area);

        /// <summary>
        /// Removes a <see cref="ICard"/> from a designated place in the specified area in a <see cref="TableZone"/>.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to remove.</param>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> that the <see cref="ICard"/> resides in.</param>
        /// <param name="area">Area identifier in the <see cref="TableZone"/>.</param>
        /// <param name="placementInArea">Specific spot in area to remove the <see cref="ICard"/> from.</param>
        public void RemoveCardFromTable(ICard card, TablePlacementZoneType zoneType, int area, int placementInArea);

        /// <summary>
        /// Removes a <see cref="ICard"/> from the specified area in a <see cref="TableZone"/>.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> that the <see cref="ICard"/> resides in.</param>
        /// <param name="area">Area identifier in the <see cref="TableZone"/>.</param>
        /// <param name="placementInArea">Specific spot in area to remove the <see cref="ICard"/> from.</param>
        public void RemoveCardFromTable(TablePlacementZoneType zoneType, int area, int placementInArea);

        /// <summary>
        /// Picks up all <see cref="ICard"/>s from the table belonging to a <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> of <see cref="TableZone"/> </param>
        /// <param name="area">Area identifier in the <see cref="TableZone"/>.</param>
        /// <returns>List of <see cref="ICard"/>s picked up from the <see cref="ITable"/>.</returns>
        public List<ICard> RemoveAllCards_FromArea(TablePlacementZoneType zoneType, int area);

        /// <summary>
        /// Draws a <see cref="ICard"/> from a <see cref="TableZone"/>, and optionally, a specific area in the zone.
        /// </summary>
        /// <param name="zoneType">Type of <see cref="TableZone"/> to target on the <see cref="ITable"/>.</param>
        /// <param name="area">Area in the <see cref="TableZone"/> to pick which <see cref="IDeck"/> in the <see cref="TableZone"/>
        /// to draw from. Default picks the first <see cref="IDeck"/>.</param>
        /// <returns>A nullable <see cref="ICard"/> that was drawn.</returns>
        public ICard? DrawCardFromDeck(TablePlacementZoneType zoneType, int area = 0);

        /// <summary>
        /// Draws a <see cref="ICard"/> from a <see cref="TableZone"/>'s <see cref="TableArea"/>.
        /// </summary>
        /// <param name="zoneType">Type of <see cref="TableZone"/> to target on the <see cref="ITable"/>.</param>
        /// <param name="areaID">ID of the <see cref="TableArea"/> belonging to the <see cref="TableZone"/>.</param>
        /// <param name="deckInArea">Specified <see cref="IDeck"/> in the <see cref="TableArea"/>. Defaults to first or only <see cref="IDeck"/>.</param>
        /// <returns>A nullable <see cref="ICard"/> that was drawn.</returns>
        public ICard? DrawCardFromDeckInArea(TablePlacementZoneType zoneType, int areaID = 0, int deckInArea = 0);


        /// <summary>
        /// Draws multiple <see cref="ICard"/>s from a <see cref="TableZone"/>, and optionally, a specific area in the zone.
        /// </summary>
        /// <param name="cardCount">Number of <see cref="ICard"/>s to draw.</param>
        /// <param name="zoneType">Type of <see cref="TableZone"/> to target on the <see cref="ITable"/>.</param>
        /// <param name = "area" > Area in the<see cref="TableZone"/> to pick which<see cref= "IDeck" /> in the<see cref="TableZone"/>
        /// to draw from. Default picks the first <see cref="IDeck"/>.</param>
        /// <returns>A list of nullable <see cref="ICard"/>s that were drawn from the specified <see cref="IDeck"/>.</returns>
        public List<ICard> DrawMultipleCardsFromDeck(int cardCount, TablePlacementZoneType zoneType, int area = 0);

        /// <summary>
        /// Shuffles a <see cref="IDeck"/> on the <see cref="ITable"/>/
        /// </summary>
        /// <param name="tablePlacementZoneType">Type of <see cref="TableZone"/> to that owns the <see cref="IDeck"/>.</param>
        /// <param name="area">Optional parameter for which area in the <see cref="TableZone"/> owns the <see cref="IDeck"/>.</param>
        public void ShuffleDeck(TablePlacementZoneType tablePlacementZoneType, int area = 0);

        /// <summary>
        /// Plays a <see cref="ICard"/> to a <see cref="TableZone"/>.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to play to the <see cref="TableZone"/>.</param>
        /// <param name="placementZone"><see cref="TablePlacementZoneType"/> of <see cref="TableZone"/> to play to.</param>
        /// <param name="area">Which area in the <see cref="TableZone"/> to play the <see cref="ICard"/> to.</param>
        public void PlayCardToZone(ICard card, TablePlacementZoneType placementZone, int area);

        /// <summary>
        /// Plays a <see cref="ICard"/> to a <see cref="TableZone"/>.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to play to the <see cref="TableZone"/>.</param>
        /// <param name="placementZone"><see cref="TablePlacementZoneType"/> of <see cref="TableZone"/> to play to.</param>
        /// <param name="area">Which area in the <see cref="TableZone"/> to play the <see cref="ICard"/> to.</param>
        /// <param name="placementInArea">Specific place in area to play the <see cref="ICard"/>.</param>
        public void PlayCardToZone(ICard card, TablePlacementZoneType placementZone, int area, int placementInArea);

        /// <summary>
        /// Plays a list of <see cref="ICard"/>s to a <see cref="TableZone"/>.
        /// </summary>
        /// <param name="cards"><see cref="ICard"/>s to play to the <see cref="TableZone"/>.</param>
        /// <param name="placementZone"><see cref="TablePlacementZoneType"/> of <see cref="TableZone"/> to play to.</param>
        /// <param name="area">Which area in the <see cref="TableZone"/> to play the <see cref="ICard"/>s to.</param>
        public void PlayCardsToZone(List<ICard> cards, TablePlacementZoneType placementZone, int area);

        /// <summary>
        /// Picks up all cards from every spot on the <see cref="ITable"/>.
        /// </summary>
        /// <returns>A list of all the <see cref="ICard"/>s removed from the <see cref="ITable"/>.</returns>
        public List<ICard> Remove_AllCardsFromTable();

        /// <summary>
        /// Gets a read-only list of <see cref="IDeck"/>s managed by a <see cref="TableZone"/>.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> of interested <see cref="TableZone"/>.</param>
        /// <returns>A read-only list <see cref="IDeck"/>s in the <see cref="TableZone"/>. If there are no
        /// <see cref="IDeck"/>s found in the <see cref="TableZone"/> or the <see cref="TableZone"/> doesn't exist,
        /// it returns an empty list.</returns>
        public IReadOnlyList<IDeck> GetDecksFromZone(TablePlacementZoneType zoneType);

        /// <summary>
        /// Gets a <see cref="IDeck"/> from an area in the <see cref="TableZone"/>.
        /// </summary>
        /// <param name="zoneType"><see cref="TablePlacementZoneType"/> of interested <see cref="TableZone"/>.</param>
        /// <param name="area">The area that owns the <see cref="IDeck"/> in the <see cref="TableZone"/>.</param>
        /// <param name="deckNum">Which <see cref="IDeck"/> to get. If unspecified, grabs the first or only <see cref="IDeck"/>.</param>
        /// <returns>A <see cref="IDeck"/> if found. Otherwise returns null.</returns>
        public IDeck? GetDeckFromAreaInZone(TablePlacementZoneType zoneType, int area, int deckNum = 0);

        /// <summary>
        /// Gets a <see cref="IDeck"/> that is the discard pile from a <see cref="TableArea"/> in a <see cref="TableZone"/>.
        /// </summary>
        /// <param name="zoneType">Type of <see cref="TablePlacementZoneType"/> to search for the <see cref="TableArea"/>.</param>
        /// <param name="areaID">ID of <see cref="TableArea"/> in <see cref="TableZone"/>.</param>
        /// <returns><see cref="IDeck"/> if discard exists, else <c>null</c>.</returns>
        public IDeck? GetDiscardFromAreaInZone(TablePlacementZoneType zoneType, int areaID);
    }
}
