using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private List<Block> _blocks = new();

    [SerializeField] private bool isOccupied;
    
    public bool IsOccupied
    {
        get => isOccupied;
        set => isOccupied = value;
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
    
    public Block GetTopBlock()
    {
        return _blocks[0];
    }
}
