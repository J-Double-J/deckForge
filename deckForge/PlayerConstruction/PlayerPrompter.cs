namespace DeckForge.PlayerConstruction
{
    public class PlayerPrompter
    {
        private string prompt;
        private int optionCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPrompter"/> class.
        /// </summary>
        /// <param name="prompt">The prompt the player will be shown when they need to make a choice. First option
        /// starts at 1.</param>
        /// <param name="optionCount">The number of valid options for the <see cref="IPlayer"/> to choose from.</param>
        public PlayerPrompter(string prompt, int optionCount)
        {
            this.prompt = prompt;
            this.optionCount = optionCount;
        }

        /// <summary>
        /// Presents the Player with the given prompt.
        /// </summary>
        /// <returns>An integer that is the response that the <see cref="IPlayer"/> chose to execute.</returns>
        public int Prompt()
        {
            string? response;
            do
            {
                Console.WriteLine(prompt);
                response = Console.ReadLine();
            }
            while (!IsValidResponse(response));

            return int.Parse(response!);
        }

        private bool IsValidResponse(string? response)
        {
            if (int.TryParse(response, out int numericResponse))
            {
                if (numericResponse > 0 && numericResponse <= optionCount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
