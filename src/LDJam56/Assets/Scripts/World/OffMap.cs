using System;
using System.Collections.Generic;
using UnityEngine;

public class OffMap : MonoBehaviour
{
    [SerializeField] private float secondsPerPlayerPositionSnapshot;
    [SerializeField] private float snapshotsToSave;

    private GameObject _player;
    private float _t;
    private Queue<Vector3> _previousPlayerLocations;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _t = secondsPerPlayerPositionSnapshot;
        _previousPlayerLocations = new Queue<Vector3>();
    }

    private void Update()
    {
        _t -= Time.deltaTime;
        if (_t <= 0)
        {
            _previousPlayerLocations.Enqueue(_player.transform.position);
            _t += secondsPerPlayerPositionSnapshot;
            if (_previousPlayerLocations.Count > snapshotsToSave)
                _previousPlayerLocations.Dequeue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentGameState.DamagePlayer(true);
        var location = _previousPlayerLocations.Dequeue();
        _previousPlayerLocations = new Queue<Vector3>();
        _previousPlayerLocations.Enqueue(location);
        Message.Publish(new TeleportPlayer { NewPosition = location });
    }
}