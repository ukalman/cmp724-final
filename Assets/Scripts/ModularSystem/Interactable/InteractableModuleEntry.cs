[System.Serializable]
public class InteractableModuleEntry
{
    public ModuleType type;
    public ModuleConfigBase config; 
    public int priority;

    public enum ModuleType
    {
        LootModule
    }
}