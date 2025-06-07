public abstract class Item
{
    public ItemConfig config;
    public int quantity;
    public float durability; // 0.0f - 1.0f arasında

    public Item(ItemConfig config, int quantity = 1)
    {
        this.config = config;
        this.quantity = quantity;

        if (config is IDurability)
            this.durability = ((IDurability)config).maxDurability; // dayanıksız item değilse dolu başlar
    }

    public string GetName() => config.itemName;
    public ItemType GetItemType() => config.itemType;
}