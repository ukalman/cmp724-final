using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class StoryRevealPanel : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] storyLines;

    public TMP_Text storyText;
    public CanvasGroup panelGroup;

    public float fadeDuration = 0.5f;
    public float lineDelay = 2.0f;

    public Button continueButton;
    
    private void Start()
    {
        continueButton.interactable = false;
        storyText.text = "";
        panelGroup.alpha = 1f;
        StartCoroutine(RevealStory());
    }

    private IEnumerator RevealStory()
    {
        foreach (string line in storyLines)
        {
            yield return StartCoroutine(FadeInLine(line));
            yield return new WaitForSeconds(lineDelay);
        }

        continueButton.interactable = true;
    }

    private IEnumerator FadeInLine(string line)
    {
        storyText.text = "";
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / fadeDuration;
            int charCount = Mathf.FloorToInt(Mathf.Lerp(0, line.Length, t));
            storyText.text = line.Substring(0, charCount);
            yield return null;
        }

        storyText.text = line;
    }

    public void OnContinueClicked()
    {
        GameManager.Instance.OnUIPanelTriggered?.Invoke(UIPanelTypes.StoryReveal,false);
    }
}