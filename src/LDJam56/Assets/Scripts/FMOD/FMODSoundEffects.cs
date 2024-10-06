
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class FMODSoundEffects : OnMessage<PlayOneShotSoundEffect, StartSoundEffect, StopSoundEffect>
{
    [SerializeField] private EventReference missingSoundEffect;
    [SerializeField] private EventReference shootOneSound;
    [SerializeField] private EventReference botExplodeSound;
    [SerializeField] private EventReference enemyShootSoundEffect;
    [SerializeField] private EventReference projectileHit;
    [SerializeField] private EventReference explode;

    //Message.Publish(new PlayOneShotSoundEffect { SoundEffect = SoundEffectEnum., Source =  });
    //Message.Publish(new StartSoundEffect { SoundEffect = SoundEffectEnum., Transform = , Moving = false, Parameters = n });
    //Message.Publish(new StopSoundEffect { SoundEffect = SoundEffectEnum. });
    
    private EventReference GetEventReference(SoundEffectEnum soundEffect)
    {
        if (soundEffect == SoundEffectEnum.BotExplode)
            return botExplodeSound;
        if (soundEffect == SoundEffectEnum.ShootOne)
            return shootOneSound;
        if (soundEffect == SoundEffectEnum.EnemyShoot)
            return enemyShootSoundEffect;
        if (soundEffect == SoundEffectEnum.ProjectileHit)
            return projectileHit;
        if (soundEffect == SoundEffectEnum.Explode)
            return explode;
        Debug.LogError($"Missing Sound Effect {soundEffect}");
        return missingSoundEffect;
    }
    
    private Dictionary<SoundEffectEnum, EventInstance> soundEffectInstances;

    private void Awake() => soundEffectInstances = new Dictionary<SoundEffectEnum, EventInstance>();

    protected override void Execute(PlayOneShotSoundEffect msg)
    {
        if (msg.Source != null)
            RuntimeManager.PlayOneShotAttached(GetEventReference(msg.SoundEffect), msg.Source);
        else 
            RuntimeManager.PlayOneShot(GetEventReference(msg.SoundEffect), msg.Position);
    }

    protected override void Execute(StartSoundEffect msg)
    {
        if (soundEffectInstances.ContainsKey(msg.SoundEffect))
            Execute(new StopSoundEffect{ SoundEffect = msg.SoundEffect });
        var sound = GetEventReference(msg.SoundEffect);
        soundEffectInstances[msg.SoundEffect] = RuntimeManager.CreateInstance(sound);
        if (msg.Transform != null)
            soundEffectInstances[msg.SoundEffect].set3DAttributes(RuntimeUtils.To3DAttributes(msg.Transform));
        if (msg.Moving)
            RuntimeManager.AttachInstanceToGameObject(soundEffectInstances[msg.SoundEffect], msg.Transform);
        foreach (var parameter in msg.Parameters)
            soundEffectInstances[msg.SoundEffect].setParameterByName(parameter.Key, parameter.Value);
        soundEffectInstances[msg.SoundEffect].start();
    }

    protected override void Execute(StopSoundEffect msg)
    {
        if (!soundEffectInstances.ContainsKey(msg.SoundEffect))
            return;
        soundEffectInstances[msg.SoundEffect].stop(STOP_MODE.ALLOWFADEOUT);
        soundEffectInstances[msg.SoundEffect].release();
        soundEffectInstances.Remove(msg.SoundEffect);
    }
}