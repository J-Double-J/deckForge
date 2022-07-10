using deckForge.PhaseActions;
using deckForge.PlayerConstruction;
using deckForge.GameElements;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction
{
    public interface IGameMediator
    {
        public void RegisterPlayer(IPlayer p);
        public void RegisterTable(Table t);
        public void RegisterGameController(IGameController game);
        public int PlayerCount { get; }
        public void StartGame();
        public void EndGame();
        public void StartPlayerTurn(int playerID);
        public void EndPlayerTurn();
        public void PlayerPlayedCard(int playerID, Card c);
        public Card? DrawCardFromDeck();
        public IPlayer GetPlayerByID(int playerID);
        public List<Card> GetPlayedCardsOfPlayer(int playerID);
        public Card FlipSingleCard(int playerID, int cardPos, bool? facedown);
        public List<Card> PickUpAllCards_FromTable_FromPlayer(int playerID);
        //public void TellPlayerToExecuteCommand(int playerID, PlayerGameAction command);
    }
}
