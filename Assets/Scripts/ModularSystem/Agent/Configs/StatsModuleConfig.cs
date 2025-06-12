using UnityEngine;

[CreateAssetMenu(fileName = "StatsModuleConfig", menuName = "Modules/Agent/Configs/Stats")]
public class StatsModuleConfig : ModuleConfigBase
{
    [Range(1, 20)] public int strength;
    [Range(1, 20)] public int perception;
    [Range(1, 20)] public int endurance;
    [Range(1, 20)] public int charisma;
    [Range(1, 20)] public int intelligence;
    [Range(1, 20)] public int agility;
    [Range(1, 20)] public int luck;
}