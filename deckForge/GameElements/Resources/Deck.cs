namespace deckForge.GameElements.Resources
{
    public class Deck
    {
        public List<Card> deck = new List<Card>();
        public Deck()
        {
            createDeck();
            Shuffle();
        }

        public Deck(List<Card> cards)
        {
            deck = cards;
        }

        public Card? DrawCard(bool drawFacedown = false)
        {
            if (deck.Count != 0)
            {
                Card c = deck[deck.Count - 1];
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

        public List<Card>? DrawMultipleCards(int count)
        {
            List<Card>? cards = new();
            for (int i = 0; i < count; i++)
            {
                Card? c = DrawCard();
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
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        public int Size
        {
            get { return deck.Count; }
        }

        //Simple 52 Card Deck for examples
        void createDeck()
        {
            for (int i = 1; i < 14; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    switch (j)
                    {
                        case 0:
                            deck.Add(new Card(i, "H"));
                            break;
                        case 1:
                            deck.Add(new Card(i, "D"));
                            break;
                        case 2:
                            deck.Add(new Card(i, "S"));
                            break;
                        case 3:
                            deck.Add(new Card(i, "C"));
                            break;
                    }
                }
            }
        }

        public void AddCardToDeck(Card card, string pos = "bottom", bool shuffleAfter = false)
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

        public void AddMultipleCardsToDeck(List<Card> cards, string pos = "bottom", bool shuffleAfter = false)
        {
            try
            {
                foreach (Card c in cards)
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
    }
}



