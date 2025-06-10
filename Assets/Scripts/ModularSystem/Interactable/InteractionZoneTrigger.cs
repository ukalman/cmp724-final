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
        Debug.Log("Player entered");
        _interactable.OnInteractionZoneEntered(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited");
        _interactable.OnInteractionZoneExited(other);
    }
}