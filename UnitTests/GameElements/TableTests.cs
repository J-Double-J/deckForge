using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PlayerConstruction;
using FluentAssertions;
using FluentAssertions.Specialized;
using System.Numerics;

namespace UnitTests.GameElements
{
    [TestClass]
    public class TableTests
    {
        private static IGameMediator gm;

        private static Table table;
        private static StringWriter output = new();

        /// <summary>
        /// Not called for all tests but for a good number of them.
        /// </summary>
        private void SetUpTableForTests()
        {
            Console.SetOut(output);

            table.PlayCardsToZone(
                new List<ICard>()
                { new PlayingCard(8, "J", facedown: false), new PlayingCard(9, "J", facedown: false) },
                TablePlacementZoneType.PlayerZone,
                0);
            table.PlayCardsToZone(
                new List<ICard>()
                { new PlayingCard(1, "Q", facedown: false), new PlayingCard(2, "Q", facedown: true) },
                TablePlacementZoneType.PlayerZone,
                1);
        }

        [ClassInitialize]
        public static void InitializeTableTestsClass(TestContext ctx)
        {
            gm = new BaseGameMediator(2);
        }

        [TestInitialize]
        public void InitializeTableTests()
        {
            List<IDeck> decks = new() { new DeckOfPlayingCards(), new DeckOfPlayingCards(), new DeckOfPlayingCards() };
            TableZone playerZone = new(TablePlacementZoneType.PlayerZone, 3, decks);
            TableZone neutralZone = new(TablePlacementZoneType.NeutralZone, 1);
            table = new(gm, new List<TableZone>() { playerZone, neutralZone });
            output = new();
            for (var i = 0; i < 2; i++)
            {
                // Registers player with GameMediator
                new BasePlayer(gm, i);
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(7)]
        public void PlaceCardOnTableInFrontOfNonexistantPlayer(int fakePlayerID)
        {
            Action a = () => table.PlayCardToZone(new PlayingCard(8, "J"), TablePlacementZoneType.PlayerZone, fakePlayerID);

            a.Should().Throw<ArgumentException>($"there is no player with the id of {fakePlayerID}");
        }

        [TestMethod]
        public void TablePrintsCardsCorrectly()
        {
            Console.SetOut(output);

            table.PlayCardsToZone(
                new List<ICard>()
                { new PlayingCard(8, "J", facedown: false), new PlayingCard(9, "J", facedown: false), new PlayingCard(10, "J", facedown: false) },
                TablePlacementZoneType.PlayerZone,
                0);
            table.PlayCardsToZone(
                new List<ICard>()
                { new PlayingCard(8, "J", facedown: false), new PlayingCard(9, "J", facedown: false), new PlayingCard(10, "J", facedown: false) },
                TablePlacementZoneType.PlayerZone,
                1);

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

            var cards = table.GetCardsInZone(TablePlacementZoneType.PlayerZone)[0];

            foreach (ICard c in cards)
            {
                s += c.PrintCard();
            }

            s.Should().Be("8J9J", "these are the cards that Player 0 has in front of them");
        }

        [TestMethod]
        public void TableFlipsAllPlayerCards()
        {
            SetUpTableForTests();

            table.FlipAllCardsInZoneCertainWay(TablePlacementZoneType.PlayerZone, true);
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

            table.FlipAllCardsInZone(TablePlacementZoneType.PlayerZone);
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

            table.FlipAllCardsInAreaInZone(TablePlacementZoneType.PlayerZone, 0);
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

            table.FlipAllCardsInAreaInZone(TablePlacementZoneType.PlayerZone, 1);
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

            table.FlipCardInZone(TablePlacementZoneType.PlayerZone, 0, 0);
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
            Action a = () => table.FlipAllCardsInAreaInZone(TablePlacementZoneType.PlayerZone, 3);
            Action b = () => table.FlipAllCardsInAreaInZoneCertainWay(TablePlacementZoneType.PlayerZone, 3, false);
            Action c = () => table.FlipCardInZone(TablePlacementZoneType.PlayerZone, 3, 0);
            string because = "there is no player 3 on the table";

            SetUpTableForTests();

            a.Should().Throw<ArgumentOutOfRangeException>(because);
            b.Should().Throw<ArgumentOutOfRangeException>(because);
            c.Should().Throw<ArgumentOutOfRangeException>(because);
        }

        [TestMethod]
        public void TableShouldThrowError_OnFlippingNonexistantCard()
        {
            SetUpTableForTests();

            Action a = () => table.FlipCardInZone(TablePlacementZoneType.PlayerZone, 0, 3);

            a.Should().Throw<ArgumentOutOfRangeException>("Player 0 doesn't have a 4th card on the board");
        }

        [TestMethod]
        public void TableRemovesACard()
        {
            SetUpTableForTests();

            table.RemoveCardFromTable(TablePlacementZoneType.PlayerZone, 0, 0);
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

            Action a = () => table.RemoveCardFromTable(TablePlacementZoneType.PlayerZone, 5, 0);
            a.Should().Throw<ArgumentException>("there is no sixth player");
        }

        [TestMethod]
        public void TableShouldThrowException_IfRemovingNonexistantCard_FromPlayer()
        {
            SetUpTableForTests();

            Action a = () => table.RemoveCardFromTable(TablePlacementZoneType.PlayerZone, 0, 4);
            a.Should().Throw<ArgumentException>("Player 0 has no fifth card");
        }

        [TestMethod]
        public void PickUpAllCards_From_APlayerTableSpot()
        {
            List<ICard> cards;

            SetUpTableForTests();

            cards = table.RemoveAllCards_FromArea(TablePlacementZoneType.PlayerZone, 0);
            cards.Count.Should().Be(2, "all of Player 0's cards were picked up");
        }

        [TestMethod]
        public void PickUpAllCards_AndTableNoLongerHasThoseCards()
        {
            SetUpTableForTests();

            table.RemoveAllCards_FromArea(TablePlacementZoneType.PlayerZone, 0);
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
            table.FlipCardInZoneCertainWay(TablePlacementZoneType.PlayerZone, 0, 0, facedown);
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
            List<IDeck> decks = new() { new DeckOfPlayingCards(), new DeckOfPlayingCards(), new DeckOfPlayingCards() };
            TableZone playerZone = new(TablePlacementZoneType.PlayerZone, 3, decks);
            TableZone neutralZone = new(TablePlacementZoneType.NeutralZone, 1);
            Table table = new(gm, new List<TableZone>() { playerZone, neutralZone });

            ICard? c = table.DrawCardFromDeck(0)!;

            c.Should().NotBeNull("because a new card is being drawn from this deck");
        }

        [TestMethod]
        public void TableCanDrawManyCards_FromSpecificDeck()
        {
            List<IDeck> decks = new() { new DeckOfPlayingCards(), new DeckOfPlayingCards(), new DeckOfPlayingCards() };
            TableZone playerZone = new(TablePlacementZoneType.PlayerZone, 3, decks);
            TableZone neutralZone = new(TablePlacementZoneType.NeutralZone, 1);
            Table table = new(gm, new List<TableZone>() { playerZone, neutralZone });
            var cards = table.DrawMultipleCardsFromDeck(5, TablePlacementZoneType.PlayerZone, 0);

            cards.Count.Should().Be(5, "5 cards was drawn from the first deck");

            foreach (ICard? c in cards)
            {
                c.Should().NotBeNull("the deck is new and should not be empty");
            }
        }

        [TestMethod]
        public void TableCannotDraw_FromNonexistantDeck()
        {
            List<IDeck> decks = new() { new DeckOfPlayingCards(), new DeckOfPlayingCards(), new DeckOfPlayingCards() };
            TableZone playerZone = new(TablePlacementZoneType.PlayerZone, 3, decks);
            TableZone neutralZone = new(TablePlacementZoneType.NeutralZone, 1);
            Table table = new(gm, new List<TableZone>() { playerZone, neutralZone });

            Action a = () => table.DrawCardFromDeck(TablePlacementZoneType.PlayerZone, 10);
            Action b = () => table.DrawMultipleCardsFromDeck(5, TablePlacementZoneType.PlayerZone, 4);

            a.Should().Throw<ArgumentException>("there is no 11th deck");
            b.Should().Throw<ArgumentException>("there is no 5th deck");
        }

        [TestMethod]
        public void TableCanAddCard_ToNeutralZone()
        {
            PlayingCard card = new(10, "J");
            TableZone zone = new(TablePlacementZoneType.NeutralZone, 2);
            Table neutralTable = new(gm, new List<TableZone>() { zone });

            neutralTable.PlayCardToZone(card, TablePlacementZoneType.NeutralZone, 1);
            Console.SetOut(output);

            neutralTable.GetCardsInZone(TablePlacementZoneType.NeutralZone)[1][0].Should().BeEquivalentTo(card);
        }

        [TestMethod]
        public void TableCanAddMultipleCards_ToNeutralZone()
        {
            List<ICard> cards = new() { new PlayingCard(10, "J"), new PlayingCard(5, "Q") };
            TableZone zone = new(TablePlacementZoneType.NeutralZone, 2);
            Table neutralTable = new(gm, new List<TableZone>() { zone });

            neutralTable.PlayCardsToZone(cards, TablePlacementZoneType.NeutralZone, 0);
            Console.SetOut(output);
            neutralTable.GetCardsInZone(TablePlacementZoneType.NeutralZone)[0].Should().BeEquivalentTo(cards);
        }

        [TestMethod]
        public void AllCardsArePickedUp_FromAllZones()
        {
            IGameMediator gm = new BaseGameMediator(4);
            TableZone playerZone = new(TablePlacementZoneType.PlayerZone, 4);
            TableZone neutralZone = new(TablePlacementZoneType.NeutralZone, 3);
            Table table = new(gm, new List<TableZone>() { playerZone, neutralZone });

            table.PlayCardsToZone(new List<ICard>() { new PlayingCard(10, "J"), new PlayingCard(2, "J") }, TablePlacementZoneType.NeutralZone, 0);
            table.PlayCardToZone(new PlayingCard(10, "Q"), TablePlacementZoneType.NeutralZone, 1);

            table.PlayCardToZone(new PlayingCard(3, "J"), TablePlacementZoneType.PlayerZone, 0);
            table.PlayCardToZone(new PlayingCard(4, "J"), TablePlacementZoneType.PlayerZone, 1);
            table.PlayCardToZone(new PlayingCard(5, "J"), TablePlacementZoneType.PlayerZone, 2);
            table.PlayCardToZone(new PlayingCard(6, "J"), TablePlacementZoneType.PlayerZone, 3);
            table.PlayCardToZone(new PlayingCard(7, "J"), TablePlacementZoneType.PlayerZone, 3);
            table.Remove_AllCardsFromTable();

            foreach (var cards in table.GetCardsInZone(TablePlacementZoneType.NeutralZone))
            {
                cards.Count.Should().Be(0, "no cards should be in the neutral zones");
            }

            foreach (var cards in table.GetCardsInZone(TablePlacementZoneType.PlayerZone))
            {
                cards.Count.Should().Be(0, "no cards should be left in player spots");
            }
        }

        [TestMethod]
        public void CardCanBeRemovedFromTable_ViaLookup()
        {
            IGameMediator gm = new BaseGameMediator(0);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 1);
            Table table = new(gm, new List<TableZone>() { zone });
            List<ICard> cardsToAdd = new()
            {
                new PlayingCard(10, "J"),
                new PlayingCard(11, "J"),
                new PlayingCard(12, "J")
            };

            table.PlayCardsToZone(cardsToAdd, TablePlacementZoneType.PlayerZone, 0);
            table.RemoveCardFromTable(cardsToAdd[1], TablePlacementZoneType.PlayerZone, 0);

            table.GetCardsInZone(TablePlacementZoneType.PlayerZone)[0].Count.Should().Be(2, "one of the cards were removed");
        }

