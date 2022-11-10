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
        private static readonly List<IDeck> Decks = new() { new DeckOfPlayingCards(), new DeckOfPlayingCards(), new DeckOfPlayingCards() };
        private static IGameMediator gm = new BaseGameMediator(2);
        private static Table table = new(gm, 2, Decks);
        private static StringWriter output = new();

        /// <summary>
        /// Not called for all tests but for a good number of them.
        /// </summary>
        private void SetUpTableForTests()
        {
            Console.SetOut(output);

            table.PlayCardTo_PlayerZone(0, new PlayingCard(8, "J", facedown: false));
            table.PlayCardTo_PlayerZone(0, new PlayingCard(9, "J", facedown: false));
            table.PlayCardTo_PlayerZone(1, new PlayingCard(1, "Q", facedown: false));
            table.PlayCardTo_PlayerZone(1, new PlayingCard(2, "Q", facedown: true));
        }

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
            Action a = () => table.PlayCardTo_PlayerZone(fakePlayerID, new PlayingCard(8, "J"));

            a.Should().Throw<ArgumentException>($"there is no player with the id of {fakePlayerID}");
        }

        [TestMethod]
        public void TablePrintsCardsCorrectly()
        {
            Console.SetOut(output);

            table.PlayCardTo_PlayerZone(0, new PlayingCard(8, "J", facedown: false));
            table.PlayCardTo_PlayerZone(0, new PlayingCard(9, "J", facedown: false));
            table.PlayCardTo_PlayerZone(0, new PlayingCard(10, "J", facedown: false));
            table.PlayCardTo_PlayerZone(1, new PlayingCard(8, "J", facedown: false));
            table.PlayCardTo_PlayerZone(1, new PlayingCard(9, "J", facedown: false));
            table.PlayCardTo_PlayerZone(1, new PlayingCard(10, "J", facedown: false));

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
            SetUpTableForTests();

            List<PlayingCard> cards = table.GetCardsForSpecificPlayer(0).ConvertAll(c => (PlayingCard)c);

            foreach (PlayingCard c in cards)
            {
                s += c.PrintCard();
            }

            s.Should().Be("8J9J", "these are the cards that Player 0 has in front of them");
        }

        [TestMethod]
        public void TableFlipsAllPlayerCards()
        {
            SetUpTableForTests();

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
            SetUpTableForTests();

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
            SetUpTableForTests();

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
            SetUpTableForTests();

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
            SetUpTableForTests();

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

            SetUpTableForTests();

            a.Should().Throw<ArgumentOutOfRangeException>(because);
            b.Should().Throw<ArgumentOutOfRangeException>(because);
            c.Should().Throw<ArgumentOutOfRangeException>(because);
        }

        [TestMethod]
        public void TableShouldThrowError_OnFlippingNonexistantCard()
        {
            Action a = () => table.Flip_SpecificCard_SpecificPlayer(0, 3);

            SetUpTableForTests();

            a.Should().Throw<ArgumentOutOfRangeException>("Player 0 doesn't have a 4th card on the board");
        }

        [TestMethod]
        public void TableRemovesACard()
        {
            SetUpTableForTests();

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
            SetUpTableForTests();

            Action a = () => table.RemoveSpecificCard_FromPlayer(0, 5);
            a.Should().Throw<ArgumentOutOfRangeException>("there is no sixth player");
        }

        [TestMethod]
        public void TableShouldThrowException_IfRemovingNonexistantCard_FromPlayer()
        {
            SetUpTableForTests();

            Action a = () => table.RemoveSpecificCard_FromPlayer(4, 0);
            a.Should().Throw<ArgumentOutOfRangeException>("Player 0 has no fifth card");
        }

        [TestMethod]
        public void PickUpAllCards_From_APlayerTableSpot()
        {
            List<ICard> cards;

            SetUpTableForTests();

            cards = table.PickUpAllCards_FromPlayer(0);
            cards.Count.Should().Be(2, "all of Player 0's cards were picked up");
        }

        [TestMethod]
        public void PickUpAllCards_AndTableNoLongerHasThoseCards()
        {
            SetUpTableForTests();

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
            SetUpTableForTests();
            table.Flip_SpecificCard_SpecificPlayer_SpecificWay(0, 0, facedown);
            table.PrintTableState();

            if (facedown)
            {
                if (OperatingSystem.IsIOS())
                {
                    output.ToString().Should().Be("COVERED\n9J\n1Q\nCOVERED\n");
                }
                else if (OperatingSystem.IsWindows())
                {
                    output.ToString().Should().Be("COVERED\r\n9J\r\n1Q\r\nCOVERED\r\n");
                }
            }
            else
            {
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
            ICard? c = table.DrawCardFromDeck(0)!;

            c.Should().NotBeNull("because a new card is being drawn from this deck");
        }

        [TestMethod]
        public void TableCanDrawManyCards_FromSpecificDeck()
        {
            List<ICard?> cards  = new();

            cards = table.DrawMultipleCardsFromDeck(5);

            cards.Count.Should().Be(5, "5 cards was drawn from the first deck");

            foreach (ICard? c in cards)
            {
                c.Should().NotBeNull("the deck is new and should not be empty");
            }
        }

        [TestMethod]
        public void TableCannotDraw_FromNonexistantDeck()
        {
            Action a = () => table.DrawCardFromDeck(10);
            Action b = () => table.DrawMultipleCardsFromDeck(5, 4);

            a.Should().Throw<ArgumentOutOfRangeException>("there is no 11th deck");
            b.Should().Throw<ArgumentOutOfRangeException>("there is no 5th deck");
        }

        [TestMethod]
        public void TableCanAddCard_ToNeutralZone()
        {
            PlayingCard card = new(10, "J");
            Table neutralTable = new(gm, 0, 2);

            neutralTable.AddCardTo_NeutralZone(card, 1);
            Console.SetOut(output);

            neutralTable.GetCardsForSpecificNeutralZone(1)[0].Should().BeEquivalentTo(card);
        }

        [TestMethod]
        public void TableCanAddMultipleCards_ToNeutralZone()
        {
            List<ICard> cards = new() { new PlayingCard(10, "J"), new PlayingCard(5, "Q") };
            Table neutralTable = new(gm, 0, 2);

            neutralTable.AddCardsTo_NeutralZone(cards, 0);
            Console.SetOut(output);
            neutralTable.GetCardsForSpecificNeutralZone(0).Should().BeEquivalentTo(cards);
        }

        [TestMethod]
        public void AllCardsArePickedUp_FromAllZones()
        {
            IGameMediator gm = new BaseGameMediator(4);
            Table table = new(gm, 4, 3);

            table.AddCardsTo_NeutralZone(
                new List<ICard>() { new PlayingCard(10, "J"), new PlayingCard(2, "J") }, 0);
            table.AddCardTo_NeutralZone(new PlayingCard(10, "Q"), 1);
            table.PlayCardTo_PlayerZone(0, new PlayingCard(3, "J"));
            table.PlayCardTo_PlayerZone(1, new PlayingCard(4, "J"));
            table.PlayCardTo_PlayerZone(2, new PlayingCard(5, "J"));
            table.PlayCardTo_PlayerZone(3, new PlayingCard(6, "J"));
            table.PlayCardTo_PlayerZone(3, new PlayingCard(7, "J"));

            table.Remove_AllCardsFromTable();

            foreach (var cards in table.TableNeutralZones)
            {
                cards.Count.Should().Be(0, "no cards should be in the neutral zones");
            }

            foreach (var cards in table.PlayerZones)
            {
                cards.Count.Should().Be(0, "no cards should be left in player spots");
            }
        }

        [TestMethod]
        public void TablePlaysCardFromDeck_ToNeutralZone()
        {
            IGameMediator gm = new BaseGameMediator(0);
            Table table = new(gm, 0, new DeckOfPlayingCards(), 1);

            table.PlayCards_FromTableDeck_ToNeutralZone(2, 0, 0);

            table.TableDecks[0].Count.Should().Be(50, "two cards were taken from the deck");
            table.TableNeutralZones[0].Count.Should().Be(2, "the two cards were placed in the neutral zone");
        }

        [TestMethod]
        public void CardCanBeRemovedFromTable_ViaLookup()
        {
            IGameMediator gm = new BaseGameMediator(0);
            Table table = new(gm, 1);
            List<ICard> cardsToAdd = new()
            {
                new PlayingCard(10, "J"),
                new PlayingCard(11, "J"),
                new PlayingCard(12, "J")
            };

            table.PlayCardsTo_PlayerZone(0, cardsToAdd);

            table.RemoveCard_FromPlayerZone(cardsToAdd[1], 0);

            table.PlayerZones[0].Count.Should().Be(2, "one of the cards were removed");
        }

        [TestMethod]
        public void SimilarCardsAreCorrectlyDistinguished_AndRemoved_ViaLookup()
        {
            IGameMediator gm = new BaseGameMediator(0);
            Table table = new(gm, 1);
            BaseCharacterCard attackingCard = new(gm, 2, 7, "Aggressor");
            List<ICard> cardsToAdd = new()
            {
                new BaseCharacterCard(gm, 5, 5, "Surveyor"),
                new BaseCharacterCard(gm, 5, 5, "Surveyor")
            };

            table.PlayCardsTo_PlayerZone(0, cardsToAdd);
            attackingCard.Attack((ICharacterCard)table.PlayerZones[0][1]);
            table.RemoveCard_FromPlayerZone(cardsToAdd[1], 0);

            BaseCharacterCard remainingCard = (BaseCharacterCard)table.PlayerZones[0][0];
            remainingCard.HealthVal.Should().Be(5, "the unattacked card should be the remaining card");
        }

        [TestMethod]
        public void CardIsRemovedWhenItDies()
        {
            IGameMediator gm = new BaseGameMediator(0);
            Table table = new(gm, 1);
            BaseCharacterCard attackingCard = new(gm, 2, 7, "Aggressor");
            BaseCharacterCard poorVictim = new(gm, 0, 1, "Villager");

            table.PlayCardTo_PlayerZone(0, poorVictim);
            attackingCard.Attack(poorVictim);

            table.PlayerZones[0].Count.Should().Be(0, "the Villager card was killed and removed from the table.");
        }
    }
}
