namespace DeckForge.PlayerConstruction
{
    public class PlayerPrompter
    {
        private Dictionary<int, string> prompt;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPrompter"/> class.
        /// </summary>
        /// <param name="prompt">The prompt the player will be shown when they need to make a choice. First option
        /// starts at 1. For any headers in the prompt set it to key 0.</param>
        /// <param name="optionCount">The number of valid options for the <see cref="IPlayer"/> to choose from.</param>
        public PlayerPrompter(Dictionary<int, string> prompt)
        {
            this.prompt = prompt;
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
                foreach (var entry in prompt)
                {
                    if (entry.Key != 0)
                    {
                        Console.WriteLine($"\t{entry.Key}) {entry.Value}");
                    }
                    else
                    {
                        Console.WriteLine(entry.Value);
                    }
                }

                response = Console.ReadLine();
            }
            while (!IsValidResponse(response));

            return int.Parse(response!);
        }

        private bool IsValidResponse(string? response)
        {
            if (int.TryParse(response, out int numericResponse))
            {
                if (numericResponse > 0 && prompt.ContainsKey(numericResponse))
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
