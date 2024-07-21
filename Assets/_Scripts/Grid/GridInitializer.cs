using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GridInitializer : MonoBehaviour
{
    [Header("Settings")] 
    [MinValue(1),MaxValue(10)]
    [SerializeField] private int _gridWidth;
    [MinValue(1),MaxValue(10)]
    [SerializeField] private int _gridHeight;
    
    
    [Header("References")] 
    [SerializeField] private InputManager _inputManager;

    
    [SerializeField] private Camera _cam;
    [SerializeField] private Grid _grid;
    [SerializeField] private Transform _gridParent;
    [SerializeField] private Tile _groundPrefab;
    [SerializeField] private Block _blockBase;
    [SerializeField] private Tile[,] _gridObjects;
    
    //Added for testing the conditions of blocks
    private Block _currentBlock;

    private void Awake()
    {
        _inputManager.OnClicked += PlaceObject;
    }

    private void Start()
    {
        _gridObjects = new Tile[_gridWidth, _gridHeight];
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                Vector3 position = _grid.GetCellCenterWorld(new Vector3Int(i, j, 0));
                Tile spawnedGameObject = Instantiate(_groundPrefab, position,_groundPrefab.transform.rotation,_gridParent);
                _gridObjects[i, j] = spawnedGameObject;
                spawnedGameObject.transform.rotation = _grid.transform.rotation;
                spawnedGameObject.GetComponent<MeshRenderer>().material.color = AlternateColors(i,j);
                spawnedGameObject.name = $"Tile {i}.{j}";
                spawnedGameObject.gridPosition = new Vector2Int(i, j);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _inputManager.OnClicked += PlaceObject;
            _inputManager.OnClicked -= DisplayCoords;
            Debug.Log("Switched to block placement mode");
        } 
        else if (Input.GetKeyDown(KeyCode.N))
        {
            _inputManager.OnClicked += DisplayCoords;
            _inputManager.OnClicked -= PlaceObject;
            Debug.Log("Switched to display coordinates mode");
        }
    }

    private static Color AlternateColors(int i, int j)
    {
        if (i % 2 == 0)
        {
            return j % 2 == 0 ? Color.black : Color.white;
        }

        return j % 2 == 0 ? Color.white : Color.black;
    }
    
    //Added for testing the conditions of blocks
    public void SetCurrentBlock(BlockData pBlockData)
    {
        _currentBlock = _blockBase;
        _currentBlock.BlockData = pBlockData;
        Debug.Log("Switch current block to:"  + _currentBlock.BlockData.blockName);
    }

    private void DisplayCoords()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _cam.nearClipPlane;
        Ray ray = _cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;
        
        Vector3Int cellPosition = _grid.WorldToCell(hit.point);
        
        if (cellPosition.x < 0 || cellPosition.x >= _gridWidth || cellPosition.y < 0 ||
            cellPosition.y >= _gridHeight) return;
        
        Tile tile = _gridObjects[cellPosition.x, cellPosition.y];
        Debug.Log($"Tile: {tile.name} at position: {tile.gridPosition}. Parity: {tile.gridPosition.y % 2}");
        Tile[] neighbours = GetNeighbours(tile);
        
        HexagonalPatternChecker.CheckPattern(HexagonalPatterns.SingularAdjacent, tile, neighbours);
    }
    
    
    private Tile[] GetNeighbours(Tile pTile)
    {
        Vector3Int[] neighbourPositions = new Vector3Int[6];
        
        Dictionary<int, Vector2Int[]> evenRDirections = HexConversion.directions;
        
        List<Tile> neighbours = new List<Tile>();
        string neighbourString = "";
        
        Vector2Int[] directions = evenRDirections[pTile.gridPosition.y % 2];
        
        neighbourString += "Parity:" + pTile.gridPosition.y % 2 + " \n";
        
       

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourPosition = pTile.gridPosition + direction;
            neighbourString += neighbourPosition + "\n";
            if (neighbourPosition.x < 0 || neighbourPosition.x >= _gridWidth || neighbourPosition.y < 0 ||
                neighbourPosition.y >= _gridHeight) continue;
            neighbours.Add(_gridObjects[neighbourPosition.x, neighbourPosition.y]);
        }
        
        Debug.Log(neighbourString);
        return neighbours.ToArray();
    }

    /// <summary>
    /// Places the object on the grid with the use of Raycasting onto the grid.
    /// </summary>
    private void PlaceObject()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _cam.nearClipPlane;
        Ray ray = _cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;
        
        Vector3Int cellPosition = _grid.WorldToCell(hit.point);
        
        if (cellPosition.x < 0 || cellPosition.x >= _gridWidth || cellPosition.y < 0 ||
            cellPosition.y >= _gridHeight) return;
        
        Tile tile = _gridObjects[cellPosition.x, cellPosition.y];

        Vector3 worldPositionBlock = _grid.CellToWorld(cellPosition);
        
        worldPositionBlock.y = _grid.cellSize.z + (tile.GetBlockCount() * _grid.cellSize.z); //grid is XZY, Unity is XYZ

        //May need to change this to a switch statement
        if (tile.IsOccupied)
        {
            //Check if the block is valid to place
            Block foundationBlock = tile.GetTopBlock();
            BlockData validFoundation = _currentBlock.BlockData.validFoundation;
            
            if (foundationBlock == null)
            {
                SpawnBlock(worldPositionBlock, tile);
                Debug.Log("Foundation block is null, placing block on top of it.");
            }
            else
            {
                
                if (foundationBlock.BlockData == validFoundation)
                {
                    Debug.Log($"Foundation block is: {foundationBlock.BlockData.blockName} and valid foundation is: {validFoundation.blockName} ");
                    SpawnBlock(worldPositionBlock, tile);
                }
                else
                {
                    Debug.Log("Invalid placement");
                }
            }
        }
        else
        {
            SpawnBlock(worldPositionBlock, tile);
            Debug.Log("Tile is not occupied, placing block on top of it.");
        }
    }

    private void SpawnBlock(Vector3 worldPositionBlock, Tile tile)
    {
        Block spawnedBlock = Instantiate(_currentBlock.BlockData.blockPrefab, worldPositionBlock,
            _currentBlock.BlockData.blockPrefab.transform.rotation, tile.transform);
        spawnedBlock.BlockData = _currentBlock.BlockData;
        tile.AddBlock(spawnedBlock);
    }
}
