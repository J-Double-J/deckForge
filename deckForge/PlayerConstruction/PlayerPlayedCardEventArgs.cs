using DeckForge.GameElements.Resources;

namespace DeckForge.PlayerConstruction.PlayerEvents
{
    public class PlayerPlayedCardEventArgs : EventArgs
    {
        public PlayerPlayedCardEventArgs(PlayingCard c)
        {
            CardPlayed = c;
        }
        
        public PlayingCard CardPlayed { get; }
    }
}
