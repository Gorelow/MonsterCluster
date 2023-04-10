using System.Collections;
using System.Collections.Generic;
using Interfaces;
using TMPro;
using UnityEngine;

public class CardDeckView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private TextMeshProUGUI _drawAmount;
    [SerializeField] private TextMeshProUGUI _discardAmount;
    /// <summary>
    /// в информации по деке показываются данные о:
    /// количестве карт в стопке добора
    /// количестве карт в стопке сброса
    /// </summary>
    private ICardDeckController _controller;
    private bool _isActive;

    public void Set(ICardDeckController controller)
    {
        _controller = controller;
        
       // UpdateUnit();
        SetConnection(true);
    }

    private void SetConnection(bool active)
    {
        if (_controller == null || _isActive == active) return;

        _isActive = active;

        if (active)
        {
            _controller.OnUpdateDrawPileAmount += UpdateDrawPileAmount;
            _controller.OnUpdateDiscardPileAmount += UpdateDiscardPileAmount;
        }
        else
        {
            _controller.OnUpdateDrawPileAmount -= UpdateDrawPileAmount;
            _controller.OnUpdateDiscardPileAmount -= UpdateDiscardPileAmount;
        }
    }

    private void UpdateDrawPileAmount(int newVal)
    {
        _drawAmount.text = newVal.ToString();
    }

    private void UpdateDiscardPileAmount(int newVal)
    {
        _discardAmount.text = newVal.ToString();
    }

    private void OnEnable()
    {
        SetConnection(true);
    }

    private void OnDisable()
    {
        SetConnection(false);
    }
}
