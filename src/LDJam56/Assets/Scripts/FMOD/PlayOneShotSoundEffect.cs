using UnityEngine;

public class PlayOneShotSoundEffect
{
    public SoundEffectEnum SoundEffect { get; set; }
    public GameObject Source { get; set; }
    public Vector3 Position { get; set; } = Vector3.zero;

    public PlayOneShotSoundEffect(SoundEffectEnum soundEffect, Vector3 pos)
    { 
        SoundEffect = soundEffect;
        Position = pos;
    }
    
    public PlayOneShotSoundEffect(SoundEffectEnum soundEffect, GameObject obj)
    {
        SoundEffect = soundEffect;
        Source = obj;
    }
}
