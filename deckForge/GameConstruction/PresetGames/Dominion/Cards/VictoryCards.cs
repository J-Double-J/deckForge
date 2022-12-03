using DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTypes;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
#pragma warning disable SA1402 // File may only contain a single type
    /// <summary>
    /// A victory card that costs 2 coins. Worth 1 point.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Numerous small base victory cards.")]
    public class EstateCard : DominionCard, IVictoryCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EstateCard"/> class.
        /// </summary>
        public EstateCard()
            : base(new Dictionary<Type, int>() { { typeof(Coin), 2 } }, "Estate", "Victory Points: 1")
        {
            VictoryPoints = 1;
        }

        /// <inheritdoc/>
        public int VictoryPoints { get; }
    }

    /// <summary>
    /// A victory card that costs 5 coins. Worth 3 points.
    /// </summary>
    public class DuchyCard : DominionCard, IVictoryCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuchyCard"/> class.
        /// </summary>
        public DuchyCard()
            : base(new Dictionary<Type, int>() { { typeof(Coin), 5 } }, "Duchy", "Victory Points: 3")
        {
            VictoryPoints = 3;
        }

        /// <inheritdoc/>
        public int VictoryPoints { get; }
    }

    /// <summary>
    /// A victory card that costs 8 coins. Worth 6 points.
    /// </summary>
    public class ProvinceCard : DominionCard, IVictoryCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProvinceCard"/> class.
        /// </summary>
        public ProvinceCard()
            : base(new Dictionary<Type, int>() { { typeof(Coin), 8 } }, "Province", "Victory Points: 6")
        {
            VictoryPoints = 6;
        }

        /// <inheritdoc/>
        public int VictoryPoints { get; }
    }

    /// <summary>
    /// A negative victory card that costs 0 coins. Worth -1 points.
    /// </summary>
    public class CurseCard : DominionCard, IVictoryCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurseCard"/> class.
        /// </summary>
        public CurseCard()
            : base(new Dictionary<Type, int>() { { typeof(Coin), 0 } }, "Curse", "Victory Points: -1")
        {
            VictoryPoints = -1;
        }

        /// <inheritdoc/>
        public int VictoryPoints { get; }
    }
#pragma warning restore SA1402 // File may only contain a single type
}
