using System;
using Basic;

namespace Interfaces
{
    public interface ICardComboController
    {
        public event Action<CardActionType, ICardController> OnTypeCardChange;
        public event Action<bool> OnChangeActive;
        public event Action<bool> OnAvailability;
        public event Action<int> OnChangeComboPrice;
    }
}