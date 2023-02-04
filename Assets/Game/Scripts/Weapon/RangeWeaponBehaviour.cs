using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Weapon
{
    public class RangeWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private Transform _weaponTip;
        [SerializeField] private BulletBehaviour _bulletPrefab;

        [SerializeField] private float _bulletSpeed;

        private Coroutine _attackCoroutine;

        public override void Attack(Vector3 direction)
        {
            _attackCoroutine = StartCoroutine(AttackCoroutine(direction));
        }

        private IEnumerator AttackCoroutine(Vector3 direction)
        {
            yield return new WaitForSeconds(0.5f);
            
            BulletBehaviour bullet = Instantiate(_bulletPrefab);
            bullet.transform.position = _weaponTip.transform.position;
            bullet.transform.localScale = Vector3.one;

            Vector3 bulletTarget = bullet.transform.position + direction * 20;
            float time = bulletTarget.magnitude / _bulletSpeed;
            bullet.transform.DOMove(bulletTarget, time).OnComplete(() => { Destroy(bullet.gameObject); })
                .SetEase(Ease.Linear);
        }
    }
}
