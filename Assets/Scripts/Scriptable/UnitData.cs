using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Unit Data", menuName = "Unit data")]
public class UnitData : ScriptableObject
{
    [Header("Base")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _cost;
    [SerializeField] private Sprite _image;

    [Header("Characteristics")]
    [Range(7, 25)]
    [SerializeField] private int _hp;
    [Range(1, 20)]
    [SerializeField] private int _initiative;
    [Range(5, 9)]
    [SerializeField] private int _speed;

    [Header("Special")]
    [SerializeField] private CardData[] _deck;

    [Header("View")]
    [SerializeField] private AnimatorController _animaton;

    public string Name => _name;
    public string Description => _description;
    public Sprite Image => _image;
    public int Cost => _cost;
    public int HP => _hp;
    public int Initiative => _initiative;
    public int Speed => _speed;
    public CardData[] Deck => _deck;
    public AnimatorController Animaton => _animaton;
    
}
