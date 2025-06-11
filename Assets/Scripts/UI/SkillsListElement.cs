using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class SkillsListElement : MonoBehaviour
{
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillValText;
    [SerializeField] private Image backgroundImage;

    private SkillType _skillType;
    
    private Color _textDefaultColor;
    
    public void Initialize(SkillType skillType, int val)
    {
        _skillType = skillType;
        _textDefaultColor = skillNameText.color;
        
        var color = backgroundImage.color;
        color.a = 0.0f;
        backgroundImage.color = color;

        switch (skillType)
        {
            case SkillType.None:
                break;
            case SkillType.Melee:
                skillNameText.text = "Melee";
                break;
            case SkillType.Ranged:
                skillNameText.text = "Ranged";
                break;
            case SkillType.Sneak:
                skillNameText.text = "Sneak";
                break;
            case SkillType.Lockpicking:
                skillNameText.text = "Lockpicking";
                break;
            case SkillType.Science:
                skillNameText.text = "Science";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(skillType), skillType, null);
        }

        skillValText.text = val.ToString();
    }

    public void OnClick()
    {
        UIManager.Instance.onSkillElementSelected?.Invoke(_skillType,this);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            var color = backgroundImage.color;
            color.a = 0.7f;
            skillNameText.color = Color.black;
            skillValText.color = Color.black;
            backgroundImage.color = color;
        }
        else
        {
            var color = backgroundImage.color;
            color.a = 0.0f;
            skillNameText.color = _textDefaultColor;
            skillValText.color = _textDefaultColor;
            backgroundImage.color = color;
        }
    }
}