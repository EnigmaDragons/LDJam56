using KinematicCharacterController;
using UnityEngine;

public class CharacterAbilities : MonoBehaviour
{
    [SerializeField] private KinematicCharacterMotor motor;
    [SerializeField] private GameplayRules rules;
    [SerializeField] private float globalCooldown;

    private float _t;
    
    private void Update()
    {
        _t -= Time.deltaTime;
        CurrentGameState.LowerCooldowns(Time.deltaTime);
        if (_t > 0 || !motor.GroundingStatus.IsStableOnGround)
            return;
        if (CurrentGameState.ReadonlyGameState.Defense.CooldownRemaining <= 0 && Input.GetButton("Defense"))
        {
            CurrentGameState.UpdateState(s => s.Defense.CooldownRemaining = s.Defense.Cooldown(rules, s.PlayerStats));
            Message.Publish(new ActivateAbility(AbilityType.Defense));
            _t = globalCooldown;
        }
        else if (CurrentGameState.ReadonlyGameState.Mobility.CooldownRemaining <= 0 && Input.GetButton("Mobility"))
        {
            CurrentGameState.UpdateState(s => s.Mobility.CooldownRemaining = s.Mobility.Cooldown(rules, s.PlayerStats));
            Message.Publish(new ActivateAbility(AbilityType.Mobility));
            _t = globalCooldown;
        }
        else if (CurrentGameState.ReadonlyGameState.Special.CooldownRemaining <= 0 && Input.GetButton("Special"))
        {
            CurrentGameState.UpdateState(s => s.Special.CooldownRemaining = s.Special.Cooldown(rules, s.PlayerStats));
            Message.Publish(new ActivateAbility(AbilityType.Special));
            _t = globalCooldown;
        }
        else if (CurrentGameState.ReadonlyGameState.Attack.CooldownRemaining <= 0 && Input.GetButton("Attack"))
        {
            CurrentGameState.UpdateState(s => s.Attack.CooldownRemaining = s.Attack.Cooldown(rules, s.PlayerStats));
            Message.Publish(new ActivateAbility(AbilityType.Attack));
            _t = globalCooldown;
        }
    }
}