using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards.CardTraits;
using DeckForge.PhaseActions;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTraits
{
    /// <summary>
    /// Cards that use a <see cref="PlayCardAction"/> from the <see cref="DominionPlayer"/> to play.
    /// </summary>
    public class ActionTrait : BaseCardTrait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTrait"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="attachedToCard">Card this <see cref="ActionTrait"/> is attached to.</param>
        public ActionTrait(IGameMediator gm, ICard attachedToCard)
            : base(gm, attachedToCard)
        {
        }

        /// <inheritdoc/>
        public override void OnPlay()
        {
            ((DominionPlayer)AttachedToCard.OwnedBy!).LoseAction(new PlayCardAction(), 1);
        }
    }
}
