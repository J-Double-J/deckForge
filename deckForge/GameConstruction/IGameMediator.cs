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
        public void EndGameWithWinner(IPlayer winner);
        public void StartPlayerTurn(int playerID);
        public void EndPlayerTurn();
        public void PlayerPlayedCard(int playerID, Card c);
        public Card? DrawCardFromDeck();
        public IPlayer GetPlayerByID(int playerID);
        public List<Card> GetPlayedCardsOfPlayer(int playerID);
        public Card FlipSingleCard(int playerID, int cardPos, bool? facedown);
        public List<Card> PickUpAllCards_FromTable_FromPlayer(int playerID);
        public object? TellPlayerToDoAction(int playerID, IAction<IPlayer> action);
        public object? TellPlayerToDoActionAgainstAnotherPlayer(int playerID, int playerTargetID, IAction<IPlayer> action);
        public object? TellPlayerToDoActionAgainstMultiplePlayers(int playerID, IAction<IPlayer> action, bool includeSelf = false);
        public object? TellPlayerToDoActionAgainstSpecificMultiplePlayers(int playerID, List<int> targets, IAction<IPlayer> action);
    }
}
