[System.Serializable]
public class AgentModuleEntry
{
    public ModuleType type;
    public ModuleConfigBase config; 
    public int priority;

    public enum ModuleType
    {
        Health,
        Movement,
        Combat,
        Inventory,
        Level,
        Skills,
        Stats
    }
}