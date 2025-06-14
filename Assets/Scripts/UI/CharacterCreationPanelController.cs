using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterCreationManager : MonoBehaviour
{
    [System.Serializable]
    public class StatEntry
    {
        public string statKey;
        public Button statButton;
        public Image statBackground;
        public TMP_Text statText;
        public TMP_Text statValue;
        public bool isSpecial;
        public string description;

        public int MinValue => isSpecial ? 1 : 0;
    }

    [Header("Stat Definitions")]
    public List<StatEntry> allStats;

    [Header("Selection Visuals")]
    public Color defaultTextColor = new Color(0.2470588f,1.0f,0.0f);
    public Color selectedTextColor = new Color(1.0f, 0.85f, 0.4f); 
    public CanvasGroup upArrow;
    public CanvasGroup downArrow;
    public TMP_Text descriptionText;

    [Header("Free Point Values")]
    public int totalSpecialPoints = 10;
    public int totalSkillPoints = 5;

    [Header("Free Point Texts")]
    public TMP_Text specialPointsText;
    public TMP_Text skillPointsText;

    [Header("Ready Button")]
    public Button readyButton;

    [Header("Panel Game Objects")] 
    public GameObject statSelectionPanel;
    public GameObject nameSelectionPanel;

    [Header("Name Selection Panel")] 
    public TMP_InputField nameInputField;
    public TMP_Text nameWarningText;
    
    private StatEntry selectedStat = null;
    private Dictionary<StatEntry, int> statValues = new Dictionary<StatEntry, int>();

    private int usedSpecialPoints = 0;
    private int usedSkillPoints = 0;

    private void Start()
    {
        foreach (var stat in allStats)
        {
            int initial = stat.isSpecial ? 1 : 0;
            statValues[stat] = initial;
            stat.statValue.text = initial.ToString();
            stat.statButton.onClick.AddListener(() => OnStatClicked(stat));
        }

        UpdateFreePointTexts();
        UpdateReadyButton();
        ClearSelection();
    }

    private void OnStatClicked(StatEntry stat)
    {
        ClearSelection();

        selectedStat = stat;
        
        stat.statBackground.color = new Color(0f, 1f, 0f, 1f);
        stat.statText.color = selectedTextColor;
        stat.statValue.color = selectedTextColor;
        descriptionText.text = stat.description;

        SetArrowActive(true);
    }

    private void ClearSelection()
    {
        selectedStat = null;
        foreach (var stat in allStats)
        {
            stat.statBackground.color = new Color(0f, 1f, 0f, 0f);
            stat.statText.color = defaultTextColor;
            stat.statValue.color = defaultTextColor;
        }
        SetArrowActive(false);
        descriptionText.text = "";
    }

    private void SetArrowActive(bool active)
    {
        upArrow.interactable = active;
        upArrow.alpha = active ? 1 : 0.25f;
        downArrow.interactable = active;
        downArrow.alpha = active ? 1 : 0.25f;
    }

    public void IncreaseSelected()
    {
        if (selectedStat == null) return;

        int current = statValues[selectedStat];
        int maxPoints = selectedStat.isSpecial ? totalSpecialPoints - usedSpecialPoints : totalSkillPoints - usedSkillPoints;

        if (maxPoints <= 0) return;

        statValues[selectedStat]++;
        selectedStat.statValue.text = statValues[selectedStat].ToString();

        if (selectedStat.isSpecial) usedSpecialPoints++;
        else usedSkillPoints++;

        UpdateFreePointTexts();
        UpdateReadyButton();
    }

    public void DecreaseSelected()
    {
        if (selectedStat == null) return;

        int current = statValues[selectedStat];
        if (current <= selectedStat.MinValue) return;

        statValues[selectedStat]--;
        selectedStat.statValue.text = statValues[selectedStat].ToString();

        if (selectedStat.isSpecial) usedSpecialPoints--;
        else usedSkillPoints--;

        UpdateFreePointTexts();
        UpdateReadyButton();
    }

    private void UpdateFreePointTexts()
    {
        specialPointsText.text = $"{totalSpecialPoints - usedSpecialPoints}";
        skillPointsText.text = $"{totalSkillPoints - usedSkillPoints}";
    }
    
    private void UpdateReadyButton()
    {
        bool ready = (totalSpecialPoints - usedSpecialPoints) == 0 && (totalSkillPoints - usedSkillPoints) == 0;
        readyButton.interactable = ready;
    }
    
    public void ResetAll()
    {
        usedSpecialPoints = 0;
        usedSkillPoints = 0;

        foreach (var stat in allStats)
        {
            int initial = stat.isSpecial ? 1 : 0;
            statValues[stat] = initial;
            stat.statValue.text = initial.ToString();
        }

        UpdateFreePointTexts();
        UpdateReadyButton();
        ClearSelection();
    }
    
    public void OnReadyButtonClicked()
    {
        GameManager.Instance.selectedSpecials.Clear();
        GameManager.Instance.selectedSkills.Clear();
        foreach (var stat in allStats)
        {
            if (stat.isSpecial)
            {
                GameManager.Instance.selectedSpecials[stat.statKey] = statValues[stat]; 
            }
            else
            {
                GameManager.Instance.selectedSkills[stat.statKey] = statValues[stat]; 
            }
            
        }
        
        statSelectionPanel.SetActive(false);
        nameSelectionPanel.SetActive(true);
        
        nameInputField.characterLimit = 10;
        nameInputField.text = "";
        nameWarningText.text = "";
        nameInputField.ActivateInputField();
    }
    
    public void OnNameConfirmed()
    {
        string name = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            nameWarningText.text = "Name cannot be empty.";
            return;
        }

        GameManager.Instance.playerName = name;
        SceneManager.LoadScene("WastelandScene");
    }
    
    public void OnBackToStats()
    {
        nameSelectionPanel.SetActive(false);
        statSelectionPanel.SetActive(true);
        ClearSelection(); 
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
