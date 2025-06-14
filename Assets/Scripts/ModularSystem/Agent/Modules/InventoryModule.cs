using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryModule : ModuleBase
{
    private InventoryModuleConfig _config;
    
    private StatsModule _stats;
    private float _currentWeight;

    private List<Item> _items;
    public Dictionary<EquipSlot, Item> equippedItems;
    
    public IReadOnlyList<Item> Items => _items;
    public event Action<Item> OnItemAdded;
    public event Action<Item> OnItemRemoved;
    public event Action<Item> OnItemEquipped;
    public event Action<Item> OnItemUnequipped;

    public int TotalAmmo;
    
    public InventoryModule(InventoryModuleConfig config)
    {
        _config = config;
        _items = new List<Item>();
        equippedItems = new Dictionary<EquipSlot, Item>();
    }
    
    public override void Initialize()
    {
        base.Initialize();
        _stats = Controller.GetModule<StatsModule>();

        foreach (var itemId in _config._items)
        {
            var item = ItemDatabaseManager.Instance.GetItem(itemId);
            if (item.GetName() == "10mm Pistol")
            {
                TryEquipItem(item);
            }

            if (item is Ammo ammo)
            {
                TotalAmmo += ammo.GetStackSize();
            }
            
            _items.Add(item);
            _currentWeight += item.config.weight;
        }

        for (int i = 0; i < _config._equippedItems.Count; i++)
        {
            if (_config._equippedItems[i] == -1) continue;
            equippedItems.Add((EquipSlot)i,ItemDatabaseManager.Instance.GetItem(_config._equippedItems[i]));
        }
    }

    public float GetMaxCarryWeight()
    {
        float strength = _stats?.Strength ?? 1.0f;
        return 20.0f + strength * 5.0f;
    }

    public float GetCurrentWeight() => _currentWeight;
    public IReadOnlyDictionary<EquipSlot, Item> GetEquippedItems() => equippedItems;

    public bool TryAddItem(Item item)
    {
        float weight = item.config.weight;
        if (_currentWeight + weight > GetMaxCarryWeight()) return false;

        _items.Add(item);
        _currentWeight += weight;
        OnItemAdded?.Invoke(item);
        return true;
    }

    public bool RemoveItem(Item item)
    {
        if (!_items.Contains(item)) return false;

        _items.Remove(item);
        _currentWeight -= item.config.weight;
        OnItemRemoved?.Invoke(item);
        return true;
    }

    public bool TryEquipItem(Item item)
    {
        if (item.config is not EquippableConfig equipConfig) return false;

        if (!_items.Contains(item)) return false;

        EquipSlot slot = equipConfig.slot;
        if (equippedItems.TryGetValue(slot, out var currentlyEquipped))
        {
            UnequipItem(currentlyEquipped);
        }

        equippedItems[slot] = item;
        OnItemEquipped?.Invoke(item);
        return true;
    }

    public void UnequipItem(Item item)
    {
        if (item.config is not EquippableConfig equipConfig) return;

        EquipSlot slot = equipConfig.slot;
        if (equippedItems.ContainsKey(slot))
        {
            equippedItems.Remove(slot);
            OnItemUnequipped?.Invoke(item);
        }
    }

    public Item GetEquippedItem(EquipSlot slot)
    {
        equippedItems.TryGetValue(slot, out var item);
        return item;
    }

    public bool UseItem(Item item)
    {
        if (item.config is not ConsumableConfig consumable) return false;

        ApplyConsumableEffects(consumable);
        RemoveItem(item);
        return true;
    }

    private void ApplyConsumableEffects(ConsumableConfig config)
    {
        foreach (var effect in config.effects)
        {
            switch (effect.effectType)
            {
                case ConsumableEffectType.HealHP:
                    Controller.GetModule<HealthModule>()?.RestoreHealth(effect.amount);
                    break;

                case ConsumableEffectType.BuffStat:
                    Controller.GetModule<StatsModule>()?.ApplyTemporaryBoost(effect.statType, effect.amount, effect.duration);
                    break;

                case ConsumableEffectType.BuffSkill:
                    Controller.GetModule<SkillsModule>()?.ApplyTemporaryBoost(effect.skillType, effect.amount, effect.duration);
                    break;
            }
        }
    }
    
    public Armor GetEquippedArmorOn(BodyPartType part)
    {
        foreach (var equipped in equippedItems.Values)
        {
            if (equipped is Armor armor &&
                armor.coveredParts != null &&
                armor.coveredParts.Contains(part))
            {
                return armor;
            }
        }
        return null;
    }
    
    public Weapon GetEquippedWeapon(EquipSlot slot)
    {
        foreach (var equipped in equippedItems)
        {
            if (equipped.Key == slot && equipped.Value is Weapon weapon)
            {
                return weapon;
            }
        }
        return null;
    }

    public int GetAmmoCount(AmmoType type)
    {
        int total = 0;
        foreach (var item in _items)
        {
            if (item.config is AmmoConfig ammo && ammo.ammoType == type)
            {
                total += item.quantity;
            }
        }

        return total;
    }

    public void UseAmmoToReload(AmmoType type, int ammoCount)
    {
        foreach (var item in _items)
        {
            if (item is Ammo ammo && ammo.GetAmmoType() == type)
            {
                ammo.DecreaseStackSize(ammoCount);
            }
        }   
    }
}


