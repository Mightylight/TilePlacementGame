using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector3Int _gridPosition;
    
    private BlockData _blockData;
    
    public Vector3Int GridPosition
    {
        get => _gridPosition;
        set => _gridPosition = value;
    }
    
    public BlockData BlockData
    {
        get => _blockData;
        set => _blockData = value;
    }
    
    
    public bool IsValidPlacement()
    {
        return _blockData.validFoundation == null;
    }
}
