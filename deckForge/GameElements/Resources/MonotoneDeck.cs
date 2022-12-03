namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// Creates a deck where all the cards are similar.
    /// </summary>
    public class MonotoneDeck : BaseDeck
    {
        private readonly Type cardType;
        private readonly int instancesCount;
        private readonly object?[]? typeConstructorParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonotoneDeck"/> class.
        /// </summary>
        /// <param name="cardType">Type of <see cref="ICard"/> that fills this <see cref="IDeck"/>.</param>
        /// <param name="count">Number of <see cref="ICard"/>s to create and fill this <see cref="IDeck"/>.</param>
        /// <param name="typeConstructorParams">Arguments to use when constructing <see cref="ICard"/>s, if needed.</param>
        /// <exception cref="ArgumentException">Throws if <paramref name="cardType"/> does not implement <see cref="ICard"/>
        /// or <paramref name="count"/> is negative.</exception>
        public MonotoneDeck(Type cardType, int count, object?[]? typeConstructorParams = null)
        {
            if (!cardType.GetInterfaces().Contains(typeof(ICard)))
            {
                throw new ArgumentException("cardType must implement ICard interface", nameof(cardType));
            }

            if (count < 0)
            {
                throw new ArgumentException("count cannot be negative", nameof(count));
            }

            this.cardType = cardType;
            this.typeConstructorParams = typeConstructorParams;
            instancesCount = count;
            CreateDeck();
            Shuffle();
        }

        /// <inheritdoc/>
        public override void AddCardToDeck(ICard card, string pos = "bottom", bool shuffleAfter = false)
        {
            if (card.GetType() != cardType)
            {
                throw new ArgumentException($"Card does not match other card types: {cardType}", nameof(card));
            }

            base.AddCardToDeck(card);
        }

        /// <inheritdoc/>
        protected override void CreateDeck()
        {
            for (int i = 0; i < instancesCount; i++)
            {
                var newCard = (ICard)Activator.CreateInstance(cardType, typeConstructorParams)!;
                Deck.Add(newCard);
            }
        }
    }
}
