using System.Linq;
using UnityEngine;

public class AbilityEngine : OnMessage<ActivateAbility>
{
    [SerializeField] private GameplayRules rules;
    [SerializeField] private AbilityData[] allAbilities;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Explode explodePrefab;
    [SerializeField] private Shield shieldPrefab;
    [SerializeField] private Speed speedPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private float forwardOffset;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

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
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitPoint, Mathf.Infinity))
            {
                var position = hitPoint.point;
                position.y = 0;
                var direction = (position - new Vector3(player.transform.position.x, 0, player.transform.position.z)).normalized;
                var startingPosition = player.transform.position + new Vector3(0, projectilePrefab.transform.localPosition.y, 0) + direction * forwardOffset;
                var projectile = Instantiate(projectilePrefab, startingPosition, Quaternion.LookRotation(direction), player.transform.parent);
                projectile.Init(startingPosition, direction, first, msg.Ability,
                    abilityData.Skip(1).ToArray());   
            }
        }
        if (first.Type == AbilityComponentType.Shield)
        {
            var shield = Instantiate(shieldPrefab, player.transform);
            shield.Init(first, msg.Ability, abilityData.Skip(1).ToArray());
        }
        if (first.Type == AbilityComponentType.Explode)
        {
            var explode = Instantiate(explodePrefab, player.transform.position, player.transform.rotation, player.transform.parent);
            explode.Init(player.transform.position, first, msg.Ability, abilityData.Skip(1).ToArray());
        }
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