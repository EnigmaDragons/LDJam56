using UnityEngine;

[CreateAssetMenu]
public sealed class Navigator : ScriptableObject
{
   
    public static void NavigateToMainMenu() => NavigateTo("MainMenu");
    public static void NavigateToCredits() => NavigateTo("CreditsScene");
    public static void NavigateToScene(string sceneName) => NavigateTo(sceneName);

    private static void NavigateTo(string sceneName)
    {
        Log.Info($"Navigating to {sceneName}");
        Message.Publish(new NavigateToSceneRequested(sceneName));
    }
}
