using UnityEngine;

[CreateAssetMenu(fileName = "HealthModuleConfig", menuName = "Modules/Configs/Health")]
public class HealthModuleConfig : ModuleConfigBase
{
    public float maxHealth = 100f;
    public float regenRate = 0f; // opsiyonel: zamanla iyileşme
}