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

        public void PlayersBet()
        {
            int playersResponded = 0;
            while (playersResponded != GetCurrentActivePlayers().Count)
            {
                List<PokerPlayer> activePlayers = GetCurrentActivePlayers();
                foreach (PokerPlayer player in activePlayers)
                {
                    string response = player.GetPreFlopBettingAction();

                    if (response == "RAISE")
                    {
                        playersResponded = 1;
                    }
                    else
                    {
                        playersResponded++;
                    }
                }
            }
        }

        private List<PokerPlayer> GetCurrentActivePlayers()
        {
            List<PokerPlayer> activePlayers = new();
            if (Players is not null)
            {
                foreach (IPlayer player in Players)
                {
                    if (player.IsActive is true && player.IsOut is false)
                    {
                        activePlayers.Add((player as PokerPlayer)!);
                    }
                }
            }

            return activePlayers;
        }
    }
}
