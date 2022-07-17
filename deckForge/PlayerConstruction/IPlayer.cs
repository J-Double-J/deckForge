using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.GameElements.Resources;

namespace deckForge.PlayerConstruction
{
    public interface IPlayer {
        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;
        public event EventHandler<SimplePlayerMessageEvent>? PlayerMessageEvent;
        public int HandSize { get; }
        public int PlayerID { get; }
        public List<Card> PlayedCards { get; }
        public void StartTurn();
        public void DrawStartingHand();
        public Card? DrawCard();
        public Card? PlayCard(bool facedown = false);
        public Card FlipSingleCard(int cardNum, bool? facedown = null);
        public List<Card> TakeAllCardsFromTable();
        public void AddPlayerResourceCollection(IResourceCollection collection);
        public int FindCorrectPoolID(object resource);
        public object? TakeResourceFromCollection(int resourceCollectionID);
        public void AddResourceToCollection(int resourceCollectionID, object resource);
        public void RemoveResourceFromCollection(int resourceCollectionID, object resource);
        public void AddMultipleResourcesToCollection(int resourceCollectionID, List<object> resources);
        public void IncrementResourceCollection(int resourceCollectionID);
        public void DecrementResourceCollection(int resourceCollectionID);
        public void ClearResourceCollection(int resourceCollectionID);

    }

    public interface IPlayer_WithPersonalDeck : IPlayer {
        public int PersonalDeckSize { get; }
        public Card AddCardToPersonalDeck(Card c, string position = "bottom", bool shuffledDeckAfter = false);
        public List<Card> AddCardsToPersonalDeck(List<Card> cards, string position = "bottom", bool shuffleDeckAfter = false);
    }
}
