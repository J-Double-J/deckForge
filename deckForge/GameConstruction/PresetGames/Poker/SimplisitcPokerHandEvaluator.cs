using DeckForge.GameElements.Resources;

namespace DeckForge.GameConstruction.PresetGames.Poker
{
    /// <summary>
    /// A simplistic hand evaluator that does not follow Poker rules but serves as an example/proof of concept
    /// </summary>
    public class SimplisitcPokerHandEvaluator
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplisitcPokerHandEvaluator"/> class.
        /// </summary>
        public SimplisitcPokerHandEvaluator()
        {
        }

        /// <summary>
        /// Evaluates a dictionary of poker hands and determines a winner based on the summation of their cards in their hand. This is not how Poker
        /// works, this is a simplistic lazy example.
        /// </summary>
        /// <param name="hands">A dictionary with a key value pair of a Player's ID and their hand.</param>
        /// <returns>A list of Player IDs that are the winners.</returns>
        public static List<int> EvaluateHands(Dictionary<int, List<PlayingCard>> hands)
        {
            int highestHandVal = 0;
            int currentHandVal = 0;
            List<int> highestHandKeys = new();

            // TODO: There are great Poker Hand Evaluators out there, but I decided that was outside the scope so I made an impractical simple evaluator
            // that does not remotely follow normal rules. At some point this may be fixed
            foreach (var hand in hands)
            {
                currentHandVal = 0;
                for (var i = 0; i < hand.Value.Count; i++)
                {
                    currentHandVal += hand.Value[i].Val;
                }

                if (currentHandVal > highestHandVal)
                {
                    highestHandKeys.Clear();
                    highestHandKeys.Add(hand.Key);
                    highestHandVal = currentHandVal;
                }
                else if (currentHandVal == highestHandVal)
                {
                    highestHandKeys.Add(hand.Key);
                }
            }

            return highestHandKeys;
        }
    }
}
