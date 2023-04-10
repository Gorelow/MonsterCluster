using System;
using System.Collections;
using System.Collections.Generic;
using Basic;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class CardComboController : MonoBehaviour, ICardComboController
{
    [SerializeField] private PlayerHandController _hand;
    [SerializeField] private CardComboView _view;

    [SerializeField] private Button _attackButton;

    private CardController _moveCard;
    private CardController _attackCard;
    private CardController _debuffCard;

    public Action<CardController, CardController, CardController> OnComboPlay;
    public Action<CardController, CardController, CardController, int> OnComboBuilding;
    public Action<CardActionType, CardController> OnComboCardChange;
    public Action<bool> OnAvalibility;
    public Action<bool> OnChangeActive;

    private int spellCost;
    [SerializeField] private int _avalibleEnergy;

    public int SpellsCost => spellCost;
    public int AvalibleEnergy => _avalibleEnergy;

    public void Set(PlayerHandController hand)
    {
        hand.OnMadUnsellect += (CardActionType type) => MadChange(type, null);
        hand.OnMadSellect += MadChange;
        _view.Set(this);
    }

    public void Activate(int avalibleEnergy)
    {
        _avalibleEnergy = avalibleEnergy;
        _moveCard = null;
        _attackCard = null;
        _debuffCard = null;
        spellCost = 0;
        OnChangeActive?.Invoke(true);
    }

    public void MadChange(CardActionType type, CardController value)
    {
        OnComboCardChange?.Invoke(type, value);
        switch (type)
        {
            case CardActionType.Moving:
                ChangeComboSlotValue(ref _moveCard, value); 
                break;
            case CardActionType.Attack:
                ChangeComboSlotValue(ref _attackCard, value);
                break;
            case CardActionType.Debuff:
                ChangeComboSlotValue(ref _debuffCard, value);
                break;
        }
    }

    private void ChangeComboSlotValue(ref CardController slot, CardController newVal)
    {
        if (slot != null)
            spellCost -= slot.Data.Cost;

        slot = newVal;
        if (slot != null)
            spellCost += slot.Data.Cost;

        OnAvalibility?.Invoke(spellCost <= _avalibleEnergy);
        OnComboBuilding?.Invoke(_moveCard, _attackCard, _debuffCard, spellCost);
    }

    public void Play()
    {
        Debug.Log("Combo box is being played!");
        OnChangeActive?.Invoke(false);
        OnComboPlay?.Invoke(_moveCard, _debuffCard, _attackCard);
    }
}
