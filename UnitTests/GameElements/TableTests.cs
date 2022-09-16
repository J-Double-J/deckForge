using DeckForge.GameElements;
using DeckForge.GameConstruction;
using FluentAssertions;
using DeckForge.PlayerConstruction;
using DeckForge.GameElements.Resources;

namespace UnitTests.GameElements
{

    [TestClass]
    public class TableTests
    {
        private static readonly List<DeckOfPlayingCards> Decks = new() { new DeckOfPlayingCards(), new DeckOfPlayingCards(), new DeckOfPlayingCards() };
        private static IGameMediator gm = new BaseGameMediator(2);
        private static Table table = new(gm, 2, Decks);
        private static StringWriter output = new();

        [ClassInitialize]
        public static void InitializeTableTestsClass(TestContext ctx)
        {
            gm = new BaseGameMediator(2);
        }

        [TestInitialize]
        public void InitializeTableTests()
        {
            table = new(gm, 2, Decks);
            output = new();
            for (var i = 0; i < 2; i++)
            {
                new BasePlayer(gm, i);
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(7)]
        public void PlaceCardOnTableInFrontOfNonexistantPlayer(int fakePlayerID)
        {
            Action a = () => table.PlaceCardOnTable(fakePlayerID, new PlayingCard(8, "J"));

            a.Should().Throw<ArgumentException>($"there is no player with the id of {fakePlayerID}");
        }

        [TestMethod]
        public void TablePrintsCardsCorrectly()
        {
            Console.SetOut(output);

            table.PlaceCardOnTable(0, new PlayingCard(8, "J", facedown: false));
            table.PlaceCardOnTable(0, new PlayingCard(9, "J", facedown: false));
            table.PlaceCardOnTable(0, new PlayingCard(10, "J", facedown: false));
            table.PlaceCardOnTable(1, new PlayingCard(8, "J", facedown: false));
            table.PlaceCardOnTable(1, new PlayingCard(9, "J", facedown: false));
            table.PlaceCardOnTable(1, new PlayingCard(10, "J", facedown: false));

            table.PrintTableState();
            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("8J\n9J\n10J\n8J\n9J\n10J\n", "these six cards were placed on the table");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("8J\r\n9J\r\n10J\r\n8J\r\n9J\r\n10J\r\n", "these six cards were placed on the table");
            }
        }

        [TestMethod]
        public void TableReturnsPlayersCards()
        {
            string s = string.Empty;
            setUpTableForTests();

            List<PlayingCard> cards = table.GetCardsForSpecificPlayer(0);

            foreach (PlayingCard c in cards)
            {
                s += c.PrintCard();
            }

            s.Should().Be("8J9J", "these are the cards that Player 0 has in front of them");
        }

        [TestMethod]
        public void TableFlipsAllPlayerCards()
        {
            setUpTableForTests();

            table.Flip_AllCardsOneWay_AllPLayers(facedown: true);
            table.PrintTableState();

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("COVERED\nCOVERED\nCOVERED\nCOVERED\n", "all 4 cards should be flipped down");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("COVERED\r\nCOVERED\r\nCOVERED\r\nCOVERED\r\n", "all 4 cards should be flipped down");
            }
        }

