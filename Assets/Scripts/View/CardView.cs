using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour, ICardView
{
    public readonly static Vector2 CARD_SIZE = new Vector2(200, 320);
    public readonly static float CARD_MOVING_SPEED = 10;
    public readonly static string ANIMATION_HOVER_PARAMETER_NAME = "IsHovered";
    public readonly static string ANIMATION_SELECTION_PARAMETER_NAME = "IsSelected";
    public readonly static string ANIMATION_DISCARD_TRIGGER_NAME = "OnDiscard";

    [SerializeField] public TextMeshProUGUI _name;
    [SerializeField] public Image _image;
    [SerializeField] public TextMeshProUGUI _description;

    [SerializeField] public Image _element;
    [SerializeField] public Image _actionType;
    [SerializeField] public Slider _cost;

    [SerializeField] public Animator _animator;

    private ICardController _controller;
    private bool _isActive;

    public void Set(ICardController controller)
    {
        _controller = controller;
        UpdateCard();

        SetConnection(true);
    }

    private void SetConnection(bool active)
    {
        if (_controller == null || _isActive == active) return;

        _isActive = active;

        if (active)
        {
            _controller.OnSelect += () => ShowSelection(true);
            _controller.OnUnselect += () => ShowSelection(false);
            _controller.OnMouseHover += () => SetHover(true);
            _controller.OnMouseStopHover += () => SetHover(false);
            _controller.OnDiscard += () => _animator.SetTrigger(ANIMATION_DISCARD_TRIGGER_NAME);
        }
        else
        {
            _controller.OnSelect -= () => ShowSelection(true);
            _controller.OnUnselect -= () => ShowSelection(false);
            _controller.OnMouseHover -= () => SetHover(true);
            _controller.OnMouseStopHover -= () => SetHover(false);
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

    public void ShowSelection(bool active) => _animator.SetBool(ANIMATION_SELECTION_PARAMETER_NAME, active);

    private void SetHover(bool active) => _animator.SetBool(ANIMATION_HOVER_PARAMETER_NAME, active);

    public void TeleportTo(Vector2 newPosition)
    {
        transform.position = newPosition;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(true);
    }
}
