using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootModuleConfig", menuName = "Modules/Interactable/Configs/Loot")]
public class LootModuleConfig : ModuleConfigBase
{
    public string name = "Unnamed Loot Module";
    public int maxSlots = 10;
    public List<int> items;
}



