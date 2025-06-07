using System.Collections.Generic;
using UnityEngine;

public class CombatModule : ModuleBase
{
    private CombatModuleConfig _config;
    private AgentController _agent;

    private StatsModule _stats;

    private float _currentAP;
    [SerializeField] private float _maxAP ;

    public float CurrentAP => _currentAP;
    public float MaxAP => _maxAP;
    
    private float _cooldownRemaining = 0.0f;

    private List<CombatAction> _availableActions;
    private Queue<CombatAction> _queuedActions;
    
    public Queue<CombatAction> GetQueuedActions() => _queuedActions;
    
    public CombatModule(CombatModuleConfig config)
    {
        _config = config;
        _maxAP = config.maxAP;
        _currentAP = _maxAP;

        _availableActions = new List<CombatAction>(_config.availableActions);
        _queuedActions = new Queue<CombatAction>();
    }

    public override void Initialize()
    {
        base.Initialize();
        _agent = Controller as AgentController;
        _stats = _agent.GetModule<StatsModule>();
        _cooldownRemaining = 0.0f;
        _maxAP = CalculateMaxAP();
        _currentAP = _maxAP;
    }

    public override bool Tick()
    {
        if (!base.Tick()) return false;

        if (_cooldownRemaining > 0.0f)
        {
            _cooldownRemaining -= Time.deltaTime;
        }

        return true;
    }
    
    private float CalculateMaxAP()
    {
        if (_stats == null) return _config.maxAP;

        // Örnek: Base + (Agility / 2)
        return _config.maxAP + (_stats.Agility * 0.5f);
    }
    
    public float CalculateInitiative()
    {
        // Örnek: Base roll + Agility etkisi
        return Random.Range(1, 100) + (_stats?.Agility ?? 0) * 2.0f;
    }
    
    public void QueueAction(CombatAction action)
    {
        if (_currentAP >= action.apCost)
        {
            Debug.Log($"Queue'lamaya haziriz!, Combat action: {action.actionName} AP: {_currentAP}");
            var weapon = GetEquippedWeapon();
            
            if (weapon.weaponType == WeaponType.Ranged && weapon.currentAmmo < action.ammoCost)
            {
                Debug.Log("Out of ammo, cannot queue action!");
                return;
            }
            
            _queuedActions.Enqueue(action);
            SpendAP(action.apCost);
            weapon.ConsumeAmmo(action.ammoCost);
            Debug.Log($"Queuelandı, Combat action: {action.actionName} AP: {_currentAP}");
        }
    }
    
    public void ClearQueuedActions()
    {
        _queuedActions.Clear();
    }
    
    public bool CanPerformAction(CombatAction action)
    {
        return _currentAP >= action.apCost && _cooldownRemaining <= 0.0f;
    }

    public void SpendAP(float amount)
    {
        _currentAP = Mathf.Max(_currentAP - amount, 0.0f);
    }

    public void RefillAP()
    {
        _currentAP = _maxAP;
        Debug.Log($"AP refillendi, current AP: {_currentAP}");
    }
    
    public List<CombatAction> GetAvailableActions()
    {
        Weapon weapon = GetEquippedWeapon();
        List<CombatAction> actions = new List<CombatAction>();

        foreach (var action in weapon.baseActions)
        {
            if (IsActionUnlocked(action))
                actions.Add(action);
        }

        return actions;
    }
    
    public void AddAvailableAction(CombatAction action)
    {
        if (!_availableActions.Contains(action))
            _availableActions.Add(action);
    }
    
    public void RemoveAvailableAction(CombatAction action)
    {
        if (_availableActions.Contains(action))
            _availableActions.Remove(action);
    }

    private bool IsActionUnlocked(CombatAction action)
    {
        // Burada action'a özel şartlara bakarız
        // Örn: skill seviyesi yeterli mi?
        var skills = Controller.GetModule<SkillsModule>();
        var stats = Controller.GetModule<StatsModule>();

        return action.requiredSkill == SkillType.None ||
               skills.GetSkill(action.requiredSkill) >= action.requiredSkillLevel;
    }
    
    public BodyPartData GetSelectedBodyPart()
    {
        return BodyPartLibrary.GetData(BodyPartType.Torso);  /* TODO placeholder simdilik */
    }

    public Weapon GetEquippedWeapon()
    {
        return Controller.GetModule<InventoryModule>().GetEquippedWeapon(EquipSlot.MainHand);
    }

    public void ReloadWeapon()
    {
        var inventory = Controller.GetModule<InventoryModule>();
        var weapon = GetEquippedWeapon();

        if (weapon == null || weapon.currentAmmo >= weapon.maxAmmo) return;

        int neededAmmo = weapon.maxAmmo - weapon.currentAmmo;
        int available = inventory.GetAmmoCount(weapon.ammoType);

        int toReload = Mathf.Min(neededAmmo, available);
        inventory.UseAmmoToReload(weapon.ammoType, toReload);

        weapon.currentAmmo += toReload;

        Debug.Log($"{Controller.name} reloaded {toReload} ammo into {weapon.GetName()}");
    }
    
    private void HandleLevelUp(LevelUpData data)
    {
        UpdateUnlockedActions(); // Gerekirse cache yenile
    }

    private void UpdateUnlockedActions()
    {
        Debug.Log("Calling UpdateUnlockedActions");
        /* TODO not implemented yet */   
    }
}