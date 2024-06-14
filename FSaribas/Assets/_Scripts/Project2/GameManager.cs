using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields

    private static GameManager m_Instance;

    [SerializeField] private PlayerController m_PlayerController;

    #endregion

    #region Properties

    public static GameManager Instance
    {
        get
        {
            if (!m_Instance)
            {
                var go = new GameObject("GameManager");
                DontDestroyOnLoad(go);

                m_Instance = go.AddComponent<GameManager>();
            }

            return m_Instance;
        }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        m_PlayerController.PlayerMovementState = PLayerMovementState.Stopped;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_PlayerController && m_PlayerController.PlayerMovementState == PLayerMovementState.Stopped)
            {
                m_PlayerController.PlayerMovementState = PLayerMovementState.Running;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_PlayerController && m_PlayerController.PlayerMovementState == PLayerMovementState.Running)
        {
            m_PlayerController.transform.position += Vector3.forward * Time.fixedDeltaTime;
            CheckPlayerPos();
        }
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    private void CheckPlayerPos()
    {
        if (m_PlayerController && m_PlayerController.transform.position.z >= 35)
        {
            m_PlayerController.PlayerMovementState = PLayerMovementState.Dead;
        }
    }

    #endregion

    #region Buttons

    public void OnRestartPressed()
    {
        if (m_PlayerController && m_PlayerController.PlayerMovementState is PLayerMovementState.Running or PLayerMovementState.Dead)
        {
            m_PlayerController.PlayerMovementState = PLayerMovementState.Stopped;
            m_PlayerController.transform.position = Vector3.zero;
        }
    }

    #endregion
}