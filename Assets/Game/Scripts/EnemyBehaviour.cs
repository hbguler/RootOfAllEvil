using System;
using System.Collections;
using UnityEngine;

namespace Game.Scripts
{
    public class EnemyBehaviour : CharacterBehaviour
    {
        [SerializeField] private CharacterMovementBehaviour _cmb;
        [SerializeField] private float _seekDistance;
        [SerializeField] private float _attackDistance;
        
        private readonly string MeleeAttack = "MeleeAttack";
        
        private PlayerBehaviour _player;
        private Coroutine _seekCoroutine;
        private Coroutine _chaseCoroutine;
        private Coroutine _attackCoroutine;

        public void Initialize(PlayerBehaviour player)
        {
            _player = player;
            
            _cmb.Initialize();
            _seekCoroutine = StartCoroutine(SeekCoroutine());
        }

        private IEnumerator SeekCoroutine()
        {
            while (true)
            {
                float distance = GetDistanceBetweenPlayerAndEnemy();
                if (distance < _seekDistance)
                {
                    StartChasing();
                    yield break;
                }

                yield return null;
            }
        }

        private void StartChasing()
        {
            _seekCoroutine = null; 
            _chaseCoroutine = StartCoroutine(ChaseCoroutine());
        }

        private IEnumerator ChaseCoroutine()
        {
            while (true)
            {
                float distance = GetDistanceBetweenPlayerAndEnemy();

                if (distance < _attackDistance)
                {
                    _cmb.Stop();

                    if (_attackCoroutine == null)
                    {
                        _attackCoroutine = StartCoroutine(AttackCoroutine());
                    }
                }
                else
                {
                    Vector3 targetVector = (_player.transform.position - transform.position).normalized;
                    _cmb.SetMovementVector(targetVector);
                }
                
                yield return null;
            }
        }

        private IEnumerator AttackCoroutine()
        {
            Animator.SetTrigger(MeleeAttack);

            yield return new WaitForSeconds(0.75f);

            if (GetDistanceBetweenPlayerAndEnemy() < _attackDistance)
            {
                _player.TakeHit(GameConfig.MeleeDamage);
            }
        }

        private float GetDistanceBetweenPlayerAndEnemy()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }
    }
}
