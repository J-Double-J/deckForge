using DeckForge.GameElements.Resources;

namespace DeckForge.PlayerConstruction.PlayerEvents
{
    public class PlayerPlayedCardEventArgs : EventArgs
    {
        public PlayerPlayedCardEventArgs(ICard c)
        {
            CardPlayed = c;
        }
        
        public ICard CardPlayed { get; }
    }
}
