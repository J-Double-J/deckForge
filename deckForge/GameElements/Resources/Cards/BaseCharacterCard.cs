using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources.Cards.CardEvents;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// Default implementation of CharacterCards for games.
    /// </summary>
    public class BaseCharacterCard : Card, ICharacterCard
    {
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
            GM = gm;
            this.name = name;
            attackVal = attack;
            healthVal = health;
            BaseAttack = attack;
            BaseHealth = health;
            PlacementDetails = null;
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

        /// <summary>
        /// Gets the base attack value of the <see cref="ICharacterCard"/>.
        /// </summary>
        public int BaseAttack { get; }

        /// <summary>
        /// Gets the base health value of the <see cref="ICharacterCard"/>.
        /// </summary>
        public int BaseHealth { get; }

        /// <summary>
        /// Gets or sets the card's placement details.
        /// Can only be set by classes that implement <see cref="BaseCharacterCard"/>.
        /// </summary>
        public CardPlacedOnTableDetails? PlacementDetails { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IGameMediator"/> that is used to interact with other game elements.
        /// </summary>
        protected IGameMediator GM { get; }

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
                throw new ArgumentException($"{GetType()} does not handle attacking a non-BaseCharacterCard");
            }
        }

        /// <inheritdoc/>
        public virtual void Die()
        {
            OnCardIsRemovedFromTable(new CardIsRemovedFromTableEventArgs());
        }

        /// <inheritdoc/>
        public override void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
            PlacementDetails = placementDetails;

            if (PlacementDetails.TablePlacementZone == TablePlacementZones.PlayerZone)
            {
                GM.ChangeCardModifierValueBy(HelperObjects.CardModifiers.CharacterCardsInPlayerZones, 1);
            }
        }

        /// <inheritdoc/>
        public override void OnRemoval()
        {
            if (PlacementDetails?.TablePlacementZone == TablePlacementZones.PlayerZone)
            {
                GM.ChangeCardModifierValueBy(HelperObjects.CardModifiers.CharacterCardsInPlayerZones, -1);
            }
        }
    }
}
