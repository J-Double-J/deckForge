using CardNamespace;
using deckForge.GameConstruction;
using DeckNameSpace;
using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.PhaseActions;

namespace deckForge.PlayerConstruction
{
    public class Player
    {
        protected readonly GameMediator _gm;
        protected Deck? _personalDeck;
        protected int _cardPlays;
        protected int _cardDraws;
        protected List<Card> _hand = new();

        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;
        public event EventHandler<SimplePlayerMessageEvent>? PlayerMessageEvent;

        public Player(GameMediator gm, int playerID = 0, int initHandSize = 5, Deck? personalDeck = null)
        {
            _gm = gm;
            _personalDeck = personalDeck;

            PlayerID = playerID;

            //TODO: remove this is for sake of testing.
            _cardPlays = 1;
            _cardDraws = 1;

            for (var i = 0; i < initHandSize; i++)
            {
                DrawCard();
            }
        }

        public int HandSize
        {
            get { return _hand.Count; }
        }

        public int PlayerID
        {
            get;
        }

        public int PersonalDeckSize
        {
            get
            {
                if (_personalDeck != null)
                {
                    return _personalDeck.Size;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<Card> PlayedCards
        {
            get
            {
                return _gm.GetPlayedCardsOfPlayer(PlayerID);
            }
        }

        virtual public void StartTurn()
        {
            for (var i = 0; i < _cardDraws; i++)
            {
                DrawCard();
            }
            for (var j = 0; j < _cardPlays; j++)
            {
                PlayCard();
            }
            _gm.EndPlayerTurn();
        }

        //Returns which card was drawn
        virtual public Card? DrawCard()
        {
            Card? c = _gm.DrawCardFromDeck();
            if (c != null)
            {
                _hand.Add(c);
            }
            else
            {
                Console.WriteLine("Deck is Empty.");
            }

            return c;
        }

        //Returns PlayedCard
        virtual public Card? PlayCard(bool facedown = false)
        {
            //TODO: Remove
            if (_hand.Count == 0)
            {
                _gm.EndGame();
                return null;
            }
            else
            {
                Console.WriteLine("Which card would you like to play?");
                for (var i = 0; i < _hand.Count; i++)
                {
                    Console.WriteLine($"{i}) {_hand[i].PrintCard()}");
                }
                string? input;
                int selectedVal;
                do
                {
                    input = Console.ReadLine();
                } while (int.TryParse(input, out selectedVal) && (selectedVal > _hand.Count || selectedVal < 0));

                Card c = _hand[selectedVal];
                _hand.RemoveAt(selectedVal);

                if (facedown)
                    c.Flip();

                //TODO: Possible conflict of ordering. Does another player/card do their events before or after a card is played?
                _gm.PlayerPlayedCard(PlayerID, c);
                OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(c));

                return c;
            }
        }

        virtual public void TellAnotherPlayerToExecuteCommand(int targetID, Action<Player> command)
        {
            Player targetPlayer = _gm.GetPlayerByID(targetID);
            command(targetPlayer);
        }
        virtual public void ExecuteCommand(Action command)
        {
            command();
        }

        virtual public void ExecuteGameAction(IAction<Player> action)
        {
            action.execute(this);
        }

        virtual public void AddToPersonalDeck(Card c, string position = "bottom", bool shuffleDeckAfter = false)
        {
            if (_personalDeck != null)
                _personalDeck.AddCardToDeck(c, pos: position, shuffleAfter: shuffleDeckAfter);
            else
            {
                throw new NotSupportedException(message: "There is no personal deck to add to");
            }
        }

        protected void OnPlayerPlayedCard(PlayerPlayedCardEventArgs e)
        {
            var handler = PlayerPlayedCard;

            if (handler != null)
                handler(this, e);
        }

        protected void RaiseSimplePlayerMessageEvent(SimplePlayerMessageEvent e)
        {
            var handler = PlayerMessageEvent;
            if (handler != null)
                handler(this, e);
        }

        //Gets Flipped Card
        public Card FlipSingleCard(int cardNum, bool? facedown = null)
        {
            return _gm.FlipSingleCard(PlayerID, cardNum, facedown);
        }

        public List<Card> TakeAllCardsFromTable()
        {
            return _gm.PickUpAllCards_FromTable_FromPlayer(PlayerID);
        }

        //Returns what cards were added to the deck
        public List<Card> AddCardsToPersonalDeck
        (List<Card> cards, string position = "bottom", bool shuffleDeckAfter = false)
        {
            if (_personalDeck is not null) {
                _personalDeck?.AddMultipleCardsToDeck(cards, position, shuffleDeckAfter);
                return cards;
            }
            else
            {
                throw new NotSupportedException($"Player {PlayerID} doesn't have a personal deck");
            }
        }
    }
}
