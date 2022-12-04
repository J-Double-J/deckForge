using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Actions
{
    /// <summary>
    /// Action that tells the <see cref="DominionPlayer"/> to buy a card.
    /// </summary>
    public class BuyAction : PlayerGameAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyAction"/> class.
        /// </summary>
        public BuyAction()
            : base("Buy", "Buy a card from the Marketplace")
        {
        }

        /// <inheritdoc/>
        public override ICard? Execute(IPlayer player)
        {
            DominionPlayer domPlayer = (DominionPlayer)player;
            return domPlayer.Buy();
        }
    }
}
