using UnityEngine;

public static class CombatMath
{
    public static float CalculateHitChance(int attackerSkill, int targetAC, float accuracyModifier)
    {
        float chance = attackerSkill - targetAC + accuracyModifier;
        return Mathf.Clamp(chance, 1.0f, 95.0f); // Fallout sınırı
    }
    
    public static int CalculateFinalDamage(int rawDamage, float damageModifier,
        int damageThreshold, float damageResistance)
    {
        float adjusted = rawDamage * damageModifier;
        float afterDT = Mathf.Max(adjusted - damageThreshold, 0.0f);
        float reduced = afterDT * (1.0f - damageResistance);
        return Mathf.FloorToInt(reduced);
    }
}