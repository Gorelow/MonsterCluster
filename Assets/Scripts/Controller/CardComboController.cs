using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Basic;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardComboController : MonoBehaviour, ICardComboController
{
    [SerializeField] private PlayerHandController _hand;
    [SerializeField] private IView<ICardComboController> _view;

    [SerializeField] private Button _attackButton;

    private Dictionary<CardActionType, ICardController> _cardCombo;
    
    public event Action<CardActionType, ICardController> OnTypeCardChange;
    public event Action<bool> OnChangeActive;
    public event Action<bool> OnAvailability;
    public event Action<int> OnChangeComboPrice;

    public Action<Dictionary<CardActionType, ICardController>> OnComboPlay;
    public Action<Dictionary<CardActionType, ICardController>, int> OnComboBuilding;
    public Action<CardActionType, ICardController> OnComboCardChange;

    private int _spellCost;
    private int _availableEnergy;

    public int SpellsCost => _spellCost;
    public int AvailableEnergy => _availableEnergy;

    public void Set(PlayerHandController hand)
    {
        hand.OnMadUnsellect += (CardActionType type) => MadChange(type, null);
        hand.OnMadSellect += MadChange;
        _view.Init(this);
    }

    public void Reboot(int avalibleEnergy)
    {
        _availableEnergy = avalibleEnergy;
        CardComboReset();
        _spellCost = 0;
        OnChangeActive?.Invoke(true);
    }

    private void CardComboReset() => _cardCombo = new Dictionary<CardActionType, ICardController>() 
                                                              { { CardActionType.Moving, null },
                                                                { CardActionType.Attack, null },
                                                                { CardActionType.Debuff, null } };

    public void MadChange(CardActionType type, ICardController value)
    {
        OnComboCardChange?.Invoke(type, value);
        ChangeComboSlotValue(type, value); 
    }

    private void ChangeComboSlotValue(CardActionType slot, ICardController newVal)
    {
        _cardCombo[slot] = newVal;
        _spellCost = CalculateComboPrice();

        OnAvailability?.Invoke(SpellsCost <= AvailableEnergy);
        OnComboBuilding?.Invoke(_cardCombo, SpellsCost);
    }

    private int CalculateComboPrice() => _cardCombo.Where(card => card.Value != null)
                                                  .Sum(card => card.Value.Cost);

    public void Play()
    {
        Debug.Log("Combo box is being played!");
        OnChangeActive?.Invoke(false);
        OnComboPlay?.Invoke(_cardCombo);
    }

}
// 86 до