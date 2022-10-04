using System.Collections;

namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// A <see cref="IDeck"/> of standard 52 Playing Cards (excludes Jokers).
    /// </summary>
    public class DeckOfPlayingCards : BaseDeck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckOfPlayingCards"/> class.
        /// </summary>
        public DeckOfPlayingCards()
            : base(defaultAddCardPos: "bottom", defaultShuffleOnAddCard: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckOfPlayingCards"/> class.
        /// </summary>
        /// <param name="defaultAddCardPos">Default position to add any cards to this <see cref="IDeck"/>.</param>
        /// <param name="defaultShuffleOnAddCard">Default option to shuffle the <see cref="IDeck"/> after any <see cref="ICard"/> is added.</param>
        public DeckOfPlayingCards(string defaultAddCardPos = "bottom", bool defaultShuffleOnAddCard = false)
            : base(defaultAddCardPos: defaultAddCardPos, defaultShuffleOnAddCard: defaultShuffleOnAddCard)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckOfPlayingCards"/> class.
        /// </summary>
        /// <param name="cards">List of <see cref="Card"/>s to create the <see cref="IDeck"/> from.</param>
        /// <param name="defaultAddCardPos">Default position to add any cards to this <see cref="IDeck"/>.</param>
        /// <param name="defaultShuffleOnAddCard">Default option to shuffle the <see cref="IDeck"/> after any <see cref="ICard"/> is added.</param>
        public DeckOfPlayingCards(List<PlayingCard> cards, string defaultAddCardPos = "bottom", bool defaultShuffleOnAddCard = false)
            : base(cards: cards.ConvertAll(c => (ICard)c), defaultAddCardPos: defaultAddCardPos, defaultShuffleOnAddCard: defaultShuffleOnAddCard)
        {
        }

        /// <inheritdoc/>
        protected override void CreateDeck()
        {
            for (int i = 1; i < 14; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    switch (j)
                    {
                        case 0:
                            Deck.Add(new PlayingCard(i, "H"));
                            break;
                        case 1:
                            Deck.Add(new PlayingCard(i, "D"));
                            break;
                        case 2:
                            Deck.Add(new PlayingCard(i, "S"));
                            break;
                        case 3:
                            Deck.Add(new PlayingCard(i, "C"));
                            break;
                    }
                }
            }
        }
    }
}
