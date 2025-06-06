using UnityEngine;

public enum EquipSlot
{
    Head,
    Torso,
    Legs,
    MainHand,
    OffHand,
    Accessory
}

public abstract class EquippableItemConfig : ItemConfig
{
    public EquipSlot slot;
    public int requiredLevel = 1;
    public float durabilityMax = 100.0f;

    private void OnEnable()
    {
        if (itemType != ItemType.Weapon && itemType != ItemType.Armor)
        {
            Debug.LogWarning($"Equippable item '{itemName}' has invalid item type. Fixing...");
            itemType = ItemType.Misc; // Developer fallback
        }
    }
}