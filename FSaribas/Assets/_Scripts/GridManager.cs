using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Fields

    private static GridManager m_Instance;
    
    [SerializeField] private float m_CellSize = 1;
    [SerializeField] private GridItem m_GridItemPrefab;
    private List<GridItem> m_ActiveGridItems = new List<GridItem>();
    [SerializeField] private CinemachineTargetGroup m_CinemachineTargetGroup;
    #endregion

    #region Properties

    public static GridManager Instance
    {
        get
        {
            if (!m_Instance)
            {
                var go = new GameObject("GridManager");
                DontDestroyOnLoad(go);

                m_Instance = go.AddComponent<GridManager>();
            }
            return m_Instance;
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        CreateGrid(3);
    }

    #endregion

    #region Public Methods

    public void CreateGrid(int n)
    {
        ClearGrid();
        float offsetX = (n - 1) * m_CellSize / 2;
        float offsetY = (n - 1) * m_CellSize / 2;

        for (int row = 0; row < n; row++)
        {
            for (int column = 0; column < n; column++)
            {
                Vector3 cellPosition = new Vector3(column * m_CellSize - offsetX, row * m_CellSize - offsetY, 0);
                var obj = Instantiate(m_GridItemPrefab, cellPosition, Quaternion.identity, transform);
                m_ActiveGridItems.Add(obj);
                //Cinemachine target group used for camera re-fitting
                if (m_CinemachineTargetGroup)
                {
                    m_CinemachineTargetGroup.AddMember(obj.transform, 1, 1.25f);
                }
            }
        }
    }
    
    public void GridItemClicked(GridItem item)
    {
        Debug.Log("Grid Item Clicked: " + item.name);
    }
    
    #endregion

    #region Private Methods
    
    private void ClearGrid()
    {
        for (int i = 0; i < m_ActiveGridItems.Count; i++)
        {
            var item = m_ActiveGridItems[i];
            if (item)
            {
                if (m_CinemachineTargetGroup)
                {
                    m_CinemachineTargetGroup.RemoveMember(item.transform);
                }
                Destroy(item.gameObject);   
            }
        }
        m_ActiveGridItems.Clear();
    }

    #endregion
}
