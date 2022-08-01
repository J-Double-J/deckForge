namespace DeckForge.GameConstruction.PresetGames.War
{
    /// <summary>
    /// The game of <see cref="War"/>'s <see cref="IGameMediator"/>.
    /// </summary>
    public class WarGameMediator : BaseGameMediator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WarGameMediator"/> class.
        /// </summary>
        /// <param name="playerCount">Number of <see cref="DeckForge.PlayerConstruction.IPlayer"/>s in the game.</param>
        public WarGameMediator(int playerCount)
            : base(playerCount)
        {
        }

        /// <inheritdoc/>
        protected override void RoundEndedHook()
        {
            TurnHandler!.ShiftTurnOrderClockwise();
            Console.WriteLine("Rounded Ended. Current Player Deck Count:");
            Console.WriteLine($"Player 0:  {GetPlayerByID(0)?.CountOfResourceCollection(0)}");
            Console.WriteLine($"Player 1:  {GetPlayerByID(1)?.CountOfResourceCollection(0)}");
        }
    }
}
