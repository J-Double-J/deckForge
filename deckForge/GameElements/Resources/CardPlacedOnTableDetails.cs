﻿using DeckForge.PlayerConstruction;

namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// For use with <see cref="Table"/> where it informs <see cref="ICard"/>s where they have been played
    /// without requiring an event for the <see cref="ICard"/> to subscribe to. This is an immutable dataclump.
    /// </summary>
    public class CardPlacedOnTableDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardPlacedOnTableDetails"/> class.
        /// </summary>
        /// <param name="playedBy">The <see cref="IPlayer"/> that played the <see cref="ICard"/>.</param>
        /// <param name="tablePlacementZone">Zone on table where the <see cref="ICard"/> was placed.</param>
        /// <param name="specificPlaceInZone">Specific spot in zone where the <see cref="ICard"/> was placed.</param>
        public CardPlacedOnTableDetails(
            TablePlacementZones tablePlacementZone,
            int specificPlaceInZone)
        {
            TablePlacementZone = tablePlacementZone;
            SpecificPlaceInZone = specificPlaceInZone;
        }

        /// <summary>
        /// Gets the enum value of where the <see cref="ICard"/> is placed.
        /// </summary>
        public TablePlacementZones TablePlacementZone { get; }

        /// <summary>
        /// Gets the value of where in the zone the <see cref="ICard"/> was placed.
        /// </summary>
        public int SpecificPlaceInZone { get; }
    }
}