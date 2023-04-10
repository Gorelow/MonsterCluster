using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TurnOrderController : MonoBehaviour, ITurnOrderController
{
    [SerializeField] private List<UnitController> _units;
    [SerializeField] private TurnOrderView _view;

    public Action<int> OnUnitDeath { get; set; }
    public Action<IUnitController> OnTurnEnd { get; set; }
    public Action<IUnitController> OnNextTurn { get; set; }
    public Action<int> OnNextTurnNumberChange { get; set; }

    private int _turnIndex;
    public int TurnIndex => _turnIndex;

    public List<UnitController> Units => _units;
    public UnitController CurrentTurn => _units[_turnIndex];

    public IUnitController Order(int index) => _units[index]; 


    public void Set(List<UnitController> units, ITurnMaster turnMaster)
    {
        _units = units;
        SetAnOrder();

        foreach (var unit in _units)
        {
            unit.OnDeath += () => UnitDeath(unit);
        }

        if (turnMaster != null)
            turnMaster.OnNextTurn += NextTurn;

        _view.Set(this);
    }

    private void Start()
    {
        Set(_units, null);
    }

    private void SetAnOrder()
    {
        _units.Sort(delegate (UnitController x, UnitController y)
        {
            if (x.Data.Initiative != y.Data.Initiative)
                return x.Data.Initiative > y.Data.Initiative ? -1 : 1;

            if (x.Data.Speed != y.Data.Speed) 
                return x.Data.Speed > y.Data.Speed ? -1 : 1;

            return 0;
        });
    }

    private void UnitDeath(UnitController unit)
    {
        int indexOfDestroyedUnit = _units.IndexOf(unit);
        OnUnitDeath?.Invoke(indexOfDestroyedUnit);
        _units.Remove(unit);

        if (indexOfDestroyedUnit <= _turnIndex) _turnIndex -= 1;
    }

    public void NextTurn()
    {
        OnTurnEnd?.Invoke(CurrentTurn);
        _turnIndex = (_turnIndex + 1) % _units.Count;
        OnNextTurn?.Invoke(CurrentTurn);
        OnNextTurnNumberChange?.Invoke(_turnIndex);
    }

}

public interface ITurnMaster
{
    Action OnNextTurn { get; set; }
}
