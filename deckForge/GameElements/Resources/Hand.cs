using System.Collections;

namespace DeckForge.GameElements.Resources
{
    // TODO: Refactor this file at some point. Cards is a public property with other helper functions that do what
    // the list can already do.

    /// <summary>
    /// Resource Collection that manages a group of <see cref="PlayingCard"/>s for a <see cref="IPlayer"/>.
    /// </summary>
    public class Hand : IResourceCollection<PlayingCard>
    {
        private List<PlayingCard> hand;
        private int maxHandSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hand"/> class.
        /// </summary>
        /// <param name="maxHandSize">Maximum number of cards for the hand. -1 for no hand limit.</param>
        public Hand(int maxHandSize = -1)
        {
            hand = new List<PlayingCard>();
            this.maxHandSize = maxHandSize;
        }

        /// <summary>
        /// Gets or sets the list of Cards in the hand.
        /// </summary>
        public List<PlayingCard> Cards
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
        /// <returns>The <see cref="PlayingCard"/> at that position.</returns>
        public PlayingCard GetCardAt(int pos)
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
            get { return typeof(PlayingCard); }
        }

        // If already at hand limit, card is not added.

        /// <inheritdoc/>
        public void AddResource(PlayingCard resource)
        {
            if (MaxHandSize < 0 || CurrentHandSize < MaxHandSize)
            {
                hand.Add((PlayingCard)resource);
            }
        }

        /// <inheritdoc/>
        public void RemoveResource(PlayingCard resource)
        {
            for (var i = 0; i < hand.Count; i++) {
                if (hand[i] == (PlayingCard)resource)
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
                    AddResource((PlayingCard)Convert.ChangeType(resources[i], typeof(PlayingCard))!);
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
        public PlayingCard? GainResource()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void IncrementResourceCollection()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public List<PlayingCard>? ClearCollection()
        {
            List<PlayingCard> cardsRemoved = hand;
            hand.Clear();

            return cardsRemoved;
        }
    }
}
