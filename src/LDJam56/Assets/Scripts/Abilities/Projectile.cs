using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject hitPrefab;

    private Vector3 _target;
    private Action _onHit;
    private Action<EnemyHandeler> _onEnemyHit;
    private float _speed;
    
    public void Init(Vector3 startingPosition, Vector3 direction, AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        _target = startingPosition + direction * data.Range;
        _onHit = () => { };
        _speed = data.Speed;
        _onEnemyHit = e => e.Damaged((int)data.Amount);
    }

    private void Update()
    {
        if (transform.position == _target)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyHandeler>();
        if (enemy != null)
            _onEnemyHit(enemy);
        _onHit();
        Instantiate(hitPrefab, transform.position, transform.rotation, transform.parent);
        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.ProjectileHit, other.gameObject));
        Destroy(gameObject);
    }
}