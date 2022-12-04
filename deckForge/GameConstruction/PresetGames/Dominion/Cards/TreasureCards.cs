using DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTraits;
using DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTypes;
using DeckForge.GameElements.Resources.Cards;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
#pragma warning disable SA1402 // File may only contain a single type
    /// <summary>
    /// A treasure card that costs nothing to buy. Worth 1 coin.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Numerous small treasure cards.")]
    public class CopperCard : DominionCard, ITreasureCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopperCard"/> class.
        /// </summary>
        public CopperCard()
            : base(new Dictionary<Type, int>() { { typeof(Coin), 0 } }, "Copper", "1")
        {
            TreasureValue = 1;
            Traits.Add(new TreasureTrait(this));
        }

        /// <inheritdoc/>
        public int TreasureValue { get; }
    }

    /// <summary>
    /// A treasure card that costs 3 coins to buy. Worth 2 coins.
    /// </summary>
    public class SilverCard : DominionCard, ITreasureCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SilverCard"/> class.
        /// </summary>
        public SilverCard()
            : base(new Dictionary<Type, int>() { { typeof(Coin), 3 } }, "Silver", "2")
        {
            TreasureValue = 2;
            Traits.Add(new TreasureTrait(this));
        }

        /// <inheritdoc/>
        public int TreasureValue { get; }
    }

    /// <summary>
    /// A treasure card that costs 6 coins to buy. Worth 3 coins.
    /// </summary>
    public class GoldCard : DominionCard, ITreasureCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoldCard"/> class.
        /// </summary>
        public GoldCard()
            : base(new Dictionary<Type, int>() { { typeof(Coin), 6 } }, "Gold", "3")
        {
            TreasureValue = 3;
            Traits.Add(new TreasureTrait(this));
        }

        /// <inheritdoc/>
        public int TreasureValue { get; }
    }
#pragma warning restore SA1402 // File may only contain a single type
}
