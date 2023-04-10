using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnController : MonoBehaviour
{

    ///Player allowed to play only when isActive is true
    [SerializeField] private bool _isActive;
    [SerializeField] private CardComboController _combo;
    [SerializeField] private TurnOrderController _turn;
    [SerializeField] private PlayerHandController _hand;
    [SerializeField] private CardDeckController _deck;
    [SerializeField] private AimController _aim;
    [SerializeField] private AbilityController _ability;
    [SerializeField] private PositionController _position;

    [SerializeField] private DeckData _deckData;
    [SerializeField] private List<UnitController> _monsters;

    // Для определения хода игрока будут использоваться объекты следующих скриптов:
    // 1. TurnOrderController. С помощью него будет понятно когда чей ход.
    // 2. List<UnitController>. В нем будут хранится все монстры на сцене и благодаря этому можно будет определить какой юнит ходит - свой или вражеский
    // 3. CardDeckController. Он определяет в самом начале какие карты вообще есть
    // 2. AimController. Он сообщит нам, куда именно показывает игрок
    // 3. ComboBox. Он сообщит нам, какая комбинация карт была выбрана для действия
    // 
    private UnitController _currentMonster;
    private int _cardsInCombo;

    public void Awake()
    {
        Set(_deckData);
        
    }

    public void Start()
    {
        SetMonsterActive(_monsters[0]);
    }

    public void Set(DeckData deckData)
    {
        _deckData = deckData;
        _turn.OnNextTurn += SetMonsterActive;
        _deck.Set(deckData);
        _hand.Set(_deck);
        _combo.Set(_hand);
        _aim.Set();
        _position.Set(_aim);
    }

    public void SetMonsterActive(UnitController unit)
    {
        if (!_monsters.Contains(unit)) return;

        _currentMonster = unit;
        _combo.Activate(unit.Data.Speed);
        _hand.Take(6);
        SetPlayerTurn(true);
    }

    public void SetPlayerTurn(bool isActive)
    {
        if (_isActive == isActive) return;

        _isActive = isActive;

        if (_isActive)
        {
            _combo.OnComboPlay += ComboActivate;
        }
        else
        {
            _combo.OnComboPlay -= ComboActivate;
            _turn.NextTurn();
        }

    }

    private void ComboActivate(CardController move, CardController attack, CardController debuff)
    {
        Debug.Log("Combo is activated!");
        _cardsInCombo = 0;
        if (move != null) _cardsInCombo++;
        if (attack != null) _cardsInCombo++;
        if (debuff != null) _cardsInCombo++;
        Debug.Log($"There are {_cardsInCombo} abilities in this combo!");
        _ability.Activate(_currentMonster, move, attack, debuff, true);
        _ability.OnAbilityPlaid += AbilityPlayed;

    }

    private void AbilityPlayed()
    {
        _cardsInCombo--;
        if (_cardsInCombo != 0) return;

        _ability.Deactivate();
        _hand.DiscardAll();
        SetPlayerTurn(false);
    }
}
