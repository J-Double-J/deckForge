using deckForge.PlayerConstruction;
using deckForge.PlayerConstruction.PlayerEvents;
using CardNamespace;
using DeckNameSpace;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarPlayer : BasePlayer_WithPersonalDeck
    {
        public WarPlayer(IGameMediator gm, int playerID, Deck personalDeck) : base(gm, playerID: playerID, initHandSize: 0, personalDeck: personalDeck)
        {
            if (personalDeck == null)
            {
                throw new ArgumentException("Cannot have a non-existant personal deck in War");
            }
        }

        public override Card? PlayCard(bool facedown = false)
        {
            Card? c = _personalDeck!.DrawCard(drawFacedown: true);
            if (c != null)
            {
                _gm.PlayerPlayedCard(PlayerID, c);
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
