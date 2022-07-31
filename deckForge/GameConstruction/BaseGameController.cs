using DeckForge.PlayerConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameConstruction;

namespace DeckForge.GameConstruction
{
    public class BaseGameController : IGameController
    {
        private int _playerCount;
        private readonly TurnHandler _turnHandler;

        public BaseGameController(int playerCount, bool turnRandomizer = false)
        {
            PlayerCount = playerCount;
            _turnHandler = new TurnHandler(playerCount, turnRandomizer);
        }

        public int PlayerCount
        {
            get { return _playerCount; }
            private set { _playerCount = value; }
        }

        public List<int> TurnOrder {
            get { return _turnHandler.TurnOrder; }
            private set { TurnOrder = value; }
        }

        public int NextPlayerTurn()
        {
            //TODO: Inter-Round Rules Abstraction (Game win? Shuffle Pieces? etc)
            _turnHandler.incrementTurnOrder();
            return _turnHandler.GetWhoseTurn();
        }

        public void UpdatePlayerList(List<int> newPlayerList) {
            _turnHandler.UpdatePlayerList(newPlayerList);
        }

        public int PlayerTurnXTurnsFromNow(int turns = 1)
        {
            return _turnHandler.GetWhoseTurnXTurnsFromNow(turns);
        }

        public void ShiftTurnOrderClockwise() {
            _turnHandler.ShiftTurnOrderClockwise();
        }

        public void ShiftTurnOrderCounterClockwise()
        {
            _turnHandler.ShiftTurnOrderCounterClockwise();
        }

        public int GetCurrentPlayer()
        {
            return _turnHandler.GetWhoseTurn();
        }

        public void EndGame()
        {
            Console.WriteLine("You have emptied your hand. Congrats! No logical flaws were found.");
            Environment.Exit(0);
        }
    }
}

public class TurnHandler : ITurnHandler
{
    private readonly Random _rng = new();
    private List<int> _order = new();
    private int _turnNum;

    public TurnHandler(int playerCount, bool turnRandomizer)
    {
        for (var i = 0; i < playerCount; i++)
        {
            _order.Add(i);
        }
        if (turnRandomizer)
        {
            var n = playerCount;
            while (n > 1)
            {
                n--;
                int k = _rng.Next(n + 1);
                int value = _order[k];
                _order[k] = _order[n];
                _order[n] = value;
            }
        }
    }

    public int TurnNum
    {
        get { return _turnNum; }
        private set { _turnNum = value; }
    }

    public List<int> TurnOrder
    {
        get { return _order; }
        private set { _order = value; }
    }

    public void ShiftTurnOrderClockwise()
    {
        _order = _order.Skip(1).Concat(_order.Take(1)).ToList();
    }

    public void ShiftTurnOrderCounterClockwise()
    {
        _order = _order.Skip(_order.Count - 1).Concat(_order.Take(_order.Count - 1)).ToList();
    }

    public void incrementTurnOrder()
    {
        _turnNum++;
    }

    public void UpdatePlayerList(List<int> newPlayerList)
    {
        _order = _order.Intersect(newPlayerList).ToList();
    }

    public int GetWhoseTurn()
    {
        return _turnNum % _order.Count;
    }

    public int GetWhoseTurnXTurnsFromNow(int turns)
    {
        return (_turnNum + turns) % _order.Count;
    }
}
