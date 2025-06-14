using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DialogNode", menuName = "Dialogue/Dialog Node")]
public class DialogNode : ScriptableObject
{
    [TextArea] public string text;

    [System.Serializable]
    public class DialogChoice
    {
        public string choiceText;
        public DialogNode nextNode;
        
        public Action onChosen;
        public string criticalChoice;
        
    }

    public List<DialogChoice> choices;

    public bool IsEnd => choices == null || choices.Count == 0;
}