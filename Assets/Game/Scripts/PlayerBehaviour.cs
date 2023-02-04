using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Interactable;
using Game.Scripts.Movement;
using UnityEngine;

namespace Game.Scripts
{
    public enum PlayerType
    {
        None = 0,
        Old = 1,
        Modern = 2
    }

    public enum InteractionType
    {
        None = 0,
        Climb = 1,
        Carry = 2
    }
    
    public class PlayerBehaviour : CharacterBehaviour
    {
        [SerializeField] private PlayerMovementBehaviour _pmb;
        [SerializeField] private List<Animator> _animators;
        private PlayerType _playerType;
        [SerializeField] private SubPlayerBehaviour _oldPlayer;
        [SerializeField] private SubPlayerBehaviour _modernPlayer;

        private float _attackIntervalTime = 2f;
        private bool _canAttack = false;
        private Coroutine _attackCoroutine;

        private InteractableBehaviour _currentInteractable;
        private InteractionType _currentInteractionType;

        public void Initialize()
        {
            _pmb.Initialize();
            _canAttack = true;
            SwitchCharacter();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _canAttack)
            {
                _attackCoroutine = StartCoroutine(AttackCoroutine());
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchCharacter();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.LogError("Interaction requested");
                switch (_currentInteractionType)
                {
                    case InteractionType.None:
                        break;
                    case InteractionType.Climb:
                        Debug.LogError("Climb started");

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
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Interactable"))
            {
                if (other.gameObject.GetComponentInParent<VineBehaviour>() != null)
                {
                    _currentInteractable = other.gameObject.GetComponentInParent<VineBehaviour>();
                    _currentInteractionType = InteractionType.Climb;
                    
                    Debug.LogError("Climb enter");
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
                
                Debug.LogError("Interaction exit");
            }
        }

        private IEnumerator AttackCoroutine()
        {
            _canAttack = false;
            
            if (_playerType == PlayerType.Modern)
            {
                _modernPlayer.Animator.SetTrigger("RangeAttack");
                _modernPlayer.Weapon.Attack(transform.forward);
                yield return new WaitForSeconds(GameConfig.RangedAttackInterval);
            }
            else if (_playerType == PlayerType.Old)
            {
                _oldPlayer.Animator.SetTrigger("MeleeAttack");
                _oldPlayer.Weapon.Attack(transform.forward);
                yield return new WaitForSeconds(GameConfig.MeleeAttackInterval);
            }
            
            _canAttack = true;
        }

        private void SwitchCharacter()
        {
            if (_playerType == PlayerType.None)
            {
                _playerType = PlayerType.Old;
            }
            
            if (_playerType == PlayerType.Modern)
            {
                _playerType = PlayerType.Old;
                _oldPlayer.gameObject.SetActive(true);
                _modernPlayer.gameObject.SetActive(false);
            }
            else if (_playerType == PlayerType.Old)
            {
                _playerType = PlayerType.Modern;
                _oldPlayer.gameObject.SetActive(false);
                _modernPlayer.gameObject.SetActive(true);
            }
        }

        public override void Die()
        {
            base.Die();
            if (_playerType == PlayerType.Modern)
            {
                _modernPlayer.Animator.SetTrigger("Die");
            }
            else if (_playerType == PlayerType.Old)
            {
                _oldPlayer.Animator.SetTrigger("Die");
            }
        }
    }
}
