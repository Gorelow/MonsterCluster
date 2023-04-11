using System.Threading.Tasks;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class CardView : View<ICardController>, ICardView
    {
        public readonly static Vector2 CARD_SIZE = new Vector2(200, 320);
        public readonly static float CARD_MOVING_SPEED = 10;

        [SerializeField] public TextMeshProUGUI _name;
        [SerializeField] public Image _image;
        [SerializeField] public TextMeshProUGUI _description;

        [SerializeField] public Image _element;
        [SerializeField] public Image _actionType;
        [SerializeField] public Slider _cost;

        [SerializeField] public Animator _animator;
    
        private static readonly int IsHovered = Animator.StringToHash("IsHovered");
        private static readonly int IsSelected = Animator.StringToHash("IsSelected");
        private static readonly int OnDiscard = Animator.StringToHash("OnDiscard");

        protected override void InitAdditional() => UpdateCard();

        protected override void SetConnectToControllerEvents(bool active)
        {
            if (active)
            {
                _controller.OnSelect += () => ShowSelection(true);
                _controller.OnUnselect += () => ShowSelection(false);
                _controller.OnMouseHover += () => SetHover(true);
                _controller.OnMouseStopHover += () => SetHover(false);
                _controller.OnDiscard += () => _animator.SetTrigger(OnDiscard);
            }
            else
            {
                _controller.OnSelect -= () => ShowSelection(true);
                _controller.OnUnselect -= () => ShowSelection(false);
                _controller.OnMouseHover -= () => SetHover(true);
                _controller.OnMouseStopHover -= () => SetHover(false);
            }
        }

        public void UpdateCard()
        {
            _name.text = _controller.Name;
            _image.sprite = _controller.Sprite;
            _description.text = _controller.Description;

            _cost.value = _controller.Cost;
            _actionType.color = GlobalData.ACTION_COLOR[_controller.ActionType];
            _element.sprite = GlobalData.ELEMENT_SPRITE[_controller.Element];
        }

        public async void MoveTo(Vector2 newPosition)
        {
            do
            {
                transform.position = Vector2.MoveTowards(transform.position, newPosition, CARD_MOVING_SPEED);
                await Task.Delay(20);
            } while (Vector2.Distance(transform.position, newPosition) > 1);
        }

        private void ShowSelection(bool active) => _animator.SetBool(IsSelected, active);

        private void SetHover(bool active) => _animator.SetBool(IsHovered, active);

        public void TeleportTo(Vector2 newPosition) => transform.position = newPosition;

        public void SetActive(bool active) => gameObject.SetActive(true);
    }
}