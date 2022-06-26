using deckForge.PhaseActions;
namespace deckForge.PlayerRoundRules
{
    public class RoundRules
    {
        int handLim;
        List<Phase> phases;


        public int HandLimit
        {
            get { return handLim; }
            private set
            {
                if (value >= 0)
                {
                    handLim = value;
                }
                else
                {
                    throw new ArgumentException("Hand Limit cannot be set to lower than 0.", "HandLimit");
                }
            }
        }
        public int CardPlayLimit { get; private set; }
        public List<Phase> Phases { get { return phases; } private set { phases = value; } }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public RoundRules(int handlimit = 64, int cardPlayLimit = 1, List<Phase>? phases = null)
        {
            HandLimit = handlimit;
            CardPlayLimit = cardPlayLimit;

            if (phases == null)
            {
                this.phases = new List<Phase>();
                Phase p = new(); //TODO: Seperate this out?
                this.phases.Add(p);
            }
            else
            {
                this.phases = phases;
            }
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    }
}
