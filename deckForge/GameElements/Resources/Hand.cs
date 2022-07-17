using System.Collections;

namespace deckForge.GameElements.Resources
{
    public class Hand : IResourceCollection<Card>
    {
        List<Card> hand;
        private int _maxHandSize;
        
        public Hand(int maxHandSize = -1) {
            hand = new List<Card>();
            _maxHandSize = maxHandSize;
        }

        public int MaxHandSize {
            get { return _maxHandSize; }
            set { _maxHandSize = value; }
        }

        public int CurrentHandSize {
            get { return hand.Count; }
        }

        public int Count {
            get { return hand.Count; }
        }

        public Card GetCardAt(int i) {
            try
            {
                return hand[i];
            }
            catch {
                throw;
            }
        }

        public Type ResourceType {
            get { return typeof(Card); }
        }

        //If already at hand limit, card is not added.
        public void AddResource(Card resource)
        {
            if (MaxHandSize < 0 || CurrentHandSize < MaxHandSize) {
                hand.Add((Card)resource);
            }
        }

        public void RemoveResource(Card resource)
        {
            for (var i = 0; i < hand.Count; i++) {
                if (hand[i] == (Card)resource) {
                    hand.RemoveAt(i);
                    i--;
                }
            }
        }

        public void AddMultipleResources(IList resources)
        {
            try
            {
                for (int i = 0; i < resources.Count; i++)
                {
                    AddResource((Card)Convert.ChangeType(resources[i], typeof(Card))!);
                }
            }
            catch
            {
                throw;
            }
        }

        public void DecrementResourceCollection()
        {
            throw new NotImplementedException();
        }

        public Card? GainResource()
        {
            throw new NotImplementedException();
        }

        public void IncrementResourceCollection()
        {
            throw new NotImplementedException();
        }

        public List<Card>? ClearCollection() {
            List<Card> cardsRemoved = hand;
            hand.Clear();

            return cardsRemoved;
        }
    }
}
