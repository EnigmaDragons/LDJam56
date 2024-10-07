using System;
using System.Collections.Generic;

[Serializable]
public class Ability
{
    public AbilityType AbilityType;
    public List<AbilityComponentType> Components;
    public float CooldownRemaining;
    public float BaseCooldown;

    public float Cooldown(PlayerStats stats) => BaseCooldown * stats.Cooldown;
}