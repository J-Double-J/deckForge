using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.Poker
{
    /// <summary>
    /// Tells the <see cref="PokerGameMediator"/> to evaluate the hands.
    /// </summary>
    public class KnockoutBrokePlayers : BaseGameAction
    {
        private readonly PokerGameMediator pGM;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnockoutBrokePlayers"/> class.
        /// </summary>
        /// <param name="pGM"><see cref="PokerGameMediator"/> that is in charge of running the game.</param>
        /// <param name="name">Name of the action.</param>
        /// <param name="description">Description of the action.</param>
        public KnockoutBrokePlayers(
            PokerGameMediator pGM,
            string name = "Pre Flop Action",
            string description = "Lets the player choose an action before the flop.")
        : base(name, description)
        {
            this.pGM = pGM;
        }

        /// <inheritdoc/>
        public override object? Execute()
        {
            pGM.HandlePotentialBrokePlayers();
            return null;
        }
    }
}
