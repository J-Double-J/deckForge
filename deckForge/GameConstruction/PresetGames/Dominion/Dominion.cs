using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameConstruction.PresetGames.Dominion.DominionTableAreas;
using DeckForge.GameConstruction.PresetGames.Dominion.Rules;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.HelperObjects;

namespace DeckForge.GameConstruction.PresetGames.Dominion
{
    /// <summary>
    /// The game of Dominion.
    /// </summary>
    public sealed class Dominion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dominion"/> class. Specifies player input and output destinations.
        /// </summary>
        /// <param name="reader">Specifies where to get user input.</param>
        /// <param name="output">Specifies where to display any output.</param>
        /// <param name="playerCount">Number of <see cref="DominionPlayer"/>s to create.</param>
        public Dominion(IInputReader reader, IOutputDisplay output, int playerCount)
        {
            Reader = reader;
            Output = output;
            GM = new(Reader, Output, playerCount);
            GM.RegisterTurnHandler(new TurnHandler(playerCount));
            Table table = new(
                GM,
                new List<TableZone>()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, CreateTableAreasForEachPlayer(playerCount)),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { new DominionMarketTableArea(GetMarketDecks(playerCount)) })
                });

            List<DominionPlayer> players = CreatePlayers(playerCount);
            DominionRound roundRules = new(GM, GM.TurnOrder);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dominion"/> class.
        /// </summary>
        /// <param name="playerCount">Number of <see cref="DominionPlayer"/>s to create.</param>
        public Dominion(int playerCount)
            : this(new ConsoleReader(), new ConsoleOutput(), playerCount)
        {
        }

        private DominionGameMediator GM { get; }

        private IInputReader Reader { get; }

        private IOutputDisplay Output { get;  }

        private List<IDeck> GivenDecks { get; } = new();

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            GM.StartGame();
        }

        private static List<TableArea> CreateTableAreasForEachPlayer(int playerCount)
        {
            List<TableArea> playerAreas = new();
            for (int i = 0; i < playerCount; i++)
            {
                playerAreas.Add(new DominionPlayerTableArea(i));
            }

            return playerAreas;
        }

        private List<IDeck> GetMarketDecks(int playerCount)
        {
            if (GivenDecks.Count == 0)
            {
                return CreateDefaultMarket(playerCount);
            }
            else
            {
                return GivenDecks;
            }
        }

        private List<IDeck> CreateDefaultMarket(int playerCount)
        {
            return new List<IDeck>()
            {
                new MonotoneDeck(typeof(CopperCard), 60 - (7 * playerCount)),
                new MonotoneDeck(typeof(SilverCard), 40),
                new MonotoneDeck(typeof(GoldCard), 30),
                new MonotoneDeck(typeof(EstateCard), 24 - (3 * playerCount)),
                new MonotoneDeck(typeof(DuchyCard), GetNumberOfCardsForDuchyOrProvince(playerCount)),
                new MonotoneDeck(typeof(ProvinceCard), GetNumberOfCardsForDuchyOrProvince(playerCount)),
                new MonotoneDeck(typeof(CurseCard), 30),
                new MonotoneDeck(typeof(VillageCard), 10),
                new MonotoneDeck(typeof(WoodcutterCard), 10),
                new MonotoneDeck(typeof(SmithyCard), 10),
                new MonotoneDeck(typeof(LaboratoryCard), 10)
            };
        }

        private int GetNumberOfCardsForDuchyOrProvince(int playerCount)
        {
            if (playerCount < 3)
            {
                return 8;
            }

            return 12;
        }

        private List<DominionPlayer> CreatePlayers(int playerCount)
        {
            List<DominionPlayer> players = new();
            for (int i = 0; i < playerCount; i++)
            {
                players.Add(new DominionPlayer(Reader, Output, GM, i));
                players[i].DrawStartingHand();
            }

            return players;
        }
    }
}
