
using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIPanelTypes
{
    MainMenu, // Start
    Settings,
    CharacterCreation,
    PipBoy,
    Loot,
    Dialogue,
    Battle,
    Pause,
    Death,
    Notification,       // Basit uyarılar (top-right corner gibi)
    Tooltip,            // Hover açıklamaları
    QuestPopup,         // Yeni görev geldiğinde çıkan küçük pencere
    ConfirmationPopup,  // Evet/Hayır gibi UI’lar
    LoadingScreen       // Sahne geçişleri için
}

public enum UILayerTypes
{
    Background,    // Ana menü arka planı gibi şeyler
    Persistent,    // Oyun boyunca açık kalan HUD (Healthbar, Compass, Objective Tracker vs.)
    Midground,     // PipBoy, Dialogue gibi büyük paneller
    Overlay,       // Pause, Death gibi tüm ekranı kaplayan UI’lar
    Popup,         // Confirmation, Tooltip, Notification gibi küçük pencereler
    System         // Loading ekranı gibi UI akışını durduranlar
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Dictionary<UIPanelTypes, UILayerTypes> _panelLayerMapping;

    [SerializeField] private LootPanelController _lootPanel;

    [SerializeField] public AgentController playerController; /* todo test için */
    [SerializeField] public InteractableController interactableController; /* todo test için */

    /* Actions - Events */
    public Action<Item, ItemListElement> onItemElementSelected;
    public Action<SkillType, SkillsListElement> onSkillElementSelected; // skill selection in pip-boy
    
    /* Pip-boy */
    public PipBoyPanels lastSelectedPipBoyPanel;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            SetupMappings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        lastSelectedPipBoyPanel = PipBoyPanels.Stats;
        GameManager.Instance.OnUIPanelTriggered += TriggerPanel;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnUIPanelTriggered -= TriggerPanel;
    }

    private void Update()
    {
    }

    private void SetupMappings()
    {
        _panelLayerMapping = new Dictionary<UIPanelTypes, UILayerTypes>
        {
            { UIPanelTypes.MainMenu, UILayerTypes.Overlay },
            { UIPanelTypes.Settings, UILayerTypes.Popup },
            { UIPanelTypes.CharacterCreation, UILayerTypes.Overlay },
            { UIPanelTypes.PipBoy, UILayerTypes.Midground },
            { UIPanelTypes.Loot, UILayerTypes.Overlay },
            { UIPanelTypes.Dialogue, UILayerTypes.Midground },
            { UIPanelTypes.Battle, UILayerTypes.Persistent },
            { UIPanelTypes.Pause, UILayerTypes.Overlay },
            { UIPanelTypes.Death, UILayerTypes.System },
            { UIPanelTypes.Notification, UILayerTypes.Popup },
            { UIPanelTypes.Tooltip, UILayerTypes.Popup },
            { UIPanelTypes.QuestPopup, UILayerTypes.Popup },
            { UIPanelTypes.ConfirmationPopup, UILayerTypes.Popup },
            { UIPanelTypes.LoadingScreen, UILayerTypes.System }
        };
    }

    private int GetLayerIndex(UILayerTypes layer)
    {
        return (int)layer;
    }

    public void TriggerPanel(UIPanelTypes type, bool isOpened)
    {
        if (isOpened)
        {
            if (_panelLayerMapping.TryGetValue(type, out var layer))
            {
                CoreUISignals.OnOpenPanel?.Invoke(type, GetLayerIndex(layer));
            }
            else
            {
                Debug.LogError($"No layer mapping defined for panel: {type}");
            }
        }
        else
        {
            if (_panelLayerMapping.TryGetValue(type, out var layer))
            {
                CoreUISignals.OnClosePanel?.Invoke(GetLayerIndex(layer));
            } 
        }
    }

    public void ClosePanel(UIPanelTypes type)
    {
        if (_panelLayerMapping.TryGetValue(type, out var layer))
        {
            CoreUISignals.OnClosePanel?.Invoke(GetLayerIndex(layer));
        }
    }

    public void CloseAllPanels()
    {
        CoreUISignals.OnCloseAllPanels?.Invoke();
    }
}
