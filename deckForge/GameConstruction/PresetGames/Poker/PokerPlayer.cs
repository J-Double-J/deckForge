using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.Poker
{
    /// <summary>
    /// Player that plays Texas Hold'em Poker.
    /// </summary>
    public class PokerPlayer : BasePlayer
    {
        private int bettingCash;

        /// <summary>
        /// Initializes a new instance of the <see cref="PokerPlayer"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that the <see cref="BasePlayer"/> will use to communicate
        /// with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="BasePlayer"/>.</param>
        /// <param name="bettingCash">Amount of cash that the <see cref="PokerPlayer"/> will
        /// start with at the table.</param>
        public PokerPlayer(IGameMediator gm, int playerID, int bettingCash)
            : base(gm, playerID)
        {
            this.bettingCash = bettingCash;
        }

        /// <summary>
        /// <see cref="PokerPlayer"/> gets a choice of calling, raising, folding and checking.
        /// </summary>
        /// <returns>A string representing their choice of action.</returns>
        public string GetPreFlopBettingAction()
        {
            string? response;
            int responseVal;

            do
            {
                Console.WriteLine("Would you like to:");
                Console.WriteLine("1) Call");
                Console.WriteLine("2) Raise");
                Console.WriteLine("3) Fold");

                // TODO: check if can check.
                Console.WriteLine("4) Check");

                response = Console.ReadLine();
            }
            while (ResponseIsNotValid(response));

            responseVal = int.Parse(response!);
            return responseVal switch
            {
                1 => "CALL",
                2 => "RAISE",
                3 => "FOLD",
                4 => "CHECK",
                _ => throw new Exception("'responseVal' was not between 1-4 in 'GetPreFlopBettingAction()'"),
            };
        }

        /// <summary>
        /// Checks if the <see cref="PokerPlayer"/> response to asking for their action is valid.
        /// </summary>
        /// <param name="response">Response from the user when asked what game action they will take.</param>
        /// <returns><see langword="true"/> if the <paramref name="response"/> is not valid,
        /// otherwise <see langword="false"/>.</returns>
        private static bool ResponseIsNotValid(string? response)
        {
            if (int.TryParse(response, out int numericResponse))
            {
                // TODO: Check if can check.
                if (numericResponse > 0 && numericResponse <= 4)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
