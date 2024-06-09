using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform m_XTransform;
    
    private (int row, int column) m_ArrayPosition;

    private bool m_Selected;

    private Vector3 m_OrgScale;

    #endregion

    #region Properties
    
    public bool Selected => m_Selected;

    #endregion

    #region Public Methods

    public void Init((int row, int column) arrayPosition)
    {
        m_ArrayPosition = arrayPosition;
        m_Selected = false;
        m_XTransform.transform.localScale = Vector3.one;
        UpdateItem();
     
        m_OrgScale = transform.localScale;
        if(DOTween.IsTweening(transform)) transform.DOKill();
        transform.localScale = Vector3.zero;
        transform.DOScale(m_OrgScale, .25f).SetEase(Ease.OutBack);
    }

    public void OnItemClicked()
    {
        m_Selected = true;
        UpdateItem();
    }

    public void ResetItem()
    {
        m_Selected = false;
        // UpdateItem();
        ResetAnimation();
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

    private void ResetAnimation()
    {
        m_XTransform.transform.DOScale(Vector3.zero, .25f).SetDelay(.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            m_XTransform.transform.localScale = Vector3.one;
            UpdateItem();
        });
    }

    #endregion
}
