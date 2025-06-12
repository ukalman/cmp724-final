using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CombatModuleConfig", menuName = "Modules/Agent/Configs/Combat")]
public class CombatModuleConfig : ModuleConfigBase
{
    public float maxAP;
    public List<CombatAction> availableActions;
}