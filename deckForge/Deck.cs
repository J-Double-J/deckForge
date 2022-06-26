using CardNamespace;

namespace DeckNameSpace{
    public class Deck
    {
        public List<Card> deck = new List<Card>();
        public Deck()
        {
            createDeck();
            Shuffle();
        }

        public Card? DrawCard()
        {
            if (deck.Count != 0)
            {
                Card c = deck[deck.Count - 1];
                deck.RemoveAt(deck.Count - 1);
                return c;
            }
            else
            {
                return null;
            }
        }

        public List<Card>? DrawMultipleCards(int count) {
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

        public int Size() {
            return deck.Count;
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

    }
}



