using DeckForge.GameElements.Resources;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
    public class StartingPlayerDeck : BaseDeck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartingPlayerDeck"/> class.
        /// </summary>
        public StartingPlayerDeck()
            : base("top", false)
        {
        }

        /// <inheritdoc/>
        protected override void CreateDeck()
        {
            for (int i = 0; i < 7; i++)
            {
                Deck.Add(new CopperCard());
            }

            for (int i = 0; i < 3; i++)
            {
                Deck.Add(new EstateCard());
            }
        }
    }
}
