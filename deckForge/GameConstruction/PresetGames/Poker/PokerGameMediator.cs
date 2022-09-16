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
            int currentActivePlayers = GetCurrentActivePlayers().Count;
            while (playersResponded != currentActivePlayers)
            {
                List<PokerPlayer> activePlayers = GetCurrentActivePlayers();
                foreach (PokerPlayer player in activePlayers)
                {
                    if (player.IsAllIn is true)
                    {
                        playersResponded++;
                    }
                    else
                    {
                        string response = player.GetPreFlopBettingAction();

                        if (response == "RAISE")
                        {
                            playersResponded = 1;
                        }
                        else if (response == "FOLD")
                        {
                            currentActivePlayers--;
                            if (currentActivePlayers == 1)
                            {
                                // TODO: End Round
                            }
                            else if (playersResponded == currentActivePlayers)
                            {
                                break;
                            }
                        }
                        else
                        {
                            playersResponded++;
                            if (playersResponded == currentActivePlayers)
                            {
                                break;
                            }
                        }
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
                        if (player is not PokerPlayer)
                        {
                            throw new InvalidCastException($"Player of type {player.GetType()} is not a PokerPlayer obj");
                        }

                        activePlayers.Add((player as PokerPlayer)!);
                    }
                }
            }

            return activePlayers;
        }
    }
}
