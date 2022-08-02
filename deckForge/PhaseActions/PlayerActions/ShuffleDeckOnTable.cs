using DeckForge.GameConstruction;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    public class ShuffleDeckOnTable : BaseGameAction
    {
        private IGameMediator gm;
        public ShuffleDeckOnTable(
            IGameMediator gm,
            int deckPosition,
            string name = "Shuffle deck on Table.",
            string description = "Takes a deck on the table and shuffles it.")
        : base(name: name, description: description)
        {
            this.gm = gm;
            DeckPosition = deckPosition;
        }

        public int DeckPosition { get; set; }
        public override object? Execute()
        {
            gm.ShuffleDeckOnTable(DeckPosition);
            return null;
        }
    }
}