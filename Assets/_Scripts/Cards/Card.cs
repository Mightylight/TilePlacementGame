using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string name;
    public int[] pointsScale;

    private int _uses;
    private int _pointsObtained;
    
    
    //Inspector filled fields
    [SerializeField] private HexagonalPatterns patternType;
    [SerializeField] private BlockData[] middleBlock;
    [SerializeField] private BlockData[] adjacentBlock;
    [HideIf("IsSingleBlockPattern")]
    [SerializeField] private BlockData[] adjacentBlock2;
    
    //Used in code
    [HideInInspector]
    public PatternData patternData;
    
    public bool IsSingleBlockPattern()
    {
        return patternType == HexagonalPatterns.SingularAdjacent;
    }
    
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

    public void Initialize()
    {
        Uses = pointsScale.Length;
        patternData = new PatternData
        {
            patternType = patternType,
            middleBlock = middleBlock,
            adjacentBlock = adjacentBlock,
            adjacentBlock2 = adjacentBlock2
        };
        Debug.Log("Card created");
    }
}

[Serializable]
public class PatternData
{
    public HexagonalPatterns patternType;
    public BlockData[] middleBlock;
    public BlockData[] adjacentBlock;
    public BlockData[] adjacentBlock2;
}