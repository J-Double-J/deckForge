using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int HandSize {
            get { return hand.Count; }
        }

        public Card getCardAt(int i) {
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
            if (MaxHandSize < 0 || HandSize < MaxHandSize) {
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

        
    }
}
