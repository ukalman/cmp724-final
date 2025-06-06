using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Quest,
    Misc
}

public abstract class ItemConfig : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    [TextArea]
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public float weight;
    public int value;
}