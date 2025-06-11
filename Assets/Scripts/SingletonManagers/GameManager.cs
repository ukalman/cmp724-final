
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

    private bool isPipBoyActive;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        isPipBoyActive = false;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isPipBoyActive = !isPipBoyActive;
            OnUIPanelTriggered?.Invoke(UIPanelTypes.PipBoy,isPipBoyActive);
        }
    }

    public void RegisterPlayer(AgentController agentController)
    {
        PlayerController = agentController;
    }
}
