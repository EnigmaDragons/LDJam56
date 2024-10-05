using System;
using KinematicCharacterController;
using UnityEngine;

namespace Character
{
    public class NanobotCharacter : MonoBehaviour, ICharacterController
    {
        [SerializeField] private KinematicCharacterMotor motor;
        [SerializeField] private float baseSpeed = 200;
        [SerializeField] private float rotationSpeed = 400;
        [SerializeField] private float deadZone = 0.2f;
        [SerializeField] private float gravity = 50;
        [SerializeField] private float drag = 0.3f;

        private Vector3 _inputDirection;
        
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
            if (motor.GroundingStatus.IsStableOnGround)
            {
                currentVelocity = _inputDirection * CurrentGameState.ReadonlyGameState.PlayerStats.Speed * deltaTime * baseSpeed;
            }
            else
            {
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
    }
}