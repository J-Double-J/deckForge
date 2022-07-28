namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarGameMediator : BaseGameMediator
    {
        public WarGameMediator(int playerCount) : base(playerCount) { }
        override protected void RoundEndedHook()
        {
            GameController!.ShiftTurnOrderClockwise();
            Console.WriteLine("Rounded Ended. Current Player Deck Count:");
            Console.WriteLine($"Player 0:  {GetPlayerByID(0)?.CountOfResourceCollection(0)}");
            Console.WriteLine($"Player 1:  {GetPlayerByID(1)?.CountOfResourceCollection(0)}");
        }
    }
}
