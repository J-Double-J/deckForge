using CardNamespace;
using deckForge.GameConstruction;
using DeckNameSpace;

namespace deckForge.PlayerConstruction
{
    public class Player
    {
        private readonly GameMediator _gm;
        private Deck? _personalDeck;
        private int _cardPlays;
        private int _cardDraws;
        private List<Card> _hand = new();

        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;

        public Player(GameMediator gm, int playerID = 0, int initHandSize = 5, Deck? personalDeck = null )
        {
            _gm = gm;

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

        public int PersonalDeckSize {
            get {
                if (_personalDeck != null) {
                    return _personalDeck.Size;
                } else {
                    return 0;
                }
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

        virtual public void DrawCard()
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
        }

        virtual public void PlayCard(bool facedown = false)
        {
            //TODO: Remove
            if (_hand.Count == 0)
            {
                _gm.EndGame();
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

                //TODO: Possible conflict of ordering. Does another player/card do their events before or after a card is played?
                _gm.PlayerPlayedCard(c, facedown);
                OnRaisePlayerPlayedCard(new PlayerPlayedCardEventArgs(c));
            }
        }

        virtual protected void OnRaisePlayerPlayedCard(PlayerPlayedCardEventArgs e)
        {
            var handler = PlayerPlayedCard;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        virtual public void TellAnotherPlayerToExecuteCommand(int targetID, Action<Player> command) {
            Player targetPlayer = _gm.GetPlayerByID(targetID);
            command(targetPlayer);
        }
        virtual public void ExecuteCommand(Action command)
        {
            command();
        }

        virtual public void AddToPersonalDeck(Card c) {
            if (_personalDeck != null)
                _personalDeck.AddCardToDeck(c);
            else {
                throw new ArgumentException(message: "There is no personal deck to add to");
            }
        }
    }
}
