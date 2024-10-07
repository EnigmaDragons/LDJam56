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
        var first = abilityData[0];
        if (first.Type == AbilityComponentType.Speed)
        {
            var speed = Instantiate(speedPrefab, player.transform);
            speed.Init(first, msg.Ability, abilityData.Skip(1).ToArray());
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

    private IEnumerator ShootProjectile(float delay, AbilityData data, AbilityType type, AbilityData[] nextAbilities)
    {
        yield return new WaitForSeconds(delay);
        var startingPosition = player.transform.position + new Vector3(0, projectilePrefab.transform.localPosition.y, 0) + _direction * forwardOffset;
        var projectile = Instantiate(projectilePrefab, startingPosition, Quaternion.LookRotation(_direction), player.transform.parent);
        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.ShootOne, projectile.gameObject));
        projectile.Init(startingPosition, _direction, data, type, nextAbilities);
    } 
}