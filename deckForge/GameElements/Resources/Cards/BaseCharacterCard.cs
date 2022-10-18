using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources.Cards.CardEvents;

namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// Default implementation of CharacterCards for games.
    /// </summary>
    public class BaseCharacterCard : Card, ICharacterCard
    {
        private readonly IGameMediator gm;
        private readonly string name;
        private int attackVal;
        private int healthVal;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCharacterCard"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that will be used to communicate with other game elements.</param>
        /// <param name="attack">Attack value of the <see cref="ICharacterCard"/>.</param>
        /// <param name="health">Health value of the <see cref="ICharacterCard"/>.</param>
        /// <param name="name">Name of the <see cref="ICharacterCard"/>.</param>
        public BaseCharacterCard(IGameMediator gm, int attack, int health, string name = "Placeholder")
        {
            this.gm = gm;
            this.name = name;
            attackVal = attack;
            healthVal = health;
        }

        /// <summary>
        /// Gets or sets the <see cref="AttackVal"/> of the <see cref="BaseCharacterCard"/>.
        /// </summary>
        public int AttackVal
        {
            get
            {
                return attackVal;
            }

            set
            {
                if (value > 0)
                {
                    attackVal = value;
                }
                else
                {
                    attackVal = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="HealthVal"/> of the <see cref="BaseCharacterCard"/>.
        /// </summary>
        public int HealthVal
        {
            get
            {
                return healthVal;
            }

            set
            {
                healthVal = value;
                if (healthVal <= 0)
                {
                    Die();
                }
            }
        }

        /// <inheritdoc/>
        public override string PrintCard()
        {
            return $"{name}: {AttackVal}/{HealthVal}";
        }

        // TODO: Raise correct event flags (before attack, after attack, etc)

        /// <inheritdoc/>
        public virtual void Attack(ICharacterCard target)
        {
            var targetCard = target as BaseCharacterCard;
            if (targetCard is not null)
            {
                targetCard.HealthVal -= AttackVal;
                HealthVal -= targetCard.AttackVal;
            }
            else
            {
                throw new ArgumentException($"{this.GetType()} does not handle attacking a non-BaseCharacterCard");
            }
        }

        /// <inheritdoc/>
        public virtual void Die()
        {
            OnCardIsRemovedFromTable(new CardIsRemovedFromTableEventArgs());
        }
    }
}
