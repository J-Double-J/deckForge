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
        /// Gets the decks on the <see cref="ITable"/>.
        /// </summary>
        public List<IDeck> TableDecks { get; }

        /// <summary>
        /// Gets a readonly list of played <see cref="ICard"/>s in front of each <see cref="IPlayer"/>.
        /// </summary>
        public IReadOnlyList<IReadOnlyList<ICard>> PlayerZones { get; }

        /// <summary>
        /// Gets a readonly list of <see cref="ICard"/>s in the different non-player owned zones on the <see cref="Table"/>.
        /// </summary>
        public IReadOnlyList<IReadOnlyList<ICard>> TableNeutralZones { get; }

        /// <summary>
        /// Prints the current <see cref="ITable"/> state.
        /// </summary>
        public void PrintTableState();

        /// <summary>
        /// Gets the list of <see cref="ICard"/>s in front of a specified <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <returns>List of <see cref="ICard"/>s that belong to the <see cref="IPlayer"/>.</returns>
        public List<ICard> GetCardsForSpecificPlayer(int playerID);

        /// <summary>
        /// Gets the list of <see cref="ICard"/>s in a specified neutral zone.
        /// </summary>
        /// <param name="neutalZone">ID of the neutral zone on the <see cref="ITable"/>.</param>
        /// <returns>List of <see cref="ICard"/>s that belong to the neutral zone.</returns>
        public List<ICard> GetCardsForSpecificNeutralZone(int neutalZone);

        /// <summary>
        /// Places the <paramref name="card"/> on the <see cref="ITable"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> who played the <see cref="ICard"/>.</param>
        /// <param name="card"><see cref="ICard"/> that is to be placed on the table.</param>
        // public void PlaceCardOnTable(int playerID, ICard card);

        /// <summary>
        /// Flips all the <see cref="ICard"/>s in a specified direction for a specified <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> who owns the <see cref="ICard"/>s.</param>
        /// <param name="facedown">Indicates whether the cards are flipped facedown or faceup.</param>
        public void Flip_AllCardsOneWay_SpecificPlayer(int playerID, bool facedown = false);

        /// <summary>
        /// Flips all cards one way for all <see cref="IPlayer"/>s at the <see cref="ITable"/>.
        /// </summary>
        /// <param name="facedown">Indicates whether the cards are flipped facedown or faceup.</param>
        public void Flip_AllCardsOneWay_AllPLayers(bool facedown = false);

        /// <summary>
        /// Flips all the <see cref="ICard"/>s for a specific <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        public void Flip_AllCardsEitherWay_SpecificPlayer(int playerID);

        /// <summary>
        /// Flips all <see cref="ICard"/>s for all <see cref="IPlayer"/>s.
        /// </summary>
        public void Flip_AllCardsEitherWay_AllPlayers();

        /// <summary>
        /// Flips a specific <see cref="ICard"/> for a specific <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <param name="cardPos">The position or index of the <see cref="ICard"/> on the table.</param>
        /// <returns>A reference to the flipped <see cref="ICard"/>.</returns>
        public ICard Flip_SpecificCard_SpecificPlayer(int playerID, int cardPos);

        /// <summary>
        /// Flips a specific <see cref="ICard"/> for a specific <see cref="IPlayer"/> in a specific direction.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <param name="cardPos">The position or index of the <see cref="ICard"/> on the table.</param>
        /// <param name="facedown">Indicates whether the cards are flipped facedown or faceup.</param>
        /// <returns>A reference to the flipped <see cref="ICard"/>.</returns>
        public ICard Flip_SpecificCard_SpecificPlayer_SpecificWay(int playerID, int cardPos, bool facedown = false);

        /// <summary>
        /// Removes a specific <see cref="ICard"/> on the <see cref="ITable"/> from a <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <param name="cardPos">The position or index of the <see cref="ICard"/> on the table.</param>
        /// <returns>A reference to the flipped <see cref="ICard"/>.</returns>
        public ICard RemoveSpecificCard_FromPlayer(int playerID, int cardPos);

        /// <summary>
        /// Picks up all <see cref="ICard"/>s from the table belonging to a <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <returns>List of <see cref="ICard"/>s picked up from the <see cref="ITable"/>.</returns>
        public List<ICard> PickUpAllCards_FromPlayer(int playerID);

        /// <summary>
        /// Draws a <see cref="ICard"/> from the specified <see cref="IDeck"/>.
        /// </summary>
        /// <param name="deckNum">ID of the <see cref="IDeck"/> of interest on the <see cref="ITable"/>.</param>
        /// <returns>A nullable <see cref="ICard"/> that was drawn.</returns>
        public ICard? DrawCardFromDeck(int deckNum);

        /// <summary>
        /// Draws multiple <see cref="ICard"/>s from the specified <see cref="IDeck"/>.
        /// </summary>
        /// <param name="cardCount">Number of <see cref="ICard"/>s to draw.</param>
        /// <param name="deckNum">ID of the <see cref="IDeck"/> of interest on the <see cref="ITable"/>.</param>
        /// <returns>A list of nullable <see cref="ICard"/>s that were drawn.</returns>
        public List<ICard?> DrawMultipleCardsFromDeck(int cardCount, int deckNum);

        /// <summary>
        /// Shuffles a <see cref="IDeck"/> on the <see cref="ITable"/>.
        /// </summary>
        /// <param name="deckPosition">Index or position of the <see cref="IDeck"/>.</param>
        public void ShuffleDeck(int deckPosition);

        /// <summary>
        /// Plays a number of <see cref="ICard"/>s from a <see cref="IDeck"/> to an area
        /// designated on the <see cref="Table"/>.
        /// </summary>
        /// <param name="numCards">Number of <see cref="ICard"/>s to attempt to draw.</param>
        /// <param name="deckPos">ID of the <see cref="IDeck"/> on the table to
        /// draw from.</param>
        /// <param name="neutralZone">Area to play the <see cref="ICard"/> to.</param>
        /// <param name="isFaceup">Whether to play the <see cref="ICard"/> faceup.</param>
        /// <returns>A list of <see cref="ICard"/>(s) that were placed on the <see cref="Table"/>.</returns>
        public List<ICard> PlayCards_FromTableDeck_ToNeutralZone(
            int numCards,
            int deckPos,
            int neutralZone,
            bool isFaceup = true);

        /// <summary>
        /// Adds a card to the player zone and registers the table to any events of interest.
        /// </summary>
        /// <param name="playerZone">Player zone to add <see cref="ICard"/>.</param>
        /// <param name="card"><see cref="ICard"/> to add to the table.</param>
        public void PlayCardTo_PlayerZone(int playerZone, ICard card);

        /// <summary>
        /// Adds a list of <see cref="ICard"/>s to the player zone and registers the table to any events of interest.
        /// </summary>
        /// <param name="playerZone">Player zone to add <see cref="ICard"/>.</param>
        /// <param name="cards"><see cref="ICard"/>s to add to the table.</param>
        public void PlayCardsTo_PlayerZone(int playerZone, List<ICard> cards);

        /// <summary>
        /// Adds a <see cref="ICard"/> to a neutral zone.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to be added.</param>
        /// <param name="neutralZone">ID of the neutral zone to add the <see cref="ICard"/> to.</param>
        public void AddCardTo_NeutralZone(ICard card, int neutralZone);

        /// <summary>
        /// Adds a list of <see cref="ICard"/>s to a neutral zone.
        /// </summary>
        /// <param name="cards">List of <see cref="ICard"/>s to add.</param>
        /// <param name="neutralZone">ID of the neutral zone to add the <see cref="ICard"/>s to.</param>
        public void AddCardsTo_NeutralZone(List<ICard> cards, int neutralZone);

        /// <summary>
        /// Removes a <see cref="ICard"/> from the table in a specific player zone
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to remove from the table.</param>
        /// <param name="playerZone">Player zone to remove the <see cref="ICard"/>.</param>
        public void RemoveCard_FromPlayerZone(ICard card, int playerZone);

        /// <summary>
        /// Picks up all cards from every spot on the <see cref="ITable"/>.
        /// </summary>
        /// <returns>A list of all the <see cref="ICard"/>s removed from the <see cref="ITable"/>.</returns>
        public List<ICard> Remove_AllCardsFromTable();
    }
}
