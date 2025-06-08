using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractableModuleLoadout", menuName = "Modules/Interactable/InteractableModuleLoadout")]
public class InteractableModuleLoadout : ScriptableObject
{
    public List<InteractableModuleEntry> modules;
}