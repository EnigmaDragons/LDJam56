using System;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Message.Publish(new BossStarted());
        Destroy(gameObject);
    }
}