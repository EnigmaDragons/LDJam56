using System;
using KinematicCharacterController;
using UnityEngine;

public class NanobotCharacter : OnMessage<TeleportPlayer, PlayerDamaged>, ICharacterController
{
    [SerializeField] private KinematicCharacterMotor motor;
    [SerializeField] private float baseSpeed = 200;
    [SerializeField] private float rotationSpeed = 400;
    [SerializeField] private float deadZone = 0.2f;
    [SerializeField] private float gravity = 50;
    [SerializeField] private float drag = 0.3f;

    [SerializeField] private AbilityData afterDamageShield;
    [SerializeField] private Shield shieldPrefab; 

    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController idle;
    [SerializeField] private RuntimeAnimatorController walk;
    [SerializeField] private RuntimeAnimatorController run;
    private enum AnimationState
    {
        Idle,
        Walk,
        Run
    }
    private AnimationState _currentAnimation = AnimationState.Idle;


    private Vector3 _inputDirection;
    private bool _isTeleporting = false;
    
    private void Start()
    {
        motor.CharacterController = this;
    }

    private void Update()
    {
        var verticalMovement = Input.GetAxis("Vertical");
        var horizontalMovement = Input.GetAxis("Horizontal");
        if (Math.Abs(verticalMovement) < deadZone)
            verticalMovement = 0;
        if (Math.Abs(horizontalMovement) < deadZone)
            horizontalMovement = 0;
        _inputDirection = Vector3.Normalize(new Vector3(horizontalMovement, 0, verticalMovement));
    }
    
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        if (_inputDirection == Vector3.zero)
            return;
        currentRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_inputDirection), rotationSpeed * Time.deltaTime);
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (_isTeleporting)
        {
            currentVelocity = Vector3.zero;
            _isTeleporting = false;
        }
        if (motor.GroundingStatus.IsStableOnGround)
        {
            if (_inputDirection == Vector3.zero)
                SetAnimation(AnimationState.Idle);
            else if (CurrentGameState.ReadonlyGameState.PlayerStats.Speed > 1.5f)
                SetAnimation(AnimationState.Run);
            else 
                SetAnimation(AnimationState.Walk);
            currentVelocity = _inputDirection * CurrentGameState.ReadonlyGameState.PlayerStats.Speed * deltaTime * baseSpeed;
        }
        else
        {
            SetAnimation(AnimationState.Idle);
            currentVelocity += new Vector3(0, -gravity, 0) * deltaTime;
            currentVelocity *= (1f / (1f + (drag * deltaTime)));
        }
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
    }

    public void PostGroundingUpdate(float deltaTime)
    {
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport)
    {
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
        Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
    }

    private void SetAnimation(AnimationState animation)
    {
        if (_currentAnimation == animation)
            return;
        _currentAnimation = animation;
        if (animation == AnimationState.Idle)
            animator.runtimeAnimatorController = idle;
        if (animation == AnimationState.Walk)
            animator.runtimeAnimatorController = walk;
        if (animation == AnimationState.Run)
            animator.runtimeAnimatorController = run;
    }

    protected override void Execute(TeleportPlayer msg)
    {
        motor.SetTransientPosition(msg.NewPosition);
        _isTeleporting = true;
    }

    protected override void Execute(PlayerDamaged msg)
    {
        var shield = Instantiate(shieldPrefab, gameObject.transform);
        shield.Init(afterDamageShield, AbilityType.Passive, Array.Empty<AbilityData>());
    }
}