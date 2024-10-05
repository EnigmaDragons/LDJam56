using UnityEngine;
using UnityEngine.Rendering;

public class EnvironmentConfig : MonoBehaviour
{
    private void Start()
    {
        // // Set Environment settings
        // RenderSettings.ambientMode = AmbientMode.Flat;
        // RenderSettings.ambientEquatorColor = new Color(25f / 255f, 157f / 255f, 185f / 255f);
        // RenderSettings.ambientGroundColor = new Color(25f / 255f, 157f / 255f, 185f / 255f);
        // RenderSettings.ambientSkyColor = new Color(25f / 255f, 157f / 255f, 185f / 255f);
        // RenderSettings.ambientIntensity = 1f;
        // RenderSettings.reflectionIntensity = 1f;
        // RenderSettings.reflectionBounces = 1;
        //
        // // Set fog settings
        // RenderSettings.fog = true;
        // RenderSettings.fogColor = new Color(52f / 255f, 230f / 255f, 1f, 1f);
        // RenderSettings.fogMode = FogMode.Linear;
        // RenderSettings.fogStartDistance = 1.3f;
        // RenderSettings.fogEndDistance = 286.3f;
        //
        // // Set halo settings
        // RenderSettings.haloStrength = 0.445f;
        // RenderSettings.flareStrength = 1f;

        // Note: Some settings from the image cannot be set directly through code
        // For example, the 'Skybox Material' and 'Halo Texture' would typically be set in the editor
        // The 'GPU Instancing' setting is typically set per-material, not globally
    }
}
