using System;
using System.Collections.Generic;
using System.Transactions;
using Basic;
using UnityEngine;
using UnityEngine.UIElements;

public class AbilityController : MonoBehaviour
{
    public Action OnAbilityPlaid;

    public PositionController _position;

    private MoveController _move;
    private AttackController _attack;
    private DebuffMaster _debuff;
    private bool _isActive = false;


    private void CheckForAction(UnitController unit)
    {
        Debug.Log($"Click on the unit detected");
        if ((_move != null) && (_move.IsActive))
            if (_move.CheckAim(unit)) 
            { 
                OnAbilityPlaid?.Invoke();
                Debug.Log($"Move card is played! Going back to card combo!");
                Activate(_delayedUnit, null, _delayedAttack, _delayedDebuff, _delayedIsPlayer);
                return; 
            }

        if ((_attack != null) && (_attack.IsActive))
            if (_attack.CheckAim(unit)) 
            { 
                OnAbilityPlaid?.Invoke();
                Debug.Log($"Attack card is played! Going back to card combo!");
                Activate(_delayedUnit, null, null, _delayedDebuff, _delayedIsPlayer);
                return; 
            }

        if ((_debuff != null) && (_debuff.IsActive))
            if (_debuff.CheckAim(unit)) OnAbilityPlaid?.Invoke();

    }

    private void CheckForAction(Vector2 pos)
    {
        Debug.Log($"Click on the empty position is detected");
        if ((_move != null) && (_move.IsActive))
            if (_move.CheckAim(pos))
            {
                OnAbilityPlaid?.Invoke();
                Debug.Log($"Move card is played! Going back to card combo!");
                Activate(_delayedUnit, null, _delayedAttack, _delayedDebuff, _delayedIsPlayer);
            }
    }

    private UnitController _delayedUnit;
    private CardController _delayedAttack;
    private CardController _delayedDebuff;
    private bool _delayedIsPlayer;

    public void Activate(UnitController unit, CardController move, CardController attack, CardController debuff, bool isPlayer)
    {
        Debug.Log($"Combo preparing");
        if (!_isActive)
        {
            Debug.Log($"Combo is position subscription is inactive, we changed that");
            SetCardBonuces(move, attack, debuff);
            _position.OnClickUnit += CheckForAction;
            _position.OnEmptyPlaceClick += CheckForAction;
            _isActive = true;
        }

        if (move != null)
        {
            Debug.Log($"Preparing move action");
            _move = new MoveController(unit, move.Data, _position.FindAffected(unit, isPlayer, move.Data), _position);
            if (!_move.IsActive) OnAbilityPlaid?.Invoke();
            else
            {
                _delayedUnit = unit;
                _delayedAttack = attack;
                _delayedDebuff = debuff;
                _delayedIsPlayer = isPlayer;
                Debug.Log($"Move is prepared other action cards should be delayed");
                return;
            }
        }
        _delayedAttack = null;

        if (attack != null)
        {
            Debug.Log($"Preparing attack action");
            _attack = new AttackController(attack.Data, _position.FindAffected(unit, isPlayer, attack.Data));

            if (!_attack.IsActive)
            {
                Debug.Log($"Attack is played automaticaly");
                OnAbilityPlaid?.Invoke();
            }
            else
            {
                _delayedUnit = unit;
                _delayedDebuff = debuff;
                _delayedIsPlayer = isPlayer;
                Debug.Log($"Attack is prepared other action cards should be delayed");
                return;
            }
        }
        else
        {
            Debug.Log($"Attack action isn't found");

        }
        _delayedDebuff = null;

        if (debuff != null)
        {
            Debug.Log($"Preparing debuff action");
            _debuff = new DebuffMaster(debuff.Data, _position.FindAffected(unit, isPlayer, debuff.Data));
            if (!_debuff.IsActive)
            {
                Debug.Log($"Debuff is played automaticaly");
                OnAbilityPlaid?.Invoke();
            }
        }

        //_position.OnClickUnit += 
    }

    public void SetCardBonuces(CardController move, CardController attack, CardController debuff)
    {
        Dictionary<CardElementType, int> bonus = new Dictionary<CardElementType, int>()
        {
            { CardElementType.Air, 0},
            { CardElementType.Water, 0},
            { CardElementType.Earth, 0},
            { CardElementType.Fire, 0}
        };
        if (move != null) bonus[move.Data.Element]++;
        if (attack != null) bonus[attack.Data.Element]++;
        if (debuff != null) bonus[debuff.Data.Element]++;

        if (move != null) move.Data.SetBonus(bonus[move.Data.Element]);
        if (attack != null) attack.Data.SetBonus(bonus[attack.Data.Element]);
        if (debuff != null) debuff.Data.SetBonus(bonus[debuff.Data.Element]);
    }

