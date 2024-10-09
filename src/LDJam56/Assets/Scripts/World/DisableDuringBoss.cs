using UnityEngine;

public class DisableDuringBoss : OnMessage<BossStarted, BossDied>
{
    [SerializeField] private GameObject[] toDisable;
        
    protected override void Execute(BossStarted msg)
    {
        toDisable.ForEach(x => x.SetActive(false));
    }

    protected override void Execute(BossDied msg)
    {
        toDisable.ForEach(x => x.SetActive(true));
    }
}