using DeckForge.GameElements.Resources.Cards;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
    /// <summary>
    /// Cards that share traits common with all other cards in <see cref="Dominion"/>.
    /// </summary>
    public abstract class DominionCard : CardWithCost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionCard"/> class.
        /// </summary>
        /// <param name="cost">Cost of the <see cref="ICard"/>.</param>
        /// <param name="name">Name of the <see cref="ICard"/>.</param>
        /// <param name="description">Description of the <see cref="ICard"/>. Useful for describing the rules of the <see cref="ICard"/>.</param>
        public DominionCard(Dictionary<Type, int> cost, string name = "Placeholder", string description = "")
            : base(cost)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets the name of the <see cref="DominionCard"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the <see cref="DominionCard"/>.
        /// </summary>
        public string Description { get; }

        /// <inheritdoc/>
        public override string PrintCard()
        {
            return Description == string.Empty ? $"{Name}" : $"{Name} - {Description}";
        }
    }
}
