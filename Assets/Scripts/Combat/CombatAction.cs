using UnityEngine;

public enum DamageType { Normal, Fire, Explosive, Energy }

[CreateAssetMenu(fileName = "NewCombatAction", menuName = "Combat/Combat Action")]
public class CombatAction : ScriptableObject
{
    public string actionName = "Unnamed Action";
    public float apCost = 3.0f;
    public int ammoCost = 0;
    
    public DamageType damageType = DamageType.Normal;
    public int minDamage = 5;
    public int maxDamage = 10;

    [Range(-100.0f, 100.0f)]
    public float accuracyModifier = 0.0f;

    [Range(0.0f, 100.0f)]
    public float criticalChance = 0.0f;
    
    public SkillType requiredSkill = SkillType.None;
    public SkillType usedSkill = SkillType.Melee; // default
    public int requiredSkillLevel = 0;
    
    public bool isBurst;
    public float burstHitPenalty = 10.0f; // her atista -10% isabet
    public int burstCount = 1;
    
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