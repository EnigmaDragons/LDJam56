using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private Explode explodePrefab;
    
    private Action _onEnemyBump;
    private Action _onComplete;
    private float _duration;

    private HashSet<Collider> _enemiesHit;

    public void Init(float potency, AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        Message.Publish(new StartSoundEffect() { SoundEffect = SoundEffectEnum.PlayerShield, Moving = true, Transform = transform.parent});
        _onEnemyBump = () => { };
        _enemiesHit = new HashSet<Collider>();
        if (nextAbilities.AnyNonAlloc() && nextAbilities[0].Type == AbilityComponentType.Explode)
        {
            _onEnemyBump = () =>
            {
                var explodeStartingPosition = new Vector3(transform.position.x, explodePrefab.transform.position.y, explodePrefab.transform.position.z);
                var explode = Instantiate(explodePrefab, explodeStartingPosition, Quaternion.identity, transform.parent.parent);
                explode.Init(potency * 0.5f, false, explodeStartingPosition, nextAbilities[0], type, nextAbilities.Skip(1).ToArray());
            };
        }
        potency = data.GetPotency(type);
        if (type == AbilityType.Passive)
            potency *= CurrentGameState.ReadonlyGameState.PlayerStats.AfterHitShieldPotency;
        var id = Guid.NewGuid().ToString(); 
        CurrentGameState.UpdateState(s => s.PlayerStats.IsInvincible.Add(id));
        _duration = data.Duration * potency;
        _onComplete = () => CurrentGameState.UpdateState(s => s.PlayerStats.IsInvincible.Remove(id));
    }

    private void Update()
    {
        _duration -= Time.deltaTime;
        if (_duration < Time.deltaTime)
        {
            _onComplete();
            Message.Publish(new StopSoundEffect() { SoundEffect = SoundEffectEnum.PlayerShield });
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enemiesHit.Contains(other))
            return;
        _enemiesHit.Add(other);
        _onEnemyBump();
    }
}