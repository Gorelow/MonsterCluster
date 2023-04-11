using Interfaces;
using TMPro;
using UnityEngine;

namespace View
{
    public class CardDeckView : View<ICardDeckController>
    {
        [SerializeField] private TextMeshProUGUI _drawAmount;
        [SerializeField] private TextMeshProUGUI _discardAmount;
    
        protected override void SetConnectToControllerEvents(bool active)
        {
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

        private void UpdateDrawPileAmount(int newVal) => _drawAmount.text = newVal.ToString();

        private void UpdateDiscardPileAmount(int newVal) =>  _discardAmount.text = newVal.ToString();
    }
}