using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;

// TODO: Do these functions need to return void or cards?
namespace DeckForge.GameElements
{
    /// <summary>
    /// Responsible for tracking played <see cref="PlayingCard"/>s and where they are played.
    /// <see cref="ITable"/> is also in charge of managing <see cref="ITable"/> resources.
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Gets the decks on the <see cref="ITable"/>.
        /// </summary>
        public List<DeckOfPlayingCards> TableDecks { get; }

        /// <summary>
        /// Gets the lists of played <see cref="PlayingCard"/>s in front of each <see cref="IPlayer"/>.
        /// </summary>
        public List<List<PlayingCard>> PlayerPlayedCards { get; }

        /// <summary>
        /// Gets the lists of <see cref="PlayingCard"/>s in the different zones on the <see cref="Table"/>.
        /// </summary>
        public List<List<PlayingCard>> TableNeutralZones { get; }

        /// <summary>
        /// Prints the current <see cref="ITable"/> state.
        /// </summary>
        public void PrintTableState();

        /// <summary>
        /// Gets the list of <see cref="PlayingCard"/>s in front of a specified <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="PlayerConstruction.IPlayer"/>.</param>
        /// <returns>List of <see cref="PlayingCard"/>s that belong to the <see cref="IPlayer"/>.</returns>
        public List<PlayingCard> GetCardsForSpecificPlayer(int playerID);

        /// <summary>
        /// Gets the list of <see cref="PlayingCard"/>s in a specified neutral zone.
        /// </summary>
        /// <param name="neutalZone">ID of the neutral zone on the <see cref="ITable"/>.</param>
        /// <returns>List of <see cref="PlayingCard"/>s that belong to the neutral zone.</returns>
        public List<PlayingCard> GetCardsForSpecificNeutralZone(int neutalZone);

        /// <summary>
        /// Places the <paramref name="card"/> on the <see cref="ITable"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> who played the <see cref="PlayingCard"/>.</param>
        /// <param name="card"><see cref="PlayingCard"/> that is to be placed on the table.</param>
        public void PlaceCardOnTable(int playerID, PlayingCard card);

        /// <summary>
        /// Flips all the <see cref="PlayingCard"/>s in a specified direction for a specified <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> who owns the <see cref="PlayingCard"/>s.</param>
        /// <param name="facedown">Indicates whether the cards are flipped facedown or faceup.</param>
        public void Flip_AllCardsOneWay_SpecificPlayer(int playerID, bool facedown = false);

        /// <summary>
        /// Flips all cards one way for all <see cref="IPlayer"/>s at the <see cref="ITable"/>.
        /// </summary>
        /// <param name="facedown">Indicates whether the cards are flipped facedown or faceup.</param>
        public void Flip_AllCardsOneWay_AllPLayers(bool facedown = false);

        /// <summary>
        /// Flips all the <see cref="PlayingCard"/>s for a specific <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        public void Flip_AllCardsEitherWay_SpecificPlayer(int playerID);

        /// <summary>
        /// Flips all <see cref="PlayingCard"/>s for all <see cref="IPlayer"/>s.
        /// </summary>
        public void Flip_AllCardsEitherWay_AllPlayers();

        /// <summary>
        /// Flips a specific <see cref="PlayingCard"/> for a specific <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <param name="cardPos">The position or index of the <see cref="PlayingCard"/> on the table.</param>
        /// <returns>A reference to the flipped <see cref="PlayingCard"/>.</returns>
        public PlayingCard Flip_SpecificCard_SpecificPlayer(int playerID, int cardPos);

        /// <summary>
        /// Flips a specific <see cref="PlayingCard"/> for a specific <see cref="IPlayer"/> in a specific direction.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <param name="cardPos">The position or index of the <see cref="PlayingCard"/> on the table.</param>
        /// <param name="facedown">Indicates whether the cards are flipped facedown or faceup.</param>
        /// <returns>A reference to the flipped <see cref="PlayingCard"/>.</returns>
        public PlayingCard Flip_SpecificCard_SpecificPlayer_SpecificWay(int playerID, int cardPos, bool facedown = false);

