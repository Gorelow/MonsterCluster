using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Basic;
using Interfaces;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using View;

public class UnitController : MonoBehaviour, IUnitController
{
    public readonly static int MAX_HEALTH = 25;

    [SerializeField] private TurnOrderController _turn;
    [SerializeField] private UnitData _data;
    [SerializeField] private UnitView _view;
    [SerializeField] private UnitUI _ui;

    private List<Debuff> _debuffs = new List<Debuff>();
    private Vector2 _position;
    private int _health;
    private bool _isAlive;

    public Action OnDeath { get; set; }
    public Action<int> OnGettingDamage { get; set; }
    public Action<int> OnGettingHealth { get; set; }
    public Action OnClick { get; set; }

    public Action<Vector2> OnMove { get; set;}

    public UnitData Data => _data;
    public Vector2 Position => _position;
    public int Health => _health;
    public bool IsAlive => _isAlive;

    public void Set(UnitData data)
    {
        _data = data;
        _health = _data.HP;
        _view.Set(this);
        _position = transform.position;
        OnClick += () => _ui.ShowInfo(this);
    }

    public void Move(CardData card, Vector2 positionPoint)
    {
        Debug.Log($"Move order is noticed");
        if (CheckDebuffConditions(DebuffType.Rooting)) return;
        if (!CheckDebuffConditions(card.DebuffCondition)) return;
        Debug.Log($"Debuff conditions are met");

        _position = positionPoint;
        OnMove?.Invoke(Position);
        Debug.Log($"Position is changed");

        transform.position = positionPoint;
        ApplyDebuff(card.Debuff);
        Debug.Log($"Debuff is applied");
    }

    public void Attack(CardData card)
    {
        Debug.Log($"Unit getting attacked");
        if (!CheckDebuffConditions(card.DebuffCondition)) return;

        int damage = card.Attack.Damage;
        Debug.Log($"the damage is = {damage}");
        if (damage > 0)
        {
            try
            {
                var protection = FindDebuff(DebuffType.Shielding);
                damage = Math.Max(0, damage - protection.Strength); 
            }
            catch
            {

            }

            OnGettingDamage?.Invoke(damage);
        }

        if (damage < 0) OnGettingHealth?.Invoke(damage * -1);

        Debug.Log($"Hp before damage is = {_health}");
        _health -= card.Attack.Damage;
        _health = Math.Min(_health, MAX_HEALTH);
        Debug.Log($"the damage is applied. New hp now is {_health}");

        if (_health <= 0) OnDeath?.Invoke();

        ApplyDebuff(card.Debuff);
    }

    public void Debuff(CardData card)
    {
        if (!CheckDebuffConditions(card.DebuffCondition)) return;

        ApplyDebuff(card.Debuff);
    }

    private void DebuffRemoval(Debuff debuff)
    {
        _debuffs.Remove(debuff);
    }

    public bool CheckDebuffConditions(DebuffType needed)
    {
        if (needed == DebuffType.Any) return true;
        if (needed == DebuffType.None) return _debuffs.Count == 0;

        foreach (Debuff debuff in _debuffs)
            if (needed == debuff.DebuffType) return true;

        return false;
    }

    public Debuff FindDebuff(DebuffType needed)
    {
        if (needed == DebuffType.Any) throw new Exception("It's unclear what debuff is supposed to be found");
        if (needed == DebuffType.None) throw new Exception("It's unclear what debuff is supposed to be found");

        foreach (Debuff debuff in _debuffs)
            if (needed == debuff.DebuffType) return debuff;

        throw new Exception("Unable to find such debuff");
    }

    // В будущем надо, чтобы при нахождении уже существующего дебафа, найденный дебафф увеличивался,  не нахожился новый
    private void ApplyDebuff(Debuff debuff)
    {
        //if (!CheckDebuffConditions(card.DebuffCondition)) return;
        Debug.Log($"Applying debuff");
        var c_debuff = debuff.Copy();

        _turn.OnTurnEnd += (UnitController controller) => { if (controller == this) c_debuff.Tick(); };
        c_debuff.OnDurationEnded += () => DebuffRemoval(c_debuff);

        _debuffs.Add(c_debuff);
        Debug.Log($"debuff is applied");
    }

    public void Start()
    {
        Set(_data);
    }

    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}

public interface IDebuffable
{
    Debuff FindDebuff();
    bool CheckDebuffConditions();
    void ApplyDebuff();
}