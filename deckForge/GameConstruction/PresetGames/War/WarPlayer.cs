using deckForge.PlayerConstruction;
using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction.PresetGames.War
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
                RaiseSimplePlayerMessageEvent(new SimplePlayerMessageEvent(message: "LOSE_GAME"));
                return c;
            }
        }
    }
}
