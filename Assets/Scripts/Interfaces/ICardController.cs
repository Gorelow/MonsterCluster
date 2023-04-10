using System;
using Basic;
using UnityEngine;

namespace Interfaces
{
    public interface ICardController 
    {
        public Action OnSelect { get; set; }
        public Action OnUnselect { get; set; }
        public Action OnDiscard { get; set; }
        public Action OnMouseHover { get; set; }
        public Action OnMouseStopHover { get; set; }

        public CardData Data { get; }

        public string Name => Data.Name;
        public Sprite Sprite => Data.Sprite;
        public string Description => Data.Description;
        public int Cost => Data.Cost;

        public CardActionType ActionType => Data.ActionType;
        public CardElementType Element => Data.Element;
        public CardElementType ElementOfBonus => Data.ElementOfBonus;

        public AimGroups AimGroup => Data.AimGroup;
        public Amount AimAmount => Data.AimAmount;
        public DebuffType DebuffCondition => Data.DebuffCondition;
        public int Range => Data.CalculatedRange;

        public int Damage => Data.Attack.Damage;
        public DebuffType Debuff => Data.Debuff.DebuffType;
        public int DebuffStrength => Data.Debuff.Strength;
        public int DebuffDuration => Data.Debuff.Duration;

    }
}
