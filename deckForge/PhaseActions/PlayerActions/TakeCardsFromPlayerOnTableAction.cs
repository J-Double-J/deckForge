using deckForge.PlayerConstruction;
using CardNamespace;

namespace deckForge.PhaseActions {
    class TakeAllCards_FromTargetPlayerTable_ToPlayerTable : PlayerGameAction {
        public TakeAllCards_FromTargetPlayerTable_ToPlayerTable(
            string name = "Take Cards from Player on Table",
            string description = "Take some Cards on the Table from a Player."
            ) : base(name: name, description: description) 
        {}

        public override List<Card> execute(Player playerExecutor, Player playerTarget)
        {
           return playerExecutor.AddCardsToPersonalDeck(playerTarget.TakeAllCardsFromTable());
        }
    }
}