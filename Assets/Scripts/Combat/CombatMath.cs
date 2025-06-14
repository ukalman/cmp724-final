using UnityEngine;

public static class CombatMath
{
    public static float CalculateHitChance(
        int rawSkill, int targetAC, float accuracyModifier, float perception = 0.0f)
    {
        float normalizedSkill = (float)rawSkill / 20.0f * 100.0f;
    
        float baseChance = normalizedSkill - targetAC + accuracyModifier;
        float perceptionBonus = perception * 1.5f;
        float finalChance = (baseChance + perceptionBonus) * 2.0f;

        return Mathf.Clamp(finalChance, 1.0f, 70.0f);
    }


    public static float CalculateFinalDamage(float rawDamage, float damageModifier,
        float damageThreshold, float damageResistance, float strength = 0.0f)
    {
        float strengthBonus = 1.0f + (strength * 0.1f); // %10 bonus per STR
        float adjusted = rawDamage * damageModifier * strengthBonus;

        float afterDT = Mathf.Max(adjusted - damageThreshold, 0.0f);
        float reduced = afterDT * (1.0f - damageResistance);

        return Mathf.Floor(reduced);
    }

    public static bool CheckCriticalHit(float baseCritChance, float luck = 0.0f)
    {
        float totalCritChance = baseCritChance + (luck * 1.5f); // %1.5 crit bonus per LCK
        return Random.Range(0.0f, 100.0f) < totalCritChance;
    }
    
    public static float GetMeleeDamageModifier(float strength = 0.0f)
    {
        return 1.0f + strength * 0.1f; // %10 per STR
    }
}