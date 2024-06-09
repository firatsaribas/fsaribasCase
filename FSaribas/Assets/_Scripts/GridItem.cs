using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform m_XTransform;
    
    private (int row, int column) m_ArrayPosition;

    private bool m_Selected;
    
    #endregion

    #region Properties
    
    public bool Selected => m_Selected;

    #endregion

    #region Public Methods

    public void Init((int row, int column) arrayPosition)
    {
        m_ArrayPosition = arrayPosition;
        m_Selected = false;
        UpdateItem();
    }

    public void OnItemClicked()
    {
        m_Selected = true;
        UpdateItem();
    }

    public void ResetItem()
    {
        m_Selected = false;
        UpdateItem();
    }
    
    public (int row, int column) GetArrayPosition()
    {
        return m_ArrayPosition;
    }

    #endregion

    #region Private Methods

    private void UpdateItem()
    {
        m_XTransform.gameObject.SetActive(m_Selected);
    }

    #endregion
}
