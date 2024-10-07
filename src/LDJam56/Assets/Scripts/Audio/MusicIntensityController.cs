using UnityEngine;
using System.Linq;

public class MusicIntensityController : MonoBehaviour
{
    [SerializeField] private Fmod_MusicState musicState;
    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private float nearnessRange = 20f;
    [SerializeField] private int enemyThresholdForIntensity1 = 3;
    [SerializeField] private int enemyThresholdForIntensity2 = 7;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            UpdateMusicIntensity();
            timer = 0f;
        }
    }

    private void UpdateMusicIntensity()
    {
        if (CurrentMiniMapState.Data.playerTransform == null)
            return;

        var playerPosition = CurrentMiniMapState.Data.playerTransform.position;
        var nearbyEnemies = CurrentMiniMapState.Data.enemyTransforms.Where(e => e.gameObject.activeSelf).Count(e => 
            Vector3.Distance(e.position, playerPosition) <= nearnessRange);

        float intensity;
        if (nearbyEnemies >= enemyThresholdForIntensity2)
            intensity = 2f;
        else if (nearbyEnemies >= enemyThresholdForIntensity1)
            intensity = 1f;
        else
            intensity = 0f;

        musicState.SetIntensity(intensity);
    }
}
