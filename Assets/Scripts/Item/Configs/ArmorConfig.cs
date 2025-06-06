using UnityEngine;

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorConfig : EquippableItemConfig
{
    public float damageThreshold;
    public float damageResistance;

    private void OnEnable()
    {
        itemType = ItemType.Armor;
    }
}

