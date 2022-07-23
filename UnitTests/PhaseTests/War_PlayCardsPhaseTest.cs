﻿using deckForge.GameConstruction;
using deckForge.GameConstruction.PresetGames.War;
using deckForge.GameElements;
using deckForge.GameElements.Resources;
using deckForge.GameRules;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.PhaseTests
{
    [TestClass]
    public class War_PlayCardsPhaseTest
    {

        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static IGameMediator gm;
        private static Table table;
        List<IPlayer> players;
        List<int> playerIDs;
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize()]
        public void InitializeTableTests() {
            gm = new BaseGameMediator(2);
            table = new(gm, 2, new Deck());

            players = new();
            for (var i = 0; i < 2; i++)
            {
                List<Card> cards = table.DrawMultipleCardsFromDeck(26)!;
                Deck deck = new(cards);
                players.Add(new WarPlayer(gm, i, deck));
            }

            playerIDs = new();
            foreach (IPlayer player in players)
            {
                playerIDs.Add(player.PlayerID);
            }
        }

        [TestMethod]
        public void WarPlaysCardsPhase() {

            IPhase phase = new WarPlayCardsPhase(gm, playerIDs, "Play Cards Phase");

            phase.StartPhase();

            table.PrintTableState();

            table.TableState.Count.Should().Be(2, "there are two players at the table");
            foreach (List<Card> playedCardsInFrontOfPlayer in table.TableState) {
                playedCardsInFrontOfPlayer.Count().Should().Be(1, "only one card was drawn and played in front of each player");
                playedCardsInFrontOfPlayer[0].Facedown.Should().Be(false, "the players were told to flip their cards faceup");
            }
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)] //Player two wins
        public void WarComparesCardsPhase_PlayerWins(bool playerZeroWins) {
            Card cardOne = new Card(7, "C");
            Card cardTwo = new Card(5, "H");

            WarComparePhase comparePhase = new(gm, playerIDs, "Compare Phase");
            if (playerZeroWins)
            {
                table.PlaceCardOnTable(0, cardOne);
                table.PlaceCardOnTable(1, cardTwo);
                comparePhase.FlippedCards.Add(cardOne);
                comparePhase.FlippedCards.Add(cardTwo);
            }
            else {
                table.PlaceCardOnTable(1, cardOne);
                table.PlaceCardOnTable(0, cardTwo);
                comparePhase.FlippedCards.Add(cardTwo);
                comparePhase.FlippedCards.Add(cardOne);
            }
            
            IPhase phase = comparePhase;
            phase.StartPhase();

            //Note: Player doesn't play out of their deck in this test, so they have init hand size (26)+2 cards picked up
            if (playerZeroWins)
                players[0].CountOfResourceCollection(0).Should().Be(28, "IPlayer took their card and opponent's off the Table and put it in their deck.");
            else
                players[1].CountOfResourceCollection(0).Should().Be(28, "IPlayer took their card and opponent's off the Table and put it in their deck.");
        }

        [TestMethod]
        public void NoClearWinnerInComparePhase() {
            Card cardOne = new(5, "C");
            Card cardTwo = new(5, "H");

            table.PlaceCardOnTable(0, cardOne);
            table.PlaceCardOnTable(1, cardTwo);

            WarComparePhase comparePhase = new(gm, playerIDs, "Compare Phase");
            comparePhase.FlippedCards.Add(cardOne);
            comparePhase.FlippedCards.Add(cardTwo);

            IPhase phase = comparePhase;
            phase.StartPhase();

            //Player did not play a card directly out of their deck, so they have init deck size (26)
            players[0].CountOfResourceCollection(0).Should().Be(26, "their card is still on the table and they didn't pick up any other cards");
            players[1].CountOfResourceCollection(0).Should().Be(26, "their card is still on the table and they didn't pick up any other cards");
        }
    }
}