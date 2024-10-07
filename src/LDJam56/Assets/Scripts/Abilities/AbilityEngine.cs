using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AbilityEngine : OnMessage<ActivateAbility, TargetingUpdated>
{
    [SerializeField] private GameplayRules rules;
    [SerializeField] private AllAbilities allAbilities;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Explode explodePrefab;
    [SerializeField] private Shield shieldPrefab;
    [SerializeField] private Speed speedPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private float forwardOffset;
    
    private Vector3 _direction;
    
    protected override void Execute(ActivateAbility msg)
    {
        var ability = CurrentGameState.GetAbility(msg.Ability);
        var abilityData = ability.Components.Select(x => allAbilities.GetAbility(x)).ToArray();
        if (abilityData.AnyNonAlloc(x => x == null))
        {
            Debug.LogError("ability missing");
            return;
        }
        while ((msg.Ability == AbilityType.Attack || msg.Ability == AbilityType.Special) && abilityData[0].Type == AbilityComponentType.Speed)
        {
            if (msg.Ability == AbilityType.Attack)
                CurrentGameState.UpdateState(s => s.Attack.CooldownRemaining *= 0.5f);
            if (msg.Ability == AbilityType.Special)
                CurrentGameState.UpdateState(s => s.Special.CooldownRemaining *= 0.5f);
            abilityData = abilityData.Skip(1).ToArray();
        }
        var first = abilityData[0];
        if (first.Type == AbilityComponentType.Speed)
        {
            var nextAbilities = abilityData.Skip(1).ToArray();
            if (nextAbilities.Length > 0 && nextAbilities[0].Type == AbilityComponentType.Shield)
            {
                var shield = Instantiate(shieldPrefab, player.transform);
                shield.Init(CurrentGameState.ReadonlyGameState.PlayerStats.Potency, first, msg.Ability, nextAbilities);
                nextAbilities = Array.Empty<AbilityData>();
            }
            var speed = Instantiate(speedPrefab, player.transform);
            speed.Init(CurrentGameState.ReadonlyGameState.PlayerStats.Potency, first, msg.Ability, nextAbilities);
        }
        if (first.Type == AbilityComponentType.Projectile)
        {
            var projectilesInARow = 0;
            foreach (var x in abilityData)
            {
                if (x.Type == AbilityComponentType.Projectile)
                    projectilesInARow += 1;
                else
                    break;
            }
            for (var i = 0; i < projectilesInARow; i++)
                StartCoroutine(ShootProjectile(i * 0.2f, first, msg.Ability, abilityData.Skip(projectilesInARow).ToArray()));
        }
        if (first.Type == AbilityComponentType.Shield)
        {
            var shield = Instantiate(shieldPrefab, player.transform);
            shield.Init(CurrentGameState.ReadonlyGameState.PlayerStats.Potency, first, msg.Ability, abilityData.Skip(1).ToArray());
        }
        if (first.Type == AbilityComponentType.Explode)
        {
            var startingPosition = player.transform.position + explodePrefab.transform.localPosition;
            var explode = Instantiate(explodePrefab, startingPosition, player.transform.rotation, player.transform.parent);
            explode.Init(CurrentGameState.ReadonlyGameState.PlayerStats.Potency, true, startingPosition, first, msg.Ability, abilityData.Skip(1).ToArray());
        }
    }

    protected override void Execute(TargetingUpdated msg)
    {
        _direction = msg.Direction;
    }

    private IEnumerator ShootProjectile(float delay, AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        yield return new WaitForSeconds(delay);
        var startingPosition = player.transform.position + new Vector3(0, projectilePrefab.transform.localPosition.y, 0) + _direction * forwardOffset;
        var projectile = Instantiate(projectilePrefab, startingPosition, Quaternion.LookRotation(_direction), player.transform.parent);
        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.ShootOne, projectile.gameObject));
        projectile.Init(CurrentGameState.ReadonlyGameState.PlayerStats.Potency, startingPosition, _direction, data, type, nextAbilities);
        for (var i = 1; i < CurrentGameState.ReadonlyGameState.PlayerStats.Projectile; i++)
        {
            var leftDirection = Quaternion.Euler(0, 15 * i, 0) * _direction;
            var rightDirection = Quaternion.Euler(0, -15 * i, 0) * _direction;
            var leftStartingPosition = player.transform.position + new Vector3(0, projectilePrefab.transform.localPosition.y, 0) + leftDirection * forwardOffset;
            var rightStartingPosition = player.transform.position + new Vector3(0, projectilePrefab.transform.localPosition.y, 0) + rightDirection * forwardOffset;
            var leftProjectile = Instantiate(projectilePrefab, leftStartingPosition, Quaternion.LookRotation(leftDirection), player.transform.parent);
            var rightProjectile = Instantiate(projectilePrefab, rightStartingPosition, Quaternion.LookRotation(rightDirection), player.transform.parent);
            leftProjectile.Init(CurrentGameState.ReadonlyGameState.PlayerStats.Potency, leftStartingPosition, leftDirection, data, type, nextAbilities);
            rightProjectile.Init(CurrentGameState.ReadonlyGameState.PlayerStats.Potency, rightStartingPosition, rightDirection, data, type, nextAbilities);
        }
    } 
}