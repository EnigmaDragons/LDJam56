using UnityEngine;

[CreateAssetMenu]
public class GameplayRules : ScriptableObject
{
    [SerializeField] private int hardHealth = 1;
    [SerializeField] private int mediumHealth = 5;
    [SerializeField] private int easyHealth = 15;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float specialCooldown;
    [SerializeField] private float mobilityCooldown;
    [SerializeField] private float defenseCooldown;
    [SerializeField] private int xpNeededToLevel;
    [SerializeField] private int maxAbilityComponents;

    public int Health(Difficulty difficulty)
    {
        if (difficulty == Difficulty.Hard)
            return hardHealth;
        if (difficulty == Difficulty.Medium)
            return mediumHealth;
        if (difficulty == Difficulty.Easy)
            return easyHealth;
        return mediumHealth;
    }
    public float AttackCooldown => attackCooldown;
    public float SpecialCooldown => specialCooldown;
    public float MobilityCooldown => mobilityCooldown;
    public float DefenseCooldown => defenseCooldown;
    public int XpNeededToLevel => xpNeededToLevel;
    public int MaxAbilityComponents => maxAbilityComponents;
}