using System.Collections;
using UnityEngine;

public enum StatType
{
    Strength,
    Perception,
    Endurance,
    Charisma,
    Intelligence,
    Agility,
    Luck
}

public class StatsModule : ModuleBase
{
    private StatsModuleConfig _config;

    public int Strength { get; private set; }
    public int Perception { get; private set; }
    public int Endurance { get; private set; }
    public int Charisma { get; private set; }
    public int Intelligence { get; private set; }
    public int Agility { get; private set; }
    public int Luck { get; private set; }

    public StatsModule(StatsModuleConfig config)
    {
        _config = config;
    }

    public override void Initialize()
    {
        Strength = _config.strength;
        Perception = _config.perception;
        Endurance = _config.endurance;
        Charisma = _config.charisma;
        Intelligence = _config.intelligence;
        Agility = _config.agility;
        Luck = _config.luck;  
        
        var levelModule = Controller.GetModule<LevelModule>();
        if (levelModule != null)
        {
            levelModule.OnLevelChanged += HandleLevelUp;
        }
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();

        var levelModule = Controller.GetModule<LevelModule>();
        if (levelModule != null)
        {
            levelModule.OnLevelChanged -= HandleLevelUp;
        }
    }
    
    public void ApplyTemporaryBoost(StatType stat, float amount, float duration)
    {
        switch (stat)
        {
            case StatType.Strength:
                Strength += (int)amount;
                break;
            case StatType.Perception:
                Perception += (int)amount;
                break;
            case StatType.Endurance:
                Endurance += (int)amount;
                break;
            case StatType.Charisma:
                Charisma += (int)amount;
                break;
            case StatType.Intelligence:
                Intelligence += (int)amount;
                break;
            case StatType.Agility:
                Agility += (int)amount;
                break;
            case StatType.Luck:
                Luck += (int)amount;
                break;
        }
        
        Debug.Log($"{Controller.name} gained temporary +{amount} {stat} for {duration} seconds");
        
        if (duration > 0)
            Controller.StartCoroutine(RemoveBoostAfterDelay(stat, (int)amount, duration));
    }

    private IEnumerator RemoveBoostAfterDelay(StatType stat, int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        switch (stat)
        {
            case StatType.Strength:
                Strength -= amount;
                break;
            case StatType.Perception:
                Perception -= amount;
                break;
            case StatType.Endurance:
                Endurance -= amount;
                break;
            case StatType.Charisma:
                Charisma -= amount;
                break;
            case StatType.Intelligence:
                Intelligence -= amount;
                break;
            case StatType.Agility:
                Agility -= amount;
                break;
            case StatType.Luck:
                Luck -= amount;
                break;
        }
        Debug.Log($"{Controller.name}'s temporary {stat} boost expired");
    }


    public void IncreaseStat(StatType type, int amount = 1)
    {
        switch (type)
        {
            case StatType.Strength: Strength += amount; break;
            case StatType.Perception: Perception += amount; break;
            case StatType.Endurance: Endurance += amount; break;
            case StatType.Charisma: Charisma += amount; break;
            case StatType.Intelligence: Intelligence += amount; break;
            case StatType.Agility: Agility += amount; break;
            case StatType.Luck: Luck += amount; break;
        }
    }

    public int GetStat(StatType type)
    {
        return type switch
        {
            StatType.Strength => Strength,
            StatType.Perception => Perception,
            StatType.Endurance => Endurance,
            StatType.Charisma => Charisma,
            StatType.Intelligence => Intelligence,
            StatType.Agility => Agility,
            StatType.Luck => Luck,
            _ => 0,
        };
    }
    
    private void HandleLevelUp(LevelUpData data)
    {
        if (data.levelUpChoice == LevelUpChoiceType.Stat && data.statType.HasValue)
        {
            IncreaseStat(data.statType.Value, data.amount);
            Debug.Log($"{Controller.name} increased {data.statType.Value} by {data.amount} → now: {GetStat(data.statType.Value)}");
            Controller.GetModule<HealthModule>().IncreaseMaxHealth(data.amount);
        }
    }

}