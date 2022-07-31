using DeckForge.PlayerConstruction;
using DeckForge.PlayerConstruction.PlayerEvents;
using DeckForge.GameElements.Resources;

namespace DeckForge.GameConstruction.PresetGames.War
{
    public class WarPlayer : BasePlayer
    {
        Deck personalDeck;
        public WarPlayer(IGameMediator gm, int playerID, Deck deck) : base(gm, playerID: playerID, initHandSize: 0)
        {
            personalDeck = deck;
            AddResourceCollection(personalDeck);
        }

        public override Card? PlayCard(bool facedown = false)
        {
            Card? c = personalDeck!.DrawCard(drawFacedown: true);
            if (c != null)
            {
                GM.PlayerPlayedCard(PlayerID, c);
                OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(c));
                return c;
            }
            else
            {
                RaiseSimplePlayerMessageEvent(new SimplePlayerMessageEventArgs(message: "LOSE_GAME"));
                return c;
            }
        }
    }
}
