
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UIPanelTypes
{
    MainMenu, // Start
    Settings,
    CharacterCreation,
    StoryReveal,
    PipBoy,
    Loot,
    Dialog,
    Battle,
    Pause,
    Death,
    Notification,       
    Tooltip,            
    QuestPopup,        
    ConfirmationPopup,  
    LoadingScreen       
}

public enum UILayerTypes
{
    Background,    
    Persistent,    
    Midground,    
    Overlay,      
    Popup,      
    System    
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject InteractionTextObject;

    private Dictionary<UIPanelTypes, UILayerTypes> _panelLayerMapping;
    
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
        InteractionTextObject = GameObject.FindGameObjectWithTag("InteractionText");
        InteractionTextObject.SetActive(false);
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
            { UIPanelTypes.StoryReveal, UILayerTypes.Overlay },
            { UIPanelTypes.PipBoy, UILayerTypes.Midground },
            { UIPanelTypes.Loot, UILayerTypes.Overlay },
            { UIPanelTypes.Dialog, UILayerTypes.Midground },
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
        Debug.Log("PANEL TRIGGERED!");
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
