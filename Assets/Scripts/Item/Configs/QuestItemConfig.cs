using UnityEngine;

[CreateAssetMenu(menuName = "Items/Quest Item")]
public class QuestItemConfig : ItemConfig
{
    public string questId;

    private void OnEnable()
    {
        itemType = ItemType.Quest;
    }
}