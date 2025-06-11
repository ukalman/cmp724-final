using System;
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
    }

    private void OnDisable()
    {
        UIManager.Instance.onSkillElementSelected -= OnSkillElementSelected;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameManager.Instance.PlayerController;
        
        _skillsListElementContainer = GameObject.FindGameObjectWithTag("SkillsContent").transform;

        _pipBoyTabTextDefaultColor = new Color(0.01433013f,0.754717f,0.0f);
        _statTextDefaultColor = new Color(0.09009103f,0.7735849f ,0.0f);
        _specialTextDefaultColor = new Color(0.2028302f, 1.0f, 0.2803903f);

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
    
    /*
    private void InitializeInventoryPanel()
    {
        inventoryPanel.SetActive(true);
        InitializeStatusPanel();
        InitializeSpecialPanel();
        InitializeSkillsPanel();
        DeactivateStatsPanels();
        ClearStatsTabSelections();
        statusPanel.SetActive(true);
    }
    */

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
    
    /* Button on-clicks */
    public void OnStatsClicked()
    {
        ClearPipBoyTabSelections();
        statsTabButtonText.color = Color.black;
        statsTabImage.color = _pipBoyTabTextDefaultColor;
        InitializeStatsPanel();
    }

    public void OnInventoryClicked()
    {
        ClearPipBoyTabSelections();
        inventoryTabButtonText.color = Color.black;
        inventoryTabImage.color = _pipBoyTabTextDefaultColor;
        //InitializeInventoryPanel();
    }

    public void OnDataClicked()
    {
        
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
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
