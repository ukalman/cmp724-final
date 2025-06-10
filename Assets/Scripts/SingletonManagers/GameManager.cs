
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    /* Actions - Events */
    public Action<UIPanelTypes,bool> OnUIPanelTriggered;
    
    /* Player Agent Controller */
    public AgentController PlayerController { get; private set; }
    
    /* Current Interactable Controller */
    public InteractableController CurrentInteractable { get; set; }
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void RegisterPlayer(AgentController agentController)
    {
        PlayerController = agentController;
    }
}
