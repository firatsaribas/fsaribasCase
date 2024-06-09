using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Animator m_PlayerAnimator;

    private const  string m_RunTrigger = "Run";
    private const  string m_Idle = "Idle";
    private const string m_Dance = "Dance";
    
    private PLayerMovementState m_PlayerMovementState = PLayerMovementState.Stopped;
    
    #endregion

    #region Properties
    
    public PLayerMovementState PlayerMovementState
    {
        get => m_PlayerMovementState;
        set
        {
            if (m_PlayerMovementState != value)
            {
                m_PlayerMovementState = value;   
                OnStateChanged();
            }
        }
    }

    #endregion

    #region Unity Methods
    
    
    #endregion

    #region Public Methods


    #endregion

    #region Private Methods
    
    private void OnStateChanged()
    {
        switch (m_PlayerMovementState)
        {
            case PLayerMovementState.Stopped:
                gameObject.SetActive(true);
                if(m_PlayerAnimator) m_PlayerAnimator.SetTrigger(m_Idle);
                break;
            case PLayerMovementState.Running:
                if(!gameObject.activeInHierarchy) gameObject.SetActive(true);
                if(m_PlayerAnimator) m_PlayerAnimator.SetTrigger(m_RunTrigger);
                break;
            case PLayerMovementState.Dead:
                gameObject.SetActive(false);
                //TODO: Ragdoll opening
                break;
            case PLayerMovementState.Dance:
                if(!gameObject.activeInHierarchy) gameObject.SetActive(true);
                if(m_PlayerAnimator) m_PlayerAnimator.SetTrigger(m_Dance);
                 break;
        }
    }


    #endregion
    
   
}

public enum PLayerMovementState
{
    Stopped,
    Running,
    Dead,
    Dance
}