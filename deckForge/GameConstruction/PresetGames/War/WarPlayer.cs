using deckForge.PlayerConstruction;
using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarPlayer : BasePlayer
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
            Card? c = PersonalDeck!.DrawCard(drawFacedown: true);
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
