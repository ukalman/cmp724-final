using UnityEngine;

public class InteractableController : ModuleController, IInteractable
{
    public string InteractableName { get; protected set; }
    
    [SerializeField] private bool isInteractable = true;

    [SerializeField] private InteractableModuleLoadout _moduleLoadout;

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
    
    public virtual bool CanInteract(AgentController agent)
    {
        return isInteractable;
    }

    public virtual void Interact(AgentController agent)
    {
        Debug.Log($"{InteractableName} interacted by {agent.name}");
        // Bu metod alt sınıflar tarafından override edilebilir
    }
    
    public virtual string GetPrompt()
    {
        return string.Empty;
    }
}


    
