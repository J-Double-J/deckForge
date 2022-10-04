using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace DeckForge.GameConstruction.PresetGames.War
{
    /// <summary>
    /// An <see cref="IPlayer"/> that plays <see cref="War"/>.
    /// </summary>
    public class WarPlayer : BasePlayer
    {
        private readonly DeckOfPlayingCards personalDeck;

        /// <summary>
        /// Initializes a new instance of the <see cref="WarPlayer"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that the <see cref="WarPlayer"/> uses to communicate
        /// with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        /// <param name="deck">Personal deck that the <see cref="WarPlayer"/> manages.</param>
        public WarPlayer(IGameMediator gm, int playerID, DeckOfPlayingCards deck)
            : base(gm, playerID: playerID, initHandSize: 0)
        {
            personalDeck = deck;
            AddResourceCollection(personalDeck);
        }

        /// <inheritdoc/>
        public override PlayingCard? PlayCard(bool facedown = false)
        {
            PlayingCard? c = (PlayingCard)personalDeck!.DrawCard(drawFacedown: true)!;
            if (c != null)
            {
                GM.PlayerPlayedCard(PlayerID, c);
                OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(c));
                return c;
            }
            else
            {
                RaiseSimplePlayerMessageEvent(new SimplePlayerMessageEventArgs(message: "LOSE_GAME"));
                return c;
            }
        }
    }
}
