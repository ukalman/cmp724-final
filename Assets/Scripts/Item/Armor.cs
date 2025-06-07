using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    public new ArmorConfig config => base.config as ArmorConfig;

    public Armor(ArmorConfig config, int quantity = 1) : base(config, quantity) { }

    public float DamageThreshold => config.damageThreshold;
    public Dictionary<DamageType, float> DamageResistances => config.damageResistances;
    public BodyPartType[] CoveredParts => config.coveredParts;
    
    public ArmorType ArmorType => config.armorType;
    public float MovementPenalty => config.movementPenalty;
    public float NoiseModifier => config.noiseModifier;
    public EquipSlot Slot => config.slot;

    public void Degrade(float amount)
    {
        durability = Mathf.Max(0.0f, durability - amount);
    }
    
    public float GetResistance(DamageType type)
    {
        if (DamageResistances.TryGetValue(type, out float res))
            return res;
        return 0.0f;
    }
    
    public bool IsBroken() => durability <= 0.0f;
}