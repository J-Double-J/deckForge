using DeckForge.GameElements.Resources;
namespace DeckForge.GameElements
{
    public interface ITable {
        public List<Deck> TableDecks { get; }
        public List<List<Card>> PlayedCards { get; }
        public void PrintTableState();
        public List<Card> GetCardsForSpecificPlayer(int playerID);
        public void PlaceCardOnTable(int playerID, Card c);
        public void Flip_AllCardsOneWay_SpecificPlayer(int playerID, bool facedown = false);
        public void Flip_AllCardsOneWay_AllPLayers(bool facedown = false);
        public void Flip_AllCardsEitherWay_SpecificPlayer(int playerID);
        public void Flip_AllCardsEitherWay_AllPlayers();
        public Card Flip_SpecificCard_SpecificPlayer(int playerID, int cardPos);
        public Card Flip_SpecificCard_SpecificPlayer_SpecificWay(int playerID, int cardPos, bool facedown = false);
        public Card RemoveSpecificCard_FromPlayer(int playerID, int cardPos);
        public List<Card> PickUpAllCards_FromPlayer(int playerID);
    }
}
