using UnityEngine;

public interface IInteractable
{
    void Interact(AgentController interactor);
    bool CanInteract(AgentController interactor);
    string GetPrompt();
}