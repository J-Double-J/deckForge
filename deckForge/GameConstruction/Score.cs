namespace deckForge.GameConstruction {
    public class Score
    {
        int rows;
        int[,] scores;

        public Score(int playerCount)
        {
            scores = new int[playerCount, 1];
            rows = playerCount;
            for (var i = 0; i < rows; i++)
            {
                scores.SetValue(0, i, 0);
            }
        }
        public int GetPlayerScore(int playerNum)
        {
            //TODO: Error Handling
            return scores[playerNum, 0];
        }
        public void IncreasePlayerScore(int playerNum, int score)
        {
            scores[playerNum, 0] += score;
        }
    }
}