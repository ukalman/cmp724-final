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
            
            case AgentModuleEntry.ModuleType.Inventory:
                var inventoryConfig = entry.config as InventoryModuleConfig;
                return new InventoryModule(inventoryConfig); 

            case AgentModuleEntry.ModuleType.Stats:
                var statsConfig = entry.config as StatsModuleConfig;
                return new StatsModule(statsConfig); 
            
            case AgentModuleEntry.ModuleType.Skills:
                var skillsConfig = entry.config as SkillsModuleConfig;
                return new SkillsModule(skillsConfig); 
            
            case AgentModuleEntry.ModuleType.Level:
                var levelConfig = entry.config as LevelModuleConfig;
                return new LevelModule(levelConfig); 

            
            default:
                Debug.LogError($"Unsupported module type: {entry.type}");
                return null;
        }
    }
}