        [TestMethod]
        public void TableFlipsAllCardsEitherWay()
        {
            setUpTableForTests();

            table.Flip_AllCardsEitherWay_AllPlayers();
            table.PrintTableState();

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("COVERED\nCOVERED\nCOVERED\n2Q\n", "only the 2 Queen should be faceup as it was facedown originally");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("COVERED\r\nCOVERED\r\nCOVERED\r\n2Q\r\n", "only the 2 Queen should be faceup as it was facedown originally");
            }
        }

        [TestMethod]
        public void TableFlipsASpecificPlayersCards()
        {
            setUpTableForTests();

            table.Flip_AllCardsOneWay_SpecificPlayer(0, facedown: true);
            table.PrintTableState();

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("COVERED\nCOVERED\n1Q\nCOVERED\n", "Player 0 should have their cards hidden now");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("COVERED\r\nCOVERED\r\n1Q\r\nCOVERED\r\n", "Player 0 should have their cards hidden now");
            }
        }

        [TestMethod]
        public void TableFlipsASpecificPlayersCards_EitherWay()
        {
            setUpTableForTests();

            table.Flip_AllCardsEitherWay_SpecificPlayer(1);
            table.PrintTableState();

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("8J\n9J\nCOVERED\n2Q\n", "Player 1's cards were flipped regardless of their original facing");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("8J\r\n9J\r\nCOVERED\r\n2Q\r\n", "Player 1's cards were flipped regardless of their original facing");
            }
        }

        [TestMethod]
        public void TableFlipsSpecificPlayersCard()
        {
            setUpTableForTests();

            table.Flip_SpecificCard_SpecificPlayer(0, 0);
            table.PrintTableState();

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("COVERED\n9J\n1Q\nCOVERED\n", "The 8J card should be flipped down while the other cards are untouched");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("COVERED\r\n9J\r\n1Q\r\nCOVERED\r\n", "The 8J card should be flipped down while the other cards are untouched");
            }
        }

        [TestMethod]
        public void TableShouldThrowError_OnAllFlippinActions_OnBadPlayerID()
        {
            Action a = () => table.Flip_AllCardsEitherWay_SpecificPlayer(3);
            Action b = () => table.Flip_AllCardsOneWay_SpecificPlayer(3);
            Action c = () => table.Flip_SpecificCard_SpecificPlayer(3, 0);
            string because = "there is no player 3 on the table";

            setUpTableForTests();

            a.Should().Throw<ArgumentOutOfRangeException>(because);
            b.Should().Throw<ArgumentOutOfRangeException>(because);
            c.Should().Throw<ArgumentOutOfRangeException>(because);
        }

        [TestMethod]
        public void TableShouldThrowError_OnFlippingNonexistantCard()
        {
            Action a = () => table.Flip_SpecificCard_SpecificPlayer(0, 3);

            setUpTableForTests();

            a.Should().Throw<ArgumentOutOfRangeException>("Player 0 doesn't have a 4th card on the board");
        }

        [TestMethod]
        public void TableRemovesACard()
        {
            setUpTableForTests();

            table.RemoveSpecificCard_FromPlayer(0, 0);
            table.PrintTableState();

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("9J\n1Q\nCOVERED\n", "First player's first card was removed");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("9J\r\n1Q\r\nCOVERED\r\n", "First player's first card was removed");
            }
        }

        [TestMethod]
        public void TableShouldThrowException_IfRemovingCard_FromNonexistantPlayer()
        {
            setUpTableForTests();

            Action a = () => table.RemoveSpecificCard_FromPlayer(0, 5);
            a.Should().Throw<ArgumentOutOfRangeException>("there is no sixth player");
        }

        [TestMethod]
        public void TableShouldThrowException_IfRemovingNonexistantCard_FromPlayer()
        {
            setUpTableForTests();

            Action a = () => table.RemoveSpecificCard_FromPlayer(4, 0);
            a.Should().Throw<ArgumentOutOfRangeException>("Player 0 has no fifth card");
        }

        [TestMethod]
        public void PickUpAllCards_From_APlayerTableSpot()
        {
            List<PlayingCard> cards;

            setUpTableForTests();

            cards = table.PickUpAllCards_FromPlayer(0);
            cards.Count.Should().Be(2, "all of Player 0's cards were picked up");
        }

        [TestMethod]
        public void PickUpAllCards_AndTableNoLongerHasThoseCards()
        {
            setUpTableForTests();

            table.PickUpAllCards_FromPlayer(0);
            table.PrintTableState();

            if (OperatingSystem.IsIOS())
            {
                output.Should().Be("1Q\nCOVERED\n");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("1Q\r\nCOVERED\r\n");
            }
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void TableFlips_ACardForAPlayer_InSpecificWay(bool facedown)
        {
            setUpTableForTests();
            table.Flip_SpecificCard_SpecificPlayer_SpecificWay(0, 0, facedown);
            table.PrintTableState();

            if (facedown)
            {
                if (OperatingSystem.IsIOS())
                {
                    output.ToString().Should().Be("COVERED\n9J\n1Q\nCOVERED\n");
                } else if (OperatingSystem.IsWindows())
                {
                    output.ToString().Should().Be("COVERED\r\n9J\r\n1Q\r\nCOVERED\r\n");
                }
            }
            else {
                if (OperatingSystem.IsIOS())
                {
                    output.ToString().Should().Be("8J\n9J\n1Q\nCOVERED\n");
                }
                else if (OperatingSystem.IsWindows())
                {
                    output.ToString().Should().Be("8J\r\n9J\r\n1Q\r\nCOVERED\r\n");
                }
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(2)]
        public void TableCanDrawCard_FromSpecificDeck(int deckNum)
        {
            PlayingCard? c = table.DrawCardFromDeck(0)!;

            c.Should().NotBeNull("because a new card is being drawn from this deck");
        }

        [TestMethod]
        public void TableCanDrawManyCards_FromSpecificDeck() {
            List<PlayingCard?> cards  = new List<PlayingCard?>();

            cards = table.DrawMultipleCardsFromDeck(5);

            cards.Count.Should().Be(5, "5 cards was drawn from the first deck");

            foreach(PlayingCard? c in cards)
            {
                c.Should().NotBeNull("the deck is new and should not be empty");
            }
        }

        [TestMethod]
        public void TableCannotDraw_FromNonexistantDeck() {
            Action a = () => table.DrawCardFromDeck(10);
            Action b = () => table.DrawMultipleCardsFromDeck(5, 4);

            a.Should().Throw<ArgumentOutOfRangeException>("there is no 11th deck");
            b.Should().Throw<ArgumentOutOfRangeException>("there is no 5th deck");
        }

        private void setUpTableForTests()
        {
            Console.SetOut(output);

            table.PlaceCardOnTable(0, new PlayingCard(8, "J", facedown: false));
            table.PlaceCardOnTable(0, new PlayingCard(9, "J", facedown: false));
            table.PlaceCardOnTable(1, new PlayingCard(1, "Q", facedown: false));
            table.PlaceCardOnTable(1, new PlayingCard(2, "Q", facedown: true));
        }
    }
}
