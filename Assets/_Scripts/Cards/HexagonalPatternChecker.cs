using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class is used to store all possible patterns that can be used in the game
/// </summary>
public static class HexagonalPatternChecker
{

    public static void CheckPattern(HexagonalPatterns pPattern,Tile pTile, Tile[] pNeighbours, PatternData pPatternData)
    {
        switch (pPattern)
        {
            case HexagonalPatterns.StraightLine:
                CheckForStraightLinePattern(pTile, pNeighbours, pPatternData);
                break;
            case HexagonalPatterns.SingularAdjacent:
                CheckForSingularAdjacentPattern(pTile, pNeighbours, pPatternData);
                break;
        }
    }

    /// <summary>
    /// Compares the names of 2 BlockData arrays
    /// </summary>
    /// <param name="array1">Blockdata array 1</param>
    /// <param name="array2">Blockdata array 1</param>
    /// <returns>true if the Blockdata names are identical</returns>
    public static bool CompareBlockDataArray(BlockData[] array1, BlockData[] array2)
    {
        string array1String = "";
        foreach (BlockData data in array1)
        {
            array1String += data.blockName + " ";
        }
        
        string array2String = "";
        foreach (BlockData data in array2)
        {
            array2String += data.blockName + " ";
        }

        return array1String == array2String;
    }

    private static void CheckForSingularAdjacentPattern(Tile pTile, Tile[] pNeighbours, PatternData pPatternData)
    {
        BlockData[] middleBlockExpected = pPatternData.middleBlock;
        BlockData[] middleBlockActual = pTile.GetBlocks();
        
        //See if the middle block is valid, otherwise don't even bother checking the rest of the pattern
        if(!CompareBlockDataArray(middleBlockActual,middleBlockExpected))
        {
            Debug.Log("Middle tile does not contain the correct block type");
            return;
        }
        
        pTile.ChangeColor(Color.green);
        BlockData[] adjacentBlockExpected = pPatternData.adjacentBlock;
        
        List<Vector2Int> cubePositions = new List<Vector2Int>();
        foreach (Tile tile in pNeighbours)
        {
            BlockData[] tileBlockDatas = tile.GetBlocks();
            
            if(!CompareBlockDataArray(tileBlockDatas,adjacentBlockExpected))
            {
                Debug.Log("Tile does not contain the correct block type");
                continue;
            }
            
            Debug.Log($"Correct tile found at: {tile.gridPosition.x},{tile.gridPosition.y}");
            tile.ChangeColor(Color.green);
            
            Vector2Int tilePos = tile.gridPosition;
            cubePositions.Add(tilePos);
        }
        
        HashSet<Vector2Int> neighboursCube = new HashSet<Vector2Int>(cubePositions);
        Vector2Int[] directions = HexConversion.directions[pTile.gridPosition.y % 2];

        foreach (Vector2Int direction in directions)
        {
            if (neighboursCube.Contains(pTile.gridPosition + direction))
            {
                Debug.Log("Singular adjacent pattern found");
                return;
            }
        }
    }

    #region StraightLinePattern

    /// <summary>
    /// This method checks for a straight line pattern
    /// </summary>
    /// <param name="tile">The middle tile you are trying to check the straight line from</param>
    /// <param name="neighbours">all the adjecant neighbours of this tile</param>
    public static void CheckForStraightLinePattern(Tile pTile, Tile[] pNeighbours, PatternData pPatternData)
    {
        
        pTile.ChangeColor(Color.green);
        
        BlockData[] middleBlockExpected = pPatternData.middleBlock;
        BlockData[] middleBlockActual = pTile.GetBlocks();
        
        //See if the middle block is valid, otherwise don't even bother checking the rest of the pattern
        if(!CompareBlockDataArray(middleBlockActual,middleBlockExpected))
        {
            Debug.Log("Middle tile does not contain the correct block type");
            pTile.ChangeColor(Color.red);
            return;
        }

        Dictionary<Vector2Int, Tile> validTiles = new();
        
        //For each neighbouring tile, check if the adjacent blocks fit the pattern that has been given
        foreach (Tile tile in pNeighbours)
        {
            BlockData[] tileBlocks = tile.GetBlocks();
            
            //comparing the arrays itself did yield the results i wanted, so this method compares the name variable in the SO
            if (!CompareBlockDataArray(tileBlocks, pPatternData.adjacentBlock) &&
                !CompareBlockDataArray(tileBlocks, pPatternData.adjacentBlock2))
            {
                Debug.Log("Tile does not contain the correct block type");
                continue;
            }
            
            Debug.Log($"Occupied tile found at: {tile.gridPosition.x},{tile.gridPosition.y}");
            tile.ChangeColor(Color.green);
            
            Vector2Int tilePos = tile.gridPosition;
            validTiles.Add(tilePos,tile);
        }

        bool patternCompleted = CheckThreeInLineFromMiddle(pTile.gridPosition, validTiles, pPatternData);
        if (!patternCompleted)
        {
            foreach (KeyValuePair<Vector2Int,Tile> validTile in validTiles)
            {
                validTile.Value.ChangeColor(Color.red);
            }

            pTile.ChangeColor(Color.red);
        }
        Debug.Log("Straight line pattern checked, outcome is " + patternCompleted);
    }
    
    // Function to check if there are three hexes in a line from a given middle hex
    private static bool CheckThreeInLineFromMiddle(Vector2Int middleHex, Dictionary<Vector2Int,Tile> hexSet, PatternData pPatternData)
    {
        var directions = HexConversion.directions[middleHex.y % 2];
        foreach (var direction in directions)
        {
            if (IsLineOfThreeFromMiddle(middleHex, direction, hexSet,pPatternData))
            {
                return true;
            }
        }
        return false;
    }

    // Function to check if there are hexes in both directions forming a line of three from the middle hex
    private static bool IsLineOfThreeFromMiddle(Vector2Int middleHex, Vector2Int direction, Dictionary<Vector2Int,Tile> hexSet, PatternData pPatternData)
    {
        Vector2Int firstHex = middleHex + direction;
        Vector2Int oppositeDirection = HexConversion.oppositeDirection[middleHex.y % 2][Array.IndexOf(HexConversion.directions[middleHex.y % 2], direction)];
        Vector2Int thirdHex = middleHex + oppositeDirection;

        //Check if there is even valid blocks in the hashset
        if (!hexSet.ContainsKey(firstHex) || !hexSet.ContainsKey(thirdHex)) return false;
        
        //check if the correct blocks are on the hexes
        BlockData[] firstHexBlock = hexSet[firstHex].GetBlocks();
        BlockData[] thirdHexBlock = hexSet[thirdHex].GetBlocks();

        return CompareBlockDataArray(firstHexBlock, pPatternData.adjacentBlock) &&
               CompareBlockDataArray(thirdHexBlock, pPatternData.adjacentBlock2);
    }
    #endregion
}

public enum HexagonalPatterns
{
    StraightLine,
    VShape_WIP,
    SingularAdjacent
}