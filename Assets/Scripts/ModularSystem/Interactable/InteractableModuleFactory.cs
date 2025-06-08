using System;
using UnityEngine;

public static class InteractableModuleFactory
{
    public static IModuleBase CreateModule(InteractableModuleEntry entry)
    {
        switch (entry.type)
        {
            case InteractableModuleEntry.ModuleType.LootModule:
                var lootConfig = entry.config as LootModuleConfig;
                return new LootModule(lootConfig);
            default:
                Debug.LogError($"Unsupported module type: {entry.type}");
                return null;
        }
    }
}