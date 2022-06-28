using deckForge.PlayerConstruction;
using deckForge.PlayerConstruction.PlayerEvents;
using CardNamespace;
using DeckNameSpace;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarPlayer : Player
    {
        public WarPlayer(GameMediator gm, int playerID, Deck? personalDeck) : base(gm, playerID: playerID, initHandSize: 0, personalDeck: personalDeck)
        {
            if (personalDeck == null)
            {
                throw new ArgumentException("Cannot have a non-existant personal deck in War");
            }
        }

        public override void PlayCard(bool facedown = false)
        {
            Card? c = _personalDeck!.DrawCard(drawFacedown: true);
            if (c != null)
            {
                _gm.PlayerPlayedCard(PlayerID, c);
                OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(c));
            }
            else
            {
                RaiseSimplePlayerMessageEvent(new SimplePlayerMessageEvent(message: "LOSE_GAME"));
            }
        }
    }
}
