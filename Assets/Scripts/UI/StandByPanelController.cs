using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandByPanelController : MonoBehaviour
{
   public GameObject mainMenuPanel;
   
   [Header("Standby Timings")]
   public float standByFadeDuration = 2.0f;
   public float standByWaitDuration = 2.5f;
   
   public CanvasGroup standByPanel;

   private void Start()
   {
      StartCoroutine(StandbySequence());
   }

   private IEnumerator StandbySequence()
   {
      standByPanel.alpha = 0f;
      
      yield return StartCoroutine(FadeCanvasGroup(standByPanel, 0f, 1f, standByFadeDuration));
      yield return new WaitForSeconds(standByWaitDuration);
      yield return StartCoroutine(FadeCanvasGroup(standByPanel, 1f, 0f, standByFadeDuration));
      standByPanel.gameObject.SetActive(false);

      mainMenuPanel.SetActive(true);
      this.gameObject.SetActive(false);
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
