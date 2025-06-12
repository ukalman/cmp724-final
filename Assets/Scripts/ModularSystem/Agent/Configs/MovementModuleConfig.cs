using UnityEngine;

[CreateAssetMenu(fileName = "MovementModuleConfig", menuName = "Modules/Agent/Configs/Movement")]
public class MovementModuleConfig : ModuleConfigBase
{
    [Header("Movement Settings")]
    public float movementSpeed;
    public float acceleration;
    public float angularSpeed;
    public float stoppingDistance;
}