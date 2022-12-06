using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameConstruction.PresetGames.Dominion.DominionTableAreas;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.HelperObjects;

namespace DeckForge.GameConstruction.PresetGames.Dominion
{
    /// <summary>
    /// GameMediator for Dominion.
    /// </summary>
    public class DominionGameMediator : BaseGameMediator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionGameMediator"/> class.
        /// </summary>
        /// <param name="playerCount">Number of <see cref="DominionPlayer"/>s in game.</param>
        public DominionGameMediator(int playerCount)
            : this(new ConsoleReader(), new ConsoleOutput(), playerCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DominionGameMediator"/> class.
        /// </summary>
        /// <param name="reader">Specifies where to get user input.</param>
        /// <param name="output">Specifies where to display any output.</param>
        /// <param name="playerCount">Number of <see cref="IPlayer"/>s in the game.</param>
        public DominionGameMediator(IInputReader reader, IOutputDisplay output, int playerCount)
            : base(reader, output, playerCount)
        {
        }

        /// <summary>
        /// Gets the Dominion game's market area.
        /// </summary>
        public DominionMarketTableArea Market
        {
            get
            {
                return (DominionMarketTableArea)Table!.FindZoneBasedOnType(TablePlacementZoneType.NeutralZone, true)!.Areas[0];
            }
        }

        /// <summary>
        /// Grabs a <see cref="ICard"/> from the Market.
        /// </summary>
        /// <param name="deckNum">Deck from Market to target.</param>
        /// <param name="purchased">Whether this action is due to a purchase or not.</param>
        /// <returns>Drawn <see cref="ICard"/> from <see cref="IDeck"/>.</returns>
        /// <exception cref="ArgumentException">Throws if <see cref="IDeck"/> is empty.</exception>
        public ICard GrabCardFromMarketPlace(int deckNum, bool purchased = true)
        {
            ICard? card = Market.DrawCardsFromDeck(deckNum, 1)[0];
            if (card is null)
            {
                throw new ArgumentException("Cannot grab card from empty deck", nameof(deckNum));
            }

            if (purchased)
            {
                // TODO: Event?
            }

            return card!;
        }

        /// <inheritdoc/>
        public override void EndGame()
        {
            Dictionary<int, int> scoreboard = new();
            foreach (var player in Players!)
            {
                scoreboard.Add(player.PlayerID, ((DominionPlayer)player).Score());
            }

            GameOver = true;
            AnnounceWinner(scoreboard);
        }

        /// <inheritdoc/>
        protected override void AfterAllRoundsEndedHook()
        {
            base.AfterAllRoundsEndedHook();
            EndGame();
        }

        // TODO: Assumes no ties.
        private void AnnounceWinner(Dictionary<int, int> scoreboard)
        {
            var scores = scoreboard
                .OrderByDescending(score => score.Value)
                .Select(score => new Tuple<int, int>(score.Key, score.Value))
                .ToList();

            OutputDisplay.Display($"Player {scores[0].Item1} wins!\n");
            foreach (var entry in scores)
            {
                OutputDisplay.Display($"Player {entry.Item1}: {entry.Item2} points");
            }
        }
    }
}
