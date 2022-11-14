using DeckForge.GameConstruction.PresetGames.Poker;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;
using FluentAssertions;

namespace UnitTests.PokerTests
{
    [TestClass]
    public class SimpleEvaluatorTests
    {
        [TestMethod]
        public void EvaluatorGetsExpectedValues()
        {
            PokerGameMediator pGM = new(2);
            TableZone playerZone = new(TablePlacementZoneType.PlayerZone, 2);
            TableZone neutralZone = new(TablePlacementZoneType.NeutralZone, 1);
            Table table = new(pGM, new List<TableZone>() { playerZone, neutralZone });
            Dictionary<int, List<PlayingCard>> hands = new();
            PlayerGameAction playCards = new PlayHandToTable();

            table.PlayCardsToZone(
                new List<ICard>()
                {
                    new PlayingCard(2, "Q"), new PlayingCard(3, "Q"), new PlayingCard(4, "Q")
                },
                TablePlacementZoneType.NeutralZone,
                0);

            table.PlayCardsToZone(new List<ICard>() { new PlayingCard(10, "J"), new PlayingCard(5, "J") }, TablePlacementZoneType.PlayerZone, 0);
            table.PlayCardsToZone(new List<ICard>() { new PlayingCard(1, "J"), new PlayingCard(2, "J") }, TablePlacementZoneType.PlayerZone, 1);

            hands.Add(0, pGM.Table!.GetCardsForSpecificPlayer(0)
                .Concat(pGM.Table!.GetCardsForSpecificNeutralZone(0)).ToList()
                    .ConvertAll(c => (PlayingCard)c));
            hands.Add(1, pGM.Table!.GetCardsForSpecificPlayer(1)
                .Concat(pGM.Table!.GetCardsForSpecificNeutralZone(0)).ToList()
                    .ConvertAll(c => (PlayingCard)c));

            List<int> winner = SimplisitcPokerHandEvaluator.EvaluateHands(hands);

            winner.Count.Should().Be(1, "only one player wins here");
            winner[0].Should().Be(0, "first player had a higher value hand");
        }

        [TestMethod]
        public void EvaluatorHandlesTiesCorrectly()
        {
            PokerGameMediator pGM = new(2);
            TableZone playerZone = new(TablePlacementZoneType.PlayerZone, 2);
            TableZone neutralZone = new(TablePlacementZoneType.NeutralZone, 1);
            Table table = new(pGM, new List<TableZone>() { playerZone, neutralZone });
            Dictionary<int, List<PlayingCard>> hands = new();
            PlayerGameAction playCards = new PlayHandToTable();

            table.PlayCardsToZone(
                new List<ICard>()
                {
                    new PlayingCard(2, "Q"), new PlayingCard(3, "Q"), new PlayingCard(4, "Q")
                },
                TablePlacementZoneType.NeutralZone,
                0);

            table.PlayCardsToZone(new List<ICard>() { new PlayingCard(10, "J"), new PlayingCard(5, "J") }, TablePlacementZoneType.PlayerZone, 0);
            table.PlayCardsToZone(new List<ICard>() { new PlayingCard(10, "D"), new PlayingCard(5, "D") }, TablePlacementZoneType.PlayerZone, 1);

            hands.Add(0, pGM.Table!.GetCardsForSpecificPlayer(0)
                .Concat(pGM.Table!.GetCardsForSpecificNeutralZone(0)).ToList()
                    .ConvertAll(c => (PlayingCard)c));
            hands.Add(1, pGM.Table!.GetCardsForSpecificPlayer(1)
                .Concat(pGM.Table!.GetCardsForSpecificNeutralZone(0)).ToList()
                    .ConvertAll(c => (PlayingCard)c));

            List<int> winner = SimplisitcPokerHandEvaluator.EvaluateHands(hands);

            winner.Count.Should().Be(2, "both players have the same hand value so there is a tie");
        }
    }
}
