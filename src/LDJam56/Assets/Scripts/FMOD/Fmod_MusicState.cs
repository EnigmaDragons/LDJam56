﻿using UnityEngine;

public class Fmod_MusicState : MonoBehaviour
{
    private FMOD.Studio.EventInstance musicEvent;

    private void Start()
    {
        musicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Music");
    }

    public void SetGameplayState()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Gamestate", 1f); // Assuming 1f represents "Gameplay" state
        Debug.Log("Music state set to Gameplay");
    }

    public void SetBossState()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Gamestate", 2f); // Assuming 2f represents "Boss" state
        Debug.Log("Music state set to Boss");
    }
    
    public void SetCreditsState()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Gamestate", 3f); // Assuming 3f represents "Credits" state
        Debug.Log("Music state set to Credits");
    }

    public void SetGameOverState()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Gamestate", 4f); // Assuming 4f represents "Game Over" state
        Debug.Log("Music state set to Game Over");
    }
    
    public void SetIntroState()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Gamestate", 5f); 
        Debug.Log("Music state set to Intro");
    }

    public void SetVictoryState()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Gamestate", 6f); 
        Debug.Log("Music state set to Victory");
    }

    public void SetIntensity(float intensity)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Intensity", Mathf.Clamp(intensity, 0f, 2f));
    }

    public void SetIsLowHealth(bool isLow)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Low Health", isLow ? 1f : 0f);
    }

    private void OnDestroy()
    {
        musicEvent.release();
    }
}
