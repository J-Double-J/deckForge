using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.Poker
{
    /// <summary>
    /// Player that plays Texas Hold'em Poker.
    /// </summary>
    public class PokerPlayer : BasePlayer
    {
        private PokerGameMediator pokerGM; // Same GM that BasePlayer has but avoids repeated casts throughout PokerPlayer.

        /// <summary>
        /// Initializes a new instance of the <see cref="PokerPlayer"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that the <see cref="BasePlayer"/> will use to communicate
        /// with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="BasePlayer"/>.</param>
        /// <param name="bettingCash">Amount of cash that the <see cref="PokerPlayer"/> will
        /// start with at the table.</param>
        public PokerPlayer(PokerGameMediator gm, int playerID, int bettingCash)
            : base(gm, playerID)
        {
            BettingCash = bettingCash;
            pokerGM = gm;
        }

        /// <summary>
        /// Gets or sets the amount of cash invested from the <see cref="PokerPlayer"/> that is
        /// sitting on the table for this round.
        /// </summary>
        public int InvestedCash { get; protected set; } = 0;

        /// <summary>
        /// Gets or sets the amount of betting cash that the <see cref="PokerPlayer"/> has left.
        /// </summary>
        public int BettingCash { get; protected set; }

        /// <summary>
        /// <see cref="PokerPlayer"/> gets a choice of calling, raising, folding and checking.
        /// </summary>
        /// <returns>A string representing their choice of action.</returns>
        public virtual string GetPreFlopBettingAction()
        {
            string preFlopPromptString = "Would you like to:\n\t1) Call\n\t2) Raise\n\t3) Fold\n\t4) Check";
            PlayerPrompter preFlopPrompt = new(preFlopPromptString, 4);
            int responseVal = preFlopPrompt.Prompt();

            switch (responseVal)
            {
                case 1:
                    Call();
                    return "CALL";
                case 2:
                    Raise();
                    return "RAISE";
                case 3:
                    Fold();
                    return "FOLD";
                default:
                    throw new Exception("'responseVal' was not between 1-4 in 'GetPreFlopBettingAction()'");
            }
        }

        /// <summary>
        /// <see cref="PokerPlayer"/> matches the current bet at the table.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws exception if <see cref="PokerPlayer"/> does not
        /// have enough betting cash to match the current bet.</exception>
        public void Call()
        {
            if (BettingCash >= pokerGM.CurrentBet)
            {
                if (InvestedCash != pokerGM.CurrentBet)
                {
                    BettingCash -= pokerGM.CurrentBet - InvestedCash;
                    InvestedCash += pokerGM.CurrentBet - InvestedCash;
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot 'Call' this bet with insufficient betting cash.");
            }
        }

        /// <summary>
        /// <see cref="PokerPlayer"/> raises the current bet to one of their choosing.
        /// </summary>
        public void Raise()
        {
            string? response;
            int raiseAmount;

            while (true)
            {
                Console.WriteLine("What would you like to raise to?");
                response = Console.ReadLine();

                if (int.TryParse(response, out raiseAmount))
                {
                    if (ValidateRaiseAmount(raiseAmount))
                    {
                        break;
                    }
                }
            }

            Raise(raiseAmount);
        }


        /// <summary>
        /// <see cref="PokerPlayer"/> removes themselves from the current round. If they have no cash as a result of this
        /// they are removed from the game.
        /// </summary>
        public void Fold()
        {
            IsActive = false;

            if (BettingCash == 0)
            {
                IsOut = true;
            }
        }

        /// <summary>
        /// <see cref="PokerPlayer"/> raises the current bet to <paramref name="raiseAmount"/>.
        /// Different from <see cref="Raise()"/> as it does not ask for user input from the console.
        /// </summary>
        /// <param name="raiseAmount">The amount to raise the current bet to.</param>
        protected void Raise(int raiseAmount)
        {
            if (ValidateRaiseAmount(raiseAmount))
            {
                BettingCash -= raiseAmount - InvestedCash;
                InvestedCash += raiseAmount - InvestedCash;
                pokerGM.CurrentBet = raiseAmount;
            }
            else
            {
                throw new ArgumentException($"raiseAmount is not a valid raise with a value of {raiseAmount}");
            }
        }

        private bool ValidateRaiseAmount(int raiseAmount)
        {
            if (raiseAmount <= BettingCash && raiseAmount > pokerGM.CurrentBet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
