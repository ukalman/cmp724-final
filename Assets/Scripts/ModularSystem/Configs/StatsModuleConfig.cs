using UnityEngine;

[CreateAssetMenu(fileName = "StatsModuleConfig", menuName = "Modules/Configs/Stats")]
public class StatsModuleConfig : ModuleConfigBase
{
    [Range(1, 20)] public int strength = 5;
    [Range(1, 20)] public int perception = 5;
    [Range(1, 20)] public int endurance = 5;
    [Range(1, 20)] public int charisma = 5;
    [Range(1, 20)] public int intelligence = 5;
    [Range(1, 20)] public int agility = 5;
    [Range(1, 20)] public int luck = 5;
}