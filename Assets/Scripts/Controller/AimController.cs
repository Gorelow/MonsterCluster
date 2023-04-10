using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class AimController : MonoBehaviour, IAimController
{
    [SerializeField] private AimView _view;

    public Action<Vector2> OnPositionClicked { get; set; }

    public void Set()
    {
        _view.Set(this);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) NotifyClickedPosition();
    }

    private void NotifyClickedPosition()
    {
        //Debug.Log("Pressed");
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector2(MathF.Round(mousePos.x), MathF.Round(mousePos.y + 0.5f) - 0.5f);
        OnPositionClicked?.Invoke(mousePos);
    }
}
