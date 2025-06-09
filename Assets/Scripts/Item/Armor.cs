using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Armor : Item
{
    public new ArmorConfig config => base.config as ArmorConfig;

    public float damageThreshold;
    public Dictionary<DamageType, float> damageResistances = new Dictionary<DamageType, float>();
    public BodyPartType[] coveredParts;

    public ArmorType armorType;
    public float movementPenalty;
    public float noiseModifier;
    public EquipSlot slot;
    
    public Armor(ArmorConfig config, int quantity = 1) : base(config, quantity)
    {
        damageThreshold = config.damageThreshold;
        coveredParts = config.coveredParts;
        armorType = config.armorType;
        movementPenalty = config.movementPenalty;
        noiseModifier = config.noiseModifier;
        slot = config.slot;

        damageResistances[DamageType.Normal] = config.damageResistances[0];
        damageResistances[DamageType.Fire] = config.damageResistances[1];
        damageResistances[DamageType.Explosive] = config.damageResistances[2];
        damageResistances[DamageType.Energy] = config.damageResistances[3];
    }
    
    public void Degrade(float amount)
    {
        durability = Mathf.Max(0.0f, durability - amount);
    }
    
    public float GetResistance(DamageType type)
    {
        if (damageResistances.TryGetValue(type, out float res))
            return res;
        return 0.0f;
    }
    
    public bool IsBroken() => durability <= 0.0f;
    
    public override string GetStatText()
    {
        string resistances = string.Join(", ", damageResistances.Select(kv => $"{kv.Key}: {kv.Value:0.0}"));
        string parts = string.Join(", ", coveredParts);

        return $"Type: {armorType}\nSlot: {slot}\nDurability: {durability:0.0}\nDamage Threshold: {damageThreshold:0.0}\nResistances: {resistances}\nCovers: {parts}\nMovement Penalty: {movementPenalty:0.0}\nNoise Modifier: {noiseModifier:0.0}\nWeight: {config.weight:0.0}";
    }
}