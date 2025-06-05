using UnityEngine;

public enum DamageType { Normal, Fire, Explosive, Energy }

[CreateAssetMenu(fileName = "NewCombatAction", menuName = "Combat/Combat Action")]
public class CombatAction : ScriptableObject
{
    public string actionName = "Unnamed Action";
    public float apCost = 3.0f;

    public DamageType damageType = DamageType.Normal;
    public int minDamage = 5;
    public int maxDamage = 10;

    [Range(-100.0f, 100.0f)]
    public float accuracyModifier = 0.0f;

    [Range(0.0f, 100.0f)]
    public float criticalChance = 0.0f;

    public bool isBurst;
    public bool isAimed;

    public int RollRawDamage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }
    
    public override string ToString()
    {
        return $"{actionName} (AP: {apCost}, DMG: {minDamage}-{maxDamage}, Crit: {criticalChance}%)";
    }

}