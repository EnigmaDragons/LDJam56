using System;
using System.Collections.Generic;

[Serializable]
public class Ability
{
    public AbilityType AbilityType;
    public List<AbilityComponentType> Components;
    public float CooldownRemaining;

    public float Cooldown(GameplayRules rules, PlayerStats stats) => GetAbilityCooldown(rules) * stats.Cooldown;

    private float GetAbilityCooldown(GameplayRules rules)
    {
        if (AbilityType == AbilityType.Attack)
            return rules.AttackCooldown;
        else if (AbilityType == AbilityType.Special)
            return rules.SpecialCooldown;
        else if (AbilityType == AbilityType.Mobility)
            return rules.MobilityCooldown;
        else if (AbilityType == AbilityType.Defense)
            return rules.DefenseCooldown;
        return 0;
    }
}