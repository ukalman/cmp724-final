using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponConfig : EquippableItemConfig
{
    public CombatAction[] baseActions;

    private void OnEnable()
    {
        itemType = ItemType.Weapon;
    }
}
