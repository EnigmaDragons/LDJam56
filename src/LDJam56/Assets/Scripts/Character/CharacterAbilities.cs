using KinematicCharacterController;
using UnityEngine;

public class CharacterAbilities : MonoBehaviour
{
    [SerializeField] private KinematicCharacterMotor motor;
    [SerializeField] private float queuedTime;

    private float _timeSinceLastQueuedInput;
    private AbilityType _queuedInput;
    
    private void Update()
    {
        if (Time.timeScale == 0)
            return;
        CurrentGameState.LowerCooldowns(Time.deltaTime);
        if (Input.GetButton("Defense"))
        {
            _queuedInput = AbilityType.Defense;
            _timeSinceLastQueuedInput = 0;
        }
        else if (Input.GetButton("Mobility"))
        {
            _queuedInput = AbilityType.Mobility;
            _timeSinceLastQueuedInput = 0;
        }
        else if (Input.GetButton("Special"))
        {
            _queuedInput = AbilityType.Special;
            _timeSinceLastQueuedInput = 0;
        }
        else if (Input.GetButton("Attack"))
        {
            _queuedInput = AbilityType.Attack;
            _timeSinceLastQueuedInput = 0;
        }
        else
        {
            _timeSinceLastQueuedInput += Time.deltaTime;
            if (_timeSinceLastQueuedInput >= queuedTime)
                _queuedInput = AbilityType.Passive;
        }

        if (!motor.GroundingStatus.IsStableOnGround || CurrentGameState.ReadonlyGameState.PlayerStats.IsSilenced.AnyNonAlloc())
            return;
            
        if (CurrentGameState.ReadonlyGameState.Defense.CooldownRemaining <= 0 && _queuedInput == AbilityType.Defense)
        {
            CurrentGameState.UpdateState(s => s.Defense.CooldownRemaining = s.Defense.Cooldown(s.PlayerStats));
            Message.Publish(new ActivateAbility(AbilityType.Defense));
            _queuedInput = AbilityType.Passive;
        }
        else if (CurrentGameState.ReadonlyGameState.Mobility.CooldownRemaining <= 0 && _queuedInput == AbilityType.Mobility)
        {
            CurrentGameState.UpdateState(s => s.Mobility.CooldownRemaining = s.Mobility.Cooldown(s.PlayerStats));
            Message.Publish(new ActivateAbility(AbilityType.Mobility));
            _queuedInput = AbilityType.Passive;
        }
        else if (CurrentGameState.ReadonlyGameState.Special.CooldownRemaining <= 0 && _queuedInput == AbilityType.Special)
        {
            CurrentGameState.UpdateState(s => s.Special.CooldownRemaining = s.Special.Cooldown(s.PlayerStats));
            Message.Publish(new ActivateAbility(AbilityType.Special));
            _queuedInput = AbilityType.Passive;
        }
        else if (CurrentGameState.ReadonlyGameState.Attack.CooldownRemaining <= 0 && _queuedInput == AbilityType.Attack)
        {
            CurrentGameState.UpdateState(s => s.Attack.CooldownRemaining = s.Attack.Cooldown(s.PlayerStats));
            Message.Publish(new ActivateAbility(AbilityType.Attack));
            _queuedInput = AbilityType.Passive;
        }
    }
}