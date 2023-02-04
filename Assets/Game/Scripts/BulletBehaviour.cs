using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class BulletBehaviour : MonoBehaviour
    {
        private Tween _moveTween;
        
        public void Fire(Vector3 bulletTarget, float time, Action action)
        {
            _moveTween = transform.DOMove(bulletTarget, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                action?.Invoke();
            });
        }

        public void Kill()
        {
            _moveTween.Kill();
            Destroy(gameObject);
        }
    }
}
