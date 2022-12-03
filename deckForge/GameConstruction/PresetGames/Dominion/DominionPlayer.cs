using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;
using DeckForge.HelperObjects;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;
using DeckForge.PlayerConstruction;
using static System.Formats.Asn1.AsnWriter;

namespace DeckForge.GameConstruction.PresetGames.Dominion
{
    /// <summary>
    /// A <see cref="IPlayer"/> in Dominion.
    /// </summary>
    public class DominionPlayer : PlayerWithActionChoices
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionPlayer"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        public DominionPlayer(IGameMediator gm, int playerID)
            : base(gm, playerID, 5)
        {
            CreateDefaultActions();
            Actions = DefaultActions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DominionPlayer"/> class.
        /// </summary>
        /// <param name="reader">Tells <see cref="IPlayer"/> where to read input.</param>
        /// <param name="output">Tells <see cref="IPlayer"/> where to display output.</param>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="IPlayer"/>.</param>
        public DominionPlayer(IInputReader reader, IOutputDisplay output, IGameMediator gm, int playerID)
            : base(reader, output, gm, playerID, 5)
        {
        }

        /// <summary>
        /// Gets the number of coins the <see cref="IPlayer"/> has in the current turn.
        /// </summary>
        public int Coins { get; private set; }

        /// <summary>
        /// Gets the Player's Deck.
        /// </summary>
        public IDeck PlayerDeck
        {
            get
            {
                return GM.Table!.GetDeckFromAreaInZone(GameElements.Table.TablePlacementZoneType.PlayerZone, PlayerID)!;
            }
        }

        /// <summary>
        /// Gets the <see cref="DominionPlayer"/>'s discard pile.
        /// </summary>
        public IDeck DiscardPile
        {
            get
            {
                return GM.Table!.GetDiscardFromAreaInZone(GameElements.Table.TablePlacementZoneType.PlayerZone, PlayerID)!;
            }
        }

        /// <inheritdoc/>
        public override ICard? DrawCard()
        {
            ICard? card = PlayerDeck.DrawCard();

            if (card is null)
            {
                ShuffleDiscardPileIntoPlayerDeck();
                card = PlayerDeck.DrawCard();
            }

            if (card != null)
            {
                card.OwnedBy = this;
                PlayerHand.AddResource(card);
            }

            return card;
        }

        /// <summary>
        /// Shuffles the <see cref="DominionPlayer"/>'s <see cref="DiscardPile"/> into their <see cref="PlayerDeck"/>.
        /// </summary>
        public void ShuffleDiscardPileIntoPlayerDeck()
        {
            var discardCards = DiscardPile.Deck;
            DiscardPile.ClearCollection();
            PlayerDeck.AddMultipleCardsToDeck(discardCards);
            PlayerDeck.Shuffle();
        }

        public void Buy()
        {
        }

        /// <summary>
        /// Scores all the <see cref="DominionPlayer"/>'s <see cref="ICard"/>'s.
        /// </summary>
        /// <returns>The total score of all their cards.</returns>
        public int Score()
        {
            int score = 0;

            foreach (var card in PlayerDeck.Deck)
            {
                score += ScoreCardIfVictoryCard(card);
            }

            foreach (var card in DiscardPile.Deck)
            {
                score += ScoreCardIfVictoryCard(card);
            }

            foreach (var card in PlayerHand.Cards)
            {
                score += ScoreCardIfVictoryCard(card);
            }

            return score;
        }

        /// <summary>
        /// Increase the number of coins the <see cref="DominionPlayer"/> owns for the turn by set amount.
        /// </summary>
        /// <param name="amount">Number of coins to increase by.</param>
        public void IncreaseCoins(int amount)
        {
            Coins += amount;
        }

        /// <summary>
        /// Reduce the number of coins the <see cref="DominionPlayer"/> owns for the turn by set amount.
        /// </summary>
        /// <param name="amount">Number of coins to reduce by.</param>
        public void ReduceCoins(int amount)
        {
            Coins -= amount;
        }

        private void CreateDefaultActions()
        {
            List<PlayerGameAction> playerActions = new() { new PlayCardAction(), new EndTurnAction() };

            foreach (var action in playerActions)
            {
                DefaultActions.Add(action.Name, (action, 1));
            }
        }

        private int ScoreCardIfVictoryCard(ICard card)
        {
            int score = 0;
            if (card is IVictoryCard vcard)
            {
                score += vcard.VictoryPoints;
            }

            return score;
        }
    }
}
