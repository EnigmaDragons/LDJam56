using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Action _onComplete;
    private float _duration;
    
    public void Init(AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        var id = Guid.NewGuid().ToString(); 
        CurrentGameState.UpdateState(s => s.PlayerStats.IsInvincible.Add(id));
        _duration = data.Duration;
        _onComplete = () => CurrentGameState.UpdateState(s => s.PlayerStats.IsInvincible.Remove(id));
    }

    private void Update()
    {
        _duration -= Time.deltaTime;
        if (_duration < Time.deltaTime)
        {
            _onComplete();
            Destroy(gameObject);
        }
    }
}