using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using View;

public class CardDeckController : MonoBehaviour, ICardDeckController
{
    [SerializeField] private DeckData _data;
    [SerializeField] private CardDeckView _view;
    [SerializeField] private PlayerHandController _handController;

    // public Action<DeckData> OnTakeCards;
    public Action<int> OnUpdateDrawPileAmount { get; set; }
    public Action<int> OnUpdateDiscardPileAmount { get; set; }
    // public Action OnRefreshDrawPile;

    private DeckData _drawPile;
    private DeckData _discardPile;
    private DeckData _onHandPile;

    public DeckData Data => _data;

    public void Set(DeckData data)
    {
        _data = data;
        _view.Set(this);

        _drawPile = ScriptableObject.CreateInstance<DeckData>();
        _drawPile.Add(_data.Cards.ToArray());
        _discardPile = ScriptableObject.CreateInstance<DeckData>();
        _onHandPile = ScriptableObject.CreateInstance<DeckData>();

        _drawPile.OnPileIsEmpty += RefreshDrawPile;
        _handController.OnDiscard += DiscardFromHand;
        _handController.OnDiscardAll += DiscardAllFromHand;

        _drawPile.Shuffle();
        _handController.Set(this);
    }


    public DeckData GetCards(int amount)
    {
        Debug.Log($"Taking {amount}");
        DeckData draw = new DeckData();

        for (int i = 0; i < amount; i++)
        {
            draw.Add(_drawPile.Take());
            Debug.Log($"Taking {draw.Cards[i].Name}");
        }
        _onHandPile.Add(draw.Copy().Cards);
        Debug.Log("Hand size is " + _onHandPile.Cards.Count);

        OnUpdateDrawPileAmount?.Invoke(_drawPile.Cards.Count);
        return draw;
    }

    public void ShowHand()
    {
        Debug.Log("Hand size is " + _onHandPile.Cards.Count);
    }

    private void DiscardAllFromHand()
    {
        Debug.Log("Hand size is " + _onHandPile.Cards.Count);
        _discardPile.Add(_onHandPile.TakeAll());
        OnUpdateDiscardPileAmount?.Invoke(_discardPile.Cards.Count);
    }

    private void DiscardFromHand(CardController[] cards)
    {
        foreach (CardController card in cards)
            _discardPile.Add(_onHandPile.Take(card.Data));

        OnUpdateDiscardPileAmount?.Invoke(_discardPile.Cards.Count);
    }

    private void RefreshDrawPile()
    {
        _drawPile.Add(_discardPile.TakeAll());
        _drawPile.Shuffle();
    }

}
