using UnityEngine;

[CreateAssetMenu(fileName = "LevelModuleConfig", menuName = "Modules/Configs/Level")]
public class LevelModuleConfig : ModuleConfigBase
{
    public int startingLevel = 1;
    public int startingXP = 0;
}