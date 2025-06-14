using UnityEngine;
using UnityEngine.AI;

public class MovementModule : ModuleBase
{
    protected AgentController _agent;
    private Camera _mainCamera;
    protected MovementModuleConfig _config;
    protected string _tag;

    protected bool isMoving;
    public MovementModule(MovementModuleConfig config)
    {
        _config = config;
    }

    public override void Initialize()
    {
        base.Initialize();
        _agent = Controller as AgentController;
        _mainCamera = Camera.main;
        _tag = Controller.tag;

        if (_agent?.NavAgent != null)
        {
            ApplyNavAgentSettings(_agent.NavAgent);
        }
    }

    protected void ApplyNavAgentSettings(NavMeshAgent navAgent)
    {
        navAgent.speed = _config.movementSpeed;
        navAgent.acceleration = _config.acceleration;
        navAgent.angularSpeed = _config.angularSpeed;
        navAgent.stoppingDistance = _config.stoppingDistance;
    }

    public override bool Tick()
    {
        if (!base.Tick()) return false;

        isMoving = _agent.NavAgent.velocity.sqrMagnitude > 0.1f && !_agent.NavAgent.pathPending && _agent.NavAgent.remainingDistance > _agent.NavAgent.stoppingDistance;
        _agent.anim.SetBool("isMoving",isMoving);
        
        if (Input.GetMouseButtonDown(0) && _tag == "Player")
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
    
    protected void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - Controller.transform.position).normalized;
        direction.y = 0f; 
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Controller.transform.rotation = Quaternion.Slerp(Controller.transform.rotation, lookRotation, Time.deltaTime * 5.0f);
        }
    }
}