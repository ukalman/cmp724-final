using System.Collections.Generic;
using UnityEngine;

public class LootModule : ModuleBase
{
    private LootModuleConfig _config;
        
    private int _maxSlots = 10;
    private List<Item> _items = new();

    public IReadOnlyList<Item> Items => _items;
    public int MaxSlots => _maxSlots;

    public LootModule(LootModuleConfig config)
    {
        _config = config;
    }
    
    public override void Initialize()
    {
        base.Initialize();
        Name = _config.name;
        _maxSlots = _config.maxSlots;

        foreach (var itemId in _config.items)
        {
            _items.Add(ItemDatabaseManager.Instance.GetItem(itemId));
        }
    }

    public bool IsFull()
    {
        return _items.Count >= _maxSlots;
    }

    public bool TryAddItem(Item item)
    {
        if (IsFull())
            return false;

        _items.Add(item);
        return true;
    }

    public bool RemoveItem(Item item)
    {
        return _items.Remove(item);
    }

    public bool ContainsItem(Item item)
    {
        return _items.Contains(item);
    }

    public void ClearChest()
    {
        _items.Clear();
    }
}