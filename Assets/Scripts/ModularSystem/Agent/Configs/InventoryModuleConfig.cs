using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryModuleConfig", menuName = "Modules/Agent/Configs/Inventory")]
public class InventoryModuleConfig : ModuleConfigBase
{
    public List<int> _items;
    public List<int> _equippedItems;
}

/* 
public enum EquipSlot
{
    Head, -> 0
    Torso, -> 1
    Legs, -> 2
    MainHand, -> 3
    OffHand, -> 4
    Accessory -> 5
}
*/