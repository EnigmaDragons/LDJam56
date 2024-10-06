using System;
using UnityEngine;

public class Speed : MonoBehaviour
{
    private Action _onComplete;
    private float _duration;

    public void Init(AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        CurrentGameState.UpdateState(s => s.PlayerStats.Speed += data.Amount);
        _duration = data.Duration;
        _onComplete = () => CurrentGameState.UpdateState(s => s.PlayerStats.Speed -= data.Amount);
    }

    private void Update()
    {
        _duration -= Time.deltaTime;
        if (_duration <= 0)
        {
            _onComplete();
            Destroy(this);
        }
    }
}