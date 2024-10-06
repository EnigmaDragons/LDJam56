using System;
using System.Collections.Generic;

[Serializable]
public class PlayerStats
{
    public float Speed = 1;
    public float MaxLife = 5;
    public float CurrentLife = 5;
    public float Cooldown = 1;
    public List<string> IsInvincible = new List<string>();
}