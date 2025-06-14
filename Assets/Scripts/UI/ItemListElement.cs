using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemListElement : MonoBehaviour
{
     public TMP_Text nameText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image backgroundImage;
    
    private Item _item;
    private Color _textDefaultColor;
    public bool isPlayerInventory;
    
    public void Initialize(Item item, bool isPlayerInventory, bool isEquipped)
    {
        _item = item;
        _textDefaultColor = nameText.color;
        this.isPlayerInventory = isPlayerInventory;
        
        var color = backgroundImage.color;
        color.a = 0.0f;
        backgroundImage.color = color;
        
        nameText.text = !isEquipped ? item.GetName() : item.GetName() + " (Equipped)";
        if (item.config is AmmoConfig config)
        {
            quantityText.text = $"({config.initialStack.ToString()})";
        }
        else
        {
            quantityText.text = item.quantity > 1 ? $"({item.quantity})" : ""; 
        }
        
    }

    public void OnClick()
    {
        UIManager.Instance.onItemElementSelected?.Invoke(_item,this);
    }

    public Item GetItem()
    {
        return _item;
    }
    
    public void SetSelected(bool selected)
    {
        // örnek: arka plan rengi değişsin
        if (selected)
        {
            var color = backgroundImage.color;
            color.a = 0.7f;
            nameText.color = Color.black;
            quantityText.color = Color.black;
            backgroundImage.color = color;
        }
        else
        {
            var color = backgroundImage.color;
            color.a = 0.0f;
            nameText.color = _textDefaultColor;
            quantityText.color = _textDefaultColor;
            backgroundImage.color = color;
        }
    }
}