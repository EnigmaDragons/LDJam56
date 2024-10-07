using UnityEngine;

public class CharacterLevelUp : OnMessage<EnemyDied>
{
    [SerializeField] private GameObject levelUpControl;
    [SerializeField] private GameplayRules rules;
    [SerializeField] private AllAbilities allAbilities;
    
    protected override void Execute(EnemyDied msg)
    {
        CurrentGameState.UpdateState(s => s.PlayerStats.XP += 1);
        if (CurrentGameState.ReadonlyGameState.PlayerStats.XP >= rules.XpNeededToLevel);
        levelUpControl.SetActive(true);
    }

    private void Update()
    {
        if (CurrentGameState.ReadonlyGameState.PlayerStats.XP >= rules.XpNeededToLevel && Input.GetButtonDown("LevelUp"))
        {
            CurrentGameState.UpdateState(s =>
            {
                s.PlayerStats.Level += 1;
                s.PlayerStats.XP -= rules.XpNeededToLevel;
            });
            Message.Publish(new PlayerGainedCode(allAbilities.Random()));
        }
    }
}