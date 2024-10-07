using UnityEngine;

public class OneLiners : OnMessage<EnemyKilled>
{
    [SerializeField] private float happyOneLinerChance = 0.05f;
    [SerializeField] private float cooldownDurationSeconds = 14f;
    
    private GameObject player;
    private float lastOneLinerTime;

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    protected override void Execute(EnemyKilled msg)
    {
        if (player == null) return;

        if (Time.time - lastOneLinerTime >= cooldownDurationSeconds && Random.value < happyOneLinerChance)
        {
            Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.PlayerHappyOneLiner, player));
            lastOneLinerTime = Time.time;
        }
    }
}
