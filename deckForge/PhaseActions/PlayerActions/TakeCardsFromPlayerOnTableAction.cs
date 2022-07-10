using deckForge.PlayerConstruction;
using CardNamespace;

namespace deckForge.PhaseActions
{
    class TakeAllCards_FromTargetPlayerTable_ToPlayerDeck : PlayerGameAction
    {
        public TakeAllCards_FromTargetPlayerTable_ToPlayerDeck(
            string name = "Take Cards from Player on Table",
            string description = "Take some Cards on the Table from a Player."
            ) : base(name: name, description: description)
        { }

        public override List<Card> execute(IPlayer playerExecutor, IPlayer playerTarget)
        {
            try {

                if (playerExecutor.GetType() == typeof(BasePlayer_WithPersonalDeck)) {
                    IPlayer_WithPersonalDeck personal = (IPlayer_WithPersonalDeck)playerExecutor;
                    return personal.AddCardsToPersonalDeck(playerTarget.TakeAllCardsFromTable());
                } else {
                    string e = "PlayerExecutor does not implement 'IPlayer_WithPersonalDeck'. Action parameters are not checked at compile time. Are you sure the right player is calling this action?";
                    throw new ArgumentException(e);
                }
            } catch {
                throw;
            }
        }
    }
}