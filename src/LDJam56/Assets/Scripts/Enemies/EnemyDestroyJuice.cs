using UnityEngine;

public class EnemyDestroyJuice : MonoBehaviour
{
    public GameObject deathVFXPrefab;

    public void OnDestroy()
    {   
        if (deathVFXPrefab != null)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
        }
        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.BotExplode, transform.position));
    }
}
