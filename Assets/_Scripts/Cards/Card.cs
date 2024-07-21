using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string name;
    public int[] pointsScale;

    private int _uses;
    private int _pointsObtained;
    
    //Pattern variables
    
    public int Uses
    {
        get => _uses;
        set => _uses = value;
    }
    
    public int PointsObtained
    {
        get => _pointsObtained;
        set => _pointsObtained = value;
    }

    private void Awake()
    {
        Uses = pointsScale.Length;
    }
}