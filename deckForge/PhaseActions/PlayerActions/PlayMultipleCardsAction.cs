using deckForge.PlayerConstruction;
using deckForge.GameElements.Resources;

namespace deckForge.PhaseActions
{
    public class PlayMultipleCardsAction : PlayerGameAction
    {
        public PlayMultipleCardsAction(int playCount, string name = "Play", bool facedown = false)
        : base()
        {
            if (playCount < 0) {
                throw new ArgumentException("Play count must be 0 or greater");
            } 
            Name = name;
            PlayCount = playCount;
            Description = $"Play {playCount} Cards";
        }

        public int PlayCount { get; }

        public override List<Card?> execute(IPlayer player)
        {
            List<Card?> drawnCards = new List<Card?>();

            for (var i = 0; i < PlayCount; i++)
                drawnCards.Add(player.PlayCard());
            
            return drawnCards;
        }
    }
}
