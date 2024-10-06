using System;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private float animationTime;

    private float _timeRemaining;
    private HashSet<EnemyHandeler> _hits;

    public Action<EnemyHandeler> _onIndividualHit;

    public void Start()
    {
        _hits = new HashSet<EnemyHandeler>();
        _timeRemaining = animationTime;
    }

    public void Init(Vector3 startingPosition, AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        _onIndividualHit = e => e.Damaged((int)data.Amount);
    }

    private void Update()
    {
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining <= 0)
            Destroy(gameObject);
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