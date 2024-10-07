using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadingController : OnMessage<NavigateToSceneRequested, HideLoadUiRequested>
{
    [SerializeField] private CanvasGroup loadUi;
    [SerializeField] private float loadFadeDuration = 0.5f;
    [SerializeField] private bool debugLoggingEnabled;
    [SerializeField] private UnityEvent onStartedLoading;

    private bool _isLoading;
    private float _startedTransitionAt;
    private AsyncOperation _loadState;

    private void Awake() => loadUi.alpha = 0;
    
    protected override void Execute(NavigateToSceneRequested msg)
    {
        Log.Info($"Loading scene {msg.SceneName} - Reload: {msg.Reload}");
        _isLoading = true;
        onStartedLoading.Invoke();
        _startedTransitionAt = Time.timeSinceLevelLoad;
        this.ExecuteAfterDelay(() =>
        {
            Log.Info($"Coroutine started - Loading scene {msg.SceneName} - Reload: {msg.Reload}");
            if (msg.Reload && msg.SceneName == SceneManager.GetActiveScene().name)
            {
                Log.Info($"Reloading scene {msg.SceneName}");
                _loadState = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                _loadState.completed += (op) => {
                    Log.Info($"Scene {msg.SceneName} unloaded, loading...");
                    _loadState = SceneManager.LoadSceneAsync(msg.SceneName, LoadSceneMode.Single);
                };
            }
            else
            {
                Log.Info($"Loading scene {msg.SceneName}");
                _loadState = SceneManager.LoadSceneAsync(msg.SceneName);
            }
            _loadState.completed += OnLoadFinished;
        }, loadFadeDuration);
    }

    protected override void Execute(HideLoadUiRequested msg)
    {
        if (!_isLoading && loadUi.alpha <= 0f)
            loadUi.alpha = 0f;
    }

    private void Update()
    {
        if (!_isLoading && loadUi.alpha <= 0f)
            return;
        
        var t = Time.timeSinceLevelLoad;
        var fadeProgress =  Mathf.Min(1, (t - _startedTransitionAt) / loadFadeDuration);
        loadUi.alpha = _isLoading 
            ? Math.Max(loadUi.alpha, Mathf.Lerp(0f, 1f, fadeProgress))
            : Mathf.Lerp(1f, 0f, fadeProgress);
        if (debugLoggingEnabled)
            Debug.Log($"Loader - Alpha {loadUi.alpha} - Fade Progress {fadeProgress}");
    }

    private void OnLoadFinished(AsyncOperation _)
    {
        _isLoading = false;
        _startedTransitionAt = Time.timeSinceLevelLoad;
        _loadState.completed -= OnLoadFinished;
    }
}
