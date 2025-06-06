using UnityEngine;

public enum ConsumableEffectType
{
    HealHP,
    BuffStat,
    BuffSkill
}

[System.Serializable]
public class ConsumableEffect
{
    public ConsumableEffectType effectType;
    [Tooltip("Only used if effectType is BuffStat")]
    public StatType statType;
    
    [Tooltip("Only used if effectType is BuffSkill")]
    public SkillType skillType;
    
    public float amount;
    public float duration; // 0 ise anlık etkidir
}

[CreateAssetMenu(menuName = "Items/Consumable")]
public class ConsumableConfig : ItemConfig
{
    public ConsumableEffect[] effects;

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
    }
}
