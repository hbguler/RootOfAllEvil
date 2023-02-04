using System;
using System.Collections;
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
            _playerType = PlayerType.Modern;
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
                
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<InteractableBehaviour>() != null)
            {
                if (other.gameObject.GetComponent<VineBehaviour>() != null)
                {
                    _currentInteractable = other.gameObject.GetComponent<VineBehaviour>();
                    _currentInteractionType = InteractionType.Climb;
                }
                else if (other.gameObject.GetComponent<RockBehaviour>() != null)
                {
                    _currentInteractable = other.gameObject.GetComponent<RockBehaviour>();
                    _currentInteractionType = InteractionType.Carry;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<InteractableBehaviour>())
            {
                _currentInteractable = null;
                _currentInteractionType = InteractionType.None;
            }
        }

        private IEnumerator AttackCoroutine()
        {
            if (_playerType == PlayerType.Modern)
            {
                _modernPlayer.Animator.SetTrigger("RangeAttack");
                _modernPlayer.Weapon.Attack(transform.forward);
            }
            else if (_playerType == PlayerType.Old)
            {
                _oldPlayer.Animator.SetTrigger("MeleeAttack");
                _oldPlayer.Weapon.Attack(transform.forward);
            }
            
            _canAttack = false;
            yield return new WaitForSeconds(_attackIntervalTime);
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
