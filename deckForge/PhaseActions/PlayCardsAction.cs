using PlayerNamespace;

namespace deckForge.PhaseActions
{
    public class PlayCardsAction : GameAction {


        public PlayCardsAction(string name = "Play", int playCount = 1) {
            Name = name;
            PlayCount = playCount;
            Description = $"Play {playCount} Cards";    
        }

        public override string Name { get; }

        public override string Description { get; }

        public int PlayCount { get; }

        public override void execute(Player p) {
            p.PlayCard();
        }
    }
}