        /// <summary>
        /// Removes a specific <see cref="PlayingCard"/> on the <see cref="ITable"/> from a <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <param name="cardPos">The position or index of the <see cref="PlayingCard"/> on the table.</param>
        /// <returns>A reference to the flipped <see cref="PlayingCard"/>.</returns>
        public PlayingCard RemoveSpecificCard_FromPlayer(int playerID, int cardPos);

        /// <summary>
        /// Picks up all <see cref="PlayingCard"/>s from the table belonging to a <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <returns>List of <see cref="PlayingCard"/>s picked up from the <see cref="ITable"/>.</returns>
        public List<PlayingCard> PickUpAllCards_FromPlayer(int playerID);

        /// <summary>
        /// Draws a <see cref="PlayingCard"/> from the specified <see cref="DeckOfPlayingCards"/>.
        /// </summary>
        /// <param name="deckNum">ID of the <see cref="DeckOfPlayingCards"/> of interest on the <see cref="ITable"/>.</param>
        /// <returns>A nullable <see cref="PlayingCard"/> that was drawn.</returns>
        public PlayingCard? DrawCardFromDeck(int deckNum);

        /// <summary>
        /// Draws multiple <see cref="PlayingCard"/>s from the specified <see cref="DeckOfPlayingCards"/>.
        /// </summary>
        /// <param name="cardCount">Number of <see cref="PlayingCard"/>s to draw.</param>
        /// <param name="deckNum">ID of the <see cref="DeckOfPlayingCards"/> of interest on the <see cref="ITable"/>.</param>
        /// <returns>A list of nullable <see cref="PlayingCard"/>s that were drawn.</returns>
        public List<PlayingCard?> DrawMultipleCardsFromDeck(int cardCount, int deckNum);

        /// <summary>
        /// Shuffles a <see cref="DeckOfPlayingCards"/> on the <see cref="ITable"/>.
        /// </summary>
        /// <param name="deckPosition">Index or position of the <see cref="DeckOfPlayingCards"/>.</param>
        public void ShuffleDeck(int deckPosition);

        /// <summary>
        /// Plays a number of <see cref="PlayingCard"/>s from a <see cref="DeckOfPlayingCards"/> to an area
        /// designated on the <see cref="Table"/>.
        /// </summary>
        /// <param name="numCards">Number of <see cref="PlayingCard"/>s to attempt to draw.</param>
        /// <param name="deckPos">ID of the <see cref="DeckOfPlayingCards"/> on the table to 
        /// draw from.</param>
        /// <param name="neutralZone">Area to play the <see cref="PlayingCard"/> to.</param>
        /// <param name="isFaceup">Whether to play the <see cref="PlayingCard"/> faceup.</param>
        /// <returns>A list of <see cref="PlayingCard"/>(s) that were placed on the <see cref="Table"/>.</returns>
        public List<PlayingCard> PlayCards_FromTableDeck_ToNeutralZone(
            int numCards,
            int deckPos,
            int neutralZone,
            bool isFaceup = true);

        /// <summary>
        /// Adds a <see cref="PlayingCard"/> to a neutral zone.
        /// </summary>
        /// <param name="card"><see cref="PlayingCard"/> to be added.</param>
        /// <param name="neutralZone">ID of the neutral zone to add the <see cref="PlayingCard"/> to.</param>
        public void AddCardTo_NeutralZone(PlayingCard card, int neutralZone);

        /// <summary>
        /// Adds a list of <see cref="PlayingCard"/>s to a neutral zone.
        /// </summary>
        /// <param name="cards">List of <see cref="PlayingCard"/>s to add.</param>
        /// <param name="neutralZone">ID of the neutral zone to add the <see cref="PlayingCard"/>s to.</param>
        public void AddCardsTo_NeutralZone(List<PlayingCard> cards, int neutralZone);
    }
}
