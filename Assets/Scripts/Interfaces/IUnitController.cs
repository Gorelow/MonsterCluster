using System;
using UnityEditor.Animations;
using UnityEngine;

namespace Interfaces
{
    public interface IUnitController 
    {
        public UnitData Data { get; }
        public AnimatorController Animation => Data.Animaton;
        public Sprite Image => Data.Image;
        public string Name => Data.Name;
        public string Description => Data.Description;
        public int Cost => Data.Cost;
        public int HP => Data.HP;
        public int Initiative => Data.Initiative;
        public int Speed => Data.Speed;
        public CardData[] Deck => Data.Deck;

        public int Health { get; }

        public Action OnDeath { get; set; }
        public Action<int> OnGettingDamage { get; set; }
        public Action<int> OnGettingHealth { get; set; }
        public Action OnClick { get; set; }
        public Action<Vector2> OnMove { get; set; }
    }
}
