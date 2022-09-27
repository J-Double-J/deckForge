using DeckForge.GameElements.Resources;
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

        /// <summary>
        /// Players all make their betting actions.
        /// </summary>
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

        /// <summary>
        /// Evaluates the winner of a round. Cards must be already played on the table.
        /// </summary>
        public void EvaluateWinner()
        {
            Dictionary<int, List<PlayingCard>> hands = new();
            foreach (PokerPlayer player in GetCurrentActivePlayers())
            {
                // We know all cards on table are PlayingCards
                List<PlayingCard> cards =
                Table!.GetCardsForSpecificPlayer(player.PlayerID)
                    .Concat(Table!.GetCardsForSpecificNeutralZone(0)).ToList()
                        .ConvertAll(c => (PlayingCard)c);
                hands.Add(
                    player.PlayerID,
                    cards);
            }

            List<int> winnerIDs = SimplisitcPokerHandEvaluator.EvaluateHands(hands);
            AwardRoundWinners(winnerIDs);
        }

        /// <summary>
        /// Awards the winners by taking each <see cref="PokerPlayer"/>'s invested cash and splitting it among the winner(s).
        /// </summary>
        /// <param name="winnerIDs">A list of the ID of each winning <see cref="PokerPlayer"/>.</param>
        public void AwardRoundWinners(List<int> winnerIDs)
        {
            int pot = 0;

            foreach (PokerPlayer player in Players!)
            {
                pot += player.InvestedCash;
                player.ClearInvestedCash();
            }

            int winnings = Convert.ToInt32(Math.Floor((double)pot / winnerIDs.Count));

            foreach (int id in winnerIDs)
            {
                PokerPlayer player = (GetPlayerByID(id) as PokerPlayer)!;
                player.GainBettingCash(winnings);
            }
        }

        /// <summary>
        /// Knocks out broke players and declares a winner if there is only one left.
        /// </summary>
        public void HandlePotentialBrokePlayers()
        {
            foreach (PokerPlayer player in Players!)
            {
                if (player.BettingCash == 0)
                {
                    player.IsOut = true;
                }
            }

            if (GetCurrentActivePlayers().Count == 1)
            {
                EndGameWithWinner(GetCurrentActivePlayers()[0]);
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
