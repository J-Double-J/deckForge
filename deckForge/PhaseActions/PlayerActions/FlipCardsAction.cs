using DeckForge.PlayerConstruction;
using DeckForge.GameElements.Resources;

namespace DeckForge.PhaseActions
{
    class FlipOneCard_OneWay_Action : PlayerGameAction
    {
        private bool? _facedown;
        private int _specificCardTablePos;
        public FlipOneCard_OneWay_Action(
            int specificCardTablePos,
            bool? facedown = null,
            string name = "Flip Action",
            string description = "Flips Cards"
            )
            : base(name: name, description: description)
        {
            _specificCardTablePos = specificCardTablePos;
            _facedown = facedown;
        }

        //Returns what card was targetted for the flip
        public override Card execute(IPlayer p)
        {
            return p.FlipSingleCard(_specificCardTablePos, _facedown);
        }
    }
}