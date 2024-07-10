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
    [SerializeField] private Tile _groundPrefab;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private Tile[,] _gridObjects;

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
                Tile spawnedGameObject = Instantiate(_groundPrefab, position,_groundPrefab.transform.rotation);
                _gridObjects[i, j] = spawnedGameObject;
                spawnedGameObject.transform.parent = transform;
                spawnedGameObject.GetComponent<MeshRenderer>().material.color = AlternateColors(i,j);
            }
        }
    }

    private static Color AlternateColors(int i, int j)
    {
        if (i % 2 == 0)
        {
            return j % 2 == 0 ? Color.black : Color.white;
        }
        else
        {
            return j % 2 == 0 ? Color.white : Color.black;
        }
    }

    private void PlaceObject()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _cam.nearClipPlane;
        Ray ray = _cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;
        
        Vector3Int cellPosition = _grid.WorldToCell(hit.point);
        if (cellPosition.x >= 0 && cellPosition.x < _gridWidth && cellPosition.y >= 0 && cellPosition.y < _gridHeight)
        {
            Tile selectedObject = _gridObjects[cellPosition.x, cellPosition.y];
            if(selectedObject.IsOccupied) return;
            selectedObject.IsOccupied = true;
            Vector3Int blockCellPosition = cellPosition + new Vector3Int(0, 0, 1); //grid is XZY, Unity is XYZ
            Instantiate(_blockPrefab,_grid.CellToWorld(blockCellPosition), _blockPrefab.transform.rotation);
        }
    }
}
