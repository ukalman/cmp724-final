using UnityEngine;

public enum InteractionTypes
{
    Loot,
    Dialogue,
    Terminal
}

public interface IInteractable
{
    void Interact(AgentController interactor);
    bool CanInteract(AgentController interactor);
    string GetPrompt();
}