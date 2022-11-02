using DeckForge.GameConstruction;

namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// A character card that may have effect that triggers at some point.
    /// </summary>
    public class CharacterCardWithEffect : BaseCharacterCard, IContinuousCardWithEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterCardWithEffect"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that will be used to communicate with other game elements.</param>
        /// <param name="attack">Attack value of the <see cref="ICharacterCard"/>.</param>
        /// <param name="health">Health value of the <see cref="ICharacterCard"/>.</param>
        /// <param name="name">Name of the <see cref="ICharacterCard"/>.</param>
        public CharacterCardWithEffect(IGameMediator gm, int attack, int health, string name)
            : base(gm, attack, health, name)
        {
        }

        /// <inheritdoc/>
        public virtual void OnEventTrigger(object? sender, EventArgs e)
        {
        }

        /// <inheritdoc/>
        public virtual void OnEndTurn()
        {
        }

        /// <inheritdoc/>
        public override void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
            base.OnPlay(placementDetails);
        }

        /// <inheritdoc/>
        public virtual void OnStartTurn()
        {
        }
    }
}
