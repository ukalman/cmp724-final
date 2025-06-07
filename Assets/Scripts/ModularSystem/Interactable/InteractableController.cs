using UnityEngine;

public class InteractableController : ModuleController, IInteractable
{
    [SerializeField] private string interactableName = "Unnamed Interactable";
    [SerializeField] private bool isInteractable = true;

    public virtual string GetInteractableName() => interactableName;

    public virtual bool CanInteract(AgentController agent)
    {
        return isInteractable;
    }

    public virtual void Interact(AgentController agent)
    {
        Debug.Log($"{interactableName} interacted by {agent.name}");
        // Bu metod alt sınıflar tarafından override edilebilir
    }
    
    public virtual string GetPrompt()
    {
        return string.Empty;
    }

    protected virtual void Awake()
    {
        // Türeyen sınıflar bunu çağırmalı
    }

    protected virtual void Start()
    {
        ActivateModules();
    }
}


    
