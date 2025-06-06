using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CombatModuleConfig", menuName = "Modules/Configs/Combat")]
public class CombatModuleConfig : ModuleConfigBase
{
    public float maxAP = 8.0f;
    public List<CombatAction> availableActions;
}