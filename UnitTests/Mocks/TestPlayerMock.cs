using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace UnitTests.Mocks
{
    /// <summary>
    /// Mocks the <see cref="BasePlayer"/> but changes some implementations to be used in testing.
    /// </summary>
    public class TestPlayerMock : BasePlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestPlayerMock"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to intereact with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        public TestPlayerMock(IGameMediator gm, int playerID)
            : base(gm, playerID: playerID)
        {
        }

        /// <summary>
        /// Mocks the PlayCard of <see cref="BasePlayer"/> but always plays the first card in hand.
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
