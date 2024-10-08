using System.Collections.Generic;
using UnityEngine;

public sealed class InitCurrentGameState : MonoBehaviour
{
    [SerializeField] private GameplayRules rules;
    [SerializeField] private AllAbilities abilities;
    [SerializeField] private SelectedDifficulty difficulty;
    
    void Awake()
    {
        if (rules == null)
        {
            Log.Error("GameplayRules is null", this);
        }
        else
        {
            CurrentGameState.Init(new GameState
            {
                PlayerStats = new PlayerStats { MaxLife = rules.Health(difficulty.Difficulty), CurrentLife = rules.Health(difficulty.Difficulty) },
                Attack = new Ability 
                { 
                    AbilityType = AbilityType.Attack, 
                    Components = new List<AbilityComponentType> { AbilityComponentType.Projectile },
                    CooldownRemaining = 0,
                    BaseCooldown = rules.AttackCooldown + abilities.GetAbility(AbilityComponentType.Projectile).AttackCooldown
                },
                Special = new Ability 
                { 
                    AbilityType = AbilityType.Special, 
                    Components = new List<AbilityComponentType> { AbilityComponentType.Explode },
                    CooldownRemaining = 0,
                    BaseCooldown = rules.SpecialCooldown + abilities.GetAbility(AbilityComponentType.Explode).SpecialCooldown
                },
                Mobility = new Ability 
                { 
                    AbilityType = AbilityType.Mobility, 
                    Components = new List<AbilityComponentType> { AbilityComponentType.Speed },
                    CooldownRemaining = 0,
                    BaseCooldown = rules.MobilityCooldown + abilities.GetAbility(AbilityComponentType.Speed).MobilityCooldown
                },
                Defense = new Ability
                {
                    AbilityType = AbilityType.Defense,
                    Components = new List<AbilityComponentType> { AbilityComponentType.Shield },
                    CooldownRemaining = 0,
                    BaseCooldown = rules.DefenseCooldown + abilities.GetAbility(AbilityComponentType.Shield).DefenseCooldown
                }
            });
        }
    }
}
