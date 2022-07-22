using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.GameElements.Resources;
using deckForge.PhaseActions;

namespace deckForge.PlayerConstruction
{
    /// <summary>
    /// IPlayer is the driver of the game and makes all the choices and key interactions
    /// in the game.
    /// </summary>
    public interface IPlayer
    {
        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;
        public event EventHandler<SimplePlayerMessageEventArgs>? PlayerMessageEvent;

        /// <value>
        /// Gets the size of <see cref="IPlayer"/>'s hand.
        /// </value>
        public int HandSize { get; }

        /// <value>
        /// Gets the ID of <see cref="IPlayer"/>.
        /// </value>
        public int PlayerID { get; }

        /// <value>
        /// Gets the list of <see cref="Card"/>s that <see cref="IPlayer"/>'s has played.
        /// </value>
        public List<Card> PlayedCards { get; }

        /// <summary>
        /// Starts <see cref="IPlayer"/>'s turn.
        /// </summary>
        public void StartTurn();

        /// <summary>
        /// Draws <see cref="IPlayer"/>'s starting hand for the game.
        /// </summary>
        public void DrawStartingHand();

        /// <summary>
        /// Draws a card.
        /// </summary>
        /// <returns>The <see cref="Card"/> that was drawn or null.</returns>
        public Card? DrawCard();

        /// <summary>
        /// Plays a card.
        /// </summary>
        /// <param name="facedown">Plays card facedown if <c>true</c>, otherwise faceup.</param>
        /// <returns>The <see cref="Card"/> that was played or null.</returns>
        public Card? PlayCard(bool facedown = false);

        /// <summary>
        /// Flips a single card belonging to the <see cref="IPlayer"/> on the <see cref="Table"/>.
        /// </summary>
        /// <param name="cardPos">Position of the card on the <see cref="Table"/></param>
        /// <param name="facedown">Flips card facedown if <c>true</c>, faceup if <c>false</c>, 
        /// otherwise flipped regardless of current orientation.</param>
        /// <returns>Reference to <see cref="Card"/> that was flipped</returns>
        public Card FlipSingleCard(int cardPos, bool? facedown = null);

        /// <summary>
        /// Takes all cards that the <see cref="IPlayer"/> owns off the table.
        /// </summary>
        /// <returns>Reference to the List of <see cref="Card"/>s that were picked up.</returns>
        public List<Card> TakeAllCardsFromTable();


        /// <summary>
        /// Adds an <see cref="IResourceCollection"/> for <see cref="IPlayer"/> to manage.
        /// </summary>
        /// <param name="collection">The collection that the <see cref="IPlayer"/> will now manage. </param>
        public void AddResourceCollection(IResourceCollection collection);

        /// <summary>
        /// Finds the <see cref="IResourceCollection"/> that manages <paramref name="resourceType"/>
        /// and returns its ID in the List of collections that <see cref="IPlayer"/> manages.
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns>An int ID that corresponds to the <see cref="IResourceCollection"/>.</returns>
        public int FindCorrectPoolID(Type resourceType);

        /// <summary>
        /// Gets the number of resources in a <see cref="IResourceCollection"/>
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/></param>
        /// <returns>The number of resources in the collection.</returns>
        public int CountOfResourceCollection(int resourceCollectionID);

        /// <summary>
        /// Takes a resource from the <see cref="IResourceCollection"/>.
        /// </summary>
        /// <param name="resourceCollectionID">ID of the <see cref="IResourceCollection"/></param>
        /// <returns>
        /// A nullable <see langword="object"/> that is of the type of resource managed by the <see cref="IResourceCollection"/>.
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
        /// <returns>A nullable <see langword="object"/> that <paramref name="action"/> interacted with.</returns>
        public object? ExecuteGameAction(IAction<IPlayer> action);

        /// <summary>
        /// <see cref="IPlayer"/> executes an action against another <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="action">Action that interacts with <see cref="IPlayer"/>s.</param>
        /// <param name="target"><see cref="IPlayer"/> to be targetted by <paramref name= "action"/></param>
        /// <returns>A nullable <see langword="object"/> that <paramref name="action"/> interacted with.</returns>
        public object? ExecuteGameActionAgainstPlayer(IAction<IPlayer> action, IPlayer target);

        /// <summary>
        /// <see cref="IPlayer"/> executes an action against multiple other <see cref="IPlayer"/>s.
        /// </summary>
        /// <param name="action">Action that interacts with <see cref="IPlayer"/>s.</param>
        /// <param name="targets">List of <see cref="IPlayer"/>s to be targetted by <paramref name="action"/>.</param>
        /// <param name="includeSelf">Specifies whether to include <see cref="IPlayer"/> executing action in target list if <c>true</c></param>
        /// <returns>A nullable <see langword="object"/> that <paramref name="action"/> interacted with.</returns>
        public object? ExecuteGameActionAgainstMultiplePlayers(IAction<IPlayer> action, List<IPlayer> targets, bool includeSelf = false);

    }
}
