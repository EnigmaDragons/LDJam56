using UnityEngine;

public class PlayOneShotSoundEffect
{
    public SoundEffectEnum SoundEffect { get; set; }
    public GameObject Source { get; set; }

    public PlayOneShotSoundEffect(SoundEffectEnum soundEffect, GameObject obj)
    {
        SoundEffect = soundEffect;
        Source = obj;
    }
}