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

public abstract class EquippableConfig : ItemConfig, IDurability
{
    public EquipSlot slot;
    public int requiredLevel = 1;
    
    [SerializeField]
    private float _maxDurability = 100.0f;

    public float maxDurability
    {
        get => _maxDurability;
        set => _maxDurability = Mathf.Max(0.0f, value);
    }

    private void OnEnable()
    {
        if (itemType != ItemType.Weapon && itemType != ItemType.Armor)
        {
            Debug.LogWarning($"Equippable item '{itemName}' has invalid item type. Fixing...");
            itemType = ItemType.Misc; // Developer fallback
        }
    }

   
}