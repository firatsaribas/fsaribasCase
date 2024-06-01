using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Fields

    private static GridManager m_Instance;
    
    [SerializeField] private float m_CellSize = 1;
    [SerializeField] private GridItem m_GridItemPrefab;
    
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

    private void Start()
    {
        CreateGrid(5);
    }

    #endregion

    #region Public Methods

    
    #endregion

    #region Private Methods
    
    private void CreateGrid(int n)
    {
        float offsetX = (n - 1) * m_CellSize / 2;
        float offsetY = (n - 1) * m_CellSize / 2;

        for (int row = 0; row < n; row++)
        {
            for (int column = 0; column < n; column++)
            {
                Vector3 cellPosition = new Vector3(column * m_CellSize - offsetX, row * m_CellSize - offsetY, 0);
                Instantiate(m_GridItemPrefab, cellPosition, Quaternion.identity, transform);
            }
        }
    }

    #endregion
}
