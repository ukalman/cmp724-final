using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class AgentController : ModuleController
{
    public string AgentName { get; protected set; }

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    public UnityEngine.AI.NavMeshAgent NavAgent => navMeshAgent;

    [SerializeField] private AgentModuleLoadout moduleLoadout;

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (moduleLoadout == null)
        {
            Debug.LogError("Module Loadout is not assigned to AgentController.");
            return;
        }

        foreach (var entry in moduleLoadout.modules)
        {
            var module = ModuleFactory.CreateModule(entry);
            if (module != null)
                AddModule(module);
        }
    }

    protected virtual void Start()
    {
        ActivateModules();
    }
}