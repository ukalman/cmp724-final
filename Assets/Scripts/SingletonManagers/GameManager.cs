
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    
    public Dictionary<string, int> selectedSpecials = new Dictionary<string, int>();
    public Dictionary<string, int> selectedSkills = new Dictionary<string, int>();
    public string playerName = "";
    
    /* Actions - Events */
    public Action<UIPanelTypes,bool> OnUIPanelTriggered;
    
    /* Player Agent Controller */
    public AgentController PlayerController { get; private set; }
    
    /* Current Interactable Controller */
    public InteractableController CurrentInteractable { get; set; }

    private bool isPipBoyActive;
    
    /* Global game settings */
    public float MouseSensitivity = 1.0f;
    
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
