﻿using DeckForge.PlayerConstruction;
using DeckForge.GameElements.Resources;

namespace DeckForge.PhaseActions
{
    public class DrawCardsAction : PlayerGameAction
    {
        public int DrawCount { get; }

        public DrawCardsAction(int drawCount = 1, string name = "Draw")
        : base(name: name)
        {
            Name = name;
            DrawCount = drawCount;
            Description = $"Draw {drawCount} Card(s)";
        }

        //Returns the list of cards that was drawn into the player's hand
        override public List<Card?> Execute(IPlayer player)
        {
            List<Card?> cards = new();
            for (int i = 0; i < DrawCount; i++)
            {
                cards.Add(player.DrawCard());
            }

            return cards;
        }
    }
}
