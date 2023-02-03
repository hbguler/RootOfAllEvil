using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class CharacterMovementBehaviour : MonoBehaviour
    {
        private readonly string _idleRunAnimationWeight = "IdleRun";
        
        [SerializeField] private float _characterMaxSpeed;
        [SerializeField] private float _characterAcceleration;
        [SerializeField] private float _characterDecceleration;

        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;

        private float _characterCurrentSpeed;
        private Vector3 _movementVector;

        private Coroutine _movementCoroutine;

        private Coroutine _rotationCoroutine;

        public void Initialize()
        {
            
        }

        public void SetMovementVector(Vector3 movementVector)
        {
            _movementVector = movementVector;
            
            if (_movementCoroutine == null)
                _movementCoroutine = StartCoroutine(MovementCoroutine());

            if (_rotationCoroutine == null)
                _rotationCoroutine = StartCoroutine(RotationCoroutine());
        }

        public void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
            _movementVector = Vector3.zero;
            _animator.SetFloat(_idleRunAnimationWeight, 0);
        }

        private IEnumerator MovementCoroutine()
        {
            while (true)
            {
                if (_movementVector.sqrMagnitude > 0)
                    _characterCurrentSpeed += Time.deltaTime * _characterAcceleration;
                else
                    _characterCurrentSpeed -= Time.deltaTime * _characterDecceleration;
                
                _characterCurrentSpeed = Mathf.Clamp(_characterCurrentSpeed, 0, _characterMaxSpeed);

                if (_movementVector.sqrMagnitude > 0)
                {
                    _rigidbody.velocity = _movementVector * _characterCurrentSpeed;
                }
                else
                {
                    _rigidbody.velocity = _rigidbody.transform.forward * _characterCurrentSpeed;
                }
                
                _animator.SetFloat(_idleRunAnimationWeight, _characterCurrentSpeed / _characterMaxSpeed);

                yield return null;
            }
        }

        private IEnumerator RotationCoroutine()
        {
            while (true)
            {
                if (_characterCurrentSpeed > 0 && _movementVector.magnitude > 0)
                {
                    _rigidbody.transform.DOLookAt(transform.position + _movementVector, 0.25f);
                }
 
                yield return null;
            }
        }

        public Vector3 GetMovementVector()
        {
            return _movementVector;
        }
    }
}
