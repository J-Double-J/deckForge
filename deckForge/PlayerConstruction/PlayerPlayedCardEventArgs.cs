using DeckForge.GameElements.Resources;

namespace DeckForge.PlayerConstruction.PlayerEvents
{
    public class PlayerPlayedCardEventArgs : EventArgs
    {
        public PlayerPlayedCardEventArgs(Card c)
        {
            CardPlayed = c;
        }
        
        public Card CardPlayed { get; }
    }
}
