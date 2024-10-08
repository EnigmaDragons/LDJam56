using System;
using DG.Tweening;
 using UnityEngine;

public class CharacterLevelUp : OnMessage<EnemyKilled>
{
    [SerializeField] private GameObject levelUpControl;
    [SerializeField] private GameplayRules rules;
    [SerializeField] private AllAbilities allAbilities;

    private void Start()
    {
        levelUpControl.SetActive(false);
    }
    
    protected override void Execute(EnemyKilled msg)
    {
        CurrentGameState.UpdateState(s => s.PlayerStats.XP += 2);
        if (CurrentGameState.ReadonlyGameState.PlayerStats.XP >= rules.XpNeededToLevel) {
            levelUpControl.SetActive(true);
            levelUpControl.transform.localScale = new Vector3(0, 1, 1);
            levelUpControl.transform.DOScaleX(1, 1.2f);
        }
    }

    private void Update()
    {
        if (Time.deltaTime == 0)
            return;
        if (CurrentGameState.ReadonlyGameState.PlayerStats.XP >= rules.XpNeededToLevel && Input.GetButtonDown("LevelUp"))
        {
            CurrentGameState.UpdateState(s =>
            {
                s.PlayerStats.Level += 1;
                s.PlayerStats.XP -= rules.XpNeededToLevel;
                s.PlayerStats.CurrentLife = Math.Min(s.PlayerStats.MaxLife, s.PlayerStats.CurrentLife + 1);
            });
            levelUpControl.SetActive(CurrentGameState.ReadonlyGameState.PlayerStats.XP >= rules.XpNeededToLevel);
            Message.Publish(new PlayerGainedCode(allAbilities.Random()));
        }
    }
}