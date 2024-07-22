using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "BlockData", order = 0)]
public class BlockData : ScriptableObject
{
    public string blockName;
    public BlockData validFoundation; //Change from block to something else if _validFoundation is null, you can't build higher
    
    //TODO: Change this to a singluar static block prefab that is changeable with a texture. For now this is a prefab with some color changes.
    public Block blockPrefab;
    
    [Header("Standard Settings")]
    [Tooltip("Could be a nice way to make for some different gameplay, for now just keep on 1")]
    public int blockHeight = 1;
    [Tooltip("Could be a nice way to make for some different gameplay, for now just keep on 1")]
    public int blockWidth = 1;
    
}
