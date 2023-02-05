using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Interactable;
using Game.Scripts.Levels;
using Game.Scripts.Movement;
using UnityEngine;

namespace Game.Scripts
{
    public enum InteractionType
    {
        None = 0,
        Climb = 1,
        Carry = 2
    }
    
    public class PlayerBehaviour : CharacterBehaviour
    {
        public static event Action CharacterDied;
        
        [SerializeField] private PlayerMovementBehaviour _pmb;
        [SerializeField] private List<Animator> _animators;
        private TimeState _playerType;
        [SerializeField] private SubPlayerBehaviour _oldPlayer;
        [SerializeField] private SubPlayerBehaviour _modernPlayer;

        public bool IsClimbing;
        
        private float _attackIntervalTime = 2f;
        private bool _canAttack = false;
        private Coroutine _attackCoroutine;

        private InteractableBehaviour _currentInteractable;
        private InteractionType _currentInteractionType;

        public void Initialize()
        {
            _pmb.Initialize();
            _canAttack = true;

            IsClimbing = false;
            
            ExitTrigger.ExitTriggered += OnExitTriggered;
            ArtifactBehaviour.ArtifactDestroyed += OnExitTriggered;

        }

        private void OnExitTriggered()
        {
            ExitTrigger.ExitTriggered -= OnExitTriggered;
            ArtifactBehaviour.ArtifactDestroyed -= OnExitTriggered;

            _pmb.ToggleMovement(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _canAttack && IsClimbing == false)
            {
                _attackCoroutine = StartCoroutine(AttackCoroutine());
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Interaction requested");
                switch (_currentInteractionType)
                {
                    case InteractionType.None:
                        break;
                    case InteractionType.Climb:
                        Debug.Log("Climb started");
                        IsClimbing = true;

                        _pmb.ToggleMovement(false);
                        foreach (Animator animator in _animators)
                        {
                            animator.SetTrigger("Climb");
                        }

                        (_currentInteractable as VineBehaviour).Climb(this);
                        VineBehaviour.ClimbCompleted += OnClimbCompleted;
                        break;
                    case InteractionType.Carry:
                        break;
                }
            }
        }

        public void OnClimbCompleted()
        {
            Debug.LogError("Climb completed");

            VineBehaviour.ClimbCompleted -= OnClimbCompleted;
            foreach (Animator animator in _animators)
                animator.SetTrigger("Idle");
            
            _pmb.ToggleMovement(true);
            IsClimbing = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Interactable"))
            {
                if (other.gameObject.GetComponentInParent<VineBehaviour>() != null)
                {
                    _currentInteractable = other.gameObject.GetComponentInParent<VineBehaviour>();
                    _currentInteractionType = InteractionType.Climb;
                    
                    Debug.Log("Climb enter");
                }
                else if (other.gameObject.GetComponentInParent<RockBehaviour>() != null)
                {
                    _currentInteractable = other.gameObject.GetComponentInParent<RockBehaviour>();
                    _currentInteractionType = InteractionType.Carry;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Interactable"))
            {
                _currentInteractable = null;
                _currentInteractionType = InteractionType.None;
                
                Debug.Log("Interaction exit");
            }
        }

        private IEnumerator AttackCoroutine()
        {
            _canAttack = false;
            
            if (_playerType == TimeState.Future)
            {
                _modernPlayer.Animator.SetTrigger("RangeAttack");
                _modernPlayer.Weapon.Attack(transform.forward);
                yield return new WaitForSeconds(GameConfig.RangedAttackInterval);
            }
            else if (_playerType == TimeState.Past)
            {
                _oldPlayer.Animator.SetTrigger("MeleeAttack");
                _oldPlayer.Weapon.Attack(transform.forward);
                yield return new WaitForSeconds(GameConfig.MeleeAttackInterval);
            }
            
            _canAttack = true;
        }

        public void SwitchCharacter(TimeState currentTimeState)
        {
            _playerType = currentTimeState;
            
            if (currentTimeState == TimeState.Past)
            {
                _oldPlayer.gameObject.SetActive(true);
                _modernPlayer.gameObject.SetActive(false);
            }
            else if (currentTimeState == TimeState.Future)
            {
                _oldPlayer.gameObject.SetActive(false);
                _modernPlayer.gameObject.SetActive(true);
            }
        }

        public override void Die()
        {
            base.Die();
            if (_playerType == TimeState.Future)
            {
                _modernPlayer.Animator.SetTrigger("Die");
            }
            else if (_playerType == TimeState.Past)
            {
                _oldPlayer.Animator.SetTrigger("Die");
            }
            
            _pmb.ToggleMovement(false);
            _canAttack = false;

            StartCoroutine(RestartCoroutine());
        }

        private IEnumerator RestartCoroutine()
        {
            yield return new WaitForSeconds(4f);
            CharacterDied?.Invoke();
        }
    }
}
