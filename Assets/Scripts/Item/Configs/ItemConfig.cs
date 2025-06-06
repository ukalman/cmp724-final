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
    public string itemName;
    public string description;
    public Sprite icon;
    public ItemType itemType;
    public float weight;
    public int value;
}