using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderView : MonoBehaviour
{
    public readonly static float POINTER_SPEED = 12;
    //public readonly static float POINTER_SPEED = 10;

    [SerializeField] private List<Animator> _animators;
    [SerializeField] private List<Image> _images;

    [SerializeField] private GameObject _turnPointer;
    [SerializeField] private RectTransform _rectTransform;

    private ITurnOrderController _controller;
    private bool _isActive;
    //private bool

    public void Set(ITurnOrderController controller)
    {
        _controller = controller;

        for (int i = 0; i < _images.Count; i++)
        {
            _images[i].sprite = controller.Order(i).Image;
        }

        SetConnection(true);
    }

    private void SetConnection(bool active)
    {
        if (_controller == null || _isActive == active) return;

        _isActive = active;

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
    private void OnEnable()
    {
        SetConnection(true);
    }

    private void OnDisable()
    {
        SetConnection(false);
    }

    private void DeleteUnit(int index)
    {
        _animators[index].SetTrigger("OnDeath");
        Destroy(_animators[index].gameObject, 1f);
    }

    private async void MoveTurnPointer(int indexPosition)
    {
        Vector2 size = _rectTransform.sizeDelta;
        Vector2 boxSize = new Vector2(size.x / _images.Count, size.y);
        Vector2 leftSide = (Vector2)_rectTransform.position / _rectTransform.lossyScale - (size + Vector2.right * boxSize.x) / 2;

        Vector2 stPos = (leftSide + Vector2.right * boxSize.x * ((indexPosition + _images.Count - 1) % _images.Count + 1)) * _rectTransform.lossyScale;
        Vector2 endPos = (leftSide + Vector2.right * boxSize.x * (indexPosition + 1)) * _rectTransform.lossyScale;

        _turnPointer.transform.position = stPos;
        do
        {
            _turnPointer.transform.position = Vector3.MoveTowards(_turnPointer.transform.position, endPos, POINTER_SPEED);
            await Task.Delay(20);
        } while (((Vector2)_turnPointer.transform.position - endPos).magnitude > 5f);
    }
}
