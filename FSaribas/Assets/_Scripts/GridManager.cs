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

    private GridItem[,] m_GridItems;

    private List<GridItem> m_Neighbours = new List<GridItem>();

    private int m_TotalClearedCount;
    
    public Action<int> OnTotalClearedCountChanged;

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
    
    public int TotalClearedCount
    {
        get => m_TotalClearedCount;
        set
        {
            m_TotalClearedCount = value;
            OnTotalClearedCountChanged?.Invoke(m_TotalClearedCount);
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
        CreateGrid(5);
    }

    #endregion

    #region Public Methods

    public void CreateGrid(int n)
    {
        ClearGrid();
        float offsetX = (n - 1) * m_CellSize / 2;
        float offsetY = (n - 1) * m_CellSize / 2;

        m_GridItems = new GridItem[n, n];

        for (int row = 0; row < n; row++)
        {
            for (int column = 0; column < n; column++)
            {
                Vector3 cellPosition = new Vector3(column * m_CellSize - offsetX, row * m_CellSize - offsetY, 0);
                var obj = Instantiate(m_GridItemPrefab, cellPosition, Quaternion.identity, transform);
                m_ActiveGridItems.Add(obj);
                m_GridItems[row, column] = obj;
                obj.Init((row, column));
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
        if (item) item.OnItemClicked();

        if (m_GridItems != null)
        {
            var (row, column) = item.GetArrayPosition();
            
            m_Neighbours.Clear();
            
            CheckNeighbours(row, column);

            if (m_Neighbours.Count >= 3)
            {
                StartCoroutine(LateCleanNeighbours());
            }
        }
    }

    #endregion

    private IEnumerator LateCleanNeighbours()
    {
        yield return new WaitForSeconds(.5f);
        foreach (var gridItem in m_Neighbours)
        {
            gridItem.ResetItem();
            TotalClearedCount++;
        }
    }
    
    private void CheckNeighbours(int row, int column)
    {
        (int rowStep, int columStep)[] directions = new (int, int)[]
        {
            (1, 0), // Rightt
            (-1, 0), // Left
            (0, 1), // Up
            (0, -1) // Downn
        };

        var length = m_GridItems.GetLength(0); //Knowing that same lenght in row and column :)
        foreach (var direction in directions)
        {
            int currentRow = row + direction.rowStep;
            int currentColumn = column + direction.columStep;
            
            if(currentRow < 0 || currentRow >= length || currentColumn < 0 || currentColumn >= length) continue;
            
            GridItem neighbor = m_GridItems[currentRow, currentColumn];
            if (neighbor != null && neighbor.Selected && !m_Neighbours.Contains(neighbor))
            {
                m_Neighbours.Add(neighbor);
                var neighborPos = neighbor.GetArrayPosition();
                CheckNeighbours(neighborPos.row, neighborPos.column);
            }
        }
    }

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