using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources.Cards;
using DeckForge.PlayerConstruction;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// A <see cref="ICard"/> that executes an effect whenever it is played.
    /// </summary>
    public abstract class InstantEffectCard : EffectCard
    {
        private IPlayer? ownedBy;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstantEffectCard"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="facedown"><see cref="ICard"/> starts facedown if <c>True</c>.</param>
        /// <param name="name">Sets the name of the <see cref="ICard"/>.</param>
        /// <param name="description">Sets the description of the <see cref="ICard"/>.</param>
        public InstantEffectCard(
            IGameMediator gm,
            bool facedown = true,
            string name = "Spell Card",
            string description = "This is a spell card.")
            : base(gm, facedown)
        {
            Name = name;
            Description = description;
        }

        /// <inheritdoc/>
        public override IPlayer? OwnedBy
        {
            get
            {
                return ownedBy;
            }

            set
            {
                ownedBy = value;

                if (ownedBy is not null)
                {
                    ownedBy.PlayerPlayedCard += OnCardPlayed;
                }
            }
        }

        /// <summary>
        /// Gets the name of the <see cref="ICard"/>.
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Gets the description of the <see cref="ICard"/>.
        /// </summary>
        public string Description { get; }

        /// <inheritdoc/>
        public override string PrintCard()
        {
            return $"{Name}: {Description}";
        }

        private void OnCardPlayed(object? sender, PlayerPlayedCardEventArgs e)
        {
            if (e.CardPlayed == this)
            {
                ExecuteEffect();
            }
        }

    }
}
