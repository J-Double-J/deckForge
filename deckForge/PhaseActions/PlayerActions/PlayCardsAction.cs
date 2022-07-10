using deckForge.PlayerConstruction;
using CardNamespace;

namespace deckForge.PhaseActions
{
    public class PlayCardsAction : PlayerGameAction
    {
        public PlayCardsAction(string name = "Play", int playCount = 1, bool facedown = false)
        : base()
        {
            Name = name;
            PlayCount = playCount;
            Description = $"Play {playCount} Cards";
        }

        public int PlayCount { get; }

        public override Card? execute(IPlayer player)
        {
            return player.PlayCard();
        }


    }
}
