using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace DeckForge.GameConstruction
{
    /// <summary>
    /// Mediates gameplay and iteractions between various objects such as <c>Table</c>,
    /// <see cref="IPlayer"/>, <c>IRoundRules</c>, etc.
    /// </summary>
    public interface IGameMediator
    {
        /// <summary>
        /// Gets the the number of <see cref="IPlayer"/>s registered with <see cref="IGameMediator"/>.
        /// </summary>
        public int PlayerCount { get; }

        /// <summary>
        /// Gets the current turn order of <see cref="IPlayer"/>s by their IDs.
        /// </summary>
        public List<int> TurnOrder { get; }

        /// <summary>
        /// Gets the current <see cref="Table"/> state of the game.
        /// </summary>
        public List<List<PlayingCard>> CurrentTableState { get; }

        /// <summary>
        /// Informs the <see cref="IGameMediator"/> to keep track of this <paramref name="IPlayer"/>.
        /// </summary>
        /// <param name="player">Player to Register.</param>
        public void RegisterPlayer(IPlayer player);

        /// <summary>
        /// Informs the <see cref="IGameMediator"/> to keep track of this <see cref="Table"/>.
        /// </summary>
        /// <param name="table">Table to Register.</param>
        public void RegisterTable(Table table);

        /// <summary>
        /// Informs the <see cref="IGameMediator"/> to keep track of this <see cref="ITurnHandler"/>.
        /// </summary>
        /// <param name="turnHandler">TurnHandler to register.</param>
        public void RegisterTurnHandler(ITurnHandler turnHandler);

        /// <summary>
        /// Informs the <see cref="IGameMediator"/> to keep track of this <see cref="IRoundRules"/>.
        /// </summary>
        /// <param name="roundRules">The Rounds Rules to keep track.</param>
        public void RegisterRoundRules(IRoundRules roundRules);

        /// <summary>
        /// Shifts the turn order clockwise.
        /// </summary>
        public void ShiftTurnOrderClockwise();

        /// <summary>
        /// Shifts the turn order counter-clockwise.
        /// </summary>
        public void ShiftTurnOrderCounterClockwise();

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame();

        /// <summary>
        /// Ends the game.
        /// </summary>
        public void EndGame();

        /// <summary>
        /// Optional way to end the game, by declaring a <paramref name="winner"/> for <see cref="IGameMediator"/> to announce.
        /// </summary>
        /// <param name="winner">Winner of the game.</param>
        public void EndGameWithWinner(IPlayer winner);

        /// <summary>
        /// Starts a specific <see cref="IPlayer"/>'s turn based on their <paramref name="playerID"/>.
        /// </summary>
        /// <param name="playerID">Player's ID.</param>
        public void StartPlayerTurn(int playerID);

        /// <summary>
        /// Called at the end of every <see cref="IPlayer"/>'s turn.
        /// </summary>
        public void EndPlayerTurn();

        /// <summary>
        /// Called everytime <see cref="IPlayer"/> notifies <see cref="IGameMediator"/> that it played a <see cref="PlayingCard"/>.
        /// </summary>
        /// <param name="playerID">Player's ID.</param>
        /// <param name="card">Card that is played.</param>
        public void PlayerPlayedCard(int playerID, PlayingCard card);

        /// <summary>
        /// Called whenever an <see cref="IPlayer"/> tries to draw a <see cref="PlayingCard"/>.
        /// </summary>
        /// <returns>A nullable <see cref="PlayingCard"/> that was drawn from a <see cref="DeckOfPlayingCards"/>.</returns>
        public PlayingCard? DrawCardFromDeck();

        /// <summary>
        /// Gets the <see cref="IPlayer"/> by their ID.
        /// </summary>
        /// <param name="playerID">ID of IPlayer to search for.</param>
        /// <returns>
        /// The <see cref="IPlayer"/> with the associated ID else
        /// if <see cref="IPlayer"/> is not found then null.
        /// </returns>
        public IPlayer? GetPlayerByID(int playerID);

        /// <summary>
        /// Gets the List of <see cref="PlayingCard"/>s the <see cref="IPlayer"/> has played.
        /// </summary>
        /// <param name="playerID">The ID of the <see cref="IPlayer"/> of interest.</param>
        /// <returns>The list of cards that was played by the <see cref="IPlayer"/>.</returns>
        public List<PlayingCard> GetPlayedCardsOfPlayer(int playerID);

        /// <summary>
        /// Flips a specified card on the <see cref="Table"/>, for a specific <see cref="IPlayer"/>, a specific way.
        /// </summary>
        /// <param name="playerID">ID of <see cref="IPlayer"/> whose card is getting flipped.</param>
        /// <param name="cardPos">Specific card's position on the table for that player.</param>
        /// <param name="facedown">Flip it facedown if true, faceup if false, or flip it regardless if null.</param>
        /// <returns>A reference to the card that was flipped.</returns>
        public PlayingCard FlipSingleCard(int playerID, int cardPos, bool? facedown);

        /// <summary>
        /// Pick up all <see cref="PlayingCard"/>s belonging to an <see cref="IPlayer"/> on the <see cref="Table"/>.
        /// </summary>
        /// <param name="playerID">ID of IPlayer whose <see cref="PlayingCard"/>s are being picked up.</param>
        /// <returns>A reference to the List of <see cref="PlayingCard"/>s picked up.</returns>
        public List<PlayingCard> PickUpAllCards_FromTable_FromPlayer(int playerID);

        /// <summary>
        /// Tells an <see cref="IPlayer"/> to execute an <see cref="IGameAction{IPlayer}"/>.
        /// </summary>
        /// <param name="playerID">ID of the executing <see cref="IPlayer"/>.</param>
        /// <param name="action"> Action that must be able to target <see cref="IPlayer"/>s.</param>
        /// <returns>A nullable <see cref="object"/> that is a reference to what the <paramref name="action"/> may have interacted with.</returns>
        public object? TellPlayerToDoAction(int playerID, IGameAction<IPlayer> action);

        /// <summary>
        /// Tells an <see cref="IPlayer"/> to execute an <see cref="IGameAction{IPlayer}"/> against another <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> triggering the Action.</param>
        /// <param name="playerTargetID">ID of the target <see cref="IPlayer"/>.</param>
        /// <param name="action"> Action that must be able to target <see cref="IPlayer"/>s.</param>
        /// <returns>A nullable <see cref="object"/> that is a reference to what the <paramref name="action"/> may have interacted with.</returns>
        public object? TellPlayerToDoActionAgainstAnotherPlayer(int playerID, int playerTargetID, IGameAction<IPlayer> action);

        /// <summary>
        /// Tells an <see cref="IPlayer"/> to execute an <see cref="IGameAction{IPlayer}"/> against many other <see cref="IPlayer"/>s.<br />
        /// If <paramref name="includeSelf"/> is true, <see cref="IPlayer"/> with <paramref name="playerID"/> includes themselves when targetting.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> triggering the Action.</param>
        /// <param name="action"> Action that must be able to target <see cref="IPlayer"/>s.</param>
        /// <param name="includeSelf">Specifies whether <see cref="IPlayer"/> with <paramref name="playerID"/> should target themself if true.</param>
        /// <returns>A nullable <see cref="object"/> that is a reference to what the <paramref name="action"/> may have interacted with.</returns>
        public object? TellPlayerToDoActionAgainstMultiplePlayers(int playerID, IGameAction<IPlayer> action, bool includeSelf = false);

        /// <summary>
        /// Tells an <see cref="IPlayer"/> to execute an <see cref="IGameAction{IPlayer}"/> against many other <see cref="IPlayer"/>s.
        /// Targets are specified by their Player IDs.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> triggering the Action.</param>
        /// <param name="targets">IDs of the target <see cref="IPlayer"/>s.</param>
        /// <param name="action"> Action that must be able to target <see cref="IPlayer"/>s.</param>
        /// <returns>A nullable <see cref="object"/> that is a reference to what the <paramref name="action"/> may have interacted with.</returns>
        public object? TellPlayerToDoActionAgainstSpecificMultiplePlayers(int playerID, List<int> targets, IGameAction<IPlayer> action);

        /// <summary>
        /// Called whenever an <see cref="IPlayer"/> raises a simple message.
        /// </summary>
        /// <param name="sender"><see cref="IPlayer"/> sender.</param>
        /// <param name="e">The SimplePlayerMessageEventArgs has only a message is attached to be interpretted.</param>
        public void OnSimplePlayerMessage(object? sender, SimplePlayerMessageEventArgs e);

        /// <summary>
        /// <see cref="IPlayer"/> is removed from the game, and calls any additional logic required by the
        /// game to handle an <see cref="IPlayer"/> losing.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> that lost.</param>
        public void PlayerLost(int playerID);
    }
}
