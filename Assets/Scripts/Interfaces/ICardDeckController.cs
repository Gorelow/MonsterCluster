using System;

namespace Interfaces
{
    public interface ICardDeckController 
    {
        public Action<int> OnUpdateDrawPileAmount { get; set; }
        public Action<int> OnUpdateDiscardPileAmount { get; set; }
    }
}
