using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [SerializeField] private AgentController _playerAgent;
    [SerializeField] private AgentController _enemyAgent;
    
    private CombatModule _playerCombat;
    private CombatModule _enemyCombat;

    private bool _combatOngoing = false;

    public bool isPlayerTurn = false;
    
    private List<TurnQueueEntry> _turnQueue = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _playerCombat = _playerAgent.GetModule<CombatModule>();
        _enemyCombat = _enemyAgent.GetModule<CombatModule>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("BASSSSSSSSSSSSSSS");
            StartCombat(_playerCombat,_enemyCombat);
        }
    }

    public void StartCombat(CombatModule player, CombatModule enemy)
    {
        _playerCombat = player;
        _enemyCombat = enemy;
       
        _combatOngoing = true;

        StartCoroutine(CombatLoop());
    }
    
    private void BuildTurnQueue(List<CombatModule> combatants)
    {
        _turnQueue.Clear();

        foreach (var cm in combatants)
        {
            var health = cm.Controller.GetModule<HealthModule>();
            float initiative = cm.CalculateInitiative();

            _turnQueue.Add(new TurnQueueEntry(cm, health, initiative));
        }
        _turnQueue.Sort((a, b) => b.initiative.CompareTo(a.initiative)); // yüksek inisiyatif önce
    }

    private IEnumerator CombatLoop()
    {
        Debug.Log("Combat loop basladi");
        BuildTurnQueue(new List<CombatModule> { _playerCombat, _enemyCombat }); // şimdilik 2 kişi
        Debug.Log($"turn queue buildlendi, turn queue count: {_turnQueue.Count}");
        
        while (_combatOngoing)
        {
            for (int i = 0; i < _turnQueue.Count; i++)
            {
                var entry = _turnQueue[i];
                if (entry.healthModule.IsDead()) continue;
                CombatModule enemy = GetRandomLivingEnemy(entry);
                Debug.Log($"Random living enemy: {enemy.Controller.transform.name}");
                if (enemy == null) continue;
                yield return RunTurn(entry.combatModule, enemy);

                if (IsCombatOver()) break;

                yield return new WaitForSeconds(0.5f);
                
            }
            Debug.Log("TUR BITTI");
            if (IsCombatOver()) break;
            
            _turnQueue.RemoveAll(entry => entry.healthModule.IsDead());
        }
        Debug.Log("Combat loop bitti");
        EndCombat();
    }

    private IEnumerator RunTurn(CombatModule attacker, CombatModule defender)
    {
        attacker.RefillAP();
        attacker.ClearQueuedActions();
        
        if (attacker.Controller.transform.CompareTag("Player"))
        {
            isPlayerTurn = true;
            Debug.Log("Sıra player'da");
            // Oyuncu sıraya aksiyon koyacak, End Turn UI'ı basıldığında devam edilecek
            /* yield return new WaitUntil(() =>
                attacker.GetQueuedActions().Count > 0 || attacker.CurrentAP <= 0f);
            */
            yield return new WaitUntil(() => isPlayerTurn == false); /* TODO bu player end turn butonuna basana kadar bekleyecek */
            Debug.Log($"Player'in action count'i: {attacker.GetQueuedActions().Count}");
        }
        else
        {
            Debug.Log("Sıra enemy'de");
            GenerateEnemyActions(attacker);
            Debug.Log($"Enemy'in action count'i: {attacker.GetQueuedActions().Count}");
        }
        
        while (attacker.GetQueuedActions().Count > 0)
        {
            var action = attacker.GetQueuedActions().Dequeue();
            
            int attackerSkill = 85;
            int targetAC = 5;
            
            var stats = attacker.Controller.GetModule<StatsModule>();
            float perception = stats?.Perception ?? 0.0f;
            float strength = stats?.Strength ?? 0.0f;
            float luck = stats?.Luck ?? 0.0f;

            float hitChance = CombatMath.CalculateHitChance(attackerSkill, targetAC, action.accuracyModifier, perception);
            bool didHit = Random.Range(0.0f, 100.0f) < hitChance;
            
            var defenderHealth = defender.Controller.GetModule<HealthModule>();

            Debug.Log($"{attacker.Controller.name} uses {action.name} on {defender.Controller.name}: " +
                      (didHit ? "HIT!" : "MISS") + $" ({hitChance:0.0}%)");

            if (didHit)
            {
                int rawDamage = action.RollRawDamage();
                bool isCrit = CombatMath.CheckCriticalHit(action.criticalChance, luck);
                
                if (isCrit)
                {
                    rawDamage = Mathf.RoundToInt(rawDamage * 1.5f);
                    Debug.Log("CRITICAL HIT!");
                }
                /* TODO melee attack ise strength kullanılacak */
                int finalDamage = CombatMath.CalculateFinalDamage(rawDamage, 1.0f, 0, 0.0f, strength);
                defenderHealth.TakeDamage(finalDamage);
                Debug.Log($"→ {defender.Controller.name} took {finalDamage} damage.");
            }

            yield return new WaitForSeconds(0.25f);
        }
        
        Debug.Log($"{attacker.Controller.transform.name} queued actionlarini kullandi." );
        yield return new WaitForSeconds(0.25f);
    }

    private void GenerateEnemyActions(CombatModule enemy)
    {
        var actions = enemy.GetAvailableActions();
        foreach (var action in actions)
        {
            if (enemy.CurrentAP >= action.apCost)
                enemy.QueueAction(action);
        }
    }
    
    CombatModule GetRandomLivingEnemy(TurnQueueEntry attacker)
    {
        return _turnQueue
            .Where(e => e != attacker && !e.healthModule.IsDead())
            .Select(e => e.combatModule)
            .OrderBy(x => Random.value)
            .FirstOrDefault();
    }
    
    private bool IsCombatOver() /* TODO degisecek */
    {
        int aliveCount = _turnQueue.Count(e => !e.healthModule.IsDead());
        return aliveCount <= 1;
    }
    
    private void EndCombat()
    {
        _combatOngoing = false;
        Debug.Log("Combat ended.");
    }
    
}
