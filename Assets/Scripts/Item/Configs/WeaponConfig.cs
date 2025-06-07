using UnityEngine;

public enum WeaponType
{
    Melee,
    Ranged
}

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponConfig : EquippableConfig
{
    public WeaponType weaponType;

    [Header("Combat")]
    public CombatAction[] baseActions;

    [Header("Ammo (for Ranged only)")]
    public int maxAmmo;
    public int defaultLoadedAmmo; // silah alınırken dolu gelirse

    private void OnEnable()
    {
        itemType = ItemType.Weapon;
    }
}