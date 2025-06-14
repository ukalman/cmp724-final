using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootPanelController : MonoBehaviour
{
    [Header("Inventory Panels")]
    private Transform _playerItemListContainer;
    private Transform _chestItemListContainer;
    [SerializeField] private ItemListElement itemListElementPrefab;

    [Header("Category")]
    [SerializeField] private TMP_Text categoryHeaderText;

    [Header("Detail Panel")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private TMP_Text detailNameText;
    [SerializeField] private TMP_Text detailDescriptionText;
    [SerializeField] private TMP_Text detailStatsText;

    [Header("Weight Info")]
    [SerializeField] private TMP_Text playerWeightText;

    [Header("Action Buttons")]
    [SerializeField] private TMP_Text takeStoreButtonText;
    [SerializeField] private TMP_Text takeStoreAllButtonText;

    private IReadOnlyList<Item> _playerItems;
    private IReadOnlyList<Item> _chestItems;

    private InventoryModule _inventoryModule;
    private InteractableController _interactableController;
    private LootModule _lootModule;
    
    private Item _selectedItem;
    private bool _selectedFromPlayer;

    private int _categoryIndex = 0;
    private readonly string[] _categories = { "All", "Weapon", "Armor", "Consumable", "Quest", "Misc" };

    private void Start()
    {
        detailPanel.SetActive(false);
        UpdateCategoryText();
    }

    private void OnEnable()
    {
        UIManager.Instance.onItemElementSelected += OnItemSelected;
    }

    private void OnDisable()
    {
        UIManager.Instance.onItemElementSelected -= OnItemSelected;
    }

    public void Initialize(AgentController agentController, InteractableController interactableController)
    {
        _inventoryModule = agentController.GetModule<InventoryModule>();
        _interactableController = interactableController;
        _lootModule = interactableController.GetModule<LootModule>();
        
        _playerItems = _inventoryModule.Items;
        _chestItems = _lootModule.Items;
        
        _playerItemListContainer = GameObject.FindGameObjectWithTag("InventoryContent").transform;
        _chestItemListContainer = GameObject.FindGameObjectWithTag("LootContent").transform;
        
        RefreshLists();
    }

    public void OnNextCategory()
    {
        _categoryIndex = (_categoryIndex + 1) % _categories.Length;
        UpdateCategoryText();
        RefreshLists();
    }

    public void OnPreviousCategory()
    {
        _categoryIndex = (_categoryIndex - 1 + _categories.Length) % _categories.Length;
        UpdateCategoryText();
        RefreshLists();
    }

    private void UpdateCategoryText()
    {
        categoryHeaderText.text = _categories[_categoryIndex];
    }

    private void RefreshLists()
    {
        ClearList(_playerItemListContainer);
        ClearList(_chestItemListContainer);

        foreach (var item in _playerItems)
        {
            Debug.Log("Creating item for player");
            
            if (IsItemInCategory(item, _categories[_categoryIndex]))
            {
                if (item.config is ArmorConfig armorConfig)
                {
                    if (_inventoryModule.equippedItems.TryGetValue(armorConfig.slot, out var equippedItem) &&
                        equippedItem == item)
                    {
                        CreateItemButton(item, _playerItemListContainer, true,true);
                    }
                    else
                    {
                        CreateItemButton(item, _playerItemListContainer, true,false);
                    }
                    
                }
                else
                {
                    CreateItemButton(item, _playerItemListContainer, true,false);
                }
                
            }
        }

        foreach (var item in _chestItems)
        {
            Debug.Log("Creating item for chest");
            CreateItemButton(item, _chestItemListContainer, false, false);
        }

        UpdateWeightInfo();
    }

    private void RefreshItemListElementSelections()
    {
        foreach (Transform child in _playerItemListContainer)
        {
            child.GetComponent<ItemListElement>().SetSelected(false);
        }
        
        foreach (Transform child in _chestItemListContainer)
        {
            child.GetComponent<ItemListElement>().SetSelected(false);
        }
    }

    private void ClearList(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }

    private void CreateItemButton(Item item, Transform parent, bool isPlayerInventory, bool isEquipped)
    {
        Debug.Log($"Parent: {parent.gameObject.scene.name}");
        var element = Instantiate(itemListElementPrefab, parent,false);
        element.Initialize(item,isPlayerInventory, isEquipped);
    }

    private void OnItemSelected(Item item, ItemListElement itemListElement)
    {
        _selectedItem = item;
        _selectedFromPlayer = itemListElement.isPlayerInventory;
        RefreshItemListElementSelections();
        itemListElement.SetSelected(true);
        UpdateDetailPanel();
    }

    private void UpdateDetailPanel()
    {
        if (_selectedItem == null)
        {
            detailPanel.SetActive(false);
            return;
        }

        detailPanel.SetActive(true);
        detailNameText.text = _selectedItem.GetName();
        detailDescriptionText.text = _selectedItem.config.description;
        detailStatsText.text = _selectedItem.GetStatText();

        // Button text
        if (_selectedFromPlayer)
        {
            takeStoreButtonText.text = "Store";
            takeStoreAllButtonText.text = "Store All";
        }
        else
        {
            takeStoreButtonText.text = "Take";
            takeStoreAllButtonText.text = "Take All";
        }
    }

    private void UpdateWeightInfo()
    {
        float current = _inventoryModule.GetCurrentWeight();
        float max = _inventoryModule.GetMaxCarryWeight();
        playerWeightText.text = $"{current:0.0} / {max:0.0}";
    }

    public void OnTakeOrStore()
    {
        if (_selectedItem == null) return;

        if (_selectedFromPlayer)
            StoreItem(_selectedItem);
        else
            TakeItem(_selectedItem);

        _selectedItem = null;
        detailPanel.SetActive(false);
        RefreshLists();
    }

    public void OnTakeOrStoreAll()
    {
        if (_selectedFromPlayer)
        {
            foreach (var item in new List<Item>(_playerItems))
                StoreItem(item);
        }
        else
        {
            foreach (var item in new List<Item>(_chestItems))
                TakeItem(item);
        }

        _selectedItem = null;
        detailPanel.SetActive(false);
        RefreshLists();
    }

    private void TakeItem(Item item)
    {
        if (_inventoryModule.TryAddItem(item))
        {
            _lootModule.RemoveItem(item);
        }
    }

    private void StoreItem(Item item)
    {
        if (_lootModule.TryAddItem(item))
        {
            _inventoryModule.RemoveItem(item);
        }
    }

    public void OnExit()
    {
        CoreUISignals.OnClosePanel?.Invoke((int)UILayerTypes.Overlay);
        GameManager.Instance.OnUIPanelTriggered?.Invoke(UIPanelTypes.Loot, false);
        _interactableController.OnInteractionEnded();
    }

    private bool IsItemInCategory(Item item, string category)
    {
        if (category == "All") return true;
        return item.config.itemType.ToString() == category;
    }
}
