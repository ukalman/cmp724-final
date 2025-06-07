using UnityEngine;

public class Weapon : Item
{
    public WeaponType weaponType;
    
    public CombatAction[] baseActions;
    
    public int currentAmmo;
    public int maxAmmo;
    public int defaultLoadedAmmo;

    
    public Weapon(WeaponConfig config, int quantity = 1) : base(config, quantity)
    {
        if (config.weaponType == WeaponType.Ranged)
        {
            currentAmmo = Mathf.Clamp(config.defaultLoadedAmmo, 0, config.maxAmmo);
            maxAmmo = config.maxAmmo;
            defaultLoadedAmmo = config.defaultLoadedAmmo;
        }
        else
        {
            currentAmmo = -1; // Melee için ammo geçersiz
            maxAmmo = -1;
            defaultLoadedAmmo = -1;
        }
        
        this.weaponType = config.weaponType;
        this.baseActions = config.baseActions;
    }

    public WeaponConfig GetWeaponConfig() => (WeaponConfig)config;

    public bool CanFire() => GetWeaponConfig().weaponType == WeaponType.Ranged && currentAmmo > 0;

    public void ConsumeAmmo(int amount)
    {
        if (GetWeaponConfig().weaponType == WeaponType.Ranged)
            currentAmmo = Mathf.Max(0, currentAmmo - amount);
    }

    public void Reload(int ammoAmount)
    {
        if (GetWeaponConfig().weaponType != WeaponType.Ranged)
            return;

        int max = GetWeaponConfig().maxAmmo;
        currentAmmo = Mathf.Min(currentAmmo + ammoAmount, max);
    }
}