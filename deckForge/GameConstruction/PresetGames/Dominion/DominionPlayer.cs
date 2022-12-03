using DeckForge.HelperObjects;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.Dominion
{
    /// <summary>
    /// A <see cref="IPlayer"/> in Dominion.
    /// </summary>
    public class DominionPlayer : PlayerWithActionChoices
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionPlayer"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        public DominionPlayer(IGameMediator gm, int playerID)
            : base(gm, playerID, 5)
        {
            CreateDefaultActions();
            Actions = DefaultActions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DominionPlayer"/> class.
        /// </summary>
        /// <param name="reader">Tells <see cref="IPlayer"/> where to read input.</param>
        /// <param name="output">Tells <see cref="IPlayer"/> where to display output.</param>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        public DominionPlayer(IInputReader reader, IOutputDisplay output, IGameMediator gm, int playerID)
            : base(reader, output, gm, playerID, 5)
        {
        }

        public int Coins { get; private set; }

        public void Buy()
        {
        }

        /// <summary>
        /// Increase the number of coins the <see cref="DominionPlayer"/> owns for the turn by set amount.
        /// </summary>
        /// <param name="amount">Number of coins to increase by.</param>
        public void IncreaseCoins(int amount)
        {
            Coins += amount;
        }

        /// <summary>
        /// Reduce the number of coins the <see cref="DominionPlayer"/> owns for the turn by set amount.
        /// </summary>
        /// <param name="amount">Number of coins to reduce by.</param>
        public void ReduceCoins(int amount)
        {
            Coins -= amount;
        }

        private void CreateDefaultActions()
        {
            List<PlayerGameAction> playerActions = new() { new PlayCardAction(), new EndTurnAction() };

            foreach (var action in playerActions)
            {
                DefaultActions.Add(action.Name, (action, 1));
            }
        }
    }
}
