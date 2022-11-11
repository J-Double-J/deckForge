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
            PokerGameMediator pGM = new PokerGameMediator(2);
            Table table = new Table(pGM, 2, 1);
            Dictionary<int, List<PlayingCard>> hands = new();
            PlayerGameAction playCards = new PlayHandToTable();

            table.AddCardsTo_NeutralZone(
                new List<ICard>()
                {
                    new PlayingCard(2, "Q"), new PlayingCard(3, "Q"), new PlayingCard(4, "Q")
                },
                0);

            table.PlayCardsTo_PlayerZone(0, new List<ICard>() { new PlayingCard(10, "J"), new PlayingCard(5, "J") });
            table.PlayCardsTo_PlayerZone(1, new List<ICard>() { new PlayingCard(1, "J"), new PlayingCard(2, "J") });

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
            PokerGameMediator pGM = new PokerGameMediator(2);
            Table table = new Table(pGM, 2, 1);
            Dictionary<int, List<PlayingCard>> hands = new();
            PlayerGameAction playCards = new PlayHandToTable();

            table.AddCardsTo_NeutralZone(
                new List<ICard>()
                {
                    new PlayingCard(2, "Q"), new PlayingCard(3, "Q"), new PlayingCard(4, "Q")
                },
                0);
            table.PlayCardsTo_PlayerZone(0, new List<ICard>() { new PlayingCard(10, "J"), new PlayingCard(5, "J") });
            table.PlayCardsTo_PlayerZone(1, new List<ICard>() { new PlayingCard(10, "D"), new PlayingCard(5, "D") });

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
