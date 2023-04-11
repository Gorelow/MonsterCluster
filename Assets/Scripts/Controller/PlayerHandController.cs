using System;
using System.Collections;
using System.Collections.Generic;
using Basic;
using Interfaces;
using UnityEngine;
using View;

public class PlayerHandController : MonoBehaviour, IPlayerHandController
{
    private DeckData _data;
    private List<CardController> _cards;
    [SerializeField] private PlayerHandView _view;
    [SerializeField] private CardDeckController _deck;

    [SerializeField] private CardController _prefCard;

    private CardController[] _madCombo = new CardController[3];

    public Action<List<ICardView>> OnTakeCards { get; set; }
    public Action<CardActionType, ICardController> OnMadSellect { get; set; }
    public Action<CardActionType> OnMadUnsellect { get; set; }
    public Action<ICardController[]> OnDiscard { get; set; }
    public Action OnDiscardAll { get; set; }
   

    public DeckData Data => _data;
    public List<CardController> Cards => _cards;

    public CardController[] MadCombo => _madCombo;


    public void Set(CardDeckController deck)
    {
        _deck = deck;
        _cards = new List<CardController>();
        _view.Set(this);
    }

    public void DiscardPartly(List<CardController> cards)
    {
        foreach (CardController card in cards)
            _cards.Remove(card);

        OnDiscard?.Invoke(cards.ToArray());
    }

    public void DiscardAll()
    {
        _cards = new List<CardController>();
        OnDiscardAll?.Invoke();
    }

    public void Take(int amount)
    {
        DeckData deck = _deck.GetCards(amount);

        for (int i = 0; i < deck.Cards.Count; i++)
        {
            CardController card = Instantiate(_prefCard, _view.transform);
            card.Set(deck.Cards[i]);
            card.OnSelect += () => CardSelect(card);
            card.OnUnselect += () => CardUnselect(card);
            _cards.Add(card);
        }

        List<ICardView> cardViews = new List<ICardView>();

        foreach (CardController controller in _cards)
        {
            cardViews.Add();
        }

        OnTakeCards?.Invoke(_cards.GetRange(Cards.Count - deck.Cards.Count, deck.Cards.Count));
    }

    public void CardSelect(CardController card)
    {
        switch (card.Data.ActionType)
        {
            case CardActionType.Moving:
                if (_madCombo[0] != null) _madCombo[0].ForceUnselection();

                _madCombo[0] = card;
                break;
            case CardActionType.Attack:
                if (_madCombo[1] != null) _madCombo[1].ForceUnselection();

                _madCombo[1] = card;
                break;
            case CardActionType.Debuff:
                if (_madCombo[2] != null) _madCombo[2].ForceUnselection();

                _madCombo[2] = card;
                break;
        }

        OnMadSellect?.Invoke(card.Data.ActionType, card);
    }

    public void CardUnselect(CardController card)
    {
        switch (card.Data.ActionType)
        {
            case CardActionType.Moving:
                _madCombo[0] = null;
                break;
            case CardActionType.Attack:
                _madCombo[1] = null;
                break;
            case CardActionType.Debuff:
                _madCombo[2] = null;
                break;
        }
        OnMadUnsellect?.Invoke(card.Data.ActionType);
    }
}
