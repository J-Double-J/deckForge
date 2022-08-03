﻿using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Draws a number of cards.
    /// </summary>
    public class DrawCardsAction : PlayerGameAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawCardsAction"/> class.
        /// </summary>
        /// <param name="drawCount">Number of <see cref="PlayingCard"/>s to draw.</param>
        /// <param name="name">Name of the <see cref="DrawCardsAction"/>.</param>
        public DrawCardsAction(int drawCount = 1, string name = "Draw")
        : base(name: name)
        {
            Name = name;
            DrawCount = drawCount;
            Description = $"Draw {drawCount} Card(s)";
        }

        /// <summary>
        /// Gets or sets the number of cards to be drawn.
        /// </summary>
        public int DrawCount { get; set; }

        /// <inheritdoc/>
        /// <returns>The list of cards that was drawn into the player's hand.</returns>
        public override List<PlayingCard?> Execute(IPlayer player)
        {
            List<PlayingCard?> cards = new();
            for (int i = 0; i < DrawCount; i++)
            {
                cards.Add(player.DrawCard());
            }

            return cards;
        }
    }
}
