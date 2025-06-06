using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryModule : ModuleBase
{
    private StatsModule _stats;
    private float _currentWeight;

    private List<Item> _items = new();
    private Dictionary<EquipSlot, Item> _equippedItems = new();

    public event Action<Item> OnItemAdded;
    public event Action<Item> OnItemRemoved;
    public event Action<Item> OnItemEquipped;
    public event Action<Item> OnItemUnequipped;

    public override void Initialize()
    {
        base.Initialize();
        _stats = Controller.GetModule<StatsModule>();
        _currentWeight = 0.0f;
    }

    public float GetMaxCarryWeight()
    {
        float strength = _stats?.Strength ?? 1.0f;
        return 20.0f + strength * 5.0f;
    }

    public float GetCurrentWeight() => _currentWeight;
    public IReadOnlyList<Item> GetItems() => _items.AsReadOnly();
    public IReadOnlyDictionary<EquipSlot, Item> GetEquippedItems() => _equippedItems;

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
        if (_equippedItems.TryGetValue(slot, out var currentlyEquipped))
        {
            UnequipItem(currentlyEquipped);
        }

        _equippedItems[slot] = item;
        OnItemEquipped?.Invoke(item);
        return true;
    }

    public void UnequipItem(Item item)
    {
        if (item.config is not EquippableConfig equipConfig) return;

        EquipSlot slot = equipConfig.slot;
        if (_equippedItems.ContainsKey(slot))
        {
            _equippedItems.Remove(slot);
            OnItemUnequipped?.Invoke(item);
        }
    }

    public Item GetEquippedItem(EquipSlot slot)
    {
        _equippedItems.TryGetValue(slot, out var item);
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
        foreach (var equipped in _equippedItems.Values)
        {
            if (equipped is Armor armor &&
                armor.CoveredParts != null &&
                armor.CoveredParts.Contains(part))
            {
                return armor;
            }
        }
        return null;
    }
    
    public Weapon GetEquippedWeapon(EquipSlot slot)
    {
        foreach (var equipped in _equippedItems)
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


