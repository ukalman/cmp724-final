using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogPanelController : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public Transform choicesParent;
    public GameObject choiceButtonPrefab;

    public DialogNode startingNode;
    
    private DialogNode currentNode;

    private void Start()
    {
        GameManager.Instance.SwitchToDialogueCamera();
        StartDialogue(startingNode);
    }

    public void StartDialogue(DialogNode rootNode)
    {
        currentNode = rootNode;
        DisplayCurrentNode();
    }

    private void DisplayCurrentNode()
    {
        dialogText.text = currentNode.text;
        
        foreach (Transform child in choicesParent)
            Destroy(child.gameObject);
        
        foreach (var choice in currentNode.choices)
        {
            var buttonObj = Instantiate(choiceButtonPrefab, choicesParent);
            var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choice.choiceText;

            if (choice.criticalChoice == "Accept")
            {
                choice.onChosen = GameManager.Instance.OnQuestAccepted;
            }
            else if (choice.criticalChoice == "Reject")
            {
                choice.onChosen = GameManager.Instance.OnQuestRejected;
            }
            //var node = choice.nextNode; 
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectChoice(choice));
        }
    }

    private void SelectChoice(DialogNode.DialogChoice choice)
    {
        
        choice.onChosen?.Invoke();

        if (choice.nextNode == null)
        {
            Debug.Log("Dialogue ended.");
            GameManager.Instance.SwitchToMainCamera();
            GameManager.Instance.OnUIPanelTriggered?.Invoke(UIPanelTypes.Dialog,false);
            return;
        }

        currentNode = choice.nextNode;
        DisplayCurrentNode();
    }
}