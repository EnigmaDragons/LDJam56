using System;
using UnityEngine;

public class OffMap : MonoBehaviour
{
    [SerializeField] private float secondsPerPlayerPositionSnapshot;
    [SerializeField] private float snapshotsToSave;
    
    private GameObject _player;
    private float _t;
    
    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }
    
    
}