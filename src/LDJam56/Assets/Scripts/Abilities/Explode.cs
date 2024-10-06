using System;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private float animationTime;
    [SerializeField] private float castTime;

    private Action _onCastFinish;
    private float _remainingCastTime;
    private float _timeRemaining;
    private HashSet<EnemyHandeler> _hits;

    public Action<EnemyHandeler> _onIndividualHit;

    public void Start()
    {
        _hits = new HashSet<EnemyHandeler>();
        _timeRemaining = animationTime;
    }

    public void Init(bool playerOriginator, AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {

        if (playerOriginator)
        {
            var id = Guid.NewGuid().ToString();
            _remainingCastTime = castTime;
            CurrentGameState.UpdateState(s => s.PlayerStats.IsRooted.Add(id));
            CurrentGameState.UpdateState(s => s.PlayerStats.IsSilenced.Add(id));
            _onCastFinish = () => CurrentGameState.UpdateState(s =>
            {
                s.PlayerStats.IsRooted.Remove(id);
                s.PlayerStats.IsSilenced.Remove(id);
            });
        }
        _onIndividualHit = e => e.Damaged((int)data.Amount);
        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.Explode, gameObject));
    }

    private void Update()
    {
        if (_remainingCastTime > 0)
        {
            _remainingCastTime -= Time.deltaTime;
            if (_remainingCastTime <= 0)
                _onCastFinish();   
        }
        else
        {
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining <= 0)
                Destroy(gameObject);   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyHandeler>();
        if (enemy != null && !_hits.Contains(enemy))
        {
            _hits.Add(enemy);
            _onIndividualHit(enemy);
        }
    }
}