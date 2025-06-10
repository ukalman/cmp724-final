using System;
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
            var module = AgentModuleFactory.CreateModule(entry);
            if (module != null)
                AddModule(module);
        }
        
        if(transform.CompareTag("Player")) GameManager.Instance.RegisterPlayer(this);
    }

    protected virtual void Start()
    {
        ActivateModules();
        
        var health = GetModule<HealthModule>();
        if (health != null)
        {
            health.OnDied += OnAgentDied;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnUIPanelTriggered += OnUIPanelTriggered;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnUIPanelTriggered -= OnUIPanelTriggered;
    }

    private void OnAgentDied()
    {
        DisableModules();
        // İstersen burada animasyon, efekt, ragdoll vs. tetikleyebilirsin
    }

    private void OnUIPanelTriggered(UIPanelTypes type, bool isOpened)
    {
        switch (type)
        {
            case UIPanelTypes.MainMenu:
                break;
            case UIPanelTypes.Settings:
                break;
            case UIPanelTypes.CharacterCreation:
                break;
            case UIPanelTypes.PipBoy:
                break;
            case UIPanelTypes.Loot:
                if (isOpened)
                {
                    GetModule<MovementModule>().Disable();
                    GetModule<CombatModule>().Disable();
                }
                else
                {
                    GetModule<MovementModule>().Activate();
                    GetModule<CombatModule>().Activate();
                }
                break;
            case UIPanelTypes.Dialogue:
                break;
            case UIPanelTypes.Battle:
                break;
            case UIPanelTypes.Pause:
                break;
            case UIPanelTypes.Death:
                break;
            case UIPanelTypes.Notification:
                break;
            case UIPanelTypes.Tooltip:
                break;
            case UIPanelTypes.QuestPopup:
                break;
            case UIPanelTypes.ConfirmationPopup:
                break;
            case UIPanelTypes.LoadingScreen:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}