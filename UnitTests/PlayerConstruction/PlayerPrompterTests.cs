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
            string prompt = "What number is your favorite?\n" +
                "1) 1!\n" +
                "2) 2!\n" +
                "3) Neither of these.";
            PlayerPrompter pp = new(prompt, 3);

            pp.Prompt().Should().Be(1, "the Player chose option 1.");
        }
    }
}
