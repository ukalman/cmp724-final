using UnityEngine;

public static class ModuleFactory
{
    public static IModuleBase CreateModule(ModuleEntry entry)
    {
        switch (entry.type)
        {
            case ModuleEntry.ModuleType.Health:
                var healthConfig = entry.config as HealthModuleConfig;
                return new HealthModule(healthConfig);

            case ModuleEntry.ModuleType.PlayerMovement:
                var moveConfig = entry.config as MovementModuleConfig;
                return new MovementModule(moveConfig);

            case ModuleEntry.ModuleType.Combat:
                var combatConfig = entry.config as CombatModuleConfig;
                return new CombatModule(combatConfig); 

            default:
                Debug.LogError($"Unsupported module type: {entry.type}");
                return null;
        }
    }
}