using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace UnitTests.Mocks
{
    public class TestPlayerMock : BasePlayer
    {
        public TestPlayerMock(IGameMediator gm, int playerID)
            : base(gm, playerID: playerID)
        {
        }

        /// <summary>
        /// Mocks the PlayCard of <see cref="BasePlayer"/> but always plays the first card in collection.
        /// </summary>
        /// <param name="facedown">If true, plays <see cref="ICard"/> facedown.</param>
        /// <returns>The <see cref="ICard"/> that was played.</returns>
        public override ICard? PlayCard(bool facedown = false)
        {
            if (PlayerHand.Count == 0)
            {
                return null;
            }

            ICard card = PlayerHand.Cards[0];
            PlayerHand.RemoveResource(card);

            if (facedown)
            {
                card.Flip();
            }

            // TODO: Possible conflict of ordering. Does another player/card do their events before or after a card is played?
            GM.PlayerPlayedCard(PlayerID, card);
            OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(card));

            return card;
        }
    }
}
