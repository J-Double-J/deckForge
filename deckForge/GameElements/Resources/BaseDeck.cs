using System.Collections;

namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// Base class for all <see cref="IDeck"/>s.
    /// </summary>
    public abstract class BaseDeck : IDeck
    {
        // TODO: Not all decks should be shuffled on creation. Add option for this.
        private static readonly Random RNG = new();
        private readonly string defaultAddCardPos;
        private readonly bool defaultShuffleOnAddCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDeck"/> class.
        /// </summary>
        /// <param name="defaultAddCardPos">Specifies where to place a <see cref="ICard"/> by default. Options
        /// are "top", "middle", or "bottom". Default can be overriden when needed.</param>
        /// <param name="defaultShuffleOnAddCard">If<c>true</c>, shuffles the <see cref="IDeck"/> after adding
        /// any <see cref="ICard"/> by default. Default can be overriden when needed.</param>
        public BaseDeck(string defaultAddCardPos = "bottom", bool defaultShuffleOnAddCard = false)
        {
            Deck = new();
            CreateDeck();
            Shuffle();
            this.defaultAddCardPos = defaultAddCardPos;
            this.defaultShuffleOnAddCard = defaultShuffleOnAddCard;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDeck"/> class.
        /// </summary>
        /// <param name="cards">List of <see cref="ICard"/>s to use as the <see cref="IDeck"/>.</param>
        /// <param name="defaultAddCardPos">Specifies where to place a <see cref="ICard"/> by default. Options
        /// are "top", "middle", or "bottom". Default can be overriden when needed.</param>
        /// <param name="defaultShuffleOnAddCard">If<c>true</c>, shuffles the <see cref="IDeck"/> after adding
        /// any <see cref="ICard"/> by default. Default can be overriden when needed.</param>
        public BaseDeck(List<ICard> cards, string defaultAddCardPos = "bottom", bool defaultShuffleOnAddCard = false)
        {
            Deck = cards;
            this.defaultAddCardPos = defaultAddCardPos;
            this.defaultShuffleOnAddCard = defaultShuffleOnAddCard;
        }

        /// <summary>
        /// Gets the size of the <see cref="IDeck"/>.
        /// </summary>
        public int Count
        {
            get { return Deck.Count; }
        }

        /// <inheritdoc/>
        public Type ResourceType
        {
            get { return typeof(ICard); }
        }

        /// <summary>
        /// Gets or sets the <see cref="IDeck"/> (or list) of <see cref="ICard"/>s.
        /// </summary>
        public List<ICard> Deck { get; protected set; }

        /// <inheritdoc/>
        public ICard? DrawCard(bool drawFacedown = false)
        {
            if (Deck.Count != 0)
            {
                ICard c = Deck[Deck.Count - 1];
                Deck.RemoveAt(Deck.Count - 1);

                if (c.Facedown != drawFacedown)
                {
                    c.Flip();
                }

                return c;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public List<ICard?> DrawMultipleCards(int count)
        {
            List<ICard?> cards = new();
            for (int i = 0; i < count; i++)
            {
                ICard? c = DrawCard();
                if (c != null)
                {
                    cards.Add(c);
                }
                else
                {
                    break;
                }
            }

            return cards;
        }

        /// <inheritdoc/>
        public void Shuffle()
        {
            int n = Deck.Count;
            while (n > 1)
            {
                n--;
                int k = RNG.Next(n + 1);
                (Deck[n], Deck[k]) = (Deck[k], Deck[n]);
            }
        }

        /// <inheritdoc/>
        public void AddCardToDeck(ICard card, string pos = "bottom", bool shuffleAfter = false)
        {
            if (pos == "bottom")
            {
                Deck.Insert(0, card);
            }
            else if (pos == "top")
            {
                Deck.Add(card);
            }
            else if (pos == "middle")
            {
                Deck.Insert(Deck.Count / 2, card);
            }
            else if (int.TryParse(pos, out int numValue))
            {
                if (numValue >= 0 && numValue <= Deck.Count)
                {
                    Deck.Insert(numValue, card);
                }
                else if (numValue < 0)
                {
                    Deck.Insert(0, card);
                }
                else if (numValue > Deck.Count)
                {
                    Deck.Add(card);
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

        /// <inheritdoc/>
        public void AddMultipleCardsToDeck(List<ICard> cards, string pos = "bottom", bool shuffleAfter = false)
        {
            try
            {
                foreach (ICard c in cards)
                {
                    AddCardToDeck(c, pos: pos);
                }
            }
            catch
            {
                throw;
            }

            if (shuffleAfter == true)
            {
                Shuffle();
            }
        }

        /// <inheritdoc/>
        public void AddResource(ICard resource)
        {
            AddCardToDeck(resource, pos: defaultAddCardPos, shuffleAfter: defaultShuffleOnAddCard);
        }

        /// <inheritdoc/>
        public void RemoveResource(ICard resource)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Deck[i] == resource)
                {
                    Deck.Remove(Deck[i]);
                    i--; // Deck shrinks so this compensates for that
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
                    if (resources[i] as ICard is not null)
                    {
                        AddResource((ICard)resources[i]!);
                    }
                    else
                    {
                        throw new InvalidCastException("Cannot cast passed resources to correct type");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void IncrementResourceCollection()
        {
            throw new NotImplementedException("Deck can't increment the collection without a card specified");
        }

        /// <inheritdoc/>
        public void DecrementResourceCollection()
        {
            Deck.RemoveAt(Deck.Count - 1);
        }

        /// <inheritdoc/>
        public ICard? GainResource()
        {
            return DrawCard();
        }

        /// <inheritdoc/>
        public List<ICard>? ClearCollection()
        {
            List<ICard> cardsRemoved = Deck;
            Deck.Clear();

            return cardsRemoved;
        }

        /// <summary>
        /// Specifies how the <see cref="IDeck"/> should be created on construction.
        /// </summary>
        protected abstract void CreateDeck(); // TODO: Likely not needed.
    }
}
