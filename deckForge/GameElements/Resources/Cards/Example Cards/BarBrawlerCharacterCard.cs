using DeckForge.GameConstruction;
using DeckForge.HelperObjects;

namespace DeckForge.GameElements.Resources.Cards.Example_Cards
{
    /// <summary>
    /// Bar Brawler is a <see cref="BaseCharacterCard"/> whose attack gets stronger or weaker
    /// depending on the number of total character cards in play in player zones.
    /// </summary>
    public class BarBrawlerCharacterCard : CharacterCardWithEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarBrawlerCharacterCard"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to interact with other game elments.</param>
        public BarBrawlerCharacterCard(IGameMediator gm)
            : base(gm, 2, 2, "Bar Brawler")
        {
            gm.GetCardModifierKeyEvent(CardModifiers.CharacterCardsInPlayerZones).KeyEvent += OnEventTrigger;
        }

        /// <inheritdoc/>
        public override void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
            base.OnPlay(placementDetails);
            AttackVal = BaseAttack + GM.GetCurrentCardModifierValue(CardModifiers.CharacterCardsInPlayerZones) - 1;
        }

        /// <inheritdoc/>
        public override void OnEventTrigger(object? sender, EventArgs e)
        {
            AttackVal = BaseAttack + ((DictionaryValueChangedEventArgs<CardModifiers, int>)e).Value - 1; // Remove 1 for self.
        }
    }
}
