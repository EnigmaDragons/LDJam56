
public class NavigateToSceneRequested
{
    public string SceneName { get; set; }
    public bool Reload { get; set; }

    public NavigateToSceneRequested(string sceneName, bool reload = false) => (SceneName, Reload) = (sceneName, reload);
}
