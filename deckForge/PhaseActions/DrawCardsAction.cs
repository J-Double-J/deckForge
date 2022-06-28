using deckForge.PlayerConstruction;

namespace deckForge.PhaseActions
{
    public class DrawCardsAction : GameAction
    {
        override public string Name { get; }
        override public string Description { get; }

        public int DrawCount { get; }

        public DrawCardsAction(string name = "Draw", int drawCount = 1)
        {
            Name = name;
            DrawCount = drawCount;
            Description = $"Draw {drawCount} Card(s)";
        }

        override public void execute(Player p)
        {
            for (int i = 0; i < DrawCount; i++)
            {
                p.DrawCard();
            }
        }
    }
}
