using deckForge.PlayerConstruction;
using CardNamespace;

namespace deckForge.PhaseActions
{
    public class DrawCardsAction : PlayerGameAction
    {
        public int DrawCount { get; }

        public DrawCardsAction(string name = "Draw", int drawCount = 1)
        : base(name: name)
        {
            Name = name;
            DrawCount = drawCount;
            Description = $"Draw {drawCount} Card(s)";
        }

        //Returns the list of cards that was drawn into the player's hand
        override public List<Card?> execute(Player player)
        {
            List<Card?> cards = new();

            for (int i = 0; i < DrawCount; i++)
            {
                cards.Add(player.DrawCard());
            }

            return cards;
        }
    }
}
