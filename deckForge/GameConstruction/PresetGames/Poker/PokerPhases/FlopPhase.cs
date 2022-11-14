using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PhaseActions;

namespace DeckForge.GameConstruction.PresetGames.Poker.PokerPhases
{
    public class FlopPhase : BasePhase
    {
        public FlopPhase(PokerGameMediator pGM)
        : base(pGM, "Flop Phase")
        {
            Actions.Add(new DealCardsFromTableDeckToTable(
                pGM,
                3,
                GameElements.Table.TablePlacementZoneType.NeutralZone,
                GameElements.Table.TablePlacementZoneType.NeutralZone,
                faceup: true));
        }
    }
}