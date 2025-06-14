using UnityEngine;

public class InteractionZoneTrigger : MonoBehaviour
{
    private InteractableController _interactable;

    private void Awake()
    {
        _interactable = GetComponentInParent<InteractableController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_interactable.CanInteract())
        {
            _interactable.OnInteractionZoneEntered(other);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        _interactable.OnInteractionZoneExited(other);
    }
}