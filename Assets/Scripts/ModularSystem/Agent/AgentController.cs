using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class AgentController : ModuleController
{
    public string AgentName { get; protected set; }

    private UnityEngine.AI.NavMeshAgent _navMeshAgent;

    public UnityEngine.AI.NavMeshAgent NavAgent => _navMeshAgent;

    [SerializeField] private AgentModuleLoadout _moduleLoadout;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (_moduleLoadout == null)
        {
            Debug.LogError("Module Loadout is not assigned to AgentController.");
            return;
        }

        foreach (var entry in _moduleLoadout.modules)
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