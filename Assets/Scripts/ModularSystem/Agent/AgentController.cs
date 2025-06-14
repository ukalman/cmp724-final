using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class AgentController : ModuleController
{
    public string AgentName;

    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    
    public UnityEngine.AI.NavMeshAgent NavAgent => _navMeshAgent;

    public Animator anim;
    
    [SerializeField] private AgentModuleLoadout _moduleLoadout;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        
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

        var statsModule = GetModule<StatsModule>();
        var skillsModule = GetModule<SkillsModule>();    
        
        if (AgentName == "Player")
        {
            statsModule.IncreaseStat(StatType.Strength, GameManager.Instance.selectedSpecials["Strength"] + statsModule.GetStat(StatType.Strength) - 1);
            statsModule.IncreaseStat(StatType.Perception, GameManager.Instance.selectedSpecials["Perception"] + statsModule.GetStat(StatType.Perception) - 1);
            statsModule.IncreaseStat(StatType.Endurance, GameManager.Instance.selectedSpecials["Endurance"] + statsModule.GetStat(StatType.Endurance) - 1);
            statsModule.IncreaseStat(StatType.Charisma, GameManager.Instance.selectedSpecials["Charisma"] + statsModule.GetStat(StatType.Charisma) - 1);
            statsModule.IncreaseStat(StatType.Intelligence, GameManager.Instance.selectedSpecials["Intelligence"] + statsModule.GetStat(StatType.Intelligence) - 1);
            statsModule.IncreaseStat(StatType.Agility, GameManager.Instance.selectedSpecials["Agility"] + statsModule.GetStat(StatType.Agility) - 1);
            statsModule.IncreaseStat(StatType.Luck, GameManager.Instance.selectedSpecials["Luck"] + statsModule.GetStat(StatType.Luck) - 1);

            foreach (var skill in GameManager.Instance.selectedSkills.Keys)
            {
                if (skill == "Melee")
                {
                    skillsModule.UnlockSkill(SkillType.Melee,GameManager.Instance.selectedSkills["Melee"]);
                }
                else if (skill == "Ranged")
                {
                    skillsModule.UnlockSkill(SkillType.Ranged,GameManager.Instance.selectedSkills["Ranged"]);
                }
                else if (skill == "Sneak")
                {
                    skillsModule.UnlockSkill(SkillType.Sneak,GameManager.Instance.selectedSkills["Sneak"]);
                }
                else if (skill == "Lockpicking")
                {
                    skillsModule.UnlockSkill(SkillType.Lockpicking,GameManager.Instance.selectedSkills["Lockpicking"]);
                }
                else if (skill == "Science")
                {
                    skillsModule.UnlockSkill(SkillType.Science,GameManager.Instance.selectedSkills["Science"]);
                }
            }
            
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
        StartCoroutine(DeathCoroutine());
        // İstersen burada animasyon, efekt, ragdoll vs. tetikleyebilirsin
    }

    private IEnumerator DeathCoroutine()
    {
        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<Rigidbody>());
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;
        anim.SetTrigger("isDead");
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
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
            case UIPanelTypes.StoryReveal:
                if (isOpened)
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Disable();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Disable();
                    }
                }
                else
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Activate();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Activate();
                    }
                }
                break;
            case UIPanelTypes.PipBoy:
                if (isOpened)
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Disable();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Disable();
                    }
                }
                else
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Activate();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Activate();
                    }
                }
                break;
            case UIPanelTypes.Loot:
                if (isOpened)
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Disable();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Disable();
                    }
                }
                else
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Activate();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Activate();
                    }
                }
                break;
            case UIPanelTypes.Dialog:
                if (isOpened)
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Disable();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Disable();
                    }

                    if (AgentName == "Player")
                    {
                        anim.enabled = false;
                    }
                }
                else
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Activate();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Activate();
                    }
                    if (AgentName == "Player")
                    {
                        anim.enabled = true;
                    }
                }
                break;
            case UIPanelTypes.Battle:
                if (isOpened)
                {
                    if (this.CompareTag("Player"))
                    {
                        GetModule<MovementModule>().Disable();
                        
                    }
                    else
                    {
                        var enemyMovementModule = GetModule<EnemyMovementModule>();
                        if (enemyMovementModule != null)
                        {
                            enemyMovementModule.Disable();
                        }
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (combatModule != null)
                    {
                        combatModule.Disable();
                    }
                   
                }
                else
                {
                    if (this.CompareTag("Player"))
                    {
                        GetModule<MovementModule>().Activate();
                        
                    }
                    else
                    {
                        var enemyMovementModule = GetModule<EnemyMovementModule>();
                        if (enemyMovementModule != null)
                        {
                            enemyMovementModule.Activate();
                        }
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (combatModule != null)
                    {
                        combatModule.Activate();
                    }
                }
                break;
            case UIPanelTypes.Pause:
                if (isOpened)
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Disable();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Disable();
                    }
                }
                else
                {
                    var movementModule = GetModule<MovementModule>();
                    if (movementModule != null)
                    {
                        movementModule.Activate();
                    }
                    var combatModule = GetModule<CombatModule>();
                    if (movementModule != null)
                    {
                        combatModule.Activate();
                    }
                }
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