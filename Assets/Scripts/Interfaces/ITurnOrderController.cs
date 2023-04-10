using System;

namespace Interfaces
{
    public interface ITurnOrderController 
    {
        public Action<int> OnUnitDeath { get; set; }
        public Action<IUnitController> OnTurnEnd { get; set; }
        public Action<IUnitController> OnNextTurn { get; set; }
        public Action<int> OnNextTurnNumberChange { get; set; }

        public IUnitController Order(int index);
    }
}
