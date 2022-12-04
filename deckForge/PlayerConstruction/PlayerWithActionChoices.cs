using DeckForge.GameConstruction;
using DeckForge.HelperObjects;
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
        /// <param name="initHandSize">Initial size of hand.</param>
        public PlayerWithActionChoices(IGameMediator gm, int playerID, int initHandSize = 5)
            : base(gm, playerID, initHandSize)
        {
            Actions = DefaultActions;
            Prompter = new(Actions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerWithActionChoices"/> class. Specifies where input and output should be done.
        /// </summary>
        /// <param name="reader">Tells <see cref="IPlayer"/> where to read input.</param>
        /// <param name="output">Tells <see cref="IPlayer"/> where to display output.</param>
        /// <param name="gm"><see cref="IGameMediator"/> that the <see cref="PlayerWithActionChoices"/> will use to communicate
        /// with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="PlayerWithActionChoices"/>.</param>
        /// <param name="initHandSize">The size of the initial hand that the <see cref="PlayerWithActionChoices"/> will start with.</param>
        public PlayerWithActionChoices(IInputReader reader, IOutputDisplay output, IGameMediator gm, int playerID, int initHandSize = 5)
            : base(reader, output, gm, playerID, initHandSize)
        {
            Actions = DefaultActions;
            Prompter = new(reader, output, Actions);
        }

        // TODO: Consider refactoring to some Interface for Action tracking so that action rule tracking can be more easily delegated.

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
                PostActionHook(pickedAction);
            }

            EndTurn();
        }

        /// <inheritdoc/>
        public override void EndTurn()
        {
            Actions = DefaultActions;
        }

        /// <summary>
        /// Gain a certain number of uses of a <see cref="PlayerGameAction"/>.
        /// </summary>
        /// <param name="action"><see cref="PlayerGameAction"/> that is gained and can be done.</param>
        /// <param name="count">Number of uses of the <see cref="PlayerGameAction"/> gained.</param>
        public void GainAction(IGameAction<IPlayer> action, int count = 1)
        {
            if (count < 1)
            {
                throw new ArgumentException("Cannot gain less than 1 count of an action", nameof(count));
            }

            if (Actions.ContainsKey(action.Name))
            {
                Actions[action.Name] = (action, Actions[action.Name].ActionCount + count);
            }
            else
            {
                Actions[action.Name] = (action, count);
            }
        }

        /// <summary>
        /// Loses a certain number of uses of a <see cref="PlayerGameAction"/>. Does nothing if Player never had that <see cref="PlayerGameAction"/>
        /// this turn.
        /// </summary>
        /// <param name="action">Uses of <see cref="PlayerGameAction"/> that is reduced.</param>
        /// <param name="count">Number of uses of the <see cref="PlayerGameAction"/> lost.</param>
        public void LoseAction(IGameAction<IPlayer> action, int count = 1)
        {
            if (Actions.ContainsKey(action.Name))
            {
                if (Actions[action.Name].ActionCount > 0)
                {
                    Actions[action.Name] = (action, Actions[action.Name].ActionCount - count);
                }
            }
        }

        /// <summary>
        /// Gain a certain number of uses of a <see cref="PlayerGameAction"/> for future turns.
        /// </summary>
        /// <param name="action"><see cref="PlayerGameAction"/> that is gained and can be done.</param>
        /// <param name="count">Number of uses of the <see cref="PlayerGameAction"/> gained.</param>
        public void GainDefaultAction(IGameAction<IPlayer> action, int count = 1)
        {
            if (count < 1)
            {
                throw new ArgumentException("Cannot gain less than 1 count of an action", nameof(count));
            }

            if (DefaultActions.ContainsKey(action.Name))
            {
                DefaultActions[action.Name] = (action, DefaultActions[action.Name].ActionCount + count);
            }
            else
            {
                DefaultActions[action.Name] = (action, count);
            }
        }

        /// <summary>
        /// Loses a certain number of uses of a <see cref="PlayerGameAction"/> for future turns.
        /// Does nothing if Player never had that <see cref="PlayerGameAction"/> this turn.
        /// </summary>
        /// <param name="action">Uses of <see cref="PlayerGameAction"/> that is reduced.</param>
        /// <param name="count">Number of uses of the <see cref="PlayerGameAction"/> lost.</param>
        public void LoseDefaultAction(IGameAction<IPlayer> action, int count = 1)
        {
            if (DefaultActions.ContainsKey(action.Name))
            {
                DefaultActions[action.Name] = (action, DefaultActions[action.Name].ActionCount - count);
            }
        }

        /// <summary>
        /// Executes after an action has been completed. Override to change behvaior. By default
        /// calls <see cref="ReduceActionCountForTurn(IGameAction{IPlayer})"/>.
        /// </summary>
        /// <param name="pickedAction">Action that was just completed.</param>
        protected virtual void PostActionHook(IGameAction<IPlayer> pickedAction)
        {
            ReduceActionCountForTurn(pickedAction);
        }

        /// <summary>
        /// Updates the Actions Dictionary to track the remaining number of times that action can be done.
        /// </summary>
        /// <param name="action"><see cref="IGameAction{T}"/> chosen to be executed.</param>
        protected void ReduceActionCountForTurn(IGameAction<IPlayer> action)
        {
            Actions[action.Name] = (action, Actions[action.Name].ActionCount - 1);
        }
    }
}
