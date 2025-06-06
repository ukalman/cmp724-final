using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SkillsModuleConfig", menuName = "Agent/SkillsModuleConfig")]
public class SkillsModuleConfig : ScriptableObject
{
    public List<SkillEntry> startingSkills;

    [System.Serializable]
    public class SkillEntry
    {
        public SkillType skillType;
        public int value;
    }
}