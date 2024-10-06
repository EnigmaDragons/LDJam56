using System.Linq;
using UnityEngine;

public class AbilityEngine : OnMessage<ActivateAbility, TargetingUpdated>
{
    [SerializeField] private GameplayRules rules;
    [SerializeField] private AbilityData[] allAbilities;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Explode explodePrefab;
    [SerializeField] private Shield shieldPrefab;
    [SerializeField] private Speed speedPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private float forwardOffset;
    
    private Vector3 _direction;
    
    protected override void Execute(ActivateAbility msg)
    {
        var ability = GetAbilityInst(msg.Ability);
        var abilityData = ability.Components.Select(x => allAbilities.FirstOrDefault(a => a.Type == x)).ToArray();
        if (abilityData.AnyNonAlloc(x => x == null))
        {
            Debug.LogError("ability missing");
            return;
        }
        var first = abilityData[0];
        if (first.Type == AbilityComponentType.Speed)
        {
            var speed = Instantiate(speedPrefab, player.transform);
            speed.Init(first, msg.Ability, abilityData.Skip(1).ToArray());
        }
        if (first.Type == AbilityComponentType.Projectile)
        {
            var startingPosition = player.transform.position + new Vector3(0, projectilePrefab.transform.localPosition.y, 0) + _direction * forwardOffset;
            var projectile = Instantiate(projectilePrefab, startingPosition, Quaternion.LookRotation(_direction), player.transform.parent);
            projectile.Init(startingPosition, _direction, first, msg.Ability, abilityData.Skip(1).ToArray());
        }
        if (first.Type == AbilityComponentType.Shield)
        {
            var shield = Instantiate(shieldPrefab, player.transform);
            shield.Init(first, msg.Ability, abilityData.Skip(1).ToArray());
        }
        if (first.Type == AbilityComponentType.Explode)
        {
            var startingPosition = player.transform.position + explodePrefab.transform.localPosition;
            var explode = Instantiate(explodePrefab, startingPosition, player.transform.rotation, player.transform.parent);
            explode.Init(true, first, msg.Ability, abilityData.Skip(1).ToArray());
        }
    }

    protected override void Execute(TargetingUpdated msg)
    {
        _direction = msg.Direction;
    }

    private Ability GetAbilityInst(AbilityType type)
    {
        Ability ability = null;
        if (type == AbilityType.Attack)
            ability = CurrentGameState.ReadonlyGameState.Attack;
        else if (type == AbilityType.Special)
            ability = CurrentGameState.ReadonlyGameState.Special;
        else if (type == AbilityType.Mobility)
            ability = CurrentGameState.ReadonlyGameState.Mobility;
        else if (type == AbilityType.Defense)
            ability = CurrentGameState.ReadonlyGameState.Defense;
        return ability;
    }
}