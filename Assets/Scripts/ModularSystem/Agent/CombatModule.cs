using System.Collections.Generic;
using UnityEngine;

public class CombatModule : ModuleBase
{
    private CombatModuleConfig _config;
    private AgentController _agent;

    private float _currentAP;
    [SerializeField] private float _maxAP ;

    public float CurrentAP => _currentAP;
    public float MaxAP => _maxAP;
    
    private float _cooldownRemaining = 0.0f;

    private List<CombatAction> _availableActions;
    private Queue<CombatAction> _queuedActions;

    public List<CombatAction> GetAvailableActions() => _availableActions;
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
        _cooldownRemaining = 0.0f;
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

    public void QueueAction(CombatAction action)
    {
        if (_currentAP >= action.apCost)
        {
            Debug.Log($"Evet queue'lıyozzz, AP: {_currentAP}");
            _queuedActions.Enqueue(action);
            SpendAP(action.apCost);
            Debug.Log($"Quelandı, current ap: {_currentAP}");
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
    
}