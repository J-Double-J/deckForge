using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace DeckForge.PlayerConstruction
{
    /// <summary>
    /// IPlayer is one of drivers of the game and makes all the choices and key interactions
    /// in the game.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Handles the event that the <see cref="IPlayer"/> played a <see cref="PlayingCard"/>.
        /// </summary>
        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;

        /// <summary>
        /// Handles the event that the <see cref="IPlayer"/> raises a message.
        /// </summary>
        public event EventHandler<SimplePlayerMessageEventArgs>? PlayerMessageEvent;

        /// <summary>
        /// Gets the size of <see cref="IPlayer"/>'s hand.
        /// </summary>
        public int HandSize { get; }

        /// <summary>
        /// Gets the ID of <see cref="IPlayer"/>.
        /// </summary>
        public int PlayerID { get; }

        /// <summary>
        /// Gets the list of <see cref="PlayingCard"/>s that <see cref="IPlayer"/>'s has played.
        /// </summary>
        public List<PlayingCard> PlayedCards { get; }

        /// <summary>
        /// Gets or sets a value indicating
        /// whether the <see cref="IPlayer"/> is active (<c>true</c>) or inactive (<c>false</c>).
        /// </summary>
        /// <remarks>
        /// This is primarily used to mark an <see cref="IPlayer"/> as active in a round or phase.
        /// If the player is out of the game permanently use <c>IsOut</c>.
        /// </remarks>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="IPlayer"/> out of the game (<c>true</c>),
        /// or is in the game (<c>false</c>). Once <see cref="IsOut"/> is
        /// <c>true</c>, <c>IsOut</c> cannot be changed.
        /// </summary>
        /// <remarks>
        /// This is used to remove an <see cref="IPlayer"/> from the game and turn order.
        /// If the player is not to be considered in play temporarily, use <c>IsActive</c>.
        /// <see cref="IPlayer"/>s on default are not removed from turn order until a round is over.
        /// </remarks>
        public bool IsOut { get; set; }

        /// <summary>
        /// Starts <see cref="IPlayer"/>'s turn.
        /// </summary>
        public void StartTurn();

        /// <summary>
        /// <see cref="IPlayer"/> loses the game.
        /// </summary>
        public void LoseGame();

        /// <summary>
        /// Draws <see cref="IPlayer"/>'s starting hand for the game.
        /// </summary>
        public void DrawStartingHand();

        /// <summary>
        /// Draws a card.
        /// </summary>
        /// <returns>The <see cref="PlayingCard"/> that was drawn or null.</returns>
        public PlayingCard? DrawCard();

        /// <summary>
        /// Plays a card.
        /// </summary>
        /// <param name="facedown">Plays card facedown if <c>true</c>, otherwise faceup.</param>
        /// <returns>The <see cref="PlayingCard"/> that was played or null.</returns>
        public PlayingCard? PlayCard(bool facedown = false);

        /// <summary>
        /// Flips a single <see cref="PlayingCard"/> belonging to the <see cref="IPlayer"/> on the <see cref="Table"/>.
        /// </summary>
        /// <param name="cardPos">Position of the card on the <see cref="Table"/></param>
        /// <param name="facedown">Flips card facedown if <c>true</c>, faceup if <c>false</c>,
        /// otherwise flipped regardless of current orientation.</param>
        /// <returns>Reference to <see cref="PlayingCard"/> that was flipped.</returns>
        public PlayingCard FlipSingleCard(int cardPos, bool? facedown = null);

        /// <summary>
        /// Takes all cards that the <see cref="IPlayer"/> owns off the table.
        /// </summary>
        /// <returns>Reference to the List of <see cref="PlayingCard"/>s that were picked up.</returns>
        public List<PlayingCard> TakeAllCardsFromTable();

        /// <summary>
        /// Adds an <see cref="IResourceCollection"/> for <see cref="IPlayer"/> to manage.
        /// </summary>
        /// <param name="collection">The collection that the <see cref="IPlayer"/> will now manage. </param>
        public void AddResourceCollection(IResourceCollection collection);

        /// <summary>
        /// Finds the <see cref="IResourceCollection"/> that manages <paramref name="resourceType"/>
        /// and returns its ID in the List of collections that <see cref="IPlayer"/> manages.
        /// </summary>
        /// <param name="resourceType">Type of the resource managed by a <see cref="IResourceCollection"/>
        /// that <see cref="IPlayer"/> owns.</param>
        /// <returns>An int ID that corresponds to the <see cref="IResourceCollection"/>.</returns>
        public int FindCorrectResourceCollectionID(Type resourceType);

        /// <summary>
        /// Gets the number of resources in an <see cref="IResourceCollection"/>.
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/></param>
        /// <returns>The number of resources in the collection.</returns>
        public int CountOfResourceCollection(int resourceCollectionID);

        /// <summary>
        /// Takes a resource from the <see cref="IResourceCollection"/>.
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/></param>
        /// <returns>
        /// A nullable <see cref="object"/> that is of the type of resource managed by the <see cref="IResourceCollection"/>.
        /// </returns>
        public object? TakeResourceFromCollection(int resourceCollectionID);

        /// <summary>
        /// Adds a resource to the <see cref="IResourceCollection"/>.
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/></param>
        /// <param name="resource">Resource of the same type that the <see cref="IResourceCollection"/> manages.</param>
        public void AddResourceToCollection(int resourceCollectionID, object resource);

        /// <summary>
        /// Removes a specific resource from the <see cref="IResourceCollection"/>. If resources
        /// are not unique, then any resource will be removed.
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/>.</param>
        /// <param name="resource">Resource to remove. Resource must match type managed by <see cref="IResourceCollection"/></param>
        public void RemoveResourceFromCollection(int resourceCollectionID, object resource);

        /// <summary>
        /// Add multiple resources at once to a <see cref="IResourceCollection"/>.
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/>.</param>
        /// <param name="resources">List of resources to add. Resources must match type managed by <see cref="IResourceCollection"/>.</param>
        public void AddMultipleResourcesToCollection(int resourceCollectionID, List<object> resources);

        /// <summary>
        /// Increases the number of available resources in a <see cref="IResourceCollection"/>.
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/>.</param>
        public void IncrementResourceCollection(int resourceCollectionID);

        /// <summary>
        /// Decreases the number of available resources in a <see cref="IResourceCollection"/>.
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/>.</param>
        public void DecrementResourceCollection(int resourceCollectionID);

        /// <summary>
        /// Removes all resources from a <see cref="IResourceCollection"/>.
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/>.</param>
        public void ClearResourceCollection(int resourceCollectionID);

        /// <summary>
        /// <see cref="IPlayer"/> executes an action.
        /// </summary>
        /// <param name="action">An action that interacts with <see cref="IPlayer"/>s.</param>
        /// <returns>A nullable <see cref="object"/> that <paramref name="action"/> interacted with.</returns>
        public object? ExecuteGameAction(IGameAction<IPlayer> action);

        /// <summary>
        /// <see cref="IPlayer"/> executes an action against another <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="action">Action that interacts with <see cref="IPlayer"/>s.</param>
        /// <param name="target"><see cref="IPlayer"/> to be targetted by <paramref name= "action"/></param>
        /// <returns>A nullable <see cref="object"/> that <paramref name="action"/> interacted with.</returns>
        public object? ExecuteGameActionAgainstPlayer(IGameAction<IPlayer> action, IPlayer target);

        /// <summary>
        /// <see cref="IPlayer"/> executes an action against multiple other <see cref="IPlayer"/>s.
        /// </summary>
        /// <param name="action">Action that interacts with <see cref="IPlayer"/>s.</param>
        /// <param name="targets">List of <see cref="IPlayer"/>s to be targetted by <paramref name="action"/>.</param>
        /// <param name="includeSelf">Specifies whether to include <see cref="IPlayer"/> executing action in target list if <c>true</c></param>
        /// <returns>A nullable <see cref="object"/> that <paramref name="action"/> interacted with.</returns>
        public object? ExecuteGameActionAgainstMultiplePlayers(IGameAction<IPlayer> action, List<IPlayer> targets, bool includeSelf = false);
    }
}
