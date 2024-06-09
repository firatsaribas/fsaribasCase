using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private LayerMask m_GridItemMask;
    private Camera m_MainCamera;
    private const string m_GridItemTag = "GridItem";

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RayControl();
        }
    }

    private void RayControl()
    {
        var ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, m_MainCamera.farClipPlane, m_GridItemMask))
        {
            var gridItem = hit.transform.GetComponent<GridItem>();
            if (gridItem)
            {
                GridManager.Instance.GridItemClicked(gridItem);    
            }
        }
    }
}