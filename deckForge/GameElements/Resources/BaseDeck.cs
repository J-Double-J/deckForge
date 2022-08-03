using System.Collections;

namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// Base class for all <see cref="IDeck"/>s.
    /// </summary>
    public abstract class BaseDeck : IDeck
    {
        private static readonly Random RNG = new();
        private readonly string defaultAddCardPos;
        private readonly bool defaultShuffleOnAddCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDeck"/> class.
        /// </summary>
        /// <param name="defaultAddCardPos">Specifies where to place a <see cref="PlayingCard"/> by default. Options
        /// are "top", "middle", or "bottom". Default can be overriden when needed.</param>
        /// <param name="defaultShuffleOnAddCard">If<c>true</c>, shuffles the <see cref="IDeck"/> after adding
        /// any <see cref="PlayingCard"/> by default. Default can be overriden when needed.</param>
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
        /// <param name="cards">List of <see cref="PlayingCard"/>s to use as the <see cref="IDeck"/>.</param>
        /// <param name="defaultAddCardPos">Specifies where to place a <see cref="PlayingCard"/> by default. Options
        /// are "top", "middle", or "bottom". Default can be overriden when needed.</param>
        /// <param name="defaultShuffleOnAddCard">If<c>true</c>, shuffles the <see cref="IDeck"/> after adding
        /// any <see cref="PlayingCard"/> by default. Default can be overriden when needed.</param>
        public BaseDeck(List<PlayingCard> cards, string defaultAddCardPos = "bottom", bool defaultShuffleOnAddCard = false)
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
            get { return typeof(PlayingCard); }
        }

        /// <summary>
        /// Gets or sets the <see cref="IDeck"/> (or list) of <see cref="PlayingCard"/>s.
        /// </summary>
        protected List<PlayingCard> Deck { get; set; }

        /// <inheritdoc/>
        public PlayingCard? DrawCard(bool drawFacedown = false)
        {
            if (Deck.Count != 0)
            {
                PlayingCard c = Deck[Deck.Count - 1];
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
        public List<PlayingCard?> DrawMultipleCards(int count)
        {
            List<PlayingCard?> cards = new();
            for (int i = 0; i < count; i++)
            {
                PlayingCard? c = DrawCard();
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
        public void AddCardToDeck(PlayingCard card, string pos = "bottom", bool shuffleAfter = false)
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
            {
                Shuffle();
            }
        }

        /// <inheritdoc/>
        public void AddResource(PlayingCard resource)
        {
            AddCardToDeck(resource, pos: defaultAddCardPos, shuffleAfter: defaultShuffleOnAddCard);
        }

        /// <inheritdoc/>
        public void RemoveResource(PlayingCard resource)
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
                    AddResource((PlayingCard)Convert.ChangeType(resources[i], typeof(PlayingCard))!);
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
        public PlayingCard? GainResource()
        {
            return DrawCard();
        }

        /// <inheritdoc/>
        public List<PlayingCard>? ClearCollection()
        {
            List<PlayingCard> cardsRemoved = Deck;
            Deck.Clear();

            return cardsRemoved;
        }

        /// <summary>
        /// Specifies how the <see cref="IDeck"/> should be created on construction.
        /// </summary>
        protected abstract void CreateDeck(); // TODO: Likely not needed.
    }
}
