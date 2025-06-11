using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private List<Transform> layers = new List<Transform>();

    private Dictionary<UIPanelTypes, GameObject> _panelPrefabs;
    private Dictionary<int, GameObject> _activePanels; // key: layer index, value: instance

    private void Awake()
    {
        CacheUIPrefabs();
        _activePanels = new Dictionary<int, GameObject>();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreUISignals.OnClosePanel += OnClosePanel;
        CoreUISignals.OnOpenPanel += OnOpenPanel;
        CoreUISignals.OnCloseAllPanels += OnCloseAllPanels;
    }

    private void UnsubscribeEvents()
    {
        CoreUISignals.OnClosePanel -= OnClosePanel;
        CoreUISignals.OnOpenPanel -= OnOpenPanel;
        CoreUISignals.OnCloseAllPanels -= OnCloseAllPanels;
    }

    private void CacheUIPrefabs()
    {
        _panelPrefabs = new Dictionary<UIPanelTypes, GameObject>();
        foreach (UIPanelTypes panelType in System.Enum.GetValues(typeof(UIPanelTypes)))
        {
            var prefab = Resources.Load<GameObject>($"UIPanels/{panelType}Panel");
            if (prefab != null)
            {
                _panelPrefabs[panelType] = prefab;
            }
            else
            {
                Debug.LogWarning($"Could not find UI prefab for {panelType} at path: UIPanels/{panelType}Panel");
            }
        }
    }

    private void OnOpenPanel(UIPanelTypes panelType, int layerIndex)
    {
        OnClosePanel(layerIndex); // Close any existing panel in the layer

        if (!_panelPrefabs.TryGetValue(panelType, out var prefab))
        {
            Debug.LogError($"No cached prefab for panel type: {panelType}");
            return;
        }

        if (layerIndex < 0 || layerIndex >= layers.Count)
        {
            Debug.LogError($"Invalid layer index: {layerIndex}");
            return;
        }

        GameObject instance = Instantiate(prefab, layers[layerIndex]);

        switch (panelType)
        {
            case UIPanelTypes.MainMenu:
                break;
            case UIPanelTypes.Settings:
                break;
            case UIPanelTypes.CharacterCreation:
                break;
            case UIPanelTypes.PipBoy:
                break;
            case UIPanelTypes.Loot:
                instance.GetComponent<LootPanelController>().Initialize(GameManager.Instance.PlayerController, GameManager.Instance.CurrentInteractable);
                /* TODO BU INITIALIZE'I DİREKT STARTTA BİLE VEREBİLİRİZ BELKİ? */
                break;
            case UIPanelTypes.Dialogue:
                break;
            case UIPanelTypes.Battle:
                break;
            case UIPanelTypes.Pause:
                break;
            case UIPanelTypes.Death:
                break;
            case UIPanelTypes.Notification:
                break;
            case UIPanelTypes.Tooltip:
                break;
            case UIPanelTypes.QuestPopup:
                break;
            case UIPanelTypes.ConfirmationPopup:
                break;
            case UIPanelTypes.LoadingScreen:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(panelType), panelType, null);
        }
        
        
        _activePanels[layerIndex] = instance;
    }

    private void OnClosePanel(int layerIndex)
    {
        if (_activePanels.TryGetValue(layerIndex, out var panel))
        {
            if (panel != null)
            {
                Destroy(panel);
            }
            _activePanels.Remove(layerIndex);
        }
    }

    private void OnCloseAllPanels()
    {
        foreach (var kvp in _activePanels)
        {
            if (kvp.Value != null)
                Destroy(kvp.Value);
        }
        _activePanels.Clear();
    }
}
