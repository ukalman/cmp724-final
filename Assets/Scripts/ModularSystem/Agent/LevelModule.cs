using System;
using UnityEngine;

public enum LevelUpChoiceType
{
    Stat,
    Skill
}

public class LevelUpData /* Event data class */
{
    public LevelUpChoiceType levelUpChoice;
    public StatType? statType;
    public SkillType? skillType;
    public int amount;

    public LevelUpData(StatType stat, int amount)
    {
        levelUpChoice = LevelUpChoiceType.Stat;
        statType = stat;
        this.amount = amount;
    }

    public LevelUpData(SkillType skill, int amount)
    {
        levelUpChoice = LevelUpChoiceType.Skill;
        skillType = skill;
        this.amount = amount;
    }
}

public class LevelModule : ModuleBase
{
    private LevelModuleConfig _config;

    private int _currentLevel;
    private int _currentXP;
    private bool _pendingConfirmation;

    public int CurrentLevel => _currentLevel;
    public int CurrentXP => _currentXP;
    public bool IsPendingConfirmation => _pendingConfirmation;

    public event Action<int> OnLevelUpPending;   // Oyuncuya UI gösterilecek
    public event Action<LevelUpData> OnLevelChanged;     // StatsModule, SkillsModule gibi yerler dinleyecek

    /* Fields that player chooses to increase when leveled up */
    private StatType? _chosenStat;
    private SkillType? _chosenSkill;
    private int _statIncreaseAmount;
    private int _skillIncreaseAmount;
    
    public LevelModule(LevelModuleConfig config)
    {
        _config = config;
    }

    public override void Initialize()
    {
        base.Initialize();
        _statIncreaseAmount = 0;
        _skillIncreaseAmount = 0;
        _currentLevel = _config.startingLevel;
        _currentXP = _config.startingXP;
        _pendingConfirmation = false;
    }

    public void GainXP(int amount)
    {
        if (_pendingConfirmation) return;

        _currentXP += amount;

        while (_currentXP >= GetXPForNextLevel())
        {
            _currentXP -= GetXPForNextLevel();
            _pendingConfirmation = true;
            OnLevelUpPending?.Invoke(_currentLevel + 1);
            break; // Seçim yapılmadan 2 kere level atlanamaz
        }
    }

    public void ChooseStatToIncrease(StatType stat, int increaseAmount)
    {
        _chosenStat = stat;
        _statIncreaseAmount = increaseAmount;
        _chosenSkill = null;
    }

    public void ChooseSkillToIncrease(SkillType skill, int increaseAmount)
    {
        _chosenSkill = skill;
        _skillIncreaseAmount = increaseAmount;
        _chosenStat = null;
    }
    
    public void ConfirmLevelUp()
    {
        if (!_pendingConfirmation) return;

        _currentLevel++;
        _pendingConfirmation = false;

        if (_chosenStat.HasValue)
        {
            OnLevelChanged?.Invoke(new LevelUpData(_chosenStat.Value,_statIncreaseAmount));
        }
        else if (_chosenSkill.HasValue)
        {
            OnLevelChanged?.Invoke(new LevelUpData(_chosenSkill.Value, _skillIncreaseAmount));
        }

        _chosenStat = null;
        _chosenSkill = null;
    }
    

    private int GetXPForNextLevel()
    {
        // Örnek bir seviye formülü
        return 100 + (_currentLevel * 50);
    }
}