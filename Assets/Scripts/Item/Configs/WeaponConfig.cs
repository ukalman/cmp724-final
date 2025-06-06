using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponConfig : ItemConfig, IDurability
{
    public List<CombatAction> baseActions;
    public float maxDurability { get; set; }

    private void OnEnable()
    {
        itemType = ItemType.Weapon;
    }
}
