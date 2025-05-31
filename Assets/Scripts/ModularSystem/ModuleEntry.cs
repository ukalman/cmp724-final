[System.Serializable]
public class ModuleEntry
{
    public ModuleType type;
    public ModuleConfigBase config; 
    public int priority;

    public enum ModuleType
    {
        Health,
        PlayerMovement,
        Combat
    }
}