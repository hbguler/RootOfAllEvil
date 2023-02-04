using System.Collections;
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
    
    public class PlayerBehaviour : CharacterBehaviour
    {
        [SerializeField] private PlayerMovementBehaviour _pmb;
        private PlayerType _playerType;
        [SerializeField] private SubPlayerBehaviour _oldPlayer;
        [SerializeField] private SubPlayerBehaviour _modernPlayer;

        private float _attackIntervalTime = 2f;
        private bool _canAttack = false;
        private Coroutine _attackCoroutine;

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

            if (Input.GetKeyDown(KeyCode.E))
            {
                SwitchCharacter();
            }
        }

        private IEnumerator AttackCoroutine()
        {
            if (_playerType == PlayerType.Modern)
            {
                _modernPlayer.Animator.SetTrigger("RangeAttack");
            }
            else if (_playerType == PlayerType.Old)
            {
                _oldPlayer.Animator.SetTrigger("MeleeAttack");
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
