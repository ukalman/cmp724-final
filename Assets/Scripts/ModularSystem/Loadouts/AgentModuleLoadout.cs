using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAgentModuleLoadout", menuName = "Modules/Agent/AgentModuleLoadout")]
public class AgentModuleLoadout : ScriptableObject
{
    public List<AgentModuleEntry> modules;
}