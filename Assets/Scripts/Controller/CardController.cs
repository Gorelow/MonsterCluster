using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, ICardController, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CardData _data;
    [SerializeField] private CardView _view;

    public Action OnSelect { get; set; }
    public Action OnUnselect { get; set; }
    public Action OnDiscard { get; set; }
    public Action OnMouseHover { get; set; }
    public Action OnMouseStopHover { get; set; }

    public CardData Data => _data;
    public CardView View => _view;
    private bool _selected; 

    public void Set(CardData data)
    {
        _data = data;
        _view.Set(this);
    }

    public void Start()
    {
        Set(_data);
    }

    public void ForceUnselection()
    {
        if (!_selected) return;

        _selected = false;
        OnUnselect?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _selected = !_selected;

        if (_selected)
            OnSelect?.Invoke();
        else
            OnUnselect?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseHover?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseStopHover?.Invoke();
    }

    public void Discard()
    {
        OnDiscard?.Invoke();
    }
}
