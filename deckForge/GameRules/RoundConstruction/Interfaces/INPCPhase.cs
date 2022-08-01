namespace DeckForge.GameRules.RoundConstruction.Interfaces
{
    public interface INPCPhase<T>
    {
        public void StartPhase(T t);    
        
        public void EndPhase();
    }
}
