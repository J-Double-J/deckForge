using DeckForge.PlayerConstruction;
using FluentAssertions;
using UnitTests.Mocks;

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

        [TestMethod]
        public void PrompterAllowsBack_MarkedWithNegativeNumber()
        {
            Dictionary<int, string> prompt = new()
            {
                { 0, "How do we think this test will go?" },
                { 1, "Good" },
                { 2, "Bad" },
                { -1, "Cancelled" }
            };
            PlayerPrompter pp = new(new ConsoleInputMock(new() { "-1" }), new ConsoleOutputMock(), prompt, true);

            pp.Prompt().Should().Be(-1, "the user elected to cancel the prompt");
        }

        [TestMethod]
        public void PrompterDoesNotAllowBadResponses()
        {
            Dictionary<int, string> prompt = new()
            {
                { 0, "How do we think this test will go?" },
                { 1, "Good" },
                { 2, "Bad" },
                { -1, "Cancelled" }
            };
            PlayerPrompter pp = new(new ConsoleInputMock(new() { "-2", "0", "99", "1" }), new ConsoleOutputMock(), prompt, true);

            pp.Prompt().Should().Be(1, "the user tried multiple bad responses before choosing the correct one");
        }

        [TestMethod]
        public void PrompterDisplaysOutputCorrectly()
        {
            ConsoleOutputMock consoleOutput = new();
            Dictionary<int, string> prompt = new()
            {
                { 0, "How do we think this test will go?" },
                { 1, "Good" },
                { 2, "Bad" },
                { -1, "Cancelled" }
            };
            PlayerPrompter pp = new(new ConsoleInputMock(new() { "1" }), consoleOutput, prompt, true);
            List<string> expectedOutput = new()
            {
                "How do we think this test will go?",
                "\t1) Good",
                "\t2) Bad",
                "\t-1) Cancelled"
            };

            pp.Prompt();

            consoleOutput.CompleteOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
