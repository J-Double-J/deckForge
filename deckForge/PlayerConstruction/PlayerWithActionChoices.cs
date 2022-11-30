using DeckForge.GameConstruction;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;

namespace DeckForge.PlayerConstruction
{
    /// <summary>
    /// A <see cref="IPlayer"/> that can execute actions from a list of choices.
    /// </summary>
    public abstract class PlayerWithActionChoices : BasePlayer, IPlayerWithActionChoices, IPlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerWithActionChoices"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        public PlayerWithActionChoices(IGameMediator gm, int playerID)
            : base(gm, playerID)
        {
            Actions = DefaultActions;
            Prompter = new(Actions);
        }

        /// <inheritdoc/>
        public Dictionary<string, (IGameAction<IPlayer> Action, int ActionCount)> Actions { get; set; }

        /// <summary>
        /// Gets or sets default actions a <see cref="PlayerWithActionChoices"/> will start off with.
        /// </summary>
        public Dictionary<string, (IGameAction<IPlayer> Action, int ActionCount)> DefaultActions { get; protected set; } = new();

        /// <summary>
        /// Gets the prompter that handles displaying and getting the action the <see cref="IPlayer"/> wants to execute
        /// on thier turn.
        /// </summary>
        protected PlayerActionChoicePrompter Prompter { get; }

        /// <inheritdoc/>
        public override void StartTurn()
        {
            while (true)
            {
                var pickedAction = Prompter.Prompt();

                if (pickedAction is EndTurnAction)
                {
                    break;
                }

                pickedAction.Execute(this);
                UpdateActionDictionaryForTurn(pickedAction);
            }

            EndTurn();
        }

        /// <inheritdoc/>
        public override void EndTurn()
        {
            Actions = DefaultActions;
        }

        /// <summary>
        /// Updates the Actions Dictionary to track the remaining number of times that action can be done.
        /// </summary>
        /// <param name="action"><see cref="IGameAction{T}"/> chosen to be executed.</param>
        protected void UpdateActionDictionaryForTurn(IGameAction<IPlayer> action)
        {
            Actions[action.Name] = (action, Actions[action.Name].ActionCount - 1);
        }
    }
}
