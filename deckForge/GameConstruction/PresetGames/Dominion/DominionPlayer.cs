using DeckForge.GameConstruction.PresetGames.Dominion.Actions;
using DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTypes;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;
using DeckForge.HelperObjects;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;
using DeckForge.PlayerConstruction;
using DeckForge.PlayerConstruction.PlayerEvents;
using static System.Formats.Asn1.AsnWriter;

namespace DeckForge.GameConstruction.PresetGames.Dominion
{
    /// <summary>
    /// A <see cref="IPlayer"/> in Dominion.
    /// </summary>
    public class DominionPlayer : PlayerWithActionChoices
    {
        private readonly BuyActionPrompter buyActionPrompter;

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
            buyActionPrompter = new(gm);
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
            CreateDefaultActions();
            Actions = DefaultActions;
            buyActionPrompter = new(gm, reader, output);
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
        /// Plays a given <see cref="ICard"/> for free, costing no actions.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to play.</param>
        /// <returns><see cref="ICard"/> played.</returns>
        public ICard? PlayCardForFree(ICard card)
        {
            var tempAction = new PlayCardAction();
            int playCardCount = Actions[tempAction.Name].ActionCount;

            // TODO: Possible conflict of ordering. Does another player/card do their events before or after a card is played?
            GM.PlayerPlayedCard(PlayerID, card);
            OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(card));

            Actions[tempAction.Name] = (Actions[tempAction.Name].Action, playCardCount);

            return card;
        }

        /// <summary>
        /// Discards the card into the <see cref="DominionPlayer"/>'s dicard pile.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to discard.</param>
        public void AddCardToDiscardPile(ICard card)
        {
            DiscardPile.AddCardToDeck(card);
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

        /// <summary>
        /// <see cref="IPlayer"/> can buy a <see cref="ICard"/> from the Market.
        /// </summary>
        /// <param name="putCardInDiscard">Puts <see cref="ICard"/> in discard if purchased if <c>true</c>. Otherwise
        /// only returns purchased <see cref="ICard"/>.</param>
        /// <returns><see cref="ICard"/> if successful purchase. <c>null</c> if no <see cref="ICard"/>
        /// was bought.</returns>
        public ICard? Buy(bool putCardInDiscard = true)
        {
            ICard? purchasedCard = null;

            while (purchasedCard == null)
            {
                OutputDisplay.Display($"You have {Coins} Coins");
                int response = buyActionPrompter.Prompt();
                if (response != -1)
                {
                    purchasedCard = BuyCardFromMarketDeck(response);
                }
                else
                {
                    break;
                }
            }

            if (putCardInDiscard && purchasedCard is not null)
            {
                DiscardPile.AddCardToDeck(purchasedCard);
            }

            return purchasedCard;
        }

        /// <inheritdoc/>
        public override void EndTurn()
        {
            base.EndTurn();
            Coins = 0;
            DiscardHandAndPlayArea();
            DrawMultipleCards(5);
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

        /// <inheritdoc/>
        protected override void PostActionHook(IGameAction<IPlayer> pickedAction)
        {
        }

        private void DiscardHandAndPlayArea()
        {
            var cards = PlayerHand.ClearCollection();
            cards.AddRange(GM.Table!.RemoveAllCards_FromArea(GameElements.Table.TablePlacementZoneType.PlayerZone, PlayerID));
            DiscardPile.AddMultipleCardsToDeck(cards);
        }

        private void CreateDefaultActions()
        {
            List<PlayerGameAction> playerActions = new() { new PlayCardAction(), new BuyAction(), new EndTurnAction() };

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

        private ICard? BuyCardFromMarketDeck(int response)
        {
            // Offset of -1 because prompter has everything offset by 1
            var cost = ((DominionGameMediator)GM).Market.GetCostOfCardsInDeck(response - 1);
            bool canAfford = CostVerifier.VerifyMinimumPayment(cost, new Dictionary<Type, int>() { { typeof(Coin), Coins } }, out var overPayedResources);

            if (canAfford)
            {
                LoseAction(new BuyAction(), 1);
                Coins = overPayedResources![typeof(Coin)];
                ICard purchasedCard = ((DominionGameMediator)GM).GrabCardFromMarketPlace(response - 1, true);
                return purchasedCard;
            }

            OutputDisplay.Display("\nNot enough Coins.\n");

            return null;
        }
    }
}
