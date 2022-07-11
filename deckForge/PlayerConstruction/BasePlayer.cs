using deckForge.GameConstruction;
using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.PhaseActions;
using deckForge.GameElements.Resources;

namespace deckForge.PlayerConstruction
{
    public class BasePlayer : IPlayer
    {
        protected readonly IGameMediator GM;
        protected Deck? PersonalDeck;
        protected int CardPlays;
        protected int CardDraws;
        protected int InitHandSize;
        protected Hand PlayerHand; 
        protected List<IResourceCollection> PlayerResourceCollections;

        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;
        public event EventHandler<SimplePlayerMessageEvent>? PlayerMessageEvent;

        public BasePlayer(IGameMediator gm, int playerID = 0, int initHandSize = 5, Deck? personalDeck = null)
        {
            PlayerHand = new();
            //PlayerHand is not considered part of PlayerResourceCollections despite being a collection
            PlayerResourceCollections = new List<IResourceCollection>();

            GM = gm;
            PersonalDeck = personalDeck;
            PlayerID = playerID;

            //TODO: remove this is for sake of testing.
            CardPlays = 1;
            CardDraws = 1;

            InitHandSize = initHandSize;
        }

        public int HandSize
        {
            get { return PlayerHand.HandSize; }
        }

        public int PlayerID
        {
            get;
        }

        public int PersonalDeckSize
        {
            get
            {
                if (PersonalDeck != null)
                {
                    return PersonalDeck.Size;
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
                return GM.GetPlayedCardsOfPlayer(PlayerID);
            }
        }

        virtual public void DrawStartingHand() {
            for (var i = 0; i < InitHandSize; i++)
            {
                DrawCard();
            }
        }

        virtual public void StartTurn()
        {
            for (var i = 0; i < CardDraws; i++)
            {
                DrawCard();
            }
            for (var j = 0; j < CardPlays; j++)
            {
                PlayCard();
            }
            GM.EndPlayerTurn();
        }

        //Returns which card was drawn
        virtual public Card? DrawCard()
        {
            Card? c = GM.DrawCardFromDeck();
            if (c != null)
            {
                PlayerHand.AddResource(c);
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
            if (HandSize == 0)
            {
                GM.EndGame();
                return null;
            }
            else
            {
                Console.WriteLine("Which card would you like to play?");
                for (var i = 0; i < HandSize; i++)
                {
                    Console.WriteLine($"{i}) {PlayerHand.getCardAt(i).PrintCard()}");
                }
                string? input;
                int selectedVal;
                do
                {
                    input = Console.ReadLine();
                } while (int.TryParse(input, out selectedVal) && (selectedVal > HandSize || selectedVal < 0));

                Card c = PlayerHand.getCardAt(selectedVal);
                PlayerHand.RemoveResource(c);

                if (facedown)
                    c.Flip();

                //TODO: Possible conflict of ordering. Does another player/card do their events before or after a card is played?
                GM.PlayerPlayedCard(PlayerID, c);
                OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(c));

                return c;
            }
        }

        virtual public void TellAnotherPlayerToExecuteCommand(int targetID, Action<IPlayer> command)
        {
            IPlayer targetPlayer = GM.GetPlayerByID(targetID);
            command(targetPlayer);
        }
        virtual public void ExecuteCommand(Action command)
        {
            command();
        }

        virtual public void ExecuteGameAction(IAction<IPlayer> action)
        {
            action.execute(this);
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
            return GM.FlipSingleCard(PlayerID, cardNum, facedown);
        }

        public List<Card> TakeAllCardsFromTable()
        {
            return GM.PickUpAllCards_FromTable_FromPlayer(PlayerID);
        }

        //Returns -1 if not found
        public virtual int FindCorrectPoolID(IResource resource)
        {
            int resourcePoolID = -1;
            for (var i = 0; i < PlayerResourceCollections.Count; i++)
            {
                if (PlayerResourceCollections[i].ResourceType == resource.GetType()) {
                    resourcePoolID = i;
                    break;
                }
            }
            return resourcePoolID;
        }

        public virtual void AddResourceToPool(int ID, IResource resource) {
            try
            {
                if (ID >= 0 || ID < PlayerResourceCollections.Count)
                {
                    if (PlayerResourceCollections[ID].ResourceType == resource.GetType())
                    {
                        IResourceCollection rs = PlayerResourceCollections[ID];
                        var casting = (IResourceCollection<object>)rs;
                        casting.AddResource(resource);
                    }
                    else {
                        throw new ArgumentException($"Resource is of Type {resource.GetType()} and cannot be put into a collection of Type ${PlayerResourceCollections[ID].ResourceType}");
                    }
                }
                else {
                    throw new ArgumentOutOfRangeException("ID is out of range of PlayerResourceCollections");
                }
            }
            catch {
                throw;
            }
        }

        public void AddPlayerResourceCollection(IResourceCollection collection) {
            PlayerResourceCollections.Add(collection);
        }

        public Object? TakeResourceFromCollection(int resourceCollectionID)
        {
            try
            {
                IResourceCollection rs = PlayerResourceCollections[resourceCollectionID];
                var casting = (IResourceCollection<object>)rs; 
                return casting.GainResource();
            }
            catch {
                throw;
            }

        }
    }

    public class BasePlayer_WithPersonalDeck : BasePlayer, IPlayer_WithPersonalDeck
    {
        new Deck _personalDeck;
        public BasePlayer_WithPersonalDeck(IGameMediator _gm, int playerID, int initHandSize, Deck personalDeck) :
        base(_gm, playerID, initHandSize)
        {
            _personalDeck = personalDeck;
        }

        //Return what card was added to the deck
        public Card AddCardToPersonalDeck(Card c, string position = "bottom", bool shuffleDeckAfter = false)
        {
            if (_personalDeck != null)
            {
                _personalDeck.AddCardToDeck(c, pos: position, shuffleAfter: shuffleDeckAfter);
                return c;
            }
            else
            {
                throw new NotSupportedException(message: "There is no personal deck to add to");
            }
        }

        //Returns what cards were added to the deck
        public List<Card> AddCardsToPersonalDeck
        (List<Card> cards, string position = "bottom", bool shuffleDeckAfter = false)
        {
            if (_personalDeck is not null)
            {
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
