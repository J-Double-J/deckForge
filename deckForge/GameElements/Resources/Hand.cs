using System.Collections;

namespace DeckForge.GameElements.Resources
{
    // TODO: Refactor this file at some point. Cards is a public property with other helper functions that do what
    // the list can already do.

    /// <summary>
    /// Resource Collection that manages a group of <see cref="ICard"/>s for a <see cref="IPlayer"/>.
    /// </summary>
    public class Hand : IResourceCollection<ICard>
    {
        private List<ICard> hand;
        private int maxHandSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hand"/> class.
        /// </summary>
        /// <param name="maxHandSize">Maximum number of cards for the hand. -1 for no hand limit.</param>
        public Hand(int maxHandSize = -1)
        {
            hand = new List<ICard>();
            this.maxHandSize = maxHandSize;
        }

        /// <summary>
        /// Gets or sets the list of Cards in the hand.
        /// </summary>
        public List<ICard> Cards
        {
            get { return hand; }
            protected set { hand = value; }
        }

        /// <summary>
        /// Gets or sets the maximum hand size.
        /// </summary>
        public int MaxHandSize
        {
            get { return maxHandSize; }
            set { maxHandSize = value; }
        }

        /// <summary>
        /// Gets the current hand size.
        /// </summary>
        public int CurrentHandSize
        {
            get { return hand.Count; }
        }

        /// <summary>
        /// Gets the number of cards in the hand.
        /// </summary>
        public int Count
        {
            get { return hand.Count; }
        }

        /// <summary>
        /// Gets the card at an index in the hand.
        /// </summary>
        /// <param name="pos">Gets the position of a card in a hand.</param>
        /// <returns>The <see cref="ICard"/> at that position.</returns>
        public ICard GetCardAt(int pos)
        {
            try
            {
                return hand[pos];
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Near other ResourceCollection methods")]
        public Type ResourceType
        {
            get { return typeof(ICard); }
        }

        // If already at hand limit, card is not added.

        /// <inheritdoc/>
        public void AddResource(ICard resource)
        {
            if (MaxHandSize < 0 || CurrentHandSize < MaxHandSize)
            {
                hand.Add(resource);
            }
        }

        /// <inheritdoc/>
        public void RemoveResource(ICard resource)
        {
            for (var i = 0; i < hand.Count; i++) {
                if (hand[i] == (ICard)resource)
                {
                    hand.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <inheritdoc/>
        public void AddMultipleResources(IList resources)
        {
            try
            {
                for (int i = 0; i < resources.Count; i++)
                {
                    AddResource((ICard)Convert.ChangeType(resources[i], typeof(ICard))!);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void DecrementResourceCollection()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ICard? GainResource()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void IncrementResourceCollection()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public List<ICard> ClearCollection()
        {
            List<ICard> cardsRemoved = new(hand);
            hand.Clear();

            return cardsRemoved;
        }
    }
}
