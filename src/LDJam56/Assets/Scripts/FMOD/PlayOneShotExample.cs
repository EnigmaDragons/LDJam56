using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Reflection;

public class PlayOneShotExample : MonoBehaviour
{
    public EventReference bulletSoundRef; //so you can pick the sound event
    private EventInstance bulletInstance;// just the var really
    Rigidbody rb;

    //both of these cannot be stopped once triggered
    public void PlayBullet()
    {
        RuntimeManager.PlayOneShot(bulletSoundRef);
    }

    public void PlayBulletProjectile()
    {
        RuntimeManager.PlayOneShotAttached(bulletSoundRef, gameObject);
    }

    //this can be stopped 
    public void PlayeBulletMoreComplex()
    {
        bulletInstance = RuntimeManager.CreateInstance(bulletSoundRef);
        bulletInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform)); //
        RuntimeManager.AttachInstanceToGameObject(bulletInstance, transform, rb);
        bulletInstance.start();
       
    }
}
