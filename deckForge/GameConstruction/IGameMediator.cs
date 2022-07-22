using deckForge.PhaseActions;
using deckForge.PlayerConstruction;
using deckForge.GameElements;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction
{
    /// <summary>
    /// Mediates gameplay and iteractions between various objects such as <c>Table</c>,
    /// <see cref="IPlayer"/>, <c>IRoundRules</c>, etc.
    /// </summary>
    public interface IGameMediator
    {
        /// <summary>
        /// Informs the IGameMediator to keep track of this <paramref name="IPlayer"/>.
        /// </summary>
        /// <param name="player">Player to Register</param>
        public void RegisterPlayer(IPlayer player);

        /// <summary>
        /// Informs the IGameMediator to keep track of this <c>Table</c>. 
        /// </summary>
        /// <param name="table">Table to Register</param>
        public void RegisterTable(Table table);

        /// <summary>
        /// Informs the IGameMediator to keep track of this <c>IGameController</c>
        /// </summary>
        /// <param name="gameController">Game Controller</param>
        public void RegisterGameController(IGameController gameController);

        /// <value>
        /// Property <c>PlayerCount</c> is the number of <see cref="IPlayer"/> registered with <see cref="IGameMediator"/>.
        /// </value>
        public int PlayerCount { get; }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame();

        /// <summary>
        /// Ends the game.
        /// </summary>
        public void EndGame();

        /// <summary>
        /// Optional way to end the game, by declaring a winner to <see cref="IGameMediator"/> to announce.
        /// </summary>
        /// <param name="winner">Winner</param>
        public void EndGameWithWinner(IPlayer winner);

        /// <summary>
        /// Starts a specific <see cref="IPlayer"/>'s turn based on their <paramref name="playerID"/>.
        /// </summary>
        /// <param name="playerID">Player's ID</param>
        public void StartPlayerTurn(int playerID);

        /// <summary>
        /// Called at the end of every <see cref="IPlayer"/>'s turn.
        /// </summary>
        public void EndPlayerTurn();

        /// <summary>
        /// Called everytime <see cref="IPlayer"/> notifies <see cref="IGameMediator"/> that it played a card.
        /// </summary>
        /// <param name="playerID">Player's ID</param>
        /// <param name="card">Card that is played</param>
        public void PlayerPlayedCard(int playerID, Card card);

        /// <summary>
        /// Called whenever an <see cref="IPlayer"/> tries to draw a card.
        /// </summary>
        /// <returns>The <see cref="Card"/> that was drawn from a deck or nothing.</returns>
        public Card? DrawCardFromDeck();

        /// <summary>
        /// Gets the <see cref="IPlayer"/> by their ID.
        /// </summary>
        /// <param name="playerID">ID of IPlayer to search for.</param>
        /// <returns>The <see cref="IPlayer"/> with the associated ID.</returns>
        public IPlayer GetPlayerByID(int playerID);

        /// <summary>
        /// Gets the list of cards the <see cref="IPlayer"/> has played.
        /// </summary>
        /// <param name="playerID">The ID of the <see cref="IPlayer"/> of interest</param>
        /// <returns>The list of cards that was played by the <see cref="IPlayer"/>.</returns>
        public List<Card> GetPlayedCardsOfPlayer(int playerID);

        /// <summary>
        /// Flips a specified card on the table, for a specific player, a specific way.
        /// </summary>
        /// <param name="playerID">ID of <see cref="IPlayer"/> whose card is getting flipped</param>
        /// <param name="cardPos">Specific card's position on the table for that player</param>
        /// <param name="facedown">Flip it facedown if true, faceup if false, or flip it regardless if null</param>
        /// <returns>A reference to the card that was flipped.</returns>
        public Card FlipSingleCard(int playerID, int cardPos, bool? facedown);

        /// <summary>
        /// Pick up all cards belonging to an <see cref="IPlayer"/> on the table.
        /// </summary>
        /// <param name="playerID">ID of IPlayer whose cards are being picked up</param>
        /// <returns>A reference to the list of cards picked up.</returns>
        public List<Card> PickUpAllCards_FromTable_FromPlayer(int playerID);

        /// <summary>
        /// Tells an <see cref="IPlayer"/> to execute an <see cref="IAction{IPlayer}"/>.
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="action"> Action that must be able to target <see cref="IPlayer"/>s</param>
        /// <returns>A nullable <see langword="object"/> that is a reference to what the action may have interacted with.</returns>
        public object? TellPlayerToDoAction(int playerID, IAction<IPlayer> action);

        /// <summary>
        /// Tells an <see cref="IPlayer"/> to execute an <see cref="IAction{IPlayer}"/> against another <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> triggering the Action</param>
        /// <param name="playerTargetID">ID of the target <see cref="IPlayer"/></param>
        /// <param name="action"> Action that must be able to target <see cref="IPlayer"/>s</param>
        /// <returns>A nullable <see langword="object"/> that is a reference to what the action may have interacted with.</returns>
        public object? TellPlayerToDoActionAgainstAnotherPlayer(int playerID, int playerTargetID, IAction<IPlayer> action);

        /// <summary>
        /// Tells an <see cref="IPlayer"/> to execute an <see cref="IAction{IPlayer}"/> against many other <see cref="IPlayer"/>s.<br />
        /// If <paramref name="includeSelf"/> is true, <see cref="IPlayer"/> with <paramref name="playerID"/> includes themselves when targetting.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> triggering the Action</param>
        /// <param name="action"> Action that must be able to target <see cref="IPlayer"/>s</param>
        /// <param name="includeSelf">Specifies whether <see cref="IPlayer"/> with <paramref name="playerID"/> should target themself if true.</param>
        /// <returns>A nullable <see langword="object"/> that is a reference to what the action may have interacted with.</returns>
        public object? TellPlayerToDoActionAgainstMultiplePlayers(int playerID, IAction<IPlayer> action, bool includeSelf = false);

        /// <summary>
        /// Tells an <see cref="IPlayer"/> to execute an <see cref="IAction{IPlayer}"/> against many other <see cref="IPlayer"/>s.
        /// Targets are specified by their Player IDs.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> triggering the Action</param>
        /// <param name="targets">IDs of the target <see cref="IPlayer"/>s</param>
        /// <param name="action"> Action that must be able to target <see cref="IPlayer"/>s</param>
        /// <returns>A nullable <see langword="object"/> that is a reference of what the action may have interacted with.</returns>
        public object? TellPlayerToDoActionAgainstSpecificMultiplePlayers(int playerID, List<int> targets, IAction<IPlayer> action);
    }
}
