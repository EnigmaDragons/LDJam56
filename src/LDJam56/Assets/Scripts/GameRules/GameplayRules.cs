using UnityEngine;

[CreateAssetMenu]
public class GameplayRules : ScriptableObject
{
    [SerializeField] private int playerHealth;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float specialCooldown;
    [SerializeField] private float mobilityCooldown;
    [SerializeField] private float defenseCooldown;

    public int PlayerHealth => playerHealth;
    public float AttackCooldown => attackCooldown;
    public float SpecialCooldown => specialCooldown;
    public float MobilityCooldown => mobilityCooldown;
    public float DefenseCooldown => defenseCooldown;
}