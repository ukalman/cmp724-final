using UnityEngine;
using UnityEngine.AI;

public class MovementModule : ModuleBase
{
    private AgentController _agent;
    private Camera _mainCamera;
    private MovementModuleConfig _config;

    public MovementModule(MovementModuleConfig config)
    {
        _config = config;
    }

    public override void Initialize()
    {
        base.Initialize();
        _agent = Controller as AgentController;
        _mainCamera = Camera.main;

        if (_agent?.NavAgent != null)
        {
            ApplyNavAgentSettings(_agent.NavAgent);
        }
    }

    private void ApplyNavAgentSettings(NavMeshAgent navAgent)
    {
        navAgent.speed = _config.movementSpeed;
        navAgent.acceleration = _config.acceleration;
        navAgent.angularSpeed = _config.angularSpeed;
        navAgent.stoppingDistance = _config.stoppingDistance;
    }

    public override bool Tick()
    {
        if (!base.Tick()) return false;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                MoveTo(hit.point);
            }
        }

        return true;
    }

    private void MoveTo(Vector3 destination)
    {
        if (_agent.NavAgent != null)
        {
            _agent.NavAgent.SetDestination(destination);
        }
    }
}