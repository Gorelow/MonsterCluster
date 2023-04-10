using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Deck Data", menuName = "Deck data")]
public class DeckData : ScriptableObject
{
    [SerializeField] private List<CardData> _cards;

    public List<CardData> Cards => _cards;

    public Action OnPileIsEmpty;


    public DeckData Copy()
    {
        var copy = new DeckData();

        foreach (CardData card in _cards)
            copy.Add(card);

        return copy;
    }

    public void Add(IEnumerable<CardData> cards)
    {
        Debug.Log(_cards);
        if (_cards == null) _cards = new List<CardData>();

        foreach (CardData card in cards)
            _cards.Add(card);
    }

    public void Add(CardData card)
    {
        if (_cards == null) _cards = new List<CardData>();
        _cards.Add(card);
    }

    public CardData Take(int i = 0)
    {
        if (_cards == null || _cards.Count <= i) return null;

        CardData card = Cards[i];
        _cards.RemoveAt(i);

        if (_cards.Count == 0)
            OnPileIsEmpty?.Invoke();

        return card;
    }

    public CardData Take(CardData card)
    {
        CardData data = card;
        int index = _cards.IndexOf(data);

        if (index == -1) return null;

        _cards.RemoveAt(index);

        if (_cards.Count == 0)
            OnPileIsEmpty?.Invoke();

        return data;
    }

    public List<CardData> TakeAll()
    {
        var cards = _cards;

        _cards = new List<CardData>();
        OnPileIsEmpty?.Invoke();

        return cards;
    }

    public void Shuffle()
    {
        List<CardData> suffled = new List<CardData>();

        while (_cards.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, _cards.Count);
            suffled.Add(_cards[index]);
            _cards.RemoveAt(index);
        }

        _cards = suffled;
    }
}
