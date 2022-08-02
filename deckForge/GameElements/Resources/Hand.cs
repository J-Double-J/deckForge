using System.Collections;

namespace DeckForge.GameElements.Resources
{
    public class Hand : IResourceCollection<PlayingCard>
    {
        List<PlayingCard> hand;
        private int _maxHandSize;
        
        public Hand(int maxHandSize = -1) {
            hand = new List<PlayingCard>();
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

        public PlayingCard GetCardAt(int i) {
            try
            {
                return hand[i];
            }
            catch {
                throw;
            }
        }

        public Type ResourceType {
            get { return typeof(PlayingCard); }
        }

        //If already at hand limit, card is not added.
        public void AddResource(PlayingCard resource)
        {
            if (MaxHandSize < 0 || CurrentHandSize < MaxHandSize) {
                hand.Add((PlayingCard)resource);
            }
        }

        public void RemoveResource(PlayingCard resource)
        {
            for (var i = 0; i < hand.Count; i++) {
                if (hand[i] == (PlayingCard)resource) {
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
                    AddResource((PlayingCard)Convert.ChangeType(resources[i], typeof(PlayingCard))!);
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

        public PlayingCard? GainResource()
        {
            throw new NotImplementedException();
        }

        public void IncrementResourceCollection()
        {
            throw new NotImplementedException();
        }

        public List<PlayingCard>? ClearCollection() {
            List<PlayingCard> cardsRemoved = hand;
            hand.Clear();

            return cardsRemoved;
        }
    }
}
