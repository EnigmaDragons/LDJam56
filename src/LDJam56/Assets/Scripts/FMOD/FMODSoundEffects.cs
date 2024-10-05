﻿using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class FMODSoundEffects : OnMessage<PlayOneShotSoundEffect, StartSoundEffect, StopSoundEffect>
{
    [SerializeField] private EventReference missingSoundEffect;
    [SerializeField] private EventReference shootOneSound;

    //Message.Publish(new PlayOneShotSoundEffect { SoundEffect = SoundEffectEnum., Source =  });
    //Message.Publish(new StartSoundEffect { SoundEffect = SoundEffectEnum., Transform = , Moving = false, Parameters = n });
    //Message.Publish(new StopSoundEffect { SoundEffect = SoundEffectEnum. });
    
    private EventReference GetEventReference(SoundEffectEnum soundEffect)
    {
        if (soundEffect == SoundEffectEnum.ShootOne)
            return shootOneSound;
        
        Debug.LogError($"Missing Sound Effect {soundEffect}");
        return missingSoundEffect;
    }
    
    private Dictionary<SoundEffectEnum, EventInstance> soundEffectInstances;

    private void Awake() => soundEffectInstances = new Dictionary<SoundEffectEnum, EventInstance>();

    protected override void Execute(PlayOneShotSoundEffect msg)
    {
        if (msg.Source == null)
            RuntimeManager.PlayOneShot(GetEventReference(msg.SoundEffect));
        else
            RuntimeManager.PlayOneShotAttached(GetEventReference(msg.SoundEffect), msg.Source);
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