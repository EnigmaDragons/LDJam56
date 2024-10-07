using UnityEngine;

[CreateAssetMenu]
public sealed class Navigator : ScriptableObject
{
    public static void NavigateToMainMenu() => NavigateTo("MainMenu");
    public static void NavigateToCredits() => NavigateTo("CreditsScene");
    public static void NavigateToGameScene() => NavigateTo("GameScene");
    public static void NavigateToScene(string sceneName) => NavigateTo(sceneName);
    public static void ReloadGameScene() => UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

    private static void NavigateTo(string sceneName)
    {
        Log.Info($"Navigating to {sceneName}");
        Message.Publish(new NavigateToSceneRequested(sceneName));
    }

    public static void QuitGame()
    {
        Log.Info("Quitting game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        #endif

         #if UNITY_WEBGL
            Log.Info("Navigating to Credits in WebGL");
            NavigateToCredits();
            return;
        #endif

        Application.Quit();
    }
}
