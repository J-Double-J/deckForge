using deckForge.PlayerConstruction;

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

        override public void execute(Player player)
        {
            for (int i = 0; i < DrawCount; i++)
            {
                player.DrawCard();
            }
        }
    }
}
