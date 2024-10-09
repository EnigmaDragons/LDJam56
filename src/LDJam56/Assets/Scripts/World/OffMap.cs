using System;
using System.Collections.Generic;
using UnityEngine;

public class OffMap : MonoBehaviour
{
    [SerializeField] private float secondsPerPlayerPositionSnapshot;
    [SerializeField] private float snapshotsToSave;

    private GameObject _player;
    private float _t;
    private Queue<Vector3> _previousPlayerLocations = new Queue<Vector3>();

    private void Start()
    {
        var pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null)
            _player = pObj.transform.gameObject;
        _t = secondsPerPlayerPositionSnapshot;
    }

    private void Update()
    {
        if (_player != null)
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
        else
        {
            var pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null)
                _player = pObj.transform.gameObject;
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

    private void OnTriggerStay(Collider other)
    {
        CurrentGameState.DamagePlayer(true);
        var location = _previousPlayerLocations.Dequeue();
        _previousPlayerLocations = new Queue<Vector3>();
        _previousPlayerLocations.Enqueue(location);
        Message.Publish(new TeleportPlayer { NewPosition = location });
    }
}