
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

    public bool isPipBoyActive;
    
    /* Global game settings */
    public float MouseSensitivity = 1.0f;

    public bool QuestTaken;
    public bool ItemRetrieved;

    public bool IsGamePaused;
    
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

    public void SwitchToDialogueCamera()
    {
        if (CombatManager.Instance.mainCamera != null) CombatManager.Instance.mainCamera.enabled = false;
        if (CombatManager.Instance.dialogueCamera != null) CombatManager.Instance.dialogueCamera.enabled = true;
    }

    public void SwitchToMainCamera()
    {
        if (CombatManager.Instance.dialogueCamera != null) CombatManager.Instance.dialogueCamera.enabled = false;
        if (CombatManager.Instance.mainCamera != null) CombatManager.Instance.mainCamera.enabled = true;
    }
    
    public void OnQuestAccepted()
    {
        Debug.Log("QUEST ACCEPTED!");
        QuestTaken = true;
        ItemRetrieved = false;
    }

    public void OnQuestRejected()
    {
        Debug.Log("QUEST REJECTED!");
        QuestTaken = false;
    }
}