    public void Deactivate()
    {
        _position.OnClickUnit -= CheckForAction;
        _position.OnEmptyPlaceClick -= CheckForAction;
        _isActive = false;
    }
}

public class MoveController
{
    private bool _isActive;
    private bool _isOnPlace;

    private UnitController _caster;
    private CardData _card;
    private List<UnitController> _possibleAims;
    private PositionController _position;

    public bool IsActive => _isActive;  

    public MoveController(UnitController caster, CardData card, List<UnitController> possibleAims, PositionController position)
    {
        _caster = caster;
        _card = card;
        _possibleAims = possibleAims;
        _position = position;

        switch (card.AimAmount)
        {
            case Amount.One: _isActive = possibleAims.Count > 0; break;
            case Amount.Two: _isActive = possibleAims.Count > 1; break;
            case Amount.All:
                foreach (UnitController aim in _possibleAims)
                    aim.Move(card, _position.FindRandomUnoccupied());
                _isActive = false;
                break;

        }
        
        _isOnPlace = card.AimGroup == AimGroups.Self;
    }

    public bool CheckAim(UnitController unit)
    {
        Debug.Log($"Move is cheching the click on the unit");
        if (_isOnPlace || _caster == unit || !_possibleAims.Contains(unit)) return false;

        Vector2 pos = _caster.Position;

        _caster.Move(_card, unit.Position);
        unit.Move(_card, pos);

        _isActive = false;
        return true;
    }

    public bool CheckAim(Vector2 pos)
    {
        Debug.Log($"Move is cheching the click on empty space");
        if (!_isOnPlace) return false;
        if (!_position.CheckForRange(_caster.Position, pos, _card.CalculatedRange)) return false;
        Debug.Log($"Succesfull!");
        _caster.Move(_card, pos);
        _isActive = false;
        return true;
    }
}

public class AttackController
{
    private bool _isActive;

    private CardData _card;
    private List<UnitController> _possibleAims;

    public bool IsActive => _isActive;

    public AttackController(CardData card, List<UnitController> possibleAims)
    {
        _isActive = true;
        _card = card;
        _possibleAims = possibleAims;
        Debug.Log($"There are {possibleAims.Count} possible aims");
        Debug.Log($"Aim amount is {card.AimAmount}");
        Debug.Log($"The damage of this card is {card.Attack.Damage}");
        Debug.Log($"The debuff it gives is {card.Debuff.DebuffType}");

        switch (card.AimAmount)
        {
            case Amount.One: _isActive = possibleAims.Count > 0; if (possibleAims.Count <= 0) Debug.Log("There are no possible aims"); break;
            case Amount.Two: _isActive = possibleAims.Count > 1; if (possibleAims.Count <= 1) Debug.Log("There are not enough aims"); break;
            case Amount.All:
                Debug.Log("Applying Attack to everyone");
                foreach (UnitController aim in _possibleAims)
                    aim.Attack(card);
                _isActive = false;
                break;

        }
    }

    public bool CheckAim(UnitController unit)
    {
        if (!_possibleAims.Contains(unit)) return false;

        unit.Attack(_card);

        _isActive = false;
        return true;
    }

    public bool CheckAim(Vector2 pos)
    {
        return false;
    }
}

public class DebuffMaster
{
    private bool _isActive;

    private CardData _card;
    private List<UnitController> _possibleAims;

    public bool IsActive => _isActive;

    public DebuffMaster(CardData card, List<UnitController> possibleAims)
    {
        _card = card;
        _possibleAims = possibleAims;
        Debug.Log($"There are {possibleAims.Count} possible aims");
        Debug.Log($"Aim amount is {card.AimAmount}");
        Debug.Log($"The damage of this card is {card.Attack.Damage}");
        Debug.Log($"The debuff it gives is {card.Debuff.DebuffType}");


        switch (card.AimAmount)
        {
            case Amount.One: _isActive = possibleAims.Count > 0; Debug.Log("There are no possible aims"); break;
            case Amount.Two: _isActive = possibleAims.Count > 1; Debug.Log("There are not enough aims"); break;
            case Amount.All:
                Debug.Log("Applying debuff to everyone");
                foreach (UnitController aim in _possibleAims)
                    aim.Debuff(card);
                _isActive = false;
                break;

        }
    }

    public bool CheckAim(UnitController unit)
    {
        if (!_possibleAims.Contains(unit)) return false;

        unit.Debuff(_card);

        _isActive = false;
        return true;
    }

    public bool CheckAim(Vector2 pos)
    {
        return false;
    }
}