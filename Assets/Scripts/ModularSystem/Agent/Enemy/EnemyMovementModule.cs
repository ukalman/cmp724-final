
using UnityEngine;

public class EnemyMovementModule : MovementModule
{
    public float detectionDistance = 10.0f;
    public float combatStartDistance = 2.0f;
    
    private bool hasDetectedPlayer = false;
    private bool hasRegisteredForCombat = false;
    
    private Transform player;
    
    public EnemyMovementModule(MovementModuleConfig config) : base(config)
    {
        
    }
    
    public override void Initialize()
    {
        base.Initialize();
        combatStartDistance = 3.0f;
        _agent = Controller as AgentController;
        _tag = Controller.tag;
        
        if (_agent?.NavAgent != null)
        {
            ApplyNavAgentSettings(_agent.NavAgent);
        }
    }

    public override bool Tick()
    {
        if (!base.Tick()) return false;
        if (player == null)  player = GameManager.Instance.PlayerController.transform;
        isMoving = _agent.NavAgent.velocity.sqrMagnitude > 0.1f && !_agent.NavAgent.pathPending && _agent.NavAgent.remainingDistance > _agent.NavAgent.stoppingDistance;
        _agent.anim.SetBool("isMoving",isMoving);
        
        float distanceToPlayer = Vector3.Distance(Controller.transform.position, player.position);

        if (!hasDetectedPlayer && distanceToPlayer <= detectionDistance)
        {
            hasDetectedPlayer = true;
        }

        if (hasDetectedPlayer && !hasRegisteredForCombat)
        {
            if (distanceToPlayer > combatStartDistance)
            {
                MoveTo(player.position);
            }
            else
            {
                _agent.NavAgent.ResetPath();
                FaceTarget(player.position);
                RegisterEnemyForCombat();
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

    private void RegisterEnemyForCombat()
    {
        hasRegisteredForCombat = true;
        CombatManager.Instance.RegisterEnemy(Controller.GetModule<CombatModule>());
    }
    
}
