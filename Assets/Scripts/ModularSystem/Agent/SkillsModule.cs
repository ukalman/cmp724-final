using System;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    None,
    Melee,
    Ranged,
    Sneak,
    Lockpicking,
    Science
}

public class SkillsModule : ModuleBase
{
    private SkillsModuleConfig _config;
    private Dictionary<SkillType, int> _skills = new();

    public int GetSkill(SkillType type) => _skills.TryGetValue(type, out int val) ? val : 0;

    public SkillsModule(SkillsModuleConfig config)
    {
        _config = config;

        foreach (var entry in _config.startingSkills)
        {
            _skills[entry.skillType] = entry.value;
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        var levelModule = Controller.GetModule<LevelModule>();
        if (levelModule != null)
        {
            levelModule.OnLevelChanged += HandleLevelUp;
        }
    }

    private void HandleLevelUp(LevelUpData data)
    {
        if (data.levelUpChoice != LevelUpChoiceType.Skill || !data.skillType.HasValue) return;

        SkillType skill = data.skillType.Value;
        int amount = data.amount;

        if (_skills.ContainsKey(skill))
            _skills[skill] += amount;
        else
            _skills[skill] = amount;

        Debug.Log($"{Controller.name} increased {skill} skill to {_skills[skill]}");
    }
}