        [TestMethod]
        public void SimilarCardsAreCorrectlyDistinguished_AndRemoved_ViaLookup()
        {
            IGameMediator gm = new BaseGameMediator(0);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 1);
            Table table = new(gm, new List<TableZone>() { zone });
            BaseCharacterCard attackingCard = new(gm, 2, 7, "Aggressor");
            List<ICard> cardsToAdd = new()
            {
                new BaseCharacterCard(gm, 5, 5, "Surveyor"),
                new BaseCharacterCard(gm, 5, 5, "Surveyor")
            };

            table.PlayCardsToZone(cardsToAdd, TablePlacementZoneType.PlayerZone, 0);
            attackingCard.Attack((ICharacterCard)table
                .GetCardsInZone(TablePlacementZoneType.PlayerZone)[0][1]);
            table.RemoveCardFromTable(cardsToAdd[1], TablePlacementZoneType.PlayerZone, 0);

            BaseCharacterCard remainingCard = (BaseCharacterCard)table.GetCardsInZone(TablePlacementZoneType.PlayerZone)[0][0];
            remainingCard.HealthVal.Should().Be(5, "the unattacked card should be the remaining card");
        }

        [TestMethod]
        public void CardIsRemovedWhenItDies()
        {
            IGameMediator gm = new BaseGameMediator(0);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 1);
            Table table = new(gm, new List<TableZone>() { zone });
            BaseCharacterCard attackingCard = new(gm, 2, 7, "Aggressor");
            BaseCharacterCard poorVictim = new(gm, 0, 1, "Villager");

            table.PlayCardToZone(poorVictim, TablePlacementZoneType.PlayerZone, 0);
            attackingCard.Attack(poorVictim);

            table.GetCardsInZone(TablePlacementZoneType.PlayerZone)[0].Count.Should().Be(0, "the Villager card was killed and removed from the table.");
        }
    }
}
