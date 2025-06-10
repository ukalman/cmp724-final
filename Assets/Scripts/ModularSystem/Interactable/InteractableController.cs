using System;
using UnityEngine;

public class InteractableController : ModuleController, IInteractable
{
    public string InteractableName { get; protected set; }
    
    [SerializeField] private bool isInteractable = true;

    [SerializeField] private InteractableModuleLoadout _moduleLoadout;

    [SerializeField] private InteractionTypes _interactionType;
    
    protected bool _isPlayerInZone = false;
    
    protected virtual void Awake()
    {
        if (_moduleLoadout == null)
        {
            Debug.LogError("Module Loadout is not assigned to AgentController.");
            return;
        }

        foreach (var entry in _moduleLoadout.modules)
        {
            var module = InteractableModuleFactory.CreateModule(entry);
            if (module != null)
                AddModule(module);
        }
    }
    
    protected virtual void Start()
    {
        ActivateModules();
    }
    
    protected virtual void Update()
    {
        if (_isPlayerInZone && isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacting...");
            Interact(GameManager.Instance.PlayerController);
        }
    }

    
    public virtual bool CanInteract(AgentController agent)
    {
        return isInteractable;
    }

    public virtual void OnInteractionZoneEntered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInZone = true;
            Debug.Log("Player interaction zone entered.");
        }
    }

    public virtual void OnInteractionZoneExited(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInZone = false;
            Debug.Log("Player interaction zone exited.");
        }
    }
    
    public virtual void Interact(AgentController agent)
    {
        Debug.Log($"{InteractableName} interacted by {agent.name}");
        switch (_interactionType)
        {
            case InteractionTypes.Loot:
                GameManager.Instance.CurrentInteractable = this;
                GameManager.Instance.OnUIPanelTriggered?.Invoke(UIPanelTypes.Loot, true);
                break;
            case InteractionTypes.Dialogue:
                break;
            case InteractionTypes.Terminal:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        isInteractable = false;
        // Bu metod alt sınıflar tarafından override edilebilir
    }
    
    public virtual string GetPrompt()
    {
        return string.Empty;
    }

    public virtual void OnInteractionEnded()
    {
        isInteractable = true;
    }
}


    
