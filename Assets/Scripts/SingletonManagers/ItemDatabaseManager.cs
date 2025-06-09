using System;
using UnityEngine;
using System.Collections.Generic;

public class ItemDatabaseManager : MonoBehaviour
{
    public static ItemDatabaseManager Instance { get; private set; }

    private Dictionary<int, Item> _itemsById;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        BuildItemDatabase();
    }

    private void BuildItemDatabase()
    {
        _itemsById = new Dictionary<int, Item>();

        var configs = Resources.LoadAll<ItemConfig>("Items");
        foreach (var config in configs)
        {
            if (_itemsById.ContainsKey(config.itemId))
            {
                Debug.LogWarning($"Duplicate item ID {config.itemId} found in configs!");
                continue;
            }

            switch (config.itemType)
            {
                case ItemType.Weapon:
                    var weapon = new Weapon((WeaponConfig)config); // default quantity = 1
                    Debug.Log("Adding weapon...");
                    _itemsById.Add(config.itemId, weapon);
                    break;
                case ItemType.Armor:
                    var armor = new Armor((ArmorConfig)config); // default quantity = 1
                    Debug.Log("Adding Armor...");
                    _itemsById.Add(config.itemId, armor);
                    break;
                case ItemType.Consumable:
                    break;
                case ItemType.Quest:
                    break;
                case ItemType.Misc:
                    var misc = new Ammo((AmmoConfig)config); /* TODO simdilik sadece ammo */
                    Debug.Log("Adding ammo...");
                    _itemsById.Add(config.itemId, misc);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public Item GetItem(int id)
    {
        _itemsById.TryGetValue(id, out var item);
        if (item != null)
        {
            return item;
        }
        Debug.LogError($"No such item! Requested Item ID: {id}");
        return null;
    }

    public ItemConfig GetItemConfig(int id)
    {
        _itemsById.TryGetValue(id, out var item);
        if (item != null)
        {
            return item.config;
        }
        Debug.LogError($"No such item config! Requested Item ID: {id}");
        return null;
    }

    /*
    public Item CloneItem(int id, int quantity = 1, float? durability = null)
    {
        if (!_itemsById.TryGetValue(id, out var original)) return null;

        var clone = new Item(original.config, quantity);
        if (durability.HasValue)
            clone.durability = durability.Value;
        return clone;
    }
    */
}