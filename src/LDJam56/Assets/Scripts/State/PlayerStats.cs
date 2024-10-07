using System;
using System.Collections.Generic;

[Serializable]
public class PlayerStats
{
    public float Speed = 1;
    public float Cooldown = 1;
    public List<string> IsInvincible = new List<string>();
    public List<string> IsRooted = new List<string>();
    public List<string> IsSilenced = new List<string>();
    public int MaxLife = 5;
    public int CurrentLife = 5;
    public int Level = 1;
    public int XP = 0;
    public int Projectile = 1;
    public float Potency = 1;
    public float AoEPotency = 1;
    public float AfterHitShieldPotency = 1;
    public float Range = 1;
}