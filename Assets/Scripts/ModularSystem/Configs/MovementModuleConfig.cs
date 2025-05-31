using UnityEngine;

[CreateAssetMenu(fileName = "MovementModuleConfig", menuName = "Modules/Configs/Movement")]
public class MovementModuleConfig : ModuleConfigBase
{
    [Header("Movement Settings")]
    public float movementSpeed = 3.5f;
    public float acceleration = 8f;
    public float angularSpeed = 120f;
    public float stoppingDistance = 0.1f;
}