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
    public StatType statType;
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
