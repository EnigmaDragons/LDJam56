using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMOD_VolControls : MonoBehaviour
{
    [SerializeField] MixerVolumeSlider masterVolSlider;
    [SerializeField] MixerVolumeSlider musicVolSlider;
    [SerializeField] MixerVolumeSlider sfxVolSlider;
    [SerializeField] MixerVolumeSlider uiVolSlider;


    private Bus masterVol;
    private Bus musicVol;
    private Bus sfxVol;
    private Bus uiVol;

    public void GetFMODBusMASTER() //master bus for all game sound and music
    {
        masterVol = RuntimeManager.GetBus("bus:/MST_BUS");
        //masterVol.setVolume(masterVolSlider.fmodVol);//from UI canvas normally
    }


    public void GetFMODBusMusic()
    {
        musicVol = RuntimeManager.GetBus("bus:/MST_BUS/MUSIC_MST");
       
    }

    public void GetFMODBusSFX()
    {
        sfxVol = RuntimeManager.GetBus("bus:/MST_BUS/SFX_MST");
       
    }

    public void GetFMODBusUI()
    {
        uiVol = RuntimeManager.GetBus("bus:/MST_BUS/UI_MST");
       
    }

}
