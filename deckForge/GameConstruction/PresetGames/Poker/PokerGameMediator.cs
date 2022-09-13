using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.Poker
{
    public class PokerGameMediator : BaseGameMediator
    {
        private int currentBet;

        public PokerGameMediator(int playerCount)
            : base(playerCount)
        {
            currentBet = 0;
        }

        public int CurrentBet
        {
            get
            {
                return currentBet;
            }

            set
            {
                if (currentBet >= value)
                {
                    throw new InvalidOperationException("Cannot make a bet that is lower than or equal to the the current bet.");
                }
                else
                {
                    currentBet = value;
                }
            }
        }
    }
}
