using UnityEngine;

public class PlayerCombatInputHandler : MonoBehaviour
{
    private CombatModule _combatModule;

    private void Start()
    {
        _combatModule = GetComponent<AgentController>().GetModule<CombatModule>();
    }

    private void Update()
    {
        if (!CombatManager.Instance.isPlayerTurn) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            TryQueueAction(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            TryQueueAction(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            TryQueueAction(2);

        if (Input.GetKeyDown(KeyCode.Return)) // Turnü bitir
            CombatManager.Instance.isPlayerTurn = false;
    }

    void TryQueueAction(int index)
    {
        var actions = _combatModule.GetAvailableActions();
        if (index >= actions.Count) return;

        var action = actions[index];
        if (_combatModule.CanPerformAction(action))
        {
            _combatModule.QueueAction(action);
            Debug.Log($"Queued {action.name} (AP: {action.apCost})");
        }
        else
        {
            Debug.Log("Not enough AP!");
        }
    }
}