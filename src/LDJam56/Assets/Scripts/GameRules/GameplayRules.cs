using UnityEngine;

[CreateAssetMenu]
public class GameplayRules : ScriptableObject
{
    [SerializeField] private int playerHealth;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float specialCooldown;
    [SerializeField] private float mobilityCooldown;
    [SerializeField] private float defenseCooldown;
    [SerializeField] private int xpNeededToLevel;
    [SerializeField] private int maxAbilityComponents;

    public int PlayerHealth => playerHealth;
    public float AttackCooldown => attackCooldown;
    public float SpecialCooldown => specialCooldown;
    public float MobilityCooldown => mobilityCooldown;
    public float DefenseCooldown => defenseCooldown;
    public int XpNeededToLevel => xpNeededToLevel;
    public int MaxAbilityComponents => maxAbilityComponents;
}