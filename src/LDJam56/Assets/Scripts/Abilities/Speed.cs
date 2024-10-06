using System;
using UnityEngine;

public class Speed : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    
    private Action _onComplete;
    private float _duration;
    private float _timeTilDeath;
    
    public void Init(AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        CurrentGameState.UpdateState(s => s.PlayerStats.Speed += data.Amount);
        _duration = data.Duration;
        _onComplete = () => CurrentGameState.UpdateState(s => s.PlayerStats.Speed -= data.Amount);
    }

    private void Update()
    {
        if (_timeTilDeath > 0)
        {
            _timeTilDeath -= Time.deltaTime;
            if (_timeTilDeath <= 0)
                Destroy(gameObject);
        }
        else
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0)
            {
                _onComplete();
                particleSystem.Stop();
                _timeTilDeath = 1;
            }   
        }
    }
}