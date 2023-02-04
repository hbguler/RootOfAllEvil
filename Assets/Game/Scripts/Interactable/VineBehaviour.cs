using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Interactable
{
    public class VineBehaviour : InteractableBehaviour
    {
        public static event Action ClimbCompleted;

        [SerializeField] private Transform _landingTransform;
        private Tween _climbTween;
        private PlayerBehaviour _player;

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerBehaviour>() == null)
            {
                return;
            }
            
            Debug.LogError("End reached, killing prev. tween and moving to end");
            _climbTween.Kill();
            _climbTween = null;
            
            _player.gameObject.transform.DOMove(_landingTransform.position, 0.25f).OnComplete(() =>
            {
                ClimbCompleted?.Invoke();
            });
        }

        public void Climb(PlayerBehaviour playerBehaviour)
        {
            _player = playerBehaviour;
            
            float distance = Vector3.Distance(playerBehaviour.transform.position, _landingTransform.position);
            float time = distance / GameConfig.ClimbSpeed;

            _climbTween = playerBehaviour.transform.DOMove(
                new Vector3(
                    playerBehaviour.transform.position.x,
                    _landingTransform.position.y,
                    playerBehaviour.transform.position.z), time).SetEase(Ease.Linear);
        }
    }
}
