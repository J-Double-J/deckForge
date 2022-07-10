using deckForge.PlayerConstruction.PlayerEvents;
using CardNamespace;

namespace deckForge.PlayerConstruction {
    public interface IPlayer {
        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;
        public event EventHandler<SimplePlayerMessageEvent>? PlayerMessageEvent;
        public int HandSize { get; }
        public int PlayerID { get; }
        public List<Card> PlayedCards { get; }
        public void StartTurn();
        public Card? DrawCard();
        public Card? PlayCard(bool facedown = false);
        public Card FlipSingleCard(int cardNum, bool? facedown = null);
        public List<Card> TakeAllCardsFromTable();
    }

    public interface IPlayer_WithPersonalDeck : IPlayer {
        public int PersonalDeckSize { get; }
        public Card AddCardToPersonalDeck(Card c, string position = "bottom", bool shuffledDeckAfter = false);
        public List<Card> AddCardsToPersonalDeck(List<Card> cards, string position = "bottom", bool shuffleDeckAfter = false);
    }
}
