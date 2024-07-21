using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store all possible patterns that can be used in the game
/// </summary>
public static class HexagonalPatternChecker
{

    public static void CheckPattern(HexagonalPatterns pPattern,Tile pTile, Tile[] pNeighbours)
    {
        switch (pPattern)
        {
            case HexagonalPatterns.StraightLine:
                CheckForStraightLinePattern(pTile, pNeighbours);
                break;
            case HexagonalPatterns.SingularAdjacent:
                CheckForSingularAdjacentPattern(pTile, pNeighbours);
                break;
        }
    }

    private static void CheckForSingularAdjacentPattern(Tile pTile, Tile[] pNeighbours)
    {
        List<Vector2Int> cubePositions = new List<Vector2Int>();
        pTile.GetComponent<MeshRenderer>().material.color = Color.green;
        foreach (Tile tile in pNeighbours)
        {
            if (!tile.IsOccupied)
            {
                Debug.Log("Tile not occupied");
                continue;
            }
            
            Debug.Log($"Occupied tile found at: {tile.gridPosition.x},{tile.gridPosition.y}");
            tile.GetComponent<MeshRenderer>().material.color = Color.green;
            
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
    public static void CheckForStraightLinePattern(Tile pTile, Tile[] pNeighbours)
    {
        List<Vector2Int> cubePositions = new List<Vector2Int>();
        pTile.GetComponent<MeshRenderer>().material.color = Color.green;
        foreach (Tile tile in pNeighbours)
        {
            if (!tile.IsOccupied)
            {
                Debug.Log("Tile not occupied");
                continue;
            }
            
            Debug.Log($"Occupied tile found at: {tile.gridPosition.x},{tile.gridPosition.y}");
            tile.GetComponent<MeshRenderer>().material.color = Color.green;
            
            Vector2Int tilePos = tile.gridPosition;
            cubePositions.Add(tilePos);
        }
        
        HashSet<Vector2Int> neighboursCube = new HashSet<Vector2Int>(cubePositions);

        bool patternCompleted = CheckThreeInLineFromMiddle(pTile.gridPosition, neighboursCube);
        Debug.Log("Straight line pattern checked, outcome is " + patternCompleted);
    }
    
    // Function to check if there are three hexes in a line from a given middle hex
    public static bool CheckThreeInLineFromMiddle(Vector2Int middleHex, HashSet<Vector2Int> hexSet)
    {
        var directions = HexConversion.directions[middleHex.y % 2];
        foreach (var direction in directions)
        {
            if (IsLineOfThreeFromMiddle(middleHex, direction, hexSet))
            {
                return true;
            }
        }
        return false;
    }

    // Function to check if there are hexes in both directions forming a line of three from the middle hex
    private static bool IsLineOfThreeFromMiddle(Vector2Int middleHex, Vector2Int direction, HashSet<Vector2Int> hexSet)
    {
        Vector2Int firstHex = middleHex + direction;
        Vector2Int oppositeDirection = HexConversion.oppositeDirection[middleHex.y % 2][Array.IndexOf(HexConversion.directions[middleHex.y % 2], direction)];
        Vector2Int thirdHex = middleHex + oppositeDirection;

        return hexSet.Contains(firstHex) && hexSet.Contains(thirdHex);
    }

    #endregion
    
    
}

public enum HexagonalPatterns
{
    StraightLine,
    VShape,
    SingularAdjacent
}