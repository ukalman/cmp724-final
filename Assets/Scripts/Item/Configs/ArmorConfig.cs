using System.Collections.Generic;
using UnityEngine;

public enum ArmorType
{
    Light,
    Medium,
    Heavy
}

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorConfig : EquippableConfig
{
    [Header("Defense Stats")]
    public float damageThreshold;
    public Dictionary<DamageType, float> damageResistances;

    [Header("Coverage")]
    public BodyPartType[] coveredParts;
    
    [Header("Armor Classification")]
    public ArmorType armorType;

    [Tooltip("Reduces movement speed, e.g., 0.1 = -10% speed")]
    public float movementPenalty;

    [Tooltip("Affects how much noise the armor makes")]
    public float noiseModifier;

    private void OnEnable()
    {
        itemType = ItemType.Armor;
    }
}