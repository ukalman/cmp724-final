using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum PipBoyPanels
{
    Stats,
    Inventory,
    Data
}

public class PipBoyPanelController : MonoBehaviour
{
    [Header("General Buttons Texts & Background Images")]
    [SerializeField] private TMP_Text statsTabButtonText;
    [SerializeField] private TMP_Text inventoryTabButtonText;
    [SerializeField] private TMP_Text dataTabButtonText;
    
    [SerializeField] private Image statsTabImage;
    [SerializeField] private Image inventoryTabImage;
    [SerializeField] private Image dataTabImage;
    private Color _pipBoyTabTextDefaultColor; 
    
    [Header("Stats Buttons Texts & Background Images")] 
    [SerializeField] private TMP_Text statusTabButtonText;
    [SerializeField] private TMP_Text specialTabButtonText;
    [SerializeField] private TMP_Text skillsTabButtonText;
    [SerializeField] private Image statusTabImage;
    [SerializeField] private Image specialTabImage;
    [SerializeField] private Image skillsTabImage;
    private Color _statTextDefaultColor;
    
    [Header("Status Tab")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text apText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private Image avatarImage;

    [Header("Special Tab")] 
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private TMP_Text strengthValText;
    [SerializeField] private TMP_Text perceptionText;
    [SerializeField] private TMP_Text perceptionValText;
    [SerializeField] private TMP_Text enduranceText;
    [SerializeField] private TMP_Text enduranceValText;
    [SerializeField] private TMP_Text charismaText;
    [SerializeField] private TMP_Text charismaValText;
    [SerializeField] private TMP_Text intelligenceText;
    [SerializeField] private TMP_Text intelligenceValText;
    [SerializeField] private TMP_Text agilityText;
    [SerializeField] private TMP_Text agilityValText;
    [SerializeField] private TMP_Text luckText;
    [SerializeField] private TMP_Text luckValText;
    [SerializeField] private Image[] specialButtonImages;
    [SerializeField] private TMP_Text specialDescriptionText;
    private Image[] specialImages; /* TODO Resources'tan yukle, stata göre image componentine set et */
    private Color _specialTextDefaultColor;
    
    [Header("Skills Tab")] 
    private Transform _skillsListElementContainer;
    [SerializeField] private TMP_Text skillDescriptionText;
    [SerializeField] private SkillsListElement skillsListElementPrefab;
    
    [Header("Inventory Buttons, Texts & Background Images")]
    [SerializeField] private TMP_Text allTabButtonText;
    [SerializeField] private TMP_Text weaponTabButtonText;
    [SerializeField] private TMP_Text armorTabButtonText;
    [SerializeField] private TMP_Text consumableTabButtonText;
    [SerializeField] private TMP_Text questTabButtonText;
    [SerializeField] private TMP_Text miscTabButtonText;
    [SerializeField] private Image allTabImage;
    [SerializeField] private Image weaponTabImage;
    [SerializeField] private Image armorTabImage;
    [SerializeField] private Image consumableTabImage;
    [SerializeField] private Image questTabImage;
    [SerializeField] private Image miscTabImage;
    
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private TMP_Text detailNameText;
    [SerializeField] private TMP_Text detailDescriptionText;
    [SerializeField] private TMP_Text detailStatsText;
    
    [SerializeField] private ItemListElement itemListElementPrefab;
    [SerializeField] private TMP_Text playerWeightText;
    [SerializeField] private Button discardButton;
    private Transform _playerItemListContainer;
    private InventoryModule _inventoryModule;
    private IReadOnlyList<Item> _playerItems;
    
    private int _categoryIndex = 0;
    private readonly string[] _categories = { "All", "Weapon", "Armor", "Consumable", "Quest", "Misc" };
    private Item _selectedItem;
    
    [Header("PipBoy Panel GOs")] 
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private GameObject statusPanel;
    [SerializeField] private GameObject specialPanel;
    [SerializeField] private GameObject skillsPanel;
    
    /* Player controller */
    private AgentController _playerController;

    private void OnEnable()
    {
        UIManager.Instance.onSkillElementSelected += OnSkillElementSelected;
        UIManager.Instance.onItemElementSelected += OnItemSelected;
    }

    private void OnDisable()
    {
        UIManager.Instance.onSkillElementSelected -= OnSkillElementSelected;
        UIManager.Instance.onItemElementSelected -= OnItemSelected;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameManager.Instance.PlayerController;
        _inventoryModule = _playerController.GetModule<InventoryModule>();
        _playerItems = _inventoryModule.Items;
        
        _skillsListElementContainer = GameObject.FindGameObjectWithTag("SkillsContent").transform;

        _pipBoyTabTextDefaultColor = new Color(0.01433013f,0.754717f,0.0f);
        _statTextDefaultColor = new Color(0.09009103f,0.7735849f ,0.0f);
        _specialTextDefaultColor = new Color(0.2028302f, 1.0f, 0.2803903f);

        _playerItemListContainer = GameObject.FindGameObjectWithTag("PipBoyInventory").transform;
        
        ClearPipBoyTabSelections();
        switch (UIManager.Instance.lastSelectedPipBoyPanel)
        {
            case PipBoyPanels.Stats:
                Debug.Log("ON STATS CLICKED LAAA");
                OnStatsClicked();
                break;
            case PipBoyPanels.Inventory:
                Debug.Log("ON INVENTORY CLICKED LAAA");
                OnInventoryClicked();
                break;
            case PipBoyPanels.Data:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ClearPipBoyTabSelections()
    {
        statsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        
        statsTabButtonText.color = _pipBoyTabTextDefaultColor;
        inventoryTabButtonText.color = _pipBoyTabTextDefaultColor;
        dataTabButtonText.color = _pipBoyTabTextDefaultColor;

        var color = statsTabImage.color;
        color.a = 0.0f;

        statsTabImage.color = color;
        inventoryTabImage.color = color;
        dataTabImage.color = color;
    }
    
    /* Stats Panel Initialize - Update */
    private void InitializeStatsPanel()
    {
        statsPanel.SetActive(true);
        InitializeStatusPanel();
        InitializeSpecialPanel();
        InitializeSkillsPanel();
        DeactivateStatsPanels();
        ClearStatsTabSelections();
        OnStatusClicked();
    }
    
    
    private void InitializeInventoryPanel()
    {
        inventoryPanel.SetActive(true);
        RefreshInventoryList();  
        ClearInventorySelections();
        OnAllClicked();
    }
    

    private void DeactivateStatsPanels()
    {
        Debug.Log("HE Mİ LAAAAAAAAAAAA");
        statusPanel.SetActive(false);
        specialPanel.SetActive(false);
        skillsPanel.SetActive(false);
    }

    private void InitializeStatusPanel()
    {
        levelText.text = "LVL " + _playerController.GetModule<LevelModule>().CurrentLevel;
        hpText.text = "HP " + _playerController.GetModule<HealthModule>().GetHealth() + "/" +
                      _playerController.GetModule<HealthModule>().GetMaxHealth();
        apText.text = "AP " + _playerController.GetModule<CombatModule>().CurrentAP + "/" +
                      _playerController.GetModule<CombatModule>().MaxAP;
        xpText.text = "XP " + _playerController.GetModule<LevelModule>().CurrentXP + "/"; /* TODO BİR SONRAKİ LEVELİN XP'Sİ EKLENECEK */
        /* TODO AVATAR RESMİNİ DE EKLE */
    }

    private void InitializeSpecialPanel()
    {
        strengthValText.text = _playerController.GetModule<StatsModule>().Strength.ToString();
        perceptionValText.text = _playerController.GetModule<StatsModule>().Perception.ToString();
        enduranceValText.text = _playerController.GetModule<StatsModule>().Endurance.ToString();
        charismaValText.text = _playerController.GetModule<StatsModule>().Charisma.ToString();
        intelligenceValText.text = _playerController.GetModule<StatsModule>().Intelligence.ToString();
        agilityValText.text = _playerController.GetModule<StatsModule>().Agility.ToString();
        luckValText.text = _playerController.GetModule<StatsModule>().Luck.ToString();
        ClearSpecialSelections();
    }
    
    private void InitializeSkillsPanel()
    {
        RefreshSkillSelections();
    }

    private void ClearStatsTabSelections()
    {
        var color = specialTabImage.color;
        color.a = 0.0f;
        statusTabImage.color = color;
        specialTabImage.color = color;
        skillsTabImage.color = color;
        statusTabButtonText.color = _statTextDefaultColor;
        specialTabButtonText.color = _statTextDefaultColor;
        skillsTabButtonText.color = _statTextDefaultColor;
    }
    
    private void ClearSpecialSelections()
    {
        specialDescriptionText.text = "";
        strengthText.color = _specialTextDefaultColor;
        strengthValText.color = _specialTextDefaultColor;
        perceptionText.color = _specialTextDefaultColor;
        perceptionValText.color = _specialTextDefaultColor;
        enduranceText.color = _specialTextDefaultColor;
        enduranceValText.color = _specialTextDefaultColor;
        charismaText.color = _specialTextDefaultColor;
        charismaValText.color = _specialTextDefaultColor;
        intelligenceText.color = _specialTextDefaultColor;
        intelligenceValText.color = _specialTextDefaultColor;
        agilityText.color = _specialTextDefaultColor;
        agilityValText.color = _specialTextDefaultColor;
        luckText.color = _specialTextDefaultColor;
        luckValText.color = _specialTextDefaultColor;

        foreach (var image in specialButtonImages)
        {
            var color = Color.black;
            color.a = 0.0f;
            image.color = color;
        }
    }

    private void RefreshSkillSelections()
    {
        skillDescriptionText.text = "";
        foreach (Transform child in _skillsListElementContainer)
            Destroy(child.gameObject);

        foreach (var skillType in _playerController.GetModule<SkillsModule>().unlockedSkillTypes)
        {
            var element = CreateSkillElementButton(skillType, _skillsListElementContainer, _playerController.GetModule<SkillsModule>().GetSkill(skillType));
            element.SetSelected(false);
        }
    }

    private SkillsListElement CreateSkillElementButton(SkillType skillType, Transform parent, int val)
    {
        var element = Instantiate(skillsListElementPrefab, parent,false);
        element.Initialize(skillType,val);

        return element;
    }
    
    private void RefreshInventoryList()
    {
        ClearList(_playerItemListContainer);

        foreach (var item in _playerItems)
        {
            Debug.Log("Creating item for player");
            if (IsItemInCategory(item, _categories[_categoryIndex]))
                CreateItemButton(item, _playerItemListContainer, true);
        }
        RefreshItemListElementSelections();
        UpdateDetailPanel();
        UpdateWeightInfo();
    }
    
    private bool IsItemInCategory(Item item, string category)
    {
        if (category == "All") return true;
        return item.config.itemType.ToString() == category;
    }
    
    private void CreateItemButton(Item item, Transform parent, bool isPlayerInventory)
    {
        Debug.Log($"Parent: {parent.gameObject.scene.name}");
        var element = Instantiate(itemListElementPrefab, parent,false);
        if (element == null)
        {
            Debug.Log("ELEMENT IS NULLLLL");
        }
        element.Initialize(item,isPlayerInventory);
    }

    private void OnItemSelected(Item item, ItemListElement itemListElement)
    {
        _selectedItem = item;
        RefreshItemListElementSelections();
        itemListElement.SetSelected(true);
        discardButton.gameObject.SetActive(true);
        UpdateDetailPanel();
    }

    private void RefreshItemListElementSelections()
    {
        discardButton.gameObject.SetActive(false);
        foreach (Transform child in _playerItemListContainer)
        {
            if (child == null) Debug.Log("CHILD IS NULLLLL");
            child.GetComponent<ItemListElement>().SetSelected(false);
        }
    }

    private void ClearInventorySelections()
    {
        allTabButtonText.color = _pipBoyTabTextDefaultColor;
        allTabImage.color = Color.black;
        weaponTabButtonText.color = _pipBoyTabTextDefaultColor;
        weaponTabImage.color = Color.black;
        armorTabButtonText.color = _pipBoyTabTextDefaultColor;
        armorTabImage.color = Color.black;
        consumableTabButtonText.color = _pipBoyTabTextDefaultColor;
        consumableTabImage.color = Color.black;
        questTabButtonText.color = _pipBoyTabTextDefaultColor;
        questTabImage.color = Color.black;
        miscTabButtonText.color = _pipBoyTabTextDefaultColor;
        miscTabImage.color = Color.black;
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
    }

    private void UpdateWeightInfo()
    {
        float current = _inventoryModule.GetCurrentWeight();
        float max = _inventoryModule.GetMaxCarryWeight();
        playerWeightText.text = $"{current:0.0} / {max:0.0}";
    }
    
    private void ClearList(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
    
    /* Button on-clicks */
    public void OnStatsClicked()
    {
        UIManager.Instance.lastSelectedPipBoyPanel = PipBoyPanels.Stats;
        ClearPipBoyTabSelections();
        statsTabButtonText.color = Color.black;
        statsTabImage.color = _pipBoyTabTextDefaultColor;
        InitializeStatsPanel();
    }

    public void OnInventoryClicked()
    {
        UIManager.Instance.lastSelectedPipBoyPanel = PipBoyPanels.Inventory;
        ClearPipBoyTabSelections();
        inventoryTabButtonText.color = Color.black;
        inventoryTabImage.color = _pipBoyTabTextDefaultColor;
        InitializeInventoryPanel();
    }

    public void OnDataClicked()
    {
        UIManager.Instance.lastSelectedPipBoyPanel = PipBoyPanels.Data;
    }
    
    public void OnStatusClicked()
    {
        DeactivateStatsPanels();
        ClearStatsTabSelections();
        statusPanel.SetActive(true);
        statusTabButtonText.color = Color.black;
        statusTabImage.color = _statTextDefaultColor;
    }

    public void OnSpecialClicked()
    {
        DeactivateStatsPanels();
        ClearStatsTabSelections();
        specialPanel.SetActive(true);
        specialTabButtonText.color = Color.black;
        specialTabImage.color = _statTextDefaultColor;
    }

    public void OnSkillsClicked()
    {
        DeactivateStatsPanels();
        ClearStatsTabSelections();
        skillsPanel.SetActive(true);
        skillsTabButtonText.color = Color.black;
        skillsTabImage.color = _statTextDefaultColor;
    }
    
    public void OnStrengthClicked()
    {
        ClearSpecialSelections();
        strengthText.color = Color.black;
        strengthValText.color = Color.black;
        specialDescriptionText.text = "STRENGTH DESCRIPTION";
        specialButtonImages[0].color = _specialTextDefaultColor;
    }
    
    public void OnPerceptionClicked()
    {
        ClearSpecialSelections();
        strengthText.color = Color.black;
        strengthValText.color = Color.black;
        specialDescriptionText.text = "STRENGTH DESCRIPTION";
        specialButtonImages[1].color = _specialTextDefaultColor;
    }
    
    public void OnEnduranceClicked()
    {
        ClearSpecialSelections();
        enduranceText.color = Color.black;
        enduranceValText.color = Color.black;
        specialDescriptionText.text = "ENDURANCE DESCRIPTION";
        specialButtonImages[2].color = _specialTextDefaultColor;
    }
    
    public void OnCharismaClicked()
    {
        ClearSpecialSelections();
        charismaText.color = Color.black;
        charismaValText.color = Color.black;
        specialDescriptionText.text = "CHARISMA DESCRIPTION";
        specialButtonImages[3].color = _specialTextDefaultColor;
    }
    
    public void OnIntelligenceClicked()
    {
        ClearSpecialSelections();
        intelligenceText.color = Color.black;
        intelligenceValText.color = Color.black;
        specialDescriptionText.text = "INTELLIGENCE DESCRIPTION";
        specialButtonImages[4].color = _specialTextDefaultColor;
    }
    
    public void OnAgilityClicked()
    {
        ClearSpecialSelections();
        agilityText.color = Color.black;
        agilityValText.color = Color.black;
        specialDescriptionText.text = "AGILITY DESCRIPTION";
        specialButtonImages[5].color = _specialTextDefaultColor;
    }
    
    public void OnLuckClicked()
    {
        ClearSpecialSelections();
        luckText.color = Color.black;
        luckValText.color = Color.black;
        specialDescriptionText.text = "LUCK DESCRIPTION";
        specialButtonImages[6].color = _specialTextDefaultColor;
    }

    public void OnSkillElementSelected(SkillType skillType, SkillsListElement element)
    {
        RefreshSkillSelections();
        element.SetSelected(true);

        switch (skillType)
        {
            case SkillType.None:
                break;
            case SkillType.Melee:
                skillDescriptionText.text = "MELEE DESCRIPTION";
                break;
            case SkillType.Ranged:
                skillDescriptionText.text = "RANGED DESCRIPTION";
                break;
            case SkillType.Sneak:
                skillDescriptionText.text = "SNEAK DESCRIPTION";
                break;
            case SkillType.Lockpicking:
                skillDescriptionText.text = "LOCKPICKING DESCRIPTION";
                break;
            case SkillType.Science:
                skillDescriptionText.text = "SCIENCE DESCRIPTION";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(skillType), skillType, null);
        }
    }

    public void OnAllClicked()
    {
        ClearInventorySelections();
        _categoryIndex = 0;
        allTabButtonText.color = Color.black;
        allTabImage.color = _pipBoyTabTextDefaultColor;
        RefreshInventoryList();
    }
    
    public void OnWeaponClicked()
    {
        ClearInventorySelections();
        _categoryIndex = 1;
        weaponTabButtonText.color = Color.black;
        weaponTabImage.color = _pipBoyTabTextDefaultColor;
        RefreshInventoryList();
    }
    
    public void OnArmorClicked()
    {
        ClearInventorySelections();
        _categoryIndex = 2;
        armorTabButtonText.color = Color.black;
        armorTabImage.color = _pipBoyTabTextDefaultColor;
        RefreshInventoryList();
    }
    
    public void OnConsumableClicked()
    {
        ClearInventorySelections();
        _categoryIndex = 3;
        consumableTabButtonText.color = Color.black;
        consumableTabImage.color = _pipBoyTabTextDefaultColor;
        RefreshInventoryList();
    }
    
    public void OnQuestClicked()
    {
        ClearInventorySelections();
        _categoryIndex = 4;
        questTabButtonText.color = Color.black;
        questTabImage.color = _pipBoyTabTextDefaultColor;
        RefreshInventoryList();
    }
    
    public void OnMiscClicked()
    {
        ClearInventorySelections();
        _categoryIndex = 5;
        miscTabButtonText.color = Color.black;
        miscTabImage.color = _pipBoyTabTextDefaultColor;
        RefreshInventoryList();
    }
    
    public void OnDiscardClicked()
    {
        if (_inventoryModule.RemoveItem(_selectedItem))
        {
            discardButton.gameObject.SetActive(false);
            _selectedItem = null;
            RefreshInventoryList();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
