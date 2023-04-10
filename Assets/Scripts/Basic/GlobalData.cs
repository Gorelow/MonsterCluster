using System.Collections;
using System.Collections.Generic;
using Basic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static Dictionary<CardElementType, Sprite> ELEMENT_SPRITE;

    public static Dictionary<CardActionType, Color> ACTION_COLOR = new Dictionary<CardActionType, Color>()
    {
        {CardActionType.Moving, new Color(1, 1, 0, 0.4f)},
        {CardActionType.Attack,  new Color(1, 0, 0, 0.4f)},
        {CardActionType.Debuff, new Color(0, 1, 0, 0.4f)}
    };

    [SerializeField] private Sprite _airSprite;
    [SerializeField] private Sprite _waterSprite;
    [SerializeField] private Sprite _earthSprite;
    [SerializeField] private Sprite _fireSprite;

    private void Awake()
    {
        ELEMENT_SPRITE = new Dictionary<CardElementType, Sprite>()
        {
            {CardElementType.Air,   _airSprite},
            {CardElementType.Water, _waterSprite},
            {CardElementType.Earth, _earthSprite},
            {CardElementType.Fire,  _fireSprite}
        };
    }
}
