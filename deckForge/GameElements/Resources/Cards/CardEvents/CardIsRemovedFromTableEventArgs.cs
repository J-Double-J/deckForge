namespace DeckForge.GameElements.Resources.Cards.CardEvents
{
    /// <summary>
    /// <see cref="EventArgs"/> raised for when a <see cref="ICard"/> is informing the <see cref="Table"/> that it needs to be removed.
    /// </summary>
    public class CardIsRemovedFromTableEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardIsRemovedFromTableEventArgs"/> class.
        /// </summary>
        /// <param name="placementDetails">Details of where the <see cref="ICard"/> is currently when being removed.</param>
        public CardIsRemovedFromTableEventArgs(CardPlacedOnTableDetails placementDetails)
        {
            PlacementDetails = placementDetails;
        }

        /// <summary>
        /// Gets the details of where the <see cref="ICard"/> is located on the <see cref="Table"/>.
        /// </summary>
        public CardPlacedOnTableDetails PlacementDetails { get; }
    }
}
