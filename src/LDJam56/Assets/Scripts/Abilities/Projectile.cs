using System;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private Collider collider;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Explode explodePrefab;

    private Vector3 _target;
    private Action _onHit;
    private Action<EnemyHandeler> _onEnemyHit;
    private float _speed;
    private float _timeTilDeath = 0;
    
    public void Init(Vector3 startingPosition, Vector3 direction, AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        _target = startingPosition + direction * data.Range;
        _onHit = () => { };
        if (nextAbilities.Length > 0 && nextAbilities[0].Type == AbilityComponentType.Explode && (type == AbilityType.Attack || type == AbilityType.Special))
            _onHit = () =>
            {
                var explode = Instantiate(explodePrefab,
                    new Vector3(transform.position.x, explodePrefab.transform.position.y, transform.position.z),
                    Quaternion.identity, transform.parent);
                explode.Init(false, nextAbilities[0], type, nextAbilities.Skip(1).ToArray());
            };

        _speed = data.Speed;
        _onEnemyHit = e => {
            e.Damaged((int)data.Amount);
            Vector3 knockbackDirection = (_target - transform.position).normalized;
            e.GetComponent<Rigidbody>().AddForce(knockbackDirection * data.KnockbackForce, ForceMode.Impulse);
        };
    }

    private void Update()
    {
        if (_timeTilDeath > 0)
        {
            _timeTilDeath -= Time.deltaTime;
            if (_timeTilDeath <= 0)
                Destroy(gameObject);
        }
        else if (transform.position == _target)
            BeginCleanup();
        else
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyHandeler>();
        if (enemy != null)
            _onEnemyHit(enemy);
        _onHit();
        BeginCleanup();
        Instantiate(hitPrefab, transform.position, transform.rotation, transform.parent);
        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.ProjectileHit, other.gameObject));
    }

    private void BeginCleanup()
    {
        _timeTilDeath = 1;
        collider.enabled = false;
        particleSystem.Stop();
    }
}