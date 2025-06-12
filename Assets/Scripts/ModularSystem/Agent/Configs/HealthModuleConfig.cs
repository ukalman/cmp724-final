using UnityEngine;

[CreateAssetMenu(fileName = "HealthModuleConfig", menuName = "Modules/Agent/Configs/Health")]
public class HealthModuleConfig : ModuleConfigBase
{
    public float maxHealth;
    public float regenRate; // opsiyonel: zamanla iyileşme
}