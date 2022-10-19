using DeckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.PlayerConstruction
{
    [TestClass]
    public class PlayerPrompterTests
    {
        [TestMethod]
        public void PlayerCanChooseFromPrompt_WithValidChoice() {
            var stringReader = new StringReader("1");
            Console.SetIn(stringReader);
            Dictionary<int, string> prompt = new()
            {
                { 0, "Which number is your favorite?" },
                { 1, "1" },
                { 2, "2" },
                { 3, "Neither of these." }
            };

            PlayerPrompter pp = new(prompt);

            pp.Prompt().Should().Be(1, "the Player chose option 1.");
        }
    }
}
