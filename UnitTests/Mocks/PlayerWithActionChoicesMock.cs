using DeckForge.GameConstruction;
using DeckForge.PhaseActions.PlayerActions;
using DeckForge.PlayerConstruction;
using UnitTests.PlayerConstruction;

namespace UnitTests.Mocks
{
    // TODO: Does not appropriately check if any given ActionChoice would be valid during runtime. This is skipped for now
    // as tests elsewhere appropriately test the prompter for this. This means that a player should never have the ability anyways to pick a non-existant
    // option.

    /// <summary>
    /// Mocks a <see cref="PlayerWithActionChoices"/>. Does not appropriately check if any given ActionChoice would be valid during runtime.
    /// </summary>
    public class PlayerWithActionChoicesMock : PlayerWithActionChoices
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerWithActionChoicesMock"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        public PlayerWithActionChoicesMock(IGameMediator gm, int playerID)
            : base(gm, playerID)
        {
        }

        /// <summary>
        /// Gets or sets the list of actions, using the action names as keys, that the mock will iterate through.
        /// </summary>
        public List<string> ActionChoices { get; set; } = new();

        /// <inheritdoc/>
        public override void StartTurn()
        {
            int actionNum = 0;
            while (true)
            {
                var pickedAction = Actions[ActionChoices[actionNum]].Action;

                if (pickedAction is EndTurnAction)
                {
                    break;
                }

                pickedAction.Execute(this);
                UpdateActionDictionaryForTurn(pickedAction);
                actionNum++;
            }

            EndTurn();
        }

        /// <summary>
        /// Starts turn but if <paramref name="callEndTurnOnEnd"/> is <c>false</c> then does not call EndTurn().
        /// Useful for seeing state before a turn reset.
        /// </summary>
        /// <param name="callEndTurnOnEnd">If <c>false</c> does not call EndTurn() when it normally would, instead it terminates the
        /// action prompts only.</param>
        public void StartTurn(bool callEndTurnOnEnd)
        {
            if (callEndTurnOnEnd)
            {
                StartTurn();
            }
            else
            {
                int actionNum = 0;
                while (true)
                {
                    var pickedAction = Actions[ActionChoices[actionNum]].Action;

                    if (pickedAction is EndTurnAction)
                    {
                        break;
                    }

                    pickedAction.Execute(this);
                    UpdateActionDictionaryForTurn(pickedAction);
                    actionNum++;
                }
            }
        }
    }
}
