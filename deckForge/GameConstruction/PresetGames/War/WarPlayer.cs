using deckForge.PlayerConstruction;
using CardNamespace;
using DeckNameSpace;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarPlayer : Player 
    {
        public WarPlayer(GameMediator gm, Deck? personalDeck) :base(gm, initHandSize: 0, personalDeck: personalDeck) {
            if (personalDeck == null) {
                throw new ArgumentException("Cannot have a non-existant personal deck in War");
            }
        }

        public override void PlayCard(bool facedown = false)
        {
            Card? c = _personalDeck!.DrawCard(drawFacedown: true);
            if (c != null) {

                //Inform other player
            }
            else {
                //GAME ENDS TRIGGER
            }

        }
    }
}
