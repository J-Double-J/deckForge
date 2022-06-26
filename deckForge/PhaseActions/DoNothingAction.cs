using PlayerNamespace;

namespace deckForge.PhaseActions
{
    public class DoNothingAction : GameAction
    {
        public override string Name { get { return "Do Nothing"; } }

        public override string Description { get { return "Do Nothing"; }
}

        public override void execute(Player p) { }
    }
}
