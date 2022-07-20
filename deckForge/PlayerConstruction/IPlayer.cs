using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.GameElements.Resources;
using deckForge.PhaseActions;

namespace deckForge.PlayerConstruction
{
    public interface IPlayer
    {
        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;
        public event EventHandler<SimplePlayerMessageEventArgs>? PlayerMessageEvent;
        public int HandSize { get; }
        public int PlayerID { get; }
        public List<Card> PlayedCards { get; }
        public void StartTurn();
        public void DrawStartingHand();
        public Card? DrawCard();
        public Card? PlayCard(bool facedown = false);
        public Card FlipSingleCard(int cardNum, bool? facedown = null);
        public List<Card> TakeAllCardsFromTable();
        public void AddResourceCollection(IResourceCollection collection);
        public int FindCorrectPoolID(Type resourceType);
        public int CountOfResourceCollection(int resourceCollectionID);
        public object? TakeResourceFromCollection(int resourceCollectionID);
        public void AddResourceToCollection(int resourceCollectionID, object resource);
        public void RemoveResourceFromCollection(int resourceCollectionID, object resource);
        public void AddMultipleResourcesToCollection(int resourceCollectionID, List<object> resources);
        public void IncrementResourceCollection(int resourceCollectionID);
        public void DecrementResourceCollection(int resourceCollectionID);
        public void ClearResourceCollection(int resourceCollectionID);
        public object? ExecuteGameAction(PlayerGameAction action);
        public object? ExecuteGameActionAgainstPlayer(PlayerGameAction action, IPlayer target);
        public object? ExecuteGameActionAgainstMultiplePlayers(PlayerGameAction action, List<IPlayer> targets, bool includeSelf = false);

    }
}
