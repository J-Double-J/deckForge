using System.Collections;

namespace DeckForge.GameElements.Resources
{
    public class DeckOfPlayingCards : IResourceCollection<PlayingCard>
    {
        private List<PlayingCard> deck = new List<PlayingCard>();
        private string _defaultAddCardPos;
        private bool _defaultShuffleOnAddCard;

        public DeckOfPlayingCards() {
            createDeck();
            Shuffle();
            _defaultAddCardPos = "bottom";
            _defaultShuffleOnAddCard = false;
        }

        public DeckOfPlayingCards(string defaultAddCardPos = "bottom", bool defaultShuffleOnAddCard = false)
        {
            createDeck();
            Shuffle();
            _defaultAddCardPos = defaultAddCardPos;
            _defaultShuffleOnAddCard = defaultShuffleOnAddCard;
        }

        public DeckOfPlayingCards(List<PlayingCard> cards, string defaultAddCardPos = "bottom", bool defaultShuffleOnAddCard = false)
        {
            deck = cards;
            _defaultAddCardPos = defaultAddCardPos;
            _defaultShuffleOnAddCard = defaultShuffleOnAddCard;
        }

        public PlayingCard? DrawCard(bool drawFacedown = false)
        {
            if (deck.Count != 0)
            {
                PlayingCard c = deck[deck.Count - 1];
                deck.RemoveAt(deck.Count - 1);

                if (c.Facedown != drawFacedown)
                    c.Flip();
                return c;
            }
            else
            {
                return null;
            }
        }

        public List<PlayingCard>? DrawMultipleCards(int count)
        {
            List<PlayingCard>? cards = new();
            for (int i = 0; i < count; i++)
            {
                PlayingCard? c = DrawCard();
                if (c != null)
                    cards.Add(c);
                else
                    break;
            }

            return cards;
        }

        private static Random rng = new Random();

        public void Shuffle()
        {
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                PlayingCard value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        public int Count
        {
            get { return deck.Count; }
        }

        //Simple 52 Card Deck for examples
        private void createDeck()
        {
            for (int i = 1; i < 14; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    switch (j)
                    {
                        case 0:
                            deck.Add(new PlayingCard(i, "H"));
                            break;
                        case 1:
                            deck.Add(new PlayingCard(i, "D"));
                            break;
                        case 2:
                            deck.Add(new PlayingCard(i, "S"));
                            break;
                        case 3:
                            deck.Add(new PlayingCard(i, "C"));
                            break;
                    }
                }
            }
        }

        public void AddCardToDeck(PlayingCard card, string pos = "bottom", bool shuffleAfter = false)
        {
            if (pos == "bottom")
            {
                deck.Insert(0, card);
            }
            else if (pos == "top")
            {
                deck.Add(card);
            }
            else if (pos == "middle")
            {
                deck.Insert(deck.Count / 2, card);
            }

            else if (int.TryParse(pos, out int numValue))
            {
                if (numValue >= 0 && numValue <= deck.Count)
                {
                    deck.Insert(numValue, card);
                }
                else if (numValue < 0)
                {
                    deck.Insert(0, card);
                }
                else if (numValue > deck.Count)
                {
                    deck.Add(card);
                }
            }
            else
            {
                throw new ArgumentException($"Invalid pos: '{pos}', for a card to be placed in the deck");
            }

            if (shuffleAfter == true)
            {
                Shuffle();
            }
        }

        public void AddMultipleCardsToDeck(List<PlayingCard> cards, string pos = "bottom", bool shuffleAfter = false)
        {
            try
            {
                foreach (PlayingCard c in cards)
                {
                    AddCardToDeck(c, pos: pos);
                }
            }
            catch
            {
                throw;
            }

            if (shuffleAfter == true)
                Shuffle();
        }

        public Type ResourceType
        {
            get { return typeof(PlayingCard); }
        }

        public void AddResource(PlayingCard resource)
        {
            AddCardToDeck(resource, pos: _defaultAddCardPos, shuffleAfter: _defaultShuffleOnAddCard);
        }

        public void RemoveResource(PlayingCard resource)
        {
            for (int i = 0; i < Count; i++) {
                if (deck[i] == resource) {
                    deck.Remove(deck[i]);
                    i--; //Deck shrinks so this compensates for that
                }
            }
        }

        public void AddMultipleResources(IList resources) {
            try
            {
                for (int i = 0; i < resources.Count; i++)
                {
                    AddResource((PlayingCard)Convert.ChangeType(resources[i], typeof(PlayingCard))!);
                }
            }
            catch {
                throw;
            }
        }

        public void IncrementResourceCollection()
        {
            throw new NotImplementedException("Deck can't increment the collection without a card specified");
        }

        public void DecrementResourceCollection()
        {
            deck.RemoveAt(deck.Count - 1);
        }

        public PlayingCard? GainResource()
        {
            return DrawCard();
        }

        public List<PlayingCard>? ClearCollection()
        {
            List<PlayingCard> cardsRemoved = deck;
            deck.Clear();

            return cardsRemoved;
        }
    }
}



