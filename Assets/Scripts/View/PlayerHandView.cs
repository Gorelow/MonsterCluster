using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace View
{
    public class PlayerHandView : MonoBehaviour
    {
        public static int CARD_SHOW_DELAY = 120;

        [SerializeField] private RectTransform _rectTransform;
        private List<ICardView> _cards = new List<ICardView>();

        private IPlayerHandController _controller;
        private bool _isActive;

        public void Set(IPlayerHandController controller)
        {
            _controller = controller;
            SetConnection(true);
        }

        private void SetConnection(bool active)
        {
            if (_controller == null || _isActive == active) return;

            _isActive = active;

            if (active)
            {
                _controller.OnTakeCards += TakeCards;
            }
            else
            {
                _controller.OnTakeCards -= TakeCards;
            }
        }
        private void OnEnable()
        {
            SetConnection(true);
        }

        private void OnDisable()
        {
            SetConnection(false);
        }

        public Vector2 GetCardPosition(int cardAmount, int cardIndex, Vector2 cardSize)
        {
            Vector2 size = _rectTransform.sizeDelta;
            Vector2 leftSide = (Vector2)_rectTransform.position / _rectTransform.lossyScale - Vector2.right * (size.x + cardSize.x) / 2;

            float gapX = (size.x - cardSize.x * cardAmount) / (cardAmount + 1);

            return (leftSide + Vector2.right * (gapX + cardSize.x) * cardIndex) * _rectTransform.lossyScale;
        }

        private async void TakeCards(List<ICardView> cards)
        {
            int cardNewAmount = cards.Count + _cards.Count;
            _cards.AddRange(cards);

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].MoveTo(GetCardPosition(cardNewAmount, i + 1, CardView.CARD_SIZE));
            }

            for (int i = _cards.Count; i < cardNewAmount; i++)
            {
                _cards[i].TeleportTo(GetCardPosition(cardNewAmount, cardNewAmount - i + 1, CardView.CARD_SIZE));

                _cards[i].SetActive(true);
                await Task.Delay(CARD_SHOW_DELAY);
            }
        }
    }
}
