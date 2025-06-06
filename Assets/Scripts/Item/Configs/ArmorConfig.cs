using UnityEngine;

[CreateAssetMenu(menuName = "Items/Armor")]
public class ArmorConfig : ItemConfig, IDurability
{
    public BodyPartType bodyPart;
    public int damageThreshold;
    public float damageResistance;
    public float maxDurability { get; set; }

    private void OnEnable()
    {
        itemType = ItemType.Armor;
    }

    
}
