
using UnityEngine;

public class Ammo : Item
{
    private int _stackSize; /* How many bullets, etc. */
    private AmmoType _ammoType;
    
    public Ammo(AmmoConfig config, int quantity = 1) : base(config, quantity)
    {
        _stackSize = config.initialStack;
        _ammoType = config.ammoType;
    }

    public int GetStackSize()
    {
        return _stackSize;
    }

    public AmmoType GetAmmoType()
    {
        return _ammoType;
    }

    public void IncreaseStackSize(int amount)
    {
        _stackSize += amount;
    }

    public void DecreaseStackSize(int amount)
    {
        _stackSize = Mathf.Min(0, _stackSize - amount);
    }
}
