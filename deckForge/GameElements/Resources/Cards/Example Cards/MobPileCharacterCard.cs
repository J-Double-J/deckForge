using DeckForge.GameConstruction;
using DeckForge.HelperObjects;

namespace DeckForge.GameElements.Resources.Cards.Example_Cards
{
    /// <summary>
    /// A character card that increases in strength as <see cref="ICard"/>s are played.
    /// </summary>
    public class MobPileCharacterCard : BaseCharacterCard
    {
        private bool firstEventRecieved = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobPileCharacterCard"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to interact with other game elements.</param>
        /// <param name="attack">Attack value of the <see cref="ICharacterCard"/>.</param>
        /// <param name="health">Health value of the <see cref="ICharacterCard"/>.</param>
        public MobPileCharacterCard(IGameMediator gm, int attack, int health)
            : base(gm, attack, health, "Mob Pile")
        {
            gm.GetCardModifierKeyEvent(CardModifiers.CardsPlayed).KeyEvent += IncreaseBy1_1;
        }

        /// <summary>
        /// Increases in strength for every card played after placed on board,
        /// and never loses it even if <see cref="CardModifiers.CardsPlayed"/> decreases. (Which does not make
        /// intuitive sense even).
        /// </summary>
        /// <param name="sender">Sender of event.</param>
        /// <param name="e">Arguments for the DictionaryValueChangedEventArgs.</param>
        private void IncreaseBy1_1(object? sender, DictionaryValueChangedEventArgs<CardModifiers, int> e)
        {
            // Card ignores the first time this event is raised because it's itself being played and
            // is increasing the number.
            if (firstEventRecieved)
            {
                HealthVal += 1;
                AttackVal += 1;
            }
            else
            {
                firstEventRecieved = true;
            }
        }
    }
}
