using System;
using System.Collections.Generic;
using Basic;

namespace Interfaces
{
    public interface IPlayerHandController 
    {
        public Action<List<ICardView>> OnTakeCards { get; set; }
        //public Action<List<ICardController>> OnTakeCards { get; set; }
        public Action<CardActionType, ICardController> OnMadSellect { get; set; }
        public Action<CardActionType> OnMadUnsellect { get; set; }
        public Action<ICardController[]> OnDiscard { get; set; }
        public Action OnDiscardAll { get; set; }
    }
}
