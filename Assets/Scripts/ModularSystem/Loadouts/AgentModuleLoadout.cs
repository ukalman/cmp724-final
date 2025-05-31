using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAgentModuleLoadout", menuName = "Modules/Agent Loadout")]
public class AgentModuleLoadout : ScriptableObject
{
    public List<ModuleEntry> modules;
}