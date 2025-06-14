using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CombatPanelController : MonoBehaviour
{
    [Header("Agent & Modules")]
    private AgentController playerController;
    private AgentController enemyController;
    private CombatModule combatModule;

    [Header("UI References")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI apText;

    [Header("Available Actions")]
    public Transform availableActionsParent;
    public GameObject actionButtonPrefab;

    [Header("Queued Actions")]
    public Transform queuedActionsParent;
    public GameObject queuedActionButtonPrefab;

    [Header("Buttons")]
    public Button queueButton;
    public Button dequeueButton;
    public Button endTurnButton;
    
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TMP_Text descriptionText;
    
    private CombatAction selectedAction;
    
    [SerializeField] private TMP_Text battleLogText;
    [SerializeField] private TMP_Text ammoText;
    
    
    private Queue<string> logQueue = new Queue<string>();
    private const int maxLogLines = 4;
    
    private void Start()
    {
        playerController = GameManager.Instance.PlayerController;
        enemyController = CombatManager.Instance._enemyCombat.Controller as AgentController;

        ammoText.text = playerController.GetModule<InventoryModule>().TotalAmmo.ToString();
        descriptionPanel.SetActive(false);
        
        combatModule = playerController.GetModule<CombatModule>();

        queueButton.onClick.AddListener(OnQueueAction);
        dequeueButton.onClick.AddListener(OnDequeueAction);
        endTurnButton.onClick.AddListener(OnEndTurn);

        queueButton.interactable = false;

        RenderAvailableActions();
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
        if (CombatManager.Instance.isPlayerTurn)
        {
            queueButton.interactable = true;
            dequeueButton.interactable = true;
            endTurnButton.interactable = true;
        }
        else
        {
            UpdateQueuedActions();
            queueButton.interactable = false;
            dequeueButton.interactable = false;
            endTurnButton.interactable = false;
        }
    }
    public void AddLog(string message)
    {
        if (logQueue.Count >= maxLogLines)
            logQueue.Dequeue(); 

        logQueue.Enqueue(message);
        
        battleLogText.text = string.Join("\n", logQueue);
    }
    

    private void RenderAvailableActions()
    {
        foreach (Transform child in availableActionsParent)
            Destroy(child.gameObject);

        foreach (CombatAction action in combatModule.GetAvailableActions())
        {
            GameObject btnObj = Instantiate(actionButtonPrefab, availableActionsParent);
            Image iconImage = btnObj.GetComponent<Image>();
            iconImage.sprite = action.icon;

            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => OnSelectAction(action));

            //AddHoverHandlers(btnObj, action.ToString());
        }
    }

    private void OnSelectAction(CombatAction action)
    {
        selectedAction = action;
        descriptionPanel.SetActive(true);
        descriptionText.text = action.ToString();
        queueButton.interactable = combatModule.CanPerformAction(action);
    }

    private void OnQueueAction()
    {
        if (selectedAction == null)
            return;

        if (playerController.GetModule<InventoryModule>().TotalAmmo - selectedAction.ammoCost < 0)
        { 
            FindObjectOfType<CombatPanelController>().AddLog($"{playerController.AgentName} hasn't enough ammo!");
            return;
        }
        
        combatModule.QueueAction(selectedAction);
        //selectedAction = null;
        queueButton.interactable = false;

        UpdateQueuedActions();
        UpdateUI();
    }

    private void OnDequeueAction()
    {
        combatModule.DequeueLastAction();
        UpdateQueuedActions();
        UpdateUI();
    }

    private void OnEndTurn()
    {
        //combatModule.EndTurn(); // Bu metodu CombatModule'a sen eklemiş olmalısın
        CombatManager.Instance.isPlayerTurn = false;
        UpdateQueuedActions();
        UpdateUI();
    }

    private void UpdateQueuedActions()
    {
        foreach (Transform child in queuedActionsParent)
            Destroy(child.gameObject);

        foreach (CombatAction action in combatModule.QueuedActions)
        {
            GameObject btnObj = Instantiate(queuedActionButtonPrefab, queuedActionsParent);
            Image iconImage = btnObj.GetComponent<Image>();
            iconImage.sprite = action.icon;

            //AddHoverHandlers(btnObj, action.ToString());
        }
    }

    private void UpdateUI()
    {
        apText.text = $"AP: {combatModule.CurrentAP:0.0}";
        playerHealthText.text = $"Player HP: {playerController.GetModule<HealthModule>().GetHealth()}";
        enemyHealthText.text = $"{enemyController.AgentName} HP: {enemyController.GetModule<HealthModule>().GetHealth()}";
        ammoText.text = playerController.GetModule<InventoryModule>().TotalAmmo.ToString();
    }

    /*
    private void AddHoverHandlers(GameObject buttonObj, string description)
    {
        EventTrigger trigger = buttonObj.AddComponent<EventTrigger>();

        EventTrigger.Entry enter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        enter.callback.AddListener((data) =>
        {
            descriptionTooltip.SetActive(true);
            descriptionTooltip.GetComponentInChildren<TMP_Text>().text = description;
            //descriptionText.text = description;
        });

        EventTrigger.Entry exit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        exit.callback.AddListener((data) =>
        {
            descriptionTooltip.SetActive(false);
        });

        Vector2 mousePos = Input.mousePosition;
        descriptionTooltip.transform.position = mousePos + new Vector2(15, -15);
        
        trigger.triggers.Add(enter);
        trigger.triggers.Add(exit);
    }
    */
}
