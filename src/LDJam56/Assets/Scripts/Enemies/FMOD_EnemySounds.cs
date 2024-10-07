using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMOD_EnemySounds : MonoBehaviour
{
    [SerializeField] private EventReference enemyMove;
    [SerializeField] private EventReference enemyFireAtt;
    [SerializeField] private EventReference enemyHitAtt;
    [SerializeField] private EventReference enemyDeath;
    [SerializeField] private EventReference enemyOther;


    public void EnemyMoveSound()
    {
        RuntimeManager.PlayOneShotAttached(enemyMove, gameObject);
    }

    public void EnemyFireAttSound()
    {
        RuntimeManager.PlayOneShotAttached(enemyFireAtt, gameObject);
    }

    public void EnemyHitAttSound()
    {
        RuntimeManager.PlayOneShot(enemyHitAtt);
    }

    public void EnemyDeathSound()
    {
        RuntimeManager.PlayOneShotAttached(enemyDeath, gameObject);
    }
    public void EnemyOtherSound()
    {
        RuntimeManager.PlayOneShot(enemyOther);
    }
}
