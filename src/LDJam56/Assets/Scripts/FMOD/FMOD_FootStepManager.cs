using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMOD_FootStepManager : MonoBehaviour
{
    [SerializeField] PhysicsPM_MatDetector pM_MatDetector;
    public EventReference footstepRef; 
    EventInstance footstep;
    EventInstance jumpStep; 

    private void Start()
    {
        footstep = RuntimeManager.CreateInstance(footstepRef);
       // jumpStep = RuntimeManager.CreateInstance("event:/CHAR/JumpLand");
    }

    public void FootStepDown()
    {
        footstep.start();
        footstep.setParameterByName("Terrain", pM_MatDetector.matIdx);
    }

    private void OnDestroy()
    {
        footstep.release();
        jumpStep.release();
    }

    public void JumpSound()
    {
            jumpStep.start();
        jumpStep.setParameterByName("Terrain", pM_MatDetector.matIdx);
    }

    public void SetFootstepRunningLength() //set from AnimatorToSoundController
    {
        footstep.setParameterByName("FootStep_Speed", 1);
    }

    public void SetFootstepWalkLength() //set from AnimatorToSoundController
    {
        footstep.setParameterByName("FootStep_Speed", 0);
    }

}
