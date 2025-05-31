using UnityEngine;

public static class ModuleFactory
{
    public static IModuleBase CreateModule(ModuleEntry entry)
    {
        switch (entry.type)
        {
            case ModuleEntry.ModuleType.Health:
                //var healthConfig = entry.config as HealthModuleConfig;
                //return new HealthModule(healthConfig);

            case ModuleEntry.ModuleType.PlayerMovement:
                //var moveConfig = entry.config as PlayerMovementConfig;
                //return new PlayerMovementModule(moveConfig);

            case ModuleEntry.ModuleType.Combat:
                //return new CombatModule(); 

            default:
                Debug.LogError($"Unsupported module type: {entry.type}");
                return null;
        }
    }
}