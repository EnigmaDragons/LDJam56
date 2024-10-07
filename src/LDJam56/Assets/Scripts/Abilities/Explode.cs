using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Explode : MonoBehaviour
{
    [SerializeField] private float animationTime;
    [SerializeField] private float castTime;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileOffset;

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
    
    public void Init(float potency, bool playerOriginator, Vector3 startingPosition, AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        var localPotency = potency * data.GetPotency(type) * CurrentGameState.ReadonlyGameState.PlayerStats.AoEPotency;
        if (nextAbilities.AnyNonAlloc() && nextAbilities[0].Type == AbilityComponentType.Projectile)
        {
            var projectiles = 6 + CurrentGameState.ReadonlyGameState.PlayerStats.Projectile * 2;
            var projectileSpawn = new Vector3(startingPosition.x, projectilePrefab.transform.localPosition.y, startingPosition.z);
            for (var i = 0; i < projectiles; i++)
            {
                var direction = Quaternion.Euler(0, (360 / projectiles) * i, 0) * Vector3.forward;
                var spawn = projectileSpawn + direction * projectileOffset;
                var projectile = Instantiate(projectilePrefab, spawn, Quaternion.LookRotation(direction), transform.parent);
                Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.ShootOne, projectile.gameObject));
                projectile.Init(potency * 0.5f, spawn, direction, nextAbilities[0], type, nextAbilities.Skip(1).ToArray());   
            }
        }
        transform.localScale *= Mathf.Sqrt(localPotency);
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
        _onIndividualHit = e =>
        {
            e.Damaged((int)Math.Ceiling(data.Amount * localPotency));
            this.ExecuteAfterDelay(0.25f, () =>
            {
                Vector3 knockbackDirection = (e.transform.position - transform.position).normalized;
                // Add upward force for a more exaggerated, cartoon-like toss
                knockbackDirection.y = Mathf.Max(knockbackDirection.y, 0.5f);
                knockbackDirection = knockbackDirection.normalized;

                Rigidbody enemyRb = e.GetComponent<Rigidbody>();
                // Use ForceMode.VelocityChange for a more immediate, cartoon-like effect
                enemyRb.AddForce(knockbackDirection * data.KnockbackForce * localPotency, ForceMode.VelocityChange);
                
                // Add a small upward torque for spin
                enemyRb.AddTorque(Random.insideUnitSphere * (data.KnockbackForce * 0.2f * localPotency), ForceMode.Impulse);
            });
        };
        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.PlayerBomb, gameObject));
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