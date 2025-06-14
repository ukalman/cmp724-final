
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsGamePaused)
        {
            GameManager.Instance.IsGamePaused = false;
            Time.timeScale = 1.0f;
            GameManager.Instance.OnUIPanelTriggered?.Invoke(UIPanelTypes.Pause,false);
        }
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene("WastelandScene");
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
