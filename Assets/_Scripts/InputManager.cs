using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 _lastMousePosition;
    [SerializeField] private LayerMask _layerMask;

    public Action OnClicked;
    public Action OnExit;
        

        
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }
        
    public bool IsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
        
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _camera.nearClipPlane;
        Ray ray = _camera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
        {
            _lastMousePosition = hit.point;
        }

        return _lastMousePosition;

    }
}
