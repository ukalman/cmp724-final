using UnityEngine;

public static class AgentModuleFactory
{
    public static IModuleBase CreateModule(AgentModuleEntry entry)
    {
        switch (entry.type)
        {
            case AgentModuleEntry.ModuleType.Health:
                var healthConfig = entry.config as HealthModuleConfig;
                return new HealthModule(healthConfig);

            case AgentModuleEntry.ModuleType.Movement:
                var moveConfig = entry.config as MovementModuleConfig;
                return new MovementModule(moveConfig);

            case AgentModuleEntry.ModuleType.Combat:
                var combatConfig = entry.config as CombatModuleConfig;
                return new CombatModule(combatConfig); 

            default:
                Debug.LogError($"Unsupported module type: {entry.type}");
                return null;
        }
    }
}