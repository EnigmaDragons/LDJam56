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

    private Camera _mainCamera;
    private GameObject _player;

    private void Start()
    {
        _mainCamera = Camera.main;
        _player = GameObject.FindWithTag("Player");
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
            var speed = Instantiate(speedPrefab, _player.transform);
            speed.Init(first, msg.Ability, abilityData.Skip(1).ToArray());
        }
        if (first.Type == AbilityComponentType.Projectile)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            if (groundPlane.Raycast(ray, out float rayDistance))
            {
                Vector3 targetPoint = ray.GetPoint(rayDistance);
                targetPoint.y = _player.transform.position.y; // Set Y to player's height
                Vector3 direction = (targetPoint - _player.transform.position).normalized;
                var projectile = Instantiate(projectilePrefab, _player.transform.position, Quaternion.identity,
                    _player.transform.parent);
                projectile.Init(_player.transform.position, direction, first, msg.Ability,
                    abilityData.Skip(1).ToArray());
            }
            else
            {
                Debug.LogError("targeting failed for projectile");
            }
        }
        if (first.Type == AbilityComponentType.Shield)
        {
            var shield = Instantiate(shieldPrefab, _player.transform);
            shield.Init(first, msg.Ability, abilityData.Skip(1).ToArray());
        }
        if (first.Type == AbilityComponentType.Explode)
        {
            var explode = Instantiate(explodePrefab, _player.transform.position, _player.transform.rotation, _player.transform.parent);
            explode.Init(_player.transform.position, first, msg.Ability, abilityData.Skip(1).ToArray());
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