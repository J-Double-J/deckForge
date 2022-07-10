using CardNamespace;

namespace deckForge.PlayerConstruction.PlayerEvents
{
    public class PlayerPlayedCardEventArgs : EventArgs
    {
        public PlayerPlayedCardEventArgs(Card c) { CardPlayed = c; }
        public Card CardPlayed { get; }
    }
}
