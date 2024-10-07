using System;
using System.Collections.Generic;

[Serializable]
public sealed class GameState
{
    public PlayerStats PlayerStats = new PlayerStats();
    public Ability Passives = new Ability { AbilityType = AbilityType.Passive, Components = new List<AbilityComponentType>(), CooldownRemaining = 0 };
    public Ability Attack = new Ability { AbilityType = AbilityType.Attack, Components = new List<AbilityComponentType> { AbilityComponentType.Projectile }, CooldownRemaining = 0 };
    public Ability Special = new Ability { AbilityType = AbilityType.Special, Components = new List<AbilityComponentType> { AbilityComponentType.Explode }, CooldownRemaining = 0 };
    public Ability Mobility = new Ability { AbilityType = AbilityType.Mobility, Components = new List<AbilityComponentType> { AbilityComponentType.Speed }, CooldownRemaining = 0 };
    public Ability Defense = new Ability { AbilityType = AbilityType.Defense, Components = new List<AbilityComponentType> { AbilityComponentType.Shield }, CooldownRemaining = 0 };
}
