using System;
using System.Collections;
using DG.Tweening;
using Game.Scripts.Movement;
using UnityEngine;

namespace Game.Scripts
{
    public class EnemyBehaviour : CharacterBehaviour
    {
        [SerializeField] private GameObject _model;
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

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                BulletBehaviour bullet = collider.gameObject.GetComponent<BulletBehaviour>();
                bullet.Kill();
                TakeHit(GameConfig.RangeDamage);
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer("MeleeWeapon"))
            {
                TakeHit(GameConfig.MeleeDamage);

                Vector3 direction = transform.position - _player.transform.position;
                _cmb.Knockback(direction);
            }
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
            //_animator.SetTrigger(MeleeAttack);
            _model.transform.DOMove(_player.transform.position, 0.25f);
            yield return new WaitForSeconds(0.25f);

            if (GetDistanceBetweenPlayerAndEnemy() < _attackDistance)
            {
                _player.TakeHit(GameConfig.MeleeDamage);
            }
        }

        public override void Die()
        {
            base.Die();
            //_animator.SetTrigger("Die");
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }

            if (_seekCoroutine != null)
            {
                StopCoroutine(_seekCoroutine);
                _seekCoroutine = null;
            }

            if (_chaseCoroutine != null)
            {
                StopCoroutine(_chaseCoroutine);
                _chaseCoroutine = null;
            }
            
            _cmb.Stop();

            _model.transform.DOShakePosition(1.5f, strength: 0.5f).OnComplete(() =>
            {
                _model.transform.DOMoveY(_model.transform.position.y - 2f, 1f).SetEase(Ease.InCubic).SetDelay(0.5f);
            });

            _model.transform.DOShakeRotation(1.5f, strength: 0.25f);
        }

        private float GetDistanceBetweenPlayerAndEnemy()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }
    }
}