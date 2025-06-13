using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("CanvasGroups")]
    public CanvasGroup parentGroup;
    public CanvasGroup mainMenuGroup;
    public CanvasGroup settingsGroup;

    [Header("Fade Settings")]
    public float parentFadeInDuration = 1.0f;
    public float delayAfterParentFade = 0.5f;
    public float panelFadeDuration = 0.5f;

    private void Start()
    {
        // Başlangıç durumları
        parentGroup.alpha = 0f;
        parentGroup.interactable = false;
        parentGroup.blocksRaycasts = false;

        mainMenuGroup.alpha = 0f;
        mainMenuGroup.interactable = false;
        mainMenuGroup.blocksRaycasts = false;

        settingsGroup.alpha = 0f;
        settingsGroup.interactable = false;
        settingsGroup.blocksRaycasts = false;

        StartCoroutine(StartupSequence());
    }

    private IEnumerator StartupSequence()
    {
        // Parent Panel fade in
        yield return StartCoroutine(FadeCanvasGroup(parentGroup, 0f, 1f, parentFadeInDuration));
        parentGroup.interactable = true;
        parentGroup.blocksRaycasts = true;

        yield return new WaitForSeconds(delayAfterParentFade);

        // Main Menu açılır
        yield return StartCoroutine(FadeCanvasGroup(mainMenuGroup, 0f, 1f, panelFadeDuration));
        mainMenuGroup.interactable = true;
        mainMenuGroup.blocksRaycasts = true;
    }
    
    public void OnSettingsButtonClicked()
    {
        StartCoroutine(SwitchPanels(mainMenuGroup, settingsGroup));
    }
    
    public void OnBackButtonClicked()
    {
        StartCoroutine(SwitchPanels(settingsGroup, mainMenuGroup));
    }
    
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("CharacterCreation");
    }
    
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private IEnumerator SwitchPanels(CanvasGroup fromGroup, CanvasGroup toGroup)
    {
        fromGroup.interactable = false;
        fromGroup.blocksRaycasts = false;
        yield return StartCoroutine(FadeCanvasGroup(fromGroup, 1f, 0f, panelFadeDuration));
        
        yield return StartCoroutine(FadeCanvasGroup(toGroup, 0f, 1f, panelFadeDuration));
        toGroup.interactable = true;
        toGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            cg.alpha = Mathf.Lerp(from, to, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        cg.alpha = to;
    }
}
