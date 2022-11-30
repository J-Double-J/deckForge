using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.HelperObjects;
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
        public IReadOnlyList<IReadOnlyList<ICard>> CurrentTableState { get; }

        /// <summary>
        /// Gets the ITable registered to the <see cref="IGameMediator"/>.
        /// </summary>
        public ITable? Table { get; }

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
        /// Informs the <see cref="IPlayer"/> with the given <paramref name="playerID"/> to start their turn.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> to inform.</param>
        void InformPlayerToStartTurn(int playerID);

        /// <summary>
        /// Called at the end of every <see cref="IPlayer"/>'s turn.
        /// </summary>
        public void EndPlayerTurn();

        /// <summary>
        /// Called everytime <see cref="IPlayer"/> notifies <see cref="IGameMediator"/> that it played a <see cref="ICard"/>.
        /// </summary>
        /// <param name="playerID">Player's ID.</param>
        /// <param name="card">Card that is played.</param>
        public void PlayerPlayedCard(int playerID, ICard card);

        /// <summary>
        /// Called whenever an <see cref="IPlayer"/> tries to draw a <see cref="ICard"/>.
        /// </summary>
        /// <param name="zoneType">Zone type that the <see cref="IDeck"/> resides in.</param>
        /// <param name="area">Optional specifier for which area the <see cref="IDeck"/> resides in.</param>
        /// <returns>A nullable <see cref="ICard"/> that was drawn from a <see cref="IDeck"/>.</returns>
        public ICard? DrawCardFromDeck(TablePlacementZoneType zoneType, int area = 0);

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
        /// Gets the List of <see cref="ICard"/>s the <see cref="IPlayer"/> has played.
        /// </summary>
        /// <param name="playerID">The ID of the <see cref="IPlayer"/> of interest.</param>
        /// <returns>A readonly list of <see cref="ICard"/>s that was played by the <see cref="IPlayer"/>.</returns>
        public IReadOnlyList<ICard> GetPlayedCardsOfPlayer(int playerID);

        /// <summary>
        /// Flips a specified card on the <see cref="Table"/>, a specific way if specified.
        /// </summary>
        /// <param name="zoneType">Type of <see cref="TableZone"/> to target on the <see cref="Table"/>.</param>
        /// <param name="area">ID of <see cref="IPlayer"/> whose card is getting flipped.</param>
        /// <param name="cardPos">Specific card's position on the table for that player.</param>
        /// <param name="facedown">Flip it facedown if true, faceup if false, or flip it regardless if null.</param>
        public void FlipSingleCard(TablePlacementZoneType zoneType, int area, int cardPos, bool? facedown);

        /// <summary>
        /// Pick up all <see cref="ICard"/>s belonging to an <see cref="IPlayer"/> on the <see cref="Table"/>.
        /// </summary>
        /// <param name="playerID">ID of IPlayer whose <see cref="ICard"/>s are being picked up.</param>
        /// <returns>A reference to the List of <see cref="ICard"/>s picked up.</returns>
        public List<ICard> PickUpAllCards_FromTable_FromPlayer(int playerID);

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

        /// <summary>
        /// Deal a number of <see cref="ICard"/>s from the specified <see cref="IDeck"/> on the <see cref="ITable"/> to each
        /// <see cref="IPlayer"/>.
        /// </summary>
        /// <remarks>
        /// By default, <see cref="IGameMediator"/> will keep dealing <see cref="ICard"/>s equally until the correct
        /// number of <see cref="ICard"/>s are dealt or until the <see cref="IDeck"/> runs out of <see cref="ICard"/>s.
        /// This can mean an unequal number of <see cref="ICard"/>s are dealt to each <see cref="IPlayer"/> if there are not
        /// enough <see cref="ICard"/>s to be divided evenly.
        /// </remarks>
        /// <param name="numberOfCardsToDeal">Number of cards each <see cref="IPlayer"/> will be dealt.</param>
        /// <param name="zoneType">Zone type that the <see cref="IDeck"/> resides in.</param>
        /// <param name="area">Optional specifier for which area the <see cref="IDeck"/> resides in.</param>
        public void DealCardsFromDeckToAllPlayers(int numberOfCardsToDeal, TablePlacementZoneType zoneType, int area = 0);

        /// <summary>
        /// Sets the CardModifiers value.
        /// </summary>
        /// <param name="interestedModifier">The CardModifier value to change.</param>
        /// <param name="value">Value to set the CardModifier value to.</param>
        public void SetCardModifierValueTo(CardModifiers interestedModifier, int value);

        /// <summary>
        /// Change the CardModifiers value by a set amount. If no key is found, then
        /// it creates a new key with a starting default value and then changes the value
        /// by <paramref name="changeBy"/>.
        /// </summary>
        /// <param name="interestedModifier">The CardModifier value to change.</param>
        /// <param name="changeBy">Value to change the CardModifier value by.</param>
        public void ChangeCardModifierValueBy(CardModifiers interestedModifier, int changeBy);

        /// <summary>
        /// Gets or creates the <see cref="IKeyValueNotifier{CardModifiers, int}"/> for the specified key.
        /// </summary>
        /// <returns>The <see cref="IKeyValueNotifier{CardModifiers, int}"/> for the specified key.</returns>
        /// <param name="interestedModifier">The CardModifier for the <see cref="IKeyValueNotifier{CardModifiers, int}"/>.</param>
        public IKeyValueNotifier<CardModifiers, int> GetCardModifierKeyEvent(CardModifiers interestedModifier);

        /// <summary>
        /// Gets the current CardModifier value for the current CardModifier.
        /// </summary>
        /// <param name="interestedModifier">CardModifier value that is of interest.</param>
        /// <returns>Value of the current modifier.</returns>
        public int GetCurrentCardModifierValue(CardModifiers interestedModifier);
    }
}
