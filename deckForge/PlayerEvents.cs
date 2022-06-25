using CardNamespace;

namespace PlayerNamespace {
    public class PlayerPlayedCardEventArgs : EventArgs {
        public PlayerPlayedCardEventArgs(Card c) { CardPlayed = c; }
        public Card CardPlayed { get; }
    }
}


