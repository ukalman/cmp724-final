using UnityEngine;

public abstract class InteractionModule : ModuleBase
{
    public abstract void Interact(GameObject interactor);
    public virtual bool CanInteract(GameObject interactor) => true;
    public virtual string GetPrompt() => "Interact";
}