namespace DeckForge.GameConstruction
{
    public class TurnHandler : ITurnHandler
    {
        private readonly Random rng = new();
        private List<int> turnOrder = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TurnHandler"/> class.
        /// </summary>
        /// <param name="playerCount">Number of <see cref="PlayerConstruction.IPlayer"/>s in the game.</param>
        /// <param name="turnRandomizer">Randomizes the turn order if <c>true</c>, otherwise
        /// <see cref="PlayerConstruction.IPlayer"/> with ID of 0 goes first.</param>
        public TurnHandler(int playerCount, bool turnRandomizer = false)
        {
            for (var i = 0; i < playerCount; i++)
            {
                turnOrder.Add(i);
            }

            if (turnRandomizer)
            {
                var n = playerCount;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    (turnOrder[n], turnOrder[k]) = (turnOrder[k], turnOrder[n]);
                }
            }
        }

        /// <inheritdoc/>
        public List<int> TurnOrder
        {
            get { return turnOrder; }
            private set { turnOrder = value; }
        }

        /// <inheritdoc/>
        public void ShiftTurnOrderClockwise()
        {
            turnOrder = turnOrder.Skip(1).Concat(turnOrder.Take(1)).ToList();
        }

        /// <inheritdoc/>
        public void ShiftTurnOrderCounterClockwise()
        {
            turnOrder = turnOrder.Skip(turnOrder.Count - 1).Concat(turnOrder.Take(turnOrder.Count - 1)).ToList();
        }

        /// <inheritdoc/>
        public void UpdatePlayerList(List<int> newPlayerList)
        {
            turnOrder = turnOrder.Intersect(newPlayerList).ToList();
        }
    }
}
