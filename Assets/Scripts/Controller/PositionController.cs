using System;
using System.Collections;
using System.Collections.Generic;
using Basic;
using Unity.VisualScripting;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    [SerializeField] private AimController _cursor;

    [SerializeField] private List<UnitController> _monsters;
    [SerializeField] private List<UnitController> _heroes;
    private List<UnitController> _units;

    [SerializeField] private Vector2 _floorSize;
    [SerializeField] private Vector2 _floorPosition;

    public Action<List<UnitController>> OnFoundAffected;

    public Action<UnitController> OnClickMoster;
    public Action<UnitController> OnClickHero;
    public Action<UnitController> OnClickUnit;

    public Action<Vector2> OnEmptyPlaceClick;

    public void Set(AimController cursor)
    {
        _cursor = cursor;
        _units = new List<UnitController>();
        _units.AddRange(_monsters);
        _units.AddRange(_heroes);
        _cursor.OnPositionClicked += CheckIfUnitClicked;
    }

    public void CheckIfUnitClicked(Vector2 pos)
    {
        Debug.Log($"Clicked position is {pos}");
        foreach (UnitController unit in _units)
        {
            Debug.Log($"unit position is {unit.Position}");
            if ((unit.Position - pos).magnitude <= 0.1f)
            {
                OnClickUnit?.Invoke(unit);
                return;
            }
        }

        OnEmptyPlaceClick?.Invoke(pos);// доработать
    }

    public List<UnitController> FindAffected(UnitController unit, bool isMonster, CardData card)
    {
        /// 1, 2, все
        List<UnitController> res = new List<UnitController>();

        switch (card.AimGroup)
        {
            case AimGroups.Self: res.Add(unit); break;

            case AimGroups.All:             res.AddRange(_units); 
                break;
            case AimGroups.AllAndSelf:      res.AddRange(_units);
                                            SetGroupSelf(ref res, unit, true); 
                break;
            case AimGroups.AllExeptSelf:    res.AddRange(_units);
                                            SetGroupSelf(ref res, unit, false);
                break;

            case AimGroups.Friends:             res.AddRange(GetGroup(isMonster, false));
                break;
            case AimGroups.FriendsAndSelf:      res.AddRange(GetGroup(isMonster, false));
                                                SetGroupSelf(ref res, unit, true);
                break;
            case AimGroups.FriendsExeptSelf:    res.AddRange(GetGroup(isMonster, true));
                                                SetGroupSelf(ref res, unit, false);
                break;


            case AimGroups.Enemies:         res.AddRange(GetGroup(isMonster, true));
                break;
            case AimGroups.EnemiesAndSelf:  res.AddRange(GetGroup(isMonster, true));
                                            SetGroupSelf(ref res, unit, true);
                break;
        }

        foreach (UnitController affected in res)
            if (Math.Max(Math.Abs(unit.Position.x - affected.Position.x),
                         Math.Abs(unit.Position.y - affected.Position.y)) > card.CalculatedRange) res.Remove(affected);

        OnFoundAffected?.Invoke(res);
        return res;
    }

    
    private List<UnitController> GetGroup(bool isMonster,bool isEnemy)
    {
        if (isMonster == isEnemy) return _heroes;
        else return _monsters;
    }

    private void SetGroupSelf(ref List<UnitController> group, UnitController unit, bool isIncluded)
    {
        if (!isIncluded && group.Contains(unit)) group.Remove(unit);
        if (isIncluded && !group.Contains(unit)) group.Add(unit);
    }

    public Vector2 FindRandomUnoccupied()
    {
        Vector2 res;
        bool isOcupied = false;

        do
        {
            res = new Vector2(UnityEngine.Random.Range(0, 6) - 2.5f, UnityEngine.Random.Range(0, 6) - 2f);

            foreach (UnitController unit in _units)
            {
                if (unit.Position == res)
                {
                    isOcupied = true;
                    break;
                }
            }
        } while (isOcupied);

        return res;
    }

    public bool CheckForRange(Vector2 a, Vector2 b, int expected)
    {
        Debug.Log($"A is {a}, B is {b} and expected range is {expected}");
        float range = Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y));
        return expected >= range;
    }
}
