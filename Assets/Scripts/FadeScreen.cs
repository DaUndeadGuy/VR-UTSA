using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;
    public AnimationCurve fadeCurve;
    public string colorPropertyName = "_Color";

    // --- NEW: Events for the GameSequencer to listen to ---
    public UnityEvent onFadeInComplete;
    public UnityEvent onFadeOutComplete;

    private Renderer rend;
    private static bool _isFade;
    public static bool IsFade
    {
        get => _isFade;
        private set => _isFade = value;
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;

        if (fadeOnStart)
            BeginFadeIn();
    }

    public void BeginFadeIn()
    {
        Fade(1, 0);
    }

    public void BeginFadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        rend.enabled = true;

        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, fadeCurve.Evaluate(timer / fadeDuration));
            rend.material.SetColor(colorPropertyName, newColor);
            timer += Time.deltaTime;
            yield return null;
        }

        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        rend.material.SetColor(colorPropertyName, finalColor);

        IsFade = alphaOut != 0;

        if (alphaOut == 0)
        {
            rend.enabled = false;
            // --- NEW: Invoke Fade In Complete Event ---
            if (onFadeInComplete != null)
            {
                onFadeInComplete.Invoke();
            }
        }
        else if (alphaOut == 1)
        {
            // --- NEW: Invoke Fade Out Complete Event ---
            if (onFadeOutComplete != null)
            {
                onFadeOutComplete.Invoke();
            }
        }
    }
}