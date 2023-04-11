using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class TurnOrderView : View<ITurnOrderController>
    {
        public readonly static float POINTER_SPEED = 12;
        //public readonly static float POINTER_SPEED = 10;

        [SerializeField] private List<Animator> _animators;
        [SerializeField] private List<Image> _images;

        [SerializeField] private GameObject _turnPointer;
        [SerializeField] private RectTransform _rectTransform;

        protected override void InitAdditional()
        {
            for (int i = 0; i < _images.Count; i++)
            {
                _images[i].sprite = _controller.Order(i).Image;
            }
        }

        protected override void SetConnectToControllerEvents(bool active)
        {
            if (active)
            {
                _controller.OnUnitDeath += DeleteUnit;
                _controller.OnNextTurnNumberChange += MoveTurnPointer;
            }
            else
            {
                _controller.OnUnitDeath -= DeleteUnit;
                _controller.OnNextTurnNumberChange -= MoveTurnPointer;
            }
        }

        private void DeleteUnit(int index)
        {
            _animators[index].SetTrigger("OnDeath");
            Destroy(_animators[index].gameObject, 1f);
        }

        private async void MoveTurnPointer(int indexPosition)
        {
            Vector2 size = _rectTransform.sizeDelta;
            Vector2 unitImageSize = new Vector2(size.x / _images.Count, 0);
            var lossyScale = _rectTransform.lossyScale;
            Vector2 startingPointerCoordinate = (Vector2)_rectTransform.position / lossyScale - (size + unitImageSize) / 2;

            Vector2 stPos = (startingPointerCoordinate + ((indexPosition + _images.Count - 1) % _images.Count + 1) * unitImageSize) * lossyScale;
            Vector2 endPos = (startingPointerCoordinate + (indexPosition + 1) * unitImageSize) * lossyScale;

            _turnPointer.transform.position = stPos;
            do
            {
                _turnPointer.transform.position = Vector3.MoveTowards(_turnPointer.transform.position, endPos, POINTER_SPEED);
                await Task.Delay(20);
            } while (((Vector2)_turnPointer.transform.position - endPos).magnitude > 5f);
        }
    }
}
//87