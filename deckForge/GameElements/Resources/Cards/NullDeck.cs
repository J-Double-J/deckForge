using System.Collections;

namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// A <see cref="IDeck"/> of nothing. Gives <see cref="NullCard"/>s if drawn from.
    /// </summary>
    public class NullDeck : IDeck
    {
        /// <inheritdoc/>
        public Type ResourceType
        {
            get { return typeof(NullCard); }
        }

        /// <inheritdoc/>
        public int Count
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        public List<ICard> Deck
        {
            get
            {
                return new List<ICard>();
            }
        }

        /// <inheritdoc/>
        public void AddCardToDeck(ICard card, string pos = "bottom", bool shuffleAfter = false)
        {
        }

        /// <inheritdoc/>
        public void AddMultipleCardsToDeck(List<ICard> cards, string pos = "bottom", bool shuffleAfter = false)
        {
        }

        /// <inheritdoc/>
        public void AddMultipleResources(IList resource)
        {
        }

        /// <inheritdoc/>
        public void AddResource(ICard resource)
        {
        }

        /// <inheritdoc/>
        public List<ICard>? ClearCollection()
        {
            return null;
        }

        /// <inheritdoc/>
        public void DecrementResourceCollection()
        {
        }

        /// <inheritdoc/>
        public ICard? DrawCard(bool drawFacedown = false)
        {
            return new NullCard();
        }

        public List<ICard?> DrawMultipleCards(int count)
        {
            return new List<ICard?>() { new NullCard() };
        }

        /// <inheritdoc/>
        public ICard? GainResource()
        {
            return new NullCard();
        }

        /// <inheritdoc/>
        public void IncrementResourceCollection()
        {
        }

        /// <inheritdoc/>
        public void RemoveResource(ICard resource)
        {
        }

        /// <inheritdoc/>
        public void Shuffle()
        {
        }
    }
}
