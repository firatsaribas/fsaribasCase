using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    #region Fields

    [SerializeField] private Button m_Button;
    [SerializeField] private TMP_InputField m_InputField;
    [SerializeField] private TextMeshProUGUI m_TotalClearedCountText;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        if(m_Button) m_Button.onClick.AddListener(OnButtonPressed);
        if (GridManager.Instance)
        {
            GridManager.Instance.OnTotalClearedCountChanged += OnTotalClearedCountChanged;
            OnTotalClearedCountChanged(GridManager.Instance.TotalClearedCount);
        }
    }

    private void OnDestroy()
    {
        if(m_Button) m_Button.onClick.RemoveListener(OnButtonPressed);
        if (GridManager.Instance)
        {
            GridManager.Instance.OnTotalClearedCountChanged += OnTotalClearedCountChanged;
        }
    }

    #endregion

    #region Button Listeners

    private void OnButtonPressed()
    {
        if(int.TryParse(m_InputField.text, out int result))
        {
            GridManager.Instance.CreateGrid(result);
        }
        else
        {
            //This should also be prevented from unity tmp settings that accepts int only
            Debug.Log($"Conversion of {m_InputField.text} failed");
        }
    }
    
    private void OnTotalClearedCountChanged(int count)
    {
        if(m_TotalClearedCountText) m_TotalClearedCountText.text = $"CLEARED: {count}";
    }

    #endregion
}
