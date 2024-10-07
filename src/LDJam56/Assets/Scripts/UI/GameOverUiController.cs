using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class GameOverUiController : OnMessage<GameOver> 
{
    [SerializeField] private CanvasGroup gameOverUi;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float greyScaleDuration = 2f;
    [SerializeField] private string postProcessVolumeTag = "PostProcessVolume";

    private bool _hasLost;
    private PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    private void Start()
    {
        gameOverUi.alpha = 0;
        gameOverUi.interactable = false;
        gameOverUi.blocksRaycasts = false;

        postProcessVolume = GameObject.FindGameObjectWithTag(postProcessVolumeTag)?.GetComponent<PostProcessVolume>();
        if (postProcessVolume == null)
        {
            postProcessVolume = FindObjectOfType<PostProcessVolume>();
        }

        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out ColorGrading cg))
        {
            colorGrading = cg;
            colorGrading.enabled.value = false;
        }
        else
        {
            Debug.LogWarning("PostProcessVolume not found or ColorGrading not available.");
        }
    }

    protected override void Execute(GameOver message)
    {
        if (_hasLost)
            return;
        _hasLost = true;
        Time.timeScale = 0;
        StartCoroutine(FadeToGreyThenShowUI());
    }

    private IEnumerator FadeToGreyThenShowUI()
    {
        if (colorGrading != null)
        {
            colorGrading.enabled.value = true;
            float elapsedTime = 0f;
            while (elapsedTime < greyScaleDuration)
            {
                float t = elapsedTime / greyScaleDuration;
                colorGrading.saturation.value = Mathf.Lerp(0f, -100f, t);
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            colorGrading.saturation.value = -100f;
        }

        // Now that the fade to grey is complete, start fading in the UI
        yield return StartCoroutine(FadeInGameOverUI());
    }

    private IEnumerator FadeInGameOverUI()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            gameOverUi.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        gameOverUi.alpha = 1f;
        gameOverUi.interactable = true;
        gameOverUi.blocksRaycasts = true;
    }
}
