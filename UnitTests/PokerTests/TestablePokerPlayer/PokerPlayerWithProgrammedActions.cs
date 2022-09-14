using DeckForge.GameConstruction.PresetGames.Poker;

namespace UnitTests.PokerTests.TestablePokerPlayer
{
    /// <summary>
    /// <see cref="PokerPlayerWithProgrammedActions"/> is a Mock of <see cref="PokerPlayer"/> that has
    /// actions pre-programmed in a set order for use in testing.
    /// </summary>
    public class PokerPlayerWithProgrammedActions : PokerPlayer
    {
        private int currentCommand = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PokerPlayerWithProgrammedActions"/> class.
        /// </summary>
        /// <param name="pGM">PokerGameMediator that the <see cref="PokerPlayer"/> uses.</param>
        /// <param name="playerID">ID of the Player.</param>
        /// <param name="bettingCash">Amount of cash that the <see cref="PokerPlayer"/> starts with.</param>
        public PokerPlayerWithProgrammedActions(PokerGameMediator pGM, int playerID, int bettingCash = 100)
            : base(pGM, playerID, bettingCash)
        {
            Commands = new();
        }

        /// <summary>
        /// Gets or sets commands for the PokerPlayer to execute in order.
        /// Accepted commands are CALL, RAISE, FOLD, and CHECK.
        /// </summary>
        public List<string> Commands { get; set; }

        /// <summary>
        /// Gets or sets the amount that the PokerPlayer tries to raise the amount to. Default 20.
        /// </summary>
        public int RaiseToAmount { get; set; } = 20;

        /// <summary>
        /// Overwrites <see cref="PokerPlayer.GetPreFlopBettingAction()"/> and uses <see cref="Commands"/>
        /// instead of command line input for what choosing what actions to take.
        /// </summary>
        /// <returns>Name of the action executed.</returns>
        /// <exception cref="NotImplementedException">At time of imp, Check() has not been made.</exception>
        /// <exception cref="ArgumentException">Throws if command not recognized.</exception>
        public override string GetPreFlopBettingAction()
        {
            if (Commands[currentCommand] == "CALL")
            {
                Call();
            }
            else if (Commands[currentCommand] == "RAISE")
            {
                Raise(RaiseToAmount);
            }
            else if (Commands[currentCommand] == "FOLD")
            {
                Fold();
            }
            else if (Commands[currentCommand] == "CHECK")
            {
                // TODO: IMPLEMENT
                throw new NotImplementedException("PokerPlayerWithProgrammedActions does not have Check() implemented");
            }
            else
            {
                throw new ArgumentException($"Unrecognized command type {Commands[currentCommand]} for PokerPlayerWithProgrammedActions");
            }

            string retVal = Commands[currentCommand];
            currentCommand++;
            return retVal;
        }
    }
}
