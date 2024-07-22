using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private List<Block> _blocks = new();
    [SerializeField] private Vector2Int _gridPosition;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _startColor;
    
    [SerializeField] private bool isOccupied;
    
    public bool IsOccupied
    {
        get => isOccupied;
        set => isOccupied = value;
    }

    public Vector2Int gridPosition
    {
        get => _gridPosition;
        set => _gridPosition = value;
    }

    public Color BaseColor
    {
        get => _startColor;
        set => _startColor = value;
    }
    
    public void AddBlock(Block block)
    {
        _blocks.Add(block);
        IsOccupied = true;
    }
    
    public void RemoveBlock(Block block)
    {
        _blocks.Remove(block);
        IsOccupied = _blocks.Count > 0;
    }
    
    public int GetBlockCount()
    {
        return _blocks.Count;
    }
    

    public BlockData[] GetBlocks()
    {
        List<BlockData> blockData = new();
        for (int i = _blocks.Count - 1; i >= 0; i--)
        {
            blockData.Add(_blocks[i].BlockData);
        }

        return blockData.ToArray();
    }

    public void ChangeColor(Color pColor, int pTimeInSeconds = 5)
    {
        StartCoroutine(ChangeColorCoroutine(pColor, pTimeInSeconds));
    }

    private IEnumerator ChangeColorCoroutine(Color pColor, int pTimeInSeconds)
    {
        _meshRenderer.material.color = pColor;
        yield return new WaitForSeconds(pTimeInSeconds);
        _meshRenderer.material.color = _startColor;
    }
    public Block GetTopBlock()
    {
        return _blocks[0];
    }
}
