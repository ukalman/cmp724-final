using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TextHoverFade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI _text;
    private Color _originalColor;
    private Coroutine _fadeCoroutine;

    public float fadedAlpha = 0.5f;
    public float fadeDuration = 0.2f;

    void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _originalColor = _text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartFade(_originalColor.a, fadedAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartFade(_text.color.a, _originalColor.a);
    }

    void StartFade(float from, float to)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeAlpha(from, to));
    }

    System.Collections.IEnumerator FadeAlpha(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            float alpha = Mathf.Lerp(from, to, t);
            _text.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        _text.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, to);
    }
}