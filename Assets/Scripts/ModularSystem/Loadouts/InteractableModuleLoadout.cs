using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractableModuleLoadout", menuName = "Modules/Interactable Loadout")]
public class InteractableModuleLoadout : ScriptableObject
{
    public List<InteractableModuleEntry> modules;
}