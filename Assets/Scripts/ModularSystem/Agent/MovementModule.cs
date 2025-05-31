using UnityEngine;
using UnityEngine.AI;

public class MovementModule : ModuleBase
{
    private AgentController agent;
    private Camera mainCamera;
    private MovementModuleConfig config;

    public MovementModule(MovementModuleConfig config)
    {
        this.config = config;
    }

    public override void Initialize()
    {
        base.Initialize();
        agent = Controller as AgentController;
        mainCamera = Camera.main;

        if (agent?.NavAgent != null)
        {
            ApplyNavAgentSettings(agent.NavAgent);
        }
    }

    private void ApplyNavAgentSettings(NavMeshAgent navAgent)
    {
        navAgent.speed = config.movementSpeed;
        navAgent.acceleration = config.acceleration;
        navAgent.angularSpeed = config.angularSpeed;
        navAgent.stoppingDistance = config.stoppingDistance;
    }

    public override bool Tick()
    {
        if (!base.Tick()) return false;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                MoveTo(hit.point);
            }
        }

        return true;
    }

    private void MoveTo(Vector3 destination)
    {
        if (agent.NavAgent != null)
        {
            agent.NavAgent.SetDestination(destination);
        }
    }
}