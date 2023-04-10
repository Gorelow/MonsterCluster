using System;
using System.Collections;
using System.Collections.Generic;
using Basic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Card Data", menuName = "Card data")]
public class CardData : ScriptableObject, IDisposable
{
    [Header("Base")]
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _description;
    [Range(1, 5)]
    [SerializeField] private int _cost;

    [Header("View")]
    [SerializeField] private CardActionType _actionType;
    [SerializeField] private CardElementType _element;
    [SerializeField] private CardElementType _elementOfBonus;

    [Header("Aim")]
    [SerializeField] private AimGroups _aimGroup;
    [SerializeField] private Amount _aimAmount;
    [SerializeField] private DebuffType _debuffCondition;
    [Range(0, 7)]
    [SerializeField] private int _range;
    [Range(0, 3)]
    [SerializeField] private int _rangeBonus;

    [Header("Effect")]
    [SerializeField] private Attack _attack;
    [SerializeField] private Debuff _debuff;

    private int _bonus;

    public int Bonus => _bonus;
    public void SetBonus(int amount)
    {
        _bonus = amount;
        Attack.SetBonus(amount);
        Debuff.SetBonus(amount);
    }
    public int CalculatedRange => _range + _bonus * _rangeBonus;

    public string Name => _name;
    public Sprite Sprite => _sprite;
    public string Description => _description;
    public int Cost => _cost;

    public CardActionType ActionType => _actionType;
    public CardElementType Element => _element;
    public CardElementType ElementOfBonus => _elementOfBonus;

    public AimGroups AimGroup => _aimGroup;
    public Amount AimAmount => _aimAmount;
    public DebuffType DebuffCondition => _debuffCondition;
    public int Range => _range;
    public int RangeBonus => _rangeBonus;

    public Attack Attack => _attack;
    public Debuff Debuff => _debuff;

    public void Dispose()
    {
        //throw new NotImplementedException();
    }
}

[Serializable]
public struct Attack
{
    [SerializeField] private int _damage;
    [SerializeField] private int _damageBonus;

    private int _bonus;

    public int BaseDamage => _damage;
    public int DamageBonus => _damageBonus;

    public int Damage => _damage + _damageBonus * _bonus;

    public void SetBonus(int amount) => _bonus = amount;
}

[Serializable]
public struct Debuff
{
    [SerializeField] private DebuffType _debuffType;

    [SerializeField] private int _strength;
    [SerializeField] private int _strengthBonus;

    [SerializeField] private int _duration;
    [SerializeField] private int _durationBonus;

    private int _bonus;
    private int _currentDuration;

    public Action OnDurationEnded;

    public int Strength => _strength + _strengthBonus * _bonus;
    public int Duration => _duration + _durationBonus * _bonus;
    public DebuffType DebuffType => _debuffType;

    public Debuff(DebuffType debuffType,
                  int strength, int strengthBonus,
                  int duration, int durationBonus,
                  int bonus)
    {
        _debuffType = debuffType;
        _strength = strength;
        _strengthBonus = strengthBonus;
        _duration = duration;
        _durationBonus = durationBonus;
        _bonus = bonus;
        _currentDuration = 0;
        OnDurationEnded = null;
    }

    public Debuff Copy()
    {
        Debuff res = new Debuff(_debuffType, _strength, _strengthBonus, _duration, _durationBonus, _bonus);

        return res;
    }

    public void SetBonus(int value) => _bonus = value;
    
    public void Tick()
    {
        _currentDuration++;

        if (_currentDuration >= Duration) OnDurationEnded?.Invoke();
    }
